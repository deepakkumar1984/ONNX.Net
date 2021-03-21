using System;
namespace Onnx
{
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
