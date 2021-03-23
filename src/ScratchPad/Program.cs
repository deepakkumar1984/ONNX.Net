using Onnx;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ScratchPad
{
    class Program
    {
        static void Main(string[] args)
        {
            SchemaMapVersionList map = new SchemaMapVersionList();
            if (NativeMethods.CS_has_schema("Add", ""))
            {
                var ptr = NativeMethods.CS_get_schema("Add", 14, "");

                FormalParameterStruct[] inputs = new FormalParameterStruct[ptr.inputs_length];
                var size = Marshal.SizeOf(typeof(FormalParameterStruct));
                long LongPtr = ptr.inputs.ToInt64();
                for (int i = 0; i < ptr.inputs_length; i++)
                {
                    var localPtr = new IntPtr(LongPtr);
                    Marshal.StructureToPtr(inputs[i], localPtr, false);
                    LongPtr += size;
                }
            }
        }
    }

    public class SchemaMapVersionList : Dictionary<string, int[]>
    {
        public void AddSchema(string schema, int value1, int value2)
        {
            this.Add(schema, new int[] { value1, value2 });
        }
    }
}
