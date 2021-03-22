using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Onnx
{
    public class onnx
    {
        private static byte[] _load_bytes(Stream f)
        {
            byte[] data = new byte[f.Length];
            f.Read(data, 0, data.Length);
            return data;
        }

        private static byte[] _load_bytes(string s)
        {
            return Encoding.UTF8.GetBytes(s);
        }

        private static void _save_bytes(byte[] str, Stream f)
        {
            f.Write(str, 0, str.Length);
        }

        private static void _save_bytes(byte[] str, string f)
        {
            File.WriteAllBytes(f, str);
        }

        private static string _get_file_path(FileStream s)
        {
            return s.Name;
        }

        private static string _get_file_path(string s)
        {
            return s;
        }

        private static byte[] _serialize(ModelProto proto)
        {
            return proto.ToByteArray();
        }

        private static byte[] _serialize(TensorProto proto)
        {
            return proto.ToByteArray();
        }

        private static ModelProto _deserialize(byte[] bytes, ModelProto proto)
        {
            proto.MergeDelimitedFrom(new MemoryStream(bytes));
            return proto;
        }

        private static TensorProto _deserialize(byte[] bytes, TensorProto proto)
        {
            proto.MergeDelimitedFrom(new MemoryStream(bytes));
            return proto;
        }

        public static ModelProto LoadModel(FileStream f, object format= null, bool load_external_data= true)
        {
            var s = _load_bytes(f);
            var model = LoadModelFromString(s, format: format);
            if (load_external_data)
            {
                var model_filepath = _get_file_path(f);
                if (!string.IsNullOrWhiteSpace(model_filepath))
                {
                    var base_dir = Path.GetDirectoryName(model_filepath);
                    ExternalDataInfo.LoadExternalDataForModel(model, base_dir);
                }
            }

            return model;
        }

        public static ModelProto LoadModel(string f, object format = null, bool load_external_data = true)
        {
            var s = _load_bytes(f);
            var model = LoadModelFromString(s, format: format);
            if (load_external_data)
            {
                var model_filepath = _get_file_path(f);
                if (!string.IsNullOrWhiteSpace(model_filepath))
                {
                    var base_dir = Path.GetDirectoryName(model_filepath);
                    ExternalDataInfo.LoadExternalDataForModel(model, base_dir);
                }
            }

            return model;
        }

        public static ModelProto Load(string f, object format = null, bool load_external_data = true)
        {
            return LoadModel(f, format, load_external_data);
        }

        public static TensorProto LoadTensor(Stream f, object format= null)
        {
            var s = _load_bytes(f);
            return LoadTensorFromString(s, format: format);
        }

        public static TensorProto LoadTensor(string f, object format = null)
        {
            var s = _load_bytes(f);
            return LoadTensorFromString(s, format: format);
        }

        public static ModelProto LoadModelFromString(byte[] f, object format = null)
        {
            return _deserialize(f, new ModelProto());
        }

        public static TensorProto LoadTensorFromString(byte[] f, object format = null)
        {
            return _deserialize(f, new TensorProto());
        }

        public static void SaveModel(ModelProto proto, FileStream f, object format= null)
        {
            var model_filepath = _get_file_path(f);
            if (!string.IsNullOrWhiteSpace(model_filepath))
            {
                var basepath = Path.GetDirectoryName(model_filepath);
                proto = ExternalDataInfo.WriteExternalDataTensors(proto, basepath);
            }

            var s = _serialize(proto);
            _save_bytes(s, f);
        }

        public static void Save(ModelProto proto, FileStream f, object format = null) => SaveModel(proto, f, format);

        public static void SaveModel(ModelProto proto, string f, object format = null)
        {
            var model_filepath = _get_file_path(f);
            if (!string.IsNullOrWhiteSpace(model_filepath))
            {
                var basepath = Path.GetDirectoryName(model_filepath);
                proto = ExternalDataInfo.WriteExternalDataTensors(proto, basepath);
            }

            var s = _serialize(proto);
            _save_bytes(s, f);
        }

        public static void Save(ModelProto proto, string f, object format = null) => SaveModel(proto, f, format);

        public static void SaveModel(byte[] bytes, FileStream f, object format = null)
        {
            var proto = _deserialize(bytes, new ModelProto());
            var model_filepath = _get_file_path(f);
            if (!string.IsNullOrWhiteSpace(model_filepath))
            {
                var basepath = Path.GetDirectoryName(model_filepath);
                proto = ExternalDataInfo.WriteExternalDataTensors(proto, basepath);
            }

            var s = _serialize(proto);
            _save_bytes(s, f);
        }

        public static void Save(byte[] bytes, FileStream f, object format = null) => SaveModel(bytes, f, format);

        public static void SaveModel(byte[] bytes, string f, object format = null)
        {
            var proto = _deserialize(bytes, new ModelProto());
            var model_filepath = _get_file_path(f);
            if (!string.IsNullOrWhiteSpace(model_filepath))
            {
                var basepath = Path.GetDirectoryName(model_filepath);
                proto = ExternalDataInfo.WriteExternalDataTensors(proto, basepath);
            }

            var s = _serialize(proto);
            _save_bytes(s, f);
        }

        public static void Save(byte[] bytes, string f, object format = null) => SaveModel(bytes, f, format);

        public static void SaveTensor(TensorProto proto, Stream f)
        {
            var s = _serialize(proto);
            _save_bytes(s, f);
        }

        public static void SaveTensor(TensorProto proto, string f)
        {
            var s = _serialize(proto);
            _save_bytes(s, f);
        }
    }
}
