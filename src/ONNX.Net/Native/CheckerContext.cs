using System;
using System.Collections.Generic;

namespace Onnx
{
    public class CheckerContext
    {
        public int ir_version;
        public Dictionary<string, int> opset_imports;
    }
}
