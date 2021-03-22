using System;
using Google.Protobuf;
namespace Onnx
{
    public class ShapeInference
    {
        public static  ModelProto infer_shapes(ModelProto model, bool check_type=false, bool strict_mode=false)
        {
            var model_str = model.ToByteArray();
            var inferred_model_str = C.infer_shapes(model_str, check_type, strict_mode);
            return onnx.LoadModelFromString(inferred_model_str);
        }
        
        public static  void infer_shapes_path(string model_path, string output_path="", bool check_type=false, bool strict_mode=false)
        {
            if (output_path == "")
            {
                output_path = model_path;
            }

            C.infer_shapes_path(model_path, output_path, check_type, strict_mode);
        }
    }
}