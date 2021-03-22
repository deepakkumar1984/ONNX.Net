using System;
using Google.Protobuf;
namespace Onnx
{
    public class Checker
    {
        private static int MAXIMUM_PROTOBUF = 2000000000;
        public static void CheckModel(ModelProto model, bool full_check= false)
        {

            // If the protobuf is larger than 2GB,
            // remind users should use the model path to check
            var protobuf_string = model.ToByteArray();
            if (protobuf_string.Length > MAXIMUM_PROTOBUF)
                throw new Exception("This protobuf of onnx model is too large (>2GB). Call check_model with model path instead.");

            C.check_model(protobuf_string);
            var m = model;
            if (full_check)
                ShapeInference.infer_shapes(m, check_type: true);
        }
        
        public static void CheckModel(string model, bool full_check= false)
        {
            C.check_model_path(model);
            var m = onnx.Load(model);
            if (full_check)
                ShapeInference.infer_shapes(m, check_type: true);
        }
    }
}