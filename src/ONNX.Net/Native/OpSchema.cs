using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Onnx
{
    [StructLayout(LayoutKind.Sequential)]
    public struct OpSchemaStruct
    {
        public unsafe char* file;
        public int line;
        public int support_level;
        public IntPtr doc;
        public int since_version;
        public int deprecated;
        public unsafe char* domain;
        public unsafe char* name;
        public int min_input;
        public int max_input;
        public int min_output;
        public int max_output;
        public unsafe char** attributesKeys;
        public unsafe AttributeStruct* attributesValues;
        public int attributes_length;
        public unsafe FormalParameterStruct* inputs;
        public int inputs_length;
        public unsafe FormalParameterStruct* outputs;
        public int outputs_length;
        public unsafe TypeConstraintParamStruct* type_constraints;
        public int type_constraints_length;
        public int has_type_and_shape_inference_function;
        public int has_function;
        public unsafe char* function_body;
        public int has_context_dependent_function;
    }

    public class OpSchema
    {

        public string file { get; set; }

        public int line { get; set; }

        public SupportType support_level { get; set; }

        public string doc { get; set; }

        public int since_version { get; set; }

        public int deprecated { get; set; }

        public string domain { get; set; }

        public string name { get; set; }

        public int min_input { get; set; }

        public int max_input { get; set; }

        public int min_output { get; set; }

        public int max_output { get; set; }

        public Dictionary<string, Attribute> attributes { get; set; }

        public FormalParameter[] inputs { get; set; }

        public FormalParameter[] outputs { get; set; }

        public TypeConstraintParam[] type_constraints { get; set; }

        public int has_type_and_shape_inference_function { get; set; }

        public static int is_infinite(int v)
        {
            throw new NotImplementedException();
        }

        public (UseType, int) consumed(OpSchema schema, int i)
        {
            throw new NotImplementedException();
        }
    }
}
