using System;
using System.Runtime.InteropServices;

namespace Onnx
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TypeConstraintParamStruct
    {
        public unsafe char* type_param_str;

        public unsafe char* description;

        public unsafe char** allowed_type_strs;

        public int allowed_type_strs_length;
    }

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
