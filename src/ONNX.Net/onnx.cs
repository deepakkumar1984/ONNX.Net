using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Onnx
{
    public class onnx
    {
        private static byte[] _load_bytes(Stream s)
        {
            throw new NotImplementedException();
        }

        private static byte[] _load_bytes(string s)
        {
            throw new NotImplementedException();
        }

        private static void _save_bytes(byte[] str, Stream f)
        {
            throw new NotImplementedException();
        }

        private static void _save_bytes(byte[] str, string f)
        {
            throw new NotImplementedException();
        }

        private static string _get_file_path(Stream s)
        {
            throw new NotImplementedException();
        }

        private static byte[] _get_file_path(string s)
        {
            throw new NotImplementedException();
        }

        private static byte[] _serialize(IMessage proto)
        {
            throw new NotImplementedException();
        }

        private static IMessage _deserialize(byte[] s, IMessage proto)
        {
            throw new NotImplementedException();
        }

        public static ModelProto LoadModel(Stream f, object format= null, bool load_external_data= true)
        {
            throw new NotImplementedException();
        }

        public static ModelProto LoadModel(string f, object format = null, bool load_external_data = true)
        {
            throw new NotImplementedException();
        }

        public static ModelProto Load(string f, object format = null, bool load_external_data = true)
        {
            return LoadModel(f, format, load_external_data) ;
        }

        public static TensorProto LoadTensor(Stream f, object format= null)
        {
            throw new NotImplementedException();
        }

        public static TensorProto LoadTensor(string f, object format = null)
        {
            throw new NotImplementedException();
        }

        public static ModelProto LoadModelFromString(byte[] f, object format = null)
        {
            throw new NotImplementedException();
        }

        public static TensorProto LoadTensorFromString(byte[] f, object format = null)
        {
            throw new NotImplementedException();
        }

        public static void SaveModel(ModelProto proto, Stream f, object format= null)
        {
            throw new NotImplementedException();
        }

        public static void SaveModel(ModelProto proto, string f, object format = null)
        {
            throw new NotImplementedException();
        }

        public static void SaveModel(byte[] proto, Stream f, object format = null)
        {
            throw new NotImplementedException();
        }

        public static void SaveModel(byte[] proto, string f, object format = null)
        {
            throw new NotImplementedException();
        }

        public static void SaveTensor(TensorProto proto, Stream f)
        {
            throw new NotImplementedException();
        }

        public static void SaveTensor(TensorProto proto, string f)
        {
            throw new NotImplementedException();
        }
    }
}
