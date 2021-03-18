using System;

namespace Onnx
{
    public class ShapeInference
    {
        public static  ModelProto infer_shapes(ModelProto model, bool check_type=false, bool strict_mode=false)
        {
            throw new NotImplementedException();
        }
        
        public static  void infer_shapes_path(string model_path, string output_path="", bool check_type=false, bool strict_mode=false)
        {
            throw new NotImplementedException();
        }
    }
}