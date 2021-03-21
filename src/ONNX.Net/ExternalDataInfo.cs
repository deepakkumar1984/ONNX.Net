using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Onnx
{
    public class ExternalDataInfo
    {
        public string basepath;
        
        public string checksum;
        
        public int? length;
        
        public string location;
        
        public int? offset;
        
        public Dictionary<string, string> attributes;
        
        public ExternalDataInfo(TensorProto tensor)
        {
            this.location = "";
            this.offset = null;
            this.length = null;
            this.checksum = null;
            this.basepath = "";
            attributes = new Dictionary<string, string>();
            foreach (var entry in tensor.ExternalDatas) {
                attributes.Add(entry.Key, entry.Value);
            }
        }

        public static void LoadExternalDataForTensor(TensorProto tensor, string base_dir)
        {
            if (tensor.RawData != null) {
                // already loaded
                return;
            }
            var info = new ExternalDataInfo(tensor);
            var file_location = _sanitize_path(info.location);
            var external_data_file_path = Path.Combine(base_dir, file_location);
            using (var data_file = File.Open(external_data_file_path, FileMode.Open))
            {
                if(info.offset != null)
                    data_file.Seek(info.offset.Value, SeekOrigin.Begin);

                byte[] bytes = null;
                if (info.length != null)
                {
                    bytes = new byte[info.length.Value];
                    data_file.Read(bytes, 0, info.length.Value);
                }
                else
                {
                    bytes = new byte[data_file.Length];
                    data_file.Read(bytes, 0, (int)data_file.Length);
                }

                tensor.RawData = bytes;
            }
        }

        public static void LoadExternalDataForModel(ModelProto model, string base_dir)
        {
            foreach (var tensor in _get_all_tensors(model)) {
                if (UsesExternalData(tensor)) {
                    LoadExternalDataForTensor(tensor, base_dir);
                    // After loading raw_data from external_data, change the state of tensors
                    tensor.data_location = TensorProto.DataLocation.Default;
                    // and remove external data
                    tensor.ExternalDatas.Remove(tensor.ExternalDatas.Find(x => x.Key == ":"));
                }
            }
        }

        public static void SetExternalData(TensorProto tensor, string location, int? offset= null,
                                        int? length= null,  string checksum= null, string basepath= null)
        {
            if (tensor.RawData == null)
            {
                throw new Exception("Tensor " + tensor.Name + "does not have raw_data field. Cannot set external data for this tensor.");
            }
            
            tensor.ExternalDatas.Remove(tensor.ExternalDatas.Find(x => x.Key == ":"));
            tensor.data_location = TensorProto.DataLocation.External;
            foreach (var _tup_1 in new Dictionary<string, object> {
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
                basepath}})
            {
                var k = _tup_1.Key;
                var v = _tup_1.Value;
                if (v != null)
                {
                    tensor.ExternalDatas.Add(new StringStringEntryProto() { Key = k, Value = v.ToString() });
                }
            }
        }

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
        public static void ConvertModelToExternalData(ModelProto model, bool all_tensors_to_one_file= true, string location= null, int size_threshold= 1024)
        {
            if (all_tensors_to_one_file)
            {
                var file_name = Guid.NewGuid().ToString();
                if (location != null)
                {
                    file_name = location;
                }
                foreach (var tensor in _get_all_tensors(model))
                {
                    if (tensor.RawData != null && tensor.RawData.Length >= size_threshold)
                    {
                        SetExternalData(tensor, file_name);
                    }
                }
            }
            else
            {
                foreach (var tensor in _get_all_tensors(model))
                {
                    if (tensor.RawData != null && tensor.RawData.Length >= size_threshold)
                    {
                        var tensor_location = tensor.Name;
                        if (!_is_valid_filename(tensor_location))
                        {
                            tensor_location = Guid.NewGuid().ToString();
                        }

                        SetExternalData(tensor, tensor_location);
                    }
                }
            }
        }

        public static void ConvertModelFromExternalData(ModelProto model)
        {
            foreach (var tensor in _get_all_tensors(model))
            {
                if (UsesExternalData(tensor))
                {
                    if (tensor.RawData == null)
                    {
                        throw new Exception("raw_data field doesn't exist.");
                    }

                    tensor.ExternalDatas.Remove(tensor.ExternalDatas.Find(x => x.Key == ":"));
                    tensor.data_location = TensorProto.DataLocation.Default;
                }
            }
        }

        public static void SaveExternalData(TensorProto tensor, string base_path)
        {
            var info = new ExternalDataInfo(tensor);
            var external_data_file_path = Path.Combine(base_path, info.location);
            // Retrieve the tensor's data from raw_data or load external file
            if (tensor.RawData == null)
            {
                throw new Exception("raw_data field doesn't exist.");
            }

            // Create file if it doesn't exist
            if (!File.Exists(external_data_file_path))
            {
                using (var f = File.CreateText(external_data_file_path))
                {
                    f.Write("ab");
                }
            }

            // Open file for reading and writing at random locations ('r+b')
            using (var data_file = File.OpenRead(external_data_file_path))
            {
                data_file.Seek(0, SeekOrigin.End);
                if (info.offset != null)
                {
                    // Pad file to required offset if needed
                    var file_size = data_file.Length;
                    if (info.offset > file_size)
                    {
                        List<byte> writeBytes = new List<byte>();
                        for(int i = 0; i< (info.offset - file_size); i++)
                        {
                            writeBytes.AddRange(Encoding.UTF8.GetBytes("\0"));
                        }

                        data_file.Write(writeBytes.ToArray());
                    }

                    data_file.Seek(info.offset.Value, SeekOrigin.Begin);
                }

                var offset = (int)data_file.Length;
                data_file.Write(tensor.RawData);
                SetExternalData(tensor, info.location, offset, (int)data_file.Length - offset);
            }
        }

        private static List<TensorProto> _get_all_tensors(ModelProto onnx_model_proto)
        {
            List<TensorProto> result = _get_initializer_tensors(onnx_model_proto).ToList();
            result.AddRange(_get_attribute_tensors(onnx_model_proto));
            return result;
        }

        private static IEnumerable<TensorProto> _get_initializer_tensors(ModelProto onnx_model_proto)
        {
            return onnx_model_proto.Graph.Initializers;
        }

        private static IEnumerable<TensorProto> _get_attribute_tensors(ModelProto onnx_model_proto)
        {
            foreach (var node in onnx_model_proto.Graph.Nodes)
            {
                foreach (var attribute in node.Attributes)
                {
                    if (attribute.T != null)
                    {
                        yield return attribute.T;
                    }
                    foreach (var tensor in attribute.Tensors)
                    {
                        yield return tensor;
                    }
                }
            }
        }

        private static string _sanitize_path(string path)
        {
            return path.TrimEnd("/.".ToArray());
        }

        private static bool _is_valid_filename(string filename)
        {
            var exp = new Regex("^[^<>:;,?\"*|/]+$");
            return exp.IsMatch(filename);
        }

        public static bool UsesExternalData(TensorProto tensor)
        {
            return tensor.data_location == TensorProto.DataLocation.External;
        }

        public static void RemoveExternalDataField(TensorProto tensor, string field_key)
        {
            foreach (var _tup_1 in tensor.ExternalDatas.Select((_p_1, _p_2) => Tuple.Create(_p_2, _p_1)))
            {
                var i = _tup_1.Item1;
                var field = _tup_1.Item2;
                if (field.Key == field_key)
                {
                    tensor.ExternalDatas.Remove(field);
                }
            }
        }

        public static ModelProto WriteExternalDataTensors(ModelProto model, string filepath)
        {
            foreach (var tensor in _get_all_tensors(model))
            {
                if (UsesExternalData(tensor))
                {
                    SaveExternalData(tensor, filepath);
                    tensor.RawData = null;
                }
            }

            return model;
        }
    }
}
