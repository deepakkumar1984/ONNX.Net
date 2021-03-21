using System;
namespace Onnx
{
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
