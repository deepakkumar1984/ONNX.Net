using System;
using System.Runtime.InteropServices;

namespace Onnx
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct FormalParameterStruct
    {
        public string name;

        public IntPtr types;

        public string typeStr;

        public string description;

        public int option;

        public bool isHomogeneous;

        public int differentiationCategory;
    }

    public class FormalParameter
    {
        public FormalParameter()
        {
        }

        public string name { get; set; }

        public string[] types { get; set; }

        public string typeStr { get; set; }

        public string description { get; set; }

        public FormalParameterOption option { get; set; }

        public bool isHomogeneous { get; set; }

        public DifferentiationCategory differentiationCategory { get; set; }
    }
}
