using System;
using Google.Protobuf;

namespace Onnx
{
    public class VersionConverter
    {
        public static ModelProto ConvertVersion(ModelProto model, int target_version)
        {
            IMessage m;
            
            var model_str = model.();
            var converted_model_str = C.convert_version(model_str, target_version);
            return onnx.LoadModelFromString(converted_model_str);
        }
    }
}