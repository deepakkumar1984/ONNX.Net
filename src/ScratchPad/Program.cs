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

                //var opSchema = Marshal.PtrToStructure<OpSchemaStruct>(ptr);
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
