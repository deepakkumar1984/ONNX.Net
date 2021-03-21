using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Onnx
{
    public class Helper
    {
        public static VersionMapType CreateOpSetIdVersionMap(VersionTableType table)
        {
            throw new NotImplementedException();
        }

        public static int FindMinIrVersionFor(OperatorSetIdProto[] opsetidlist)
        {
            throw new NotImplementedException();
        }

        public static NodeProto MakeNode(
                                            string op_type,
                                            string[] inputs,
                                            string[] outputs,
                                            string name = null,
                                            string doc_string = null,
                                            string domain = null,
                                            Dictionary<string, object> kwargs = null
                                        )
        {
            throw new NotImplementedException();
        }

        public static OperatorSetIdProto MakeOperatorSetId(string domain, int version)
        {
            throw new NotImplementedException();
        }

        public static GraphProto MakeGraph(
                                                NodeProto[] nodes,
                                                string name,
                                                ValueInfoProto[] inputs,
                                                ValueInfoProto[] outputs,
                                                TensorProto[] initializer = null,
                                                string doc_string = null,
                                                ValueInfoProto[] value_info = null,
                                                SparseTensorProto[] sparse_initializer = null
                                            )
        {
            throw new NotImplementedException();
        }

        public static OperatorSetIdProto MakeOpSetId(string domain, int version)
        {
            throw new NotImplementedException();
        }

        public static ModelProto MakeModel(GraphProto graph, Dictionary<string, object> kwargs)
        {
            throw new NotImplementedException();
        }

        public static ModelProto MakeModelGenVersion(GraphProto graph, Dictionary<string, object> kwargs)
        {
            throw new NotImplementedException();
        }

        public static void SetModelProps(ModelProto model, Dictionary<string, string> dict_value)
        {
            throw new NotImplementedException();
        }

        public static int[] SplitComplexToPairs(Complex[] ca)
        {
            throw new NotImplementedException();
        }

        public static SparseTensorProto MakeSparseTensor(TensorProto values, TensorProto indices, int[] dims)
        {
            throw new NotImplementedException();
        }

        public static SequenceProto MakeSequence(string name, int elem_type, List<object> values)
        {
            throw new NotImplementedException();
        }

        public static MapProto MakeMap(string  name, string key_type, int keys, SequenceProto values)
        {
            throw new NotImplementedException();
        }

        private static byte[] _to_bytes(string val)
        {
            throw new NotImplementedException();
        }

        public static AttributeProto MakeAttribute(string key, object value, string doc_string= null)
        {
            throw new NotImplementedException();
        }

        public static object GetAttributeValue(AttributeProto attr)
        {
            throw new NotImplementedException();
        }

        public static ValueInfoProto MakeEmptyTensorValueInfo(string name)
        {
            throw new NotImplementedException();
        }

        public static ValueInfoProto MakeTensorValueInfo(
                                                    string name,
                                                    int elem_type,
                                                    string[] shape,
                                                    string doc_string= "",
                                                    string[] shape_denotation= null
                                                )
        {
            throw new NotImplementedException();
        }

        public static ValueInfoProto MakeTensorValueInfo(
                                                    string name,
                                                    int elem_type,
                                                    int[] shape,
                                                    string doc_string = "",
                                                    string[] shape_denotation = null
                                                )
        {
            throw new NotImplementedException();
        }

        public static ValueInfoProto MakeSequenceValueInfo(
                                                    string name,
                                                    int elem_type,
                                                    string[] shape,
                                                    string doc_string = "",
                                                    string[] elem_shape_denotation = null
                                                )
        {
            throw new NotImplementedException();
        }

        public static ValueInfoProto MakeSequenceValueInfo(
                                                    string name,
                                                    int elem_type,
                                                    int[] shape,
                                                    string doc_string = "",
                                                    string[] elem_shape_denotation = null
                                                )
        {
            throw new NotImplementedException();
        }

        private static string _sanitize_str(string s)
        {
            throw new NotImplementedException();
        }

        private static string _sanitize_str(byte[] s)
        {
            throw new NotImplementedException();
        }

        public static (string, GraphProto[]) PrintableAttribute(AttributeProto attr, bool subgraphs= false)
        {
            throw new NotImplementedException();
        }

        public static string PrintableDim(TensorShapeProto.Types.Dimension dim)
        {
            throw new NotImplementedException();
        }

        public static string PrintableType(TypeProto t)
        {
            throw new NotImplementedException();
        }

        public static string PrintableValueInfo(ValueInfoProto v)
        {
            throw new NotImplementedException();
        }

        public static string PrintableTensorProto(TensorProto t)
        {
            throw new NotImplementedException();
        }

        public static (string, GraphProto[]) PrintableNode(NodeProto node, string prefix= "", bool subgraphs= false)
        {
            throw new NotImplementedException();
        }

        public static string PrintableGraph(GraphProto graph, string prefix = "")
        {
            throw new NotImplementedException();
        }

        public static void StripDocDtring(Google.Protobuf.IMessage proto)
        {
            throw new NotImplementedException();
        }

        public static TrainingInfoProto make_training_info(GraphProto algorithm, AssignmentBindingType algorithm_bindings,
                GraphProto initialization, AssignmentBindingType initialization_bindings)
        {
            throw new NotImplementedException();
        }
    }
}
