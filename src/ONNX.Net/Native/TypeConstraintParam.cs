using System;
using System.Runtime.InteropServices;

namespace Onnx
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct TypeConstraintParamStruct
    {
        public string type_param_str;

        public string description;

        public IntPtr allowed_type_strs;
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
