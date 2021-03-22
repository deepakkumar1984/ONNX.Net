using System;
using Google.Protobuf;

namespace Onnx
{
    public class VersionConverter
    {
        public static ModelProto ConvertVersion(ModelProto model, int target_version)
        {
            var model_str = model.ToByteArray();
            var converted_model_str = C.convert_version(model_str, target_version);
            return onnx.LoadModelFromString(converted_model_str);
        }
    }
}