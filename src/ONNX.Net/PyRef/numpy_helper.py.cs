
using @absolute_import = @@__future__.absolute_import;

using @division = @@__future__.division;

using @print_function = @@__future__.print_function;

using @unicode_literals = @@__future__.unicode_literals;

using sys;

using np = numpy;

using TensorProto = onnx.TensorProto;

using MapProto = onnx.MapProto;

using SequenceProto = onnx.SequenceProto;

using TypeProto = onnx.TypeProto;

using mapping = onnx.mapping;

using helper = onnx.helper;

using text_type = six.text_type;

using binary_type = six.binary_type;

using Sequence = typing.Sequence;

using Any = typing.Any;

using Optional = typing.Optional;

using Text = typing.Text;

using List = typing.List;

using Dict = typing.Dict;

using System.Linq;

using System.Numerics;

using System;

using System.Numeric;

using System.Collections.Generic;

public static class numpy_helper {
    
    // SPDX-License-Identifier: Apache-2.0
    public static List<Complex> combine_pairs_to_complex(Tuple<object> fa) {
        // type: (Sequence[int]) -> Sequence[np.complex64]
        return (from i in Enumerable.Range(0, fa.Count / 2)
            select new Complex(fa[i * 2], fa[i * 2 + 1])).ToList();
    }
    
    // Converts a tensor def object to a numpy array.
    // 
    //     Inputs:
    //         tensor: a TensorProto object.
    //     Returns:
    //         arr: the converted array.
    //     
    public static void to_array(object tensor) {
        // type: (TensorProto) -> np.ndarray[Any]
        if (tensor.HasField("segment")) {
            throw new ValueError("Currently not supporting loading segments.");
        }
        if (tensor.data_type == TensorProto.UNDEFINED) {
            throw new TypeError("The element type in the input tensor is not defined.");
        }
        var tensor_dtype = tensor.data_type;
        var np_dtype = mapping.TENSOR_TYPE_TO_NP_TYPE[tensor_dtype];
        var storage_type = mapping.TENSOR_TYPE_TO_STORAGE_TENSOR_TYPE[tensor_dtype];
        var storage_np_dtype = mapping.TENSOR_TYPE_TO_NP_TYPE[storage_type];
        var storage_field = mapping.STORAGE_TENSOR_TYPE_TO_FIELD[storage_type];
        var dims = tensor.dims;
        if (tensor.data_type == TensorProto.STRING) {
            var utf8_strings = getattr(tensor, storage_field);
            var ss = (from s in utf8_strings
                select s.decode("utf-8")).ToList();
            return np.asarray(ss).astype(np_dtype).reshape(dims);
        }
        if (tensor.HasField("raw_data")) {
            // Raw_bytes support: using frombuffer.
            if (sys.byteorder == "big") {
                // Convert endian from little to big
                convert_endian(tensor);
            }
            return np.frombuffer(tensor.raw_data, dtype: np_dtype).reshape(dims);
        } else {
            var data = Tuple.Create(getattr(tensor, storage_field));
            if (tensor_dtype == TensorProto.COMPLEX64 || tensor_dtype == TensorProto.COMPLEX128) {
                data = combine_pairs_to_complex(data);
            }
            // F16 is stored as int32; Need view to get the original value
            if (tensor_dtype == TensorProto.FLOAT16) {
                return np.asarray(tensor.int32_data, dtype: np.uint16).reshape(dims).view(np.float16);
            } else {
                // Otherwise simply use astype to convert; e.g., int->float, float->float
                return np.asarray(data, dtype: storage_np_dtype).astype(np_dtype).reshape(dims);
            }
        }
    }
    
    // Converts a numpy array to a tensor def.
    // 
    //     Inputs:
    //         arr: a numpy array.
    //         name: (optional) the name of the tensor.
    //     Returns:
    //         tensor_def: the converted tensor def.
    //     
    public static object from_array(object arr, void name = null) {
        // type: (np.ndarray[Any], Optional[Text]) -> TensorProto
        var tensor = TensorProto();
        tensor.dims.extend(arr.shape);
        if (name) {
            tensor.name = name;
        }
        if (arr.dtype == np.object) {
            // Special care for strings.
            tensor.data_type = mapping.NP_TYPE_TO_TENSOR_TYPE[arr.dtype];
            // TODO: Introduce full string support.
            // We flatten the array in case there are 2-D arrays are specified
            // We throw the error below if we have a 3-D array or some kind of other
            // object. If you want more complex shapes then follow the below instructions.
            // Unlike other types where the shape is automatically inferred from
            // nested arrays of values, the only reliable way now to feed strings
            // is to put them into a flat array then specify type astype(np.object)
            // (otherwise all strings may have different types depending on their length)
            // and then specify shape .reshape([x, y, z])
            var flat_array = arr.flatten();
            foreach (var e in flat_array) {
                if (e is text_type) {
                    tensor.string_data.append(e.encode("utf-8"));
                } else if (e is np.ndarray) {
                    foreach (var s in e) {
                        if (s is text_type) {
                            tensor.string_data.append(s.encode("utf-8"));
                        } else if (s is bytes) {
                            tensor.string_data.append(s);
                        }
                    }
                } else if (e is bytes) {
                    tensor.string_data.append(e);
                } else {
                    throw new NotImplementedException("Unrecognized object in the object array, expect a string, or array of bytes: ", type(e).ToString());
                }
            }
            return tensor;
        }
        // For numerical types, directly use numpy raw bytes.
        try {
            var dtype = mapping.NP_TYPE_TO_TENSOR_TYPE[arr.dtype];
        } catch (KeyError) {
            throw new RuntimeError("Numpy data type not understood yet: {}".format(arr.dtype.ToString()));
        }
        tensor.data_type = dtype;
        tensor.raw_data = arr.tobytes();
        if (sys.byteorder == "big") {
            // Convert endian from big to little
            convert_endian(tensor);
        }
        return tensor;
    }
    
    // Converts a sequence def to a Python list.
    // 
    //     Inputs:
    //         sequence: a SequenceProto object.
    //     Returns:
    //         lst: the converted list.
    //     
    public static List<object> to_list(object sequence) {
        // type: (SequenceProto) -> List[Any]
        var lst = new List<object>();
        var elem_type = sequence.elem_type;
        var value_field = mapping.STORAGE_ELEMENT_TYPE_TO_FIELD[elem_type];
        var values = getattr(sequence, value_field);
        foreach (var value in values) {
            if (elem_type == SequenceProto.TENSOR || elem_type == SequenceProto.SPARSE_TENSOR) {
                lst.append(to_array(value));
            } else if (elem_type == SequenceProto.SEQUENCE) {
                lst.append(to_list(value));
            } else if (elem_type == SequenceProto.MAP) {
                lst.append(to_dict(value));
            } else {
                throw new TypeError("The element type in the input sequence is not supported.");
            }
        }
        return lst;
    }
    
    // Converts a list into a sequence def.
    // 
    //     Inputs:
    //         lst: a Python list
    //         name: (optional) the name of the sequence.
    //         dtype: (optional) type of element in the input list, used for specifying
    //                           sequence values when converting an empty list.
    //     Returns:
    //         sequence: the converted sequence def.
    //     
    public static void from_list(list lst, void name = null, void dtype = null) {
        // type: (List[Any], Optional[Text], Optional[int]) -> SequenceProto
        var sequence = SequenceProto();
        if (name) {
            sequence.name = name;
        }
        if (dtype) {
            var elem_type = dtype;
        } else if (lst.Count > 0) {
            var first_elem = lst[0];
            if (first_elem is dict) {
                elem_type = SequenceProto.MAP;
            } else if (first_elem is list) {
                elem_type = SequenceProto.SEQUENCE;
            } else {
                elem_type = SequenceProto.TENSOR;
            }
        } else {
            // if empty input list and no dtype specified
            // choose sequence of tensors on default
            elem_type = SequenceProto.TENSOR;
        }
        sequence.elem_type = elem_type;
        if (lst.Count > 0 && !all(from elem in lst
            select elem is type(lst[0]))) {
            throw new TypeError("The element type in the input list is not the same for all elements and therefore is not supported as a sequence.");
        }
        if (elem_type == SequenceProto.TENSOR) {
            foreach (var tensor in lst) {
                sequence.tensor_values.extend(new List<object> {
                    from_array(tensor)
                });
            }
        } else if (elem_type == SequenceProto.SEQUENCE) {
            foreach (var seq in lst) {
                sequence.sequence_values.extend(new List<object> {
                    from_list(seq)
                });
            }
        } else if (elem_type == SequenceProto.MAP) {
            foreach (var map in lst) {
                sequence.map_values.extend(new List<void> {
                    from_dict(map)
                });
            }
        } else {
            throw new TypeError("The element type in the input list is not a tensor, sequence, or map and is not supported.");
        }
        return sequence;
    }
    
    // Converts a map def to a Python dictionary.
    // 
    //     Inputs:
    //         map: a MapProto object.
    //     Returns:
    //         dict: the converted dictionary.
    //     
    public static dict to_dict(object map) {
        // type: (MapProto) -> np.ndarray[Any]
        var key_list = new List<object>();
        if (map.key_type == TensorProto.STRING) {
            key_list = map.string_keys.ToList();
        } else {
            key_list = map.keys.ToList();
        }
        var value_list = to_list(map.values);
        if (key_list.Count != value_list.Count) {
            throw new IndexError("Length of keys and values for MapProto (map name: ", map.name, ") are not the same.");
        }
        var dictionary = new dict(zip(key_list, value_list));
        return dictionary;
    }
    
    // Converts a Python dictionary into a map def.
    // 
    //     Inputs:
    //         dict: Python dictionary
    //         name: (optional) the name of the map.
    //     Returns:
    //         map: the converted map def.
    //     
    public static void from_dict(list dict, void name = null) {
        // type: (Dict[Any, Any], Optional[Text]) -> MapProto
        var map = MapProto();
        if (name) {
            map.name = name;
        }
        var keys = dict.keys().ToList();
        var raw_key_type = np.array(keys[0]).dtype;
        var key_type = mapping.NP_TYPE_TO_TENSOR_TYPE[raw_key_type];
        var valid_key_int_types = new List<object> {
            TensorProto.INT8,
            TensorProto.INT16,
            TensorProto.INT32,
            TensorProto.INT64,
            TensorProto.UINT8,
            TensorProto.UINT16,
            TensorProto.UINT32,
            TensorProto.UINT64
        };
        if (!all(from key in keys
            select key is raw_key_type)) {
            throw new TypeError("The key type in the input dictionary is not the same for all keys and therefore is not valid as a map.");
        }
        var values = dict.values().ToList();
        var raw_value_type = type(values[0]);
        if (!all(from val in values
            select val is raw_value_type)) {
            throw new TypeError("The value type in the input dictionary is not the same for all values and therefore is not valid as a map.");
        }
        var value_seq = from_list(values);
        map.key_type = key_type;
        if (key_type == TensorProto.STRING) {
            map.string_keys.extend(keys);
        } else if (valid_key_int_types.Contains(key_type)) {
            map.keys.extend(keys);
        }
        map.values.CopyFrom(value_seq);
        return map;
    }
    
    // 
    //     call to convert endianess of raw data in tensor.
    //     @params
    //     TensorProto: TensorProto to be converted.
    //     
    public static void convert_endian(object tensor) {
        // type: (TensorProto) -> None
        var tensor_dtype = tensor.data_type;
        var np_dtype = mapping.TENSOR_TYPE_TO_NP_TYPE[tensor_dtype];
        tensor.raw_data = np.frombuffer(tensor.raw_data, dtype: np_dtype).byteswap().tobytes();
    }
}
