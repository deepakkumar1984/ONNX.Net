using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Onnx
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct OpSchemaStruct
    {
        public string file;

        public int line;

        public int support_level;

        public string doc;

        public int since_version;

        public bool deprecated;

        public string domain;

        public string name;

        public int min_input;

        public int max_input;

        public int min_output;

        public int max_output;

        public string[] attributesKeys;

        public AttributeStruct[] attributesValues;

        public FormalParameterStruct[] inputs;

        public FormalParameterStruct[] outputs;

        public TypeConstraintParamStruct[] type_constraints;

        public bool has_type_and_shape_inference_function;
    }

    public class OpSchema
    {

        public string file { get; set; }

        public int line { get; set; }

        public SupportType support_level { get; set; }

        public string doc { get; set; }

        public int since_version { get; set; }

        public bool deprecated { get; set; }

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

        public bool has_type_and_shape_inference_function { get; set; }

        public static bool is_infinite(int v)
        {
            throw new NotImplementedException();
        }

        public (UseType, int) consumed(OpSchema schema, int i)
        {
            throw new NotImplementedException();
        }
    }
}
