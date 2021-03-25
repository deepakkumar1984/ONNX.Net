using Onnx;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ScratchPad
{
    class Program
    {
        static void Main(string[] args)
        {
            SchemaMapVersionList map = new SchemaMapVersionList();
            if (NativeMethods.CS_has_schema("Conv", ""))
            {
                unsafe
                {
                    var ptr = NativeMethods.CS_get_schema("Conv", 14, "");

                    if (ptr.has_function == 1)
                    {
                        string function_body = Marshal.PtrToStringAnsi((IntPtr)ptr.function_body);
                    }

                    string[] attrKeys = new string[ptr.attributes_length];
                    AttributeStruct[] attrValues = new AttributeStruct[ptr.attributes_length];

                    for(int i = 0; i < ptr.attributes_length; i++)
                    {
                        attrKeys[i] = Marshal.PtrToStringAnsi((IntPtr)ptr.attributesKeys[i]);
                        attrValues[i] = ptr.attributesValues[i];
                        var proto = AttributeProto.Parser.ParseFrom(Encoding.ASCII.GetBytes(Marshal.PtrToStringAnsi((IntPtr)attrValues[i].default_value)));
                    }

                    FormalParameterStruct[] inputs = new FormalParameterStruct[ptr.inputs_length];
                    for (int i = 0; i < ptr.inputs_length; i++)
                    {
                        inputs[i] = ptr.inputs[i];
                    }

                    FormalParameterStruct[] outputs = new FormalParameterStruct[ptr.outputs_length];
                    for (int i = 0; i < ptr.outputs_length; i++)
                    {
                        outputs[i] = ptr.outputs[i];
                    }

                    TypeConstraintParamStruct[] constraints = new TypeConstraintParamStruct[ptr.type_constraints_length];
                    for (int i = 0; i < ptr.type_constraints_length; i++)
                    {
                        constraints[i] = ptr.type_constraints[i];
                    }
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
