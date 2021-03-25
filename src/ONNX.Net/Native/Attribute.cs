using System;
using System.Runtime.InteropServices;

namespace Onnx
{
    [StructLayout(LayoutKind.Sequential)]
    public struct AttributeStruct
    {
        public unsafe char* name;

        public unsafe char* description;

        public int type;

        public unsafe char* default_value;

        public int required;
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
