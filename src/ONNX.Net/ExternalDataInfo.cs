using System;
using System.Collections.Generic;
using System.Text;

namespace Onnx
{
    public class ExternalDataInfo
    {
        public ExternalDataInfo(TensorProto tensor)
        {
            throw new NotImplementedException();
        }

        public static ExternalDataInfo LoadExternalDataForTensor(TensorProto tensor, string base_dir)
        {
            throw new NotImplementedException();
        }

        public static ExternalDataInfo LoadExternalDataForModel(ModelProto tensor, string base_dir)
        {
            throw new NotImplementedException();
        }

        public static void SetExternalData(TensorProto tensor, string location, int? offset= null,
                                        int? length= null,  string checksum= null, string basepath= null)
        {
            throw new NotImplementedException();
        }

        public static void ConvertModelToExternalData(ModelProto model, bool all_tensors_to_one_file= true, string location= null, int size_threshold= 1024)
        {
            throw new NotImplementedException();
        }

        public static void ConvertModelFromExternalData(ModelProto model)
        {
            throw new NotImplementedException();
        }

        public static void SaveExternalData(TensorProto tensor, string base_path)
        {
            throw new NotImplementedException();
        }

        private static List<TensorProto> _get_all_tensors(ModelProto onnx_model_proto)
        {
            throw new NotImplementedException();
        }

        private static List<TensorProto> _get_initializer_tensors(ModelProto onnx_model_proto)
        {
            throw new NotImplementedException();
        }

        private static List<TensorProto> _get_attribute_tensors(ModelProto onnx_model_proto)
        {
            throw new NotImplementedException();
        }

        private static string _sanitize_path(string path)
        {
            throw new NotImplementedException();
        }

        private static bool _is_valid_filename(string filename)
        {
            throw new NotImplementedException();
        }

        public static bool UsesExternalData(TensorProto tensor)
        {
            throw new NotImplementedException();
        }

        public static void RemoveExternalDataField(TensorProto tensor, string field_key)
        {
            throw new NotImplementedException();
        }

        public static void WriteExternalDataTensors(ModelProto model, string filepath)
        {
            throw new NotImplementedException();
        }
    }
}
