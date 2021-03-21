using System;
namespace Onnx
{
    public class TypeConstraintParam
    {
        public TypeConstraintParam()
        {
        }

        public string type_param_str { get; set; }

        public string description { get; set; }

        public string[] allowed_type_strs { get; set; }
    }
}
