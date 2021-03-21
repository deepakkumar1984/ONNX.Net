using System;
namespace Onnx
{
    public class Defs
    {
        public Defs()
        {
        }

        public static FunctionProto FunctionProto
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public static AttributeProto AttributeDefauttProto
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public static int OnnxOpsetVersion()
        {
            throw new NotImplementedException();
        }

        public OpSchema[] GetFunctionOps()
        {
            throw new NotImplementedException();
        }
    }
}
