
using @absolute_import = @@__future__.absolute_import;

using @division = @@__future__.division;

using @print_function = @@__future__.print_function;

using @unicode_literals = @@__future__.unicode_literals;

using uuid;

using os;

using re;

using sys;

using chain = itertools.chain;

using Iterable = typing.Iterable;

using Text = typing.Text;

using Optional = typing.Optional;

using TensorProto = onnx_pb.TensorProto;

using ModelProto = onnx_pb.ModelProto;

using System;

using System.Collections.Generic;

using System.Linq;

public static class external_data_helper {
    
    public class ExternalDataInfo
        : object {
        
        public string basepath;
        
        public void checksum;
        
        public int length;
        
        public string location;
        
        public int offset;
        
        public ExternalDataInfo(object tensor) {
            // type: (TensorProto) -> None
            this.location = "";
            this.offset = null;
            this.length = null;
            this.checksum = null;
            this.basepath = "";
            foreach (var entry in tensor.external_data) {
                setattr(this, entry.key, entry.value);
            }
            if (this.offset) {
                this.offset = Convert.ToInt32(this.offset);
            }
            if (this.length) {
                this.length = Convert.ToInt32(this.length);
            }
        }
    }
    
    // SPDX-License-Identifier: Apache-2.0
    // 
    //     Load data from an external file for tensor.
    // 
    //     @params
    //     tensor: a TensorProto object.
    //     base_dir: directory that contains the external data.
    //     
    public static void load_external_data_for_tensor(object tensor, object base_dir) {
        // type: (TensorProto, Text) -> None
        if (tensor.HasField("raw_data")) {
            // already loaded
            return;
        }
        var info = new ExternalDataInfo(tensor);
        var file_location = _sanitize_path(info.location);
        var external_data_file_path = os.path.join(base_dir, file_location);
        using (var data_file = open(external_data_file_path, "rb")) {
            if (info.offset) {
                data_file.seek(info.offset);
            }
            if (info.length) {
                tensor.raw_data = data_file.read(info.length);
            } else {
                tensor.raw_data = data_file.read();
            }
        }
    }
    
    // 
    //     Loads external tensors into model
    // 
    //     @params
    //     model: ModelProto to load external data to
    //     base_dir: directory that contains external data
    //     
    public static void load_external_data_for_model(object model, object base_dir) {
        // type: (ModelProto, Text) -> None
        foreach (var tensor in _get_all_tensors(model)) {
            if (uses_external_data(tensor)) {
                load_external_data_for_tensor(tensor, base_dir);
                // After loading raw_data from external_data, change the state of tensors
                tensor.data_location = TensorProto.DEFAULT;
                // and remove external data
                tensor.external_data.Remove(":");
            }
        }
    }
    
    public static void set_external_data(
        object tensor,
        string location,
        int offset = null,
        int length = null,
        void checksum = null,
        void basepath = null) {
        // type: (...) -> None
        if (!tensor.HasField("raw_data")) {
            throw new ValueError("Tensor " + tensor.name + "does not have raw_data field. Cannot set external data for this tensor.");
        }
        tensor.external_data.Remove(":");
        tensor.data_location = TensorProto.EXTERNAL;
        foreach (var _tup_1 in new Dictionary<object, object> {
            {
                "location",
                location},
            {
                "offset",
                offset != null ? Convert.ToInt32(offset) : null},
            {
                "length",
                length != null ? Convert.ToInt32(length) : null},
            {
                "checksum",
                checksum},
            {
                "basepath",
                basepath}}.items()) {
            var k = _tup_1.Item1;
            var v = _tup_1.Item2;
            if (v != null) {
                var entry = tensor.external_data.add();
                entry.key = k;
                entry.value = v.ToString();
            }
        }
    }
    
    // 
    //     Call to set all tensors with raw data as external data. This call should preceed 'save_model'.
    //     'save_model' saves all the tensors data as external data after calling this function.
    //     @params
    //     model: ModelProto to be converted.
    //     all_tensors_to_one_file: If true, save all tensors to one external file specified by location.
    //                              If false, save each tensor to a file named with the tensor name.
    //     location: specify the external file that all tensors to save to.
    //               If not specified, will use the model name.
    //     size_threshold: Threshold for size of data. Only when tensor's data is >= the size_threshold
    //     it will be converted to external data. To convert every tensor with raw data to external data set size_threshold=0.
    //     
    public static void convert_model_to_external_data(object model, bool all_tensors_to_one_file = true, void location = null, int size_threshold = 1024) {
        // type: (ModelProto, bool, Optional[Text], int) -> None
        if (all_tensors_to_one_file) {
            var file_name = Text(uuid.uuid1());
            if (location) {
                file_name = location;
            }
            foreach (var tensor in _get_all_tensors(model)) {
                if (tensor.HasField("raw_data") && sys.getsizeof(tensor.raw_data) >= size_threshold) {
                    set_external_data(tensor, file_name);
                }
            }
        } else {
            foreach (var tensor in _get_all_tensors(model)) {
                if (tensor.HasField("raw_data") && sys.getsizeof(tensor.raw_data) >= size_threshold) {
                    var tensor_location = tensor.name;
                    if (!_is_valid_filename(tensor_location)) {
                        tensor_location = Text(uuid.uuid1());
                    }
                    set_external_data(tensor, tensor_location);
                }
            }
        }
    }
    
    // 
    //     Call to set all tensors which use external data as embedded data. save_model saves all the tensors data as embedded data after calling this function.
    //     @params
    //     model: ModelProto to be converted.
    //     
    public static void convert_model_from_external_data(object model) {
        // type: (ModelProto) -> None
        foreach (var tensor in _get_all_tensors(model)) {
            if (uses_external_data(tensor)) {
                if (!tensor.HasField("raw_data")) {
                    throw new ValueError("raw_data field doesn't exist.");
                }
                tensor.external_data.Remove(":");
                tensor.data_location = TensorProto.DEFAULT;
            }
        }
    }
    
    // 
    //     Write tensor data to an external file according to information in the `external_data` field.
    // 
    //     @params
    //     tensor: Tensor object to be serialized
    //     base_path: System path of a folder where tensor data is to be stored
    //     
    public static void save_external_data(object tensor, object base_path) {
        // type: (TensorProto, Text) -> None
        var info = new ExternalDataInfo(tensor);
        var external_data_file_path = os.path.join(base_path, info.location);
        // Retrieve the tensor's data from raw_data or load external file
        if (!tensor.HasField("raw_data")) {
            throw new ValueError("raw_data field doesn't exist.");
        }
        // Create file if it doesn't exist
        if (!os.path.isfile(external_data_file_path)) {
            open(external_data_file_path, "ab").close();
        }
        // Open file for reading and writing at random locations ('r+b')
        using (var data_file = open(external_data_file_path, "r+b")) {
            data_file.seek(0, 2);
            if (info.offset != null) {
                // Pad file to required offset if needed
                file_size = data_file.tell();
                if (info.offset > file_size) {
                    data_file.write(new byte[] { \0 } * (info.offset - file_size));
                }
                data_file.seek(info.offset);
            }
            offset = data_file.tell();
            data_file.write(tensor.raw_data);
            set_external_data(tensor, info.location, offset, data_file.tell() - offset);
        }
    }
    
    // Scan an ONNX model for all tensors and return as an iterator.
    public static iterator _get_all_tensors(object onnx_model_proto) {
        // type: (ModelProto) -> Iterable[TensorProto]
        return new iterator(_get_initializer_tensors(onnx_model_proto), _get_attribute_tensors(onnx_model_proto));
    }
    
    // Create an iterator of initializer tensors from ONNX model.
    public static List<List<object>> _get_initializer_tensors(object onnx_model_proto) {
        // type: (ModelProto) -> Iterable[TensorProto]
        foreach (var initializer in onnx_model_proto.graph.initializer) {
            yield return initializer;
        }
    }
    
    // Create an iterator of tensors from node attributes of an ONNX model.
    public static List<List<object>> _get_attribute_tensors(object onnx_model_proto) {
        // type: (ModelProto) -> Iterable[TensorProto]
        foreach (var node in onnx_model_proto.graph.node) {
            foreach (var attribute in node.attribute) {
                if (attribute.HasField("t")) {
                    yield return attribute.t;
                }
                foreach (var tensor in attribute.tensors) {
                    yield return tensor;
                }
            }
        }
    }
    
    // Remove path components which would allow traversing up a directory tree from a base path.
    // 
    //     Note: This method is currently very basic and should be expanded.
    //     
    public static void _sanitize_path(object path) {
        // type: (Text) -> Text
        return path.lstrip("/.");
    }
    
    // Utility to check whether the provided filename is valid.
    public static bool _is_valid_filename(object filename) {
        // type: (Text) -> bool
        var exp = re.compile("^[^<>:;,?\"*|/]+$");
        var match = exp.match(filename);
        if (match) {
            return true;
        } else {
            return false;
        }
    }
    
    // Return true if the tensor stores data in an external location.
    public static bool uses_external_data(object tensor) {
        // type: (TensorProto) -> bool
        return tensor.HasField("data_location") && tensor.data_location == TensorProto.EXTERNAL;
    }
    
    // 
    //     Remove a field from a Tensor's external_data key-value store.
    // 
    //     Modifies tensor object in place.
    // 
    //     @params
    //     tensor: Tensor object from which value will be removed
    //     field_key: The key of the field to be removed
    //     
    public static void remove_external_data_field(object tensor, object field_key) {
        // type: (TensorProto, Text) -> None
        foreach (var _tup_1 in tensor.external_data.Select((_p_1,_p_2) => Tuple.Create(_p_2, _p_1))) {
            var i = _tup_1.Item1;
            var field = _tup_1.Item2;
            if (field.key == field_key) {
                tensor.external_data.Remove(i);
            }
        }
    }
    
    // 
    //     Serializes data for all the tensors which have data location set to TensorProto.External.
    // 
    //     Note: This function also strips basepath information from all tensors' external_data fields.
    // 
    //     @params
    //     model: Model object which is the source of tensors to serialize.
    //     filepath: System path to the directory which should be treated as base path for external data.
    // 
    //     @return
    //     The modified model object.
    //     
    public static void write_external_data_tensors(object model, object filepath) {
        // type: (ModelProto, Text) -> ModelProto
        foreach (var tensor in _get_all_tensors(model)) {
            if (uses_external_data(tensor)) {
                save_external_data(tensor, filepath);
                tensor.ClearField("raw_data".ToString());
            }
        }
        return model;
    }
}
