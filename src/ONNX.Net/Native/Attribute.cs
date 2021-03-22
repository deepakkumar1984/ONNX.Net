using System;
using System.Runtime.InteropServices;

namespace Onnx
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct AttributeStruct
    {
        public string name;

        public string description;

        public int type;

        public IntPtr default_value;

        public bool required;
    }

    public class Attribute
    {
        public Attribute()
        {
        }

        public string name { get; set; }

        public string description { get; set; }

        public AttrType type { get; set; }

        public AttributeProto default_value { get; set; }

        public bool required { get; set; }
    }
}
