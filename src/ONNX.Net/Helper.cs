using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Onnx
{
    public class Helper
    {
        

        public static VersionTableType VERSION_TABLE = new VersionTableType()
        {
            new VersionRowType("1.0", 3, 1, 1),
            new VersionRowType("1.1", 3, 5, 1),
            new VersionRowType("1.1.2", 3, 6, 1),
            new VersionRowType("1.2", 3, 7, 1),
            new VersionRowType("1.3", 3, 8, 1),
            new VersionRowType("1.4.1", 4, 9, 1),
            new VersionRowType("1.5.0", 5, 10, 1),
            new VersionRowType("1.6.0", 6, 11, 2),
            new VersionRowType("1.7.0", 7, 12, 2, 1),
            new VersionRowType("1.8.0", 7, 13, 2, 1),
            new VersionRowType("1.8.1", 7, 13, 2, 1)
        };

        public static VersionMapType OP_SET_ID_VERSION_MAP
        {
            get
            {
                return CreateOpSetIdVersionMap(VERSION_TABLE);
            }
        }

        public static VersionMapType CreateOpSetIdVersionMap(VersionTableType table)
        {
            var result = new VersionMapType();
            Action<VersionRowType> process = (version) => {

                var argList = new List<(string, int)> {
                    ("ai.onnx", version.OnnxVersion),
                    ("ai.onnx.ml", version.OnnxMlVersion)
                };

                if (version.TrainingVersion.HasValue)
                    argList.Add(("ai.onnx.training", version.TrainingVersion.Value));

                foreach (var pair in argList)
                {
                    if (!result.ContainsKey(pair))
                    {
                        result[pair] = version.IRVersion;
                    }
                }
            };

            foreach (var row in table)
            {
                process(row);
            }

            return result;
        }

        public static int FindMinIrVersionFor(OperatorSetIdProto[] opsetidlist)
        {
            var default_min_version = 3;
            Func<string, int, int> find_min = (domain, version) => {
                // type: (Union[Text, None], int) -> int
                var key = (!string.IsNullOrWhiteSpace(domain) ? domain : "ai.onnx", version);
                if (OP_SET_ID_VERSION_MAP.ContainsKey(key))
                {
                    return OP_SET_ID_VERSION_MAP[key];
                }
                else
                {
                    throw new Exception("Unsupported opset-version.");
                }
            };

            if (opsetidlist != null)
            {
                return (from x in opsetidlist
                            select find_min(x.Domain, (int)x.Version)).Max();
            }

            return default_min_version;
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
            var node = new NodeProto();
            node.OpType = op_type;
            node.Inputs.AddRange(inputs);
            node.Outputs.AddRange(outputs);
            if (name != null)
                node.Name = name;

            if (doc_string != null)
                node.DocString = doc_string;

            if (domain != null)
                node.Domain = domain;

            if (kwargs != null)
            {
                foreach (var (k, v) in kwargs)
                {
                    node.Attributes.Add(MakeAttribute(k, v));
                }
            }

            return node;
        }

        public static OperatorSetIdProto MakeOperatorSetId(string domain, int version)
        {
            var opsetid = new OperatorSetIdProto();
            opsetid.Domain = domain;
            opsetid.Version = version;
            return opsetid;
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
            if (initializer == null)
                initializer = new TensorProto[0];

            if (sparse_initializer == null)
                sparse_initializer = new SparseTensorProto[0];

            if (value_info == null)
                value_info = new ValueInfoProto[0];

            var graph = new GraphProto();
            graph.Nodes.AddRange(nodes);
            graph.Name = name;
            graph.Inputs.AddRange(inputs);
            graph.Outputs.AddRange(outputs);
            graph.Initializers.AddRange(initializer);
            graph.SparseInitializers.AddRange(sparse_initializer);
            graph.ValueInfoes.AddRange(value_info);
            if (doc_string != null)
                graph.DocString = doc_string;

            return graph;
        }

        public static OperatorSetIdProto MakeOpSetId(string domain, int version)
        {
            var opsetid = new OperatorSetIdProto();
            opsetid.Domain = domain;
            opsetid.Version = version;
            return opsetid;
        }

        public static ModelProto MakeModel(GraphProto graph, Dictionary<string, object> kwargs)
        {
            
            var model = new ModelProto();
            // Touch model.ir_version so it is stored as the version from which it is
            // generated.
            model.IrVersion = (long)Version.IrVersion;
            
            model.Graph = graph;
            OperatorSetIdProto[] opset_imports = null;
            if (kwargs.ContainsKey("opset_imports"))
                opset_imports = (OperatorSetIdProto[])kwargs["opset_imports"];

            if (opset_imports != null)
            {
                model.OpsetImports.AddRange(opset_imports);
            }
            else
            {
                model.OpsetImports.Add(new OperatorSetIdProto() { Version = Defs.OnnxOpsetVersion() });
            }

            foreach (var (k, v) in kwargs)
            {
                var field = model.GetType().GetField(k.Replace("_", ""), System.Reflection.BindingFlags.IgnoreCase);
                if(field != null)
                {
                    field.SetValue(model, v);
                    continue;
                }

                var prop = model.GetType().GetProperty(k.Replace("_", ""), System.Reflection.BindingFlags.IgnoreCase);
                if(prop != null)
                {
                    prop.SetValue(model, v);
                }
            }
            
            return model;
        }

        public static ModelProto MakeModelGenVersion(GraphProto graph, Dictionary<string, object> kwargs)
        {
            var ir_version_field = "ir_version";
            if (!kwargs.ContainsKey(ir_version_field))
            {
                var opset_imports_field = "opset_imports";
                var imports = kwargs.ContainsKey(opset_imports_field) ? (OperatorSetIdProto[])kwargs[opset_imports_field] : new OperatorSetIdProto[0];
                kwargs[ir_version_field] = FindMinIrVersionFor(imports);
            }

            return MakeModel(graph, kwargs);
        }

        public static void SetModelProps(ModelProto model, Dictionary<string, string> dict_value)
        {
            model.MetadataProps.Remove(model.MetadataProps.Find(x=>x.Key == ":"));
            foreach (var (k, v) in dict_value)
            {
                model.MetadataProps.Add(new StringStringEntryProto() { Key = k, Value = v });
            }
        }

        public static double[] SplitComplexToPairs(Complex[] ca)
        {
            return (from i in Enumerable.Range(0, ca.Length * 2)
                    select i % 2 == 0 ? ca[i / 2].Real : ca[i / 2].Imaginary).ToArray();
        }

        public static TensorProto MakeTensor(
                                            string name,
                                            TensorProto.DataType data_type,
                                            int[] dims,
                                            Array vals,
                                            bool raw = false)
        {
            // type: (...) -> TensorProto
            var tensor = new TensorProto();
            tensor.data_type = (int)data_type;
            tensor.Name = name;
            if (data_type == TensorProto.DataType.String)
            {
                Debug.Assert(!raw, "Can not use raw_data to store string type");
            }

            // Check number of vals specified equals tensor size
            var size = !raw ? 1 : Mapping.TENSOR_TYPE_TO_NP_TYPE[(int)data_type].itemsize;
            foreach (var d in dims)
            {
                size = size * d;
            }

            if (vals.Length != size)
            {
                throw new Exception("Number of values does not match tensor's size.");
            }

            if (data_type == TensorProto.DataType.Complex64 || data_type == TensorProto.DataType.Complex128)
            {
                vals = SplitComplexToPairs(vals.OfType<Complex>().ToArray());
            }

            if (raw)
            {
                tensor.RawData = vals.OfType<byte>().ToArray();
            }
            else if(Mapping.TENSOR_TYPE_TO_STORAGE_TENSOR_TYPE[(int)data_type] == (int)TensorProto.DataType.Double)
            {
                tensor.DoubleDatas = vals.OfType<double>().ToArray();
            }
            else if (Mapping.TENSOR_TYPE_TO_STORAGE_TENSOR_TYPE[(int)data_type] == (int)TensorProto.DataType.Float)
            {
                tensor.FloatDatas = vals.OfType<float>().ToArray();
            }
            else if (Mapping.TENSOR_TYPE_TO_STORAGE_TENSOR_TYPE[(int)data_type] == (int)TensorProto.DataType.Int32)
            {
                tensor.Int32Datas = vals.OfType<int>().ToArray();
            }
            else if (Mapping.TENSOR_TYPE_TO_STORAGE_TENSOR_TYPE[(int)data_type] == (int)TensorProto.DataType.Int64)
            {
                tensor.Int64Datas = vals.OfType<long>().ToArray();
            }
            else if (Mapping.TENSOR_TYPE_TO_STORAGE_TENSOR_TYPE[(int)data_type] == (int)TensorProto.DataType.Uint64)
            {
                tensor.Uint64Datas = vals.OfType<ulong>().ToArray();
            }

            var tensorDims = tensor.Dims.ToList();
            tensorDims.AddRange(dims.Select(x=>(long)x));
            tensor.Dims = tensorDims.ToArray();
            return tensor;
        }

        public static SparseTensorProto MakeSparseTensor(TensorProto values, TensorProto indices, int[] dims)
        {
            var sparse = new SparseTensorProto();
            sparse.Values = values;
            sparse.Indices = indices;
            var sparseDims = sparse.Dims.ToList();
            sparseDims.AddRange(dims.Select(x => (long)x));
            sparse.Dims = sparseDims.ToArray();
            return sparse;
        }

        public static SequenceProto MakeSequence<T>(string name, int elem_type, List<T> values)
        {
            var sequence = new SequenceProto();
            sequence.Name = name;
            sequence.ElemType = elem_type;
            var values_field = Mapping.STORAGE_ELEMENT_TYPE_TO_FIELD[elem_type];
            sequence.GetType().GetProperty(values_field).SetValue(sequence, values);
            return sequence;
        }

        public static MapProto MakeMap(string name, TensorProto.DataType key_type, Array keys, SequenceProto values)
        {
            var map = new MapProto();
            var valid_key_int_types = new TensorProto.DataType[]
                                        {
                                            TensorProto.DataType.Int8, TensorProto.DataType.Int16, TensorProto.DataType.Int32,
                                            TensorProto.DataType.Int64, TensorProto.DataType.Uint8, TensorProto.DataType.Uint16,
                                            TensorProto.DataType.Uint32, TensorProto.DataType.Uint64
                                        };
            map.Name = name;
            map.KeyType = (int)key_type;
            if (key_type == TensorProto.DataType.String)
                map.StringKeys.AddRange(keys.OfType<string>().Select(x => (Encoding.UTF8.GetBytes(x))));
            else if (valid_key_int_types.Contains(key_type))
                map.Keys = keys.OfType<long>().ToArray();

            map.Values = values;
            return map;
        }

        private static byte[] _to_bytes(string val)
        {
            return Encoding.UTF8.GetBytes(val);
        }

        public static AttributeProto MakeAttribute(string key, object value, string doc_string= null)
        {
            var attr = new AttributeProto();
            attr.Name = key;
            if (doc_string != null)
            {
                attr.DocString = doc_string;
            }

            var is_iterable = value.GetType().IsArray;
            
            // First, singular cases
            // float
            if (value is float)
            {
                attr.F = Convert.ToSingle(value);
                attr.Type = AttributeProto.AttributeType.Float;
            }
            else if (value is int)
            {
                // integer
                attr.I = Convert.ToInt32(value);
                attr.Type = AttributeProto.AttributeType.Int;
            }
            else if (value is string)
            {
                attr.S = _to_bytes(((string)value).ToString());
                attr.Type = AttributeProto.AttributeType.String;
            }
            else if (value is TensorProto)
            {
                attr.T = (TensorProto)value;
                attr.Type = AttributeProto.AttributeType.Tensor;
            }
            else if (value is SparseTensorProto)
            {
                attr.SparseTensor = (SparseTensorProto)value;
                attr.Type = AttributeProto.AttributeType.SparseTensor;
            }
            else if (value is GraphProto)
            {
                attr.G = (GraphProto)value;
                attr.Type = AttributeProto.AttributeType.Graph;
            }
            else if (is_iterable)
            {
                if(value.GetType().GetElementType().Name == "Int32")
                {
                    attr.Ints = ((int[])value).Select(x=>(long)x).ToArray();
                    attr.Type = AttributeProto.AttributeType.Ints;
                }
                else if (value.GetType().GetElementType().Name == "Single")
                {
                    attr.Floats = ((float[])value);
                    attr.Type = AttributeProto.AttributeType.Floats;
                }
                else if (value.GetType().GetElementType().Name == "String")
                {
                    attr.Strings.AddRange(((string[])value).Select(x => Encoding.UTF8.GetBytes(x)));
                    attr.Type = AttributeProto.AttributeType.Strings;
                }
                else if (value.GetType().GetElementType().Name == "TensorProto")
                {
                    attr.Tensors.AddRange((TensorProto[])value);
                    attr.Type = AttributeProto.AttributeType.Tensors;
                }
                else if (value.GetType().GetElementType().Name == "SparseTensorProto")
                {
                    attr.SparseTensors.AddRange((SparseTensorProto[])value);
                    attr.Type = AttributeProto.AttributeType.SparseTensors;
                }
                else if (value.GetType().GetElementType().Name == "GraphProto")
                {
                    attr.Graphs.AddRange((GraphProto[])value);
                    attr.Type = AttributeProto.AttributeType.Graphs;
                }
                else
                {
                    throw new Exception("You passed in an iterable attribute but I cannot figure out its applicable type.");
                }
            }
            else
            {
                throw new ArgumentException($"value \"{value}\" is not valid attribute data type.");
            }

            return attr;
        }

        public static object GetAttributeValue(AttributeProto attr)
        {
            if (attr.Type == AttributeProto.AttributeType.Float)
            {
                return attr.F;
            }
            if (attr.Type == AttributeProto.AttributeType.Int)
            {
                return attr.I;
            }
            if (attr.Type == AttributeProto.AttributeType.String)
            {
                return attr.S;
            }
            if (attr.Type == AttributeProto.AttributeType.Tensor)
            {
                return attr.T;
            }
            if (attr.Type == AttributeProto.AttributeType.Graph)
            {
                return attr.G;
            }
            if (attr.Type == AttributeProto.AttributeType.Floats)
            {
                return attr.Floats;
            }
            if (attr.Type == AttributeProto.AttributeType.Ints)
            {
                return attr.Ints;
            }
            if (attr.Type == AttributeProto.AttributeType.Strings)
            {
                return attr.Strings.Select(x=>Encoding.UTF8.GetString(x)).ToArray();
            }
            if (attr.Type == AttributeProto.AttributeType.Tensors)
            {
                return attr.Tensors;
            }
            if (attr.Type == AttributeProto.AttributeType.Graphs)
            {
                return attr.Graphs;
            }

            throw new ArgumentException($"Unsupported ONNX attribute: {attr}");
        }

        public static ValueInfoProto MakeEmptyTensorValueInfo(string name)
        {
            var value_info_proto = new ValueInfoProto();
            value_info_proto.Name = name;
            return value_info_proto;
        }

        public static ValueInfoProto MakeTensorValueInfo(
                                                    string name,
                                                    int elem_type,
                                                    string[] shape,
                                                    string doc_string= "",
                                                    string[] shape_denotation= null
                                                )
        {
            var value_info_proto = new ValueInfoProto();
            value_info_proto.Name = name;
            if (doc_string != null)
                value_info_proto.DocString = doc_string;

            var tensor_type_proto = value_info_proto.Type.TensorType;
            tensor_type_proto.ElemType = elem_type;

            var tensor_shape_proto = tensor_type_proto.Shape;

            if (shape != null)
                // You might think this is a no-op (extending a normal Python
                // list by [] certainly is), but protobuf lists work a little
                // differently; if a field is never set, it is omitted from the
                // resulting protobuf; a list that is explicitly set to be
                // empty will get an (empty) entry in the protobuf. This
                // difference is visible to our consumers, so make sure we emit
                // an empty shape!

                tensor_shape_proto.Dims.AddRange(new TensorShapeProto.Dimension[0]);

            if (shape_denotation != null)
            {
                if (shape_denotation.Length != shape.Length)
                    throw new Exception("Invalid shape_denotation. Must be of the same length as shape.");

                for (int i = 0; i < shape.Length; i++)
                {
                    var d = shape[i];
                    var dim = new TensorShapeProto.Dimension();
                    if (d == null)
                        continue;
                    dim.DimParam = d;

                    if (shape_denotation != null)
                    {
                        dim.Denotation = shape_denotation[i];
                    }

                    tensor_shape_proto.Dims.Add(dim);
                }
            }

            return value_info_proto;
        }

        public static ValueInfoProto MakeTensorValueInfo(
                                                    string name,
                                                    int elem_type,
                                                    int[] shape,
                                                    string doc_string = "",
                                                    string[] shape_denotation = null
                                                )
        {
            var value_info_proto = new ValueInfoProto();
            value_info_proto.Name = name;
            if (doc_string != null)
                value_info_proto.DocString = doc_string;

            var tensor_type_proto = value_info_proto.Type.TensorType;
            tensor_type_proto.ElemType = elem_type;

            var tensor_shape_proto = tensor_type_proto.Shape;

            if (shape != null)
                // You might think this is a no-op (extending a normal Python
                // list by [] certainly is), but protobuf lists work a little
                // differently; if a field is never set, it is omitted from the
                // resulting protobuf; a list that is explicitly set to be
                // empty will get an (empty) entry in the protobuf. This
                // difference is visible to our consumers, so make sure we emit
                // an empty shape!

                tensor_shape_proto.Dims.AddRange(new TensorShapeProto.Dimension[0]);

            if (shape_denotation != null)
            {
                if (shape_denotation.Length != shape.Length)
                    throw new Exception("Invalid shape_denotation. Must be of the same length as shape.");

                for (int i = 0; i < shape.Length; i++)
                {
                    var d = shape[i];
                    var dim = new TensorShapeProto.Dimension();
                    
                    dim.DimValue = Convert.ToInt64(d);

                    if (shape_denotation != null)
                    {
                        dim.Denotation = shape_denotation[i];
                    }

                    tensor_shape_proto.Dims.Add(dim);
                }
            }

            return value_info_proto;
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
            var value_info_proto = new ValueInfoProto();
            value_info_proto.Name = name;
            if (doc_string != null)
                value_info_proto.DocString = doc_string;

            var sequence_type_proto = value_info_proto.Type.SequenceType;
            sequence_type_proto.ElemType.TensorType.ElemType = elem_type;

            var tensor_value_info = MakeTensorValueInfo(name, elem_type, shape, doc_string, elem_shape_denotation);

            if (shape != null)
                sequence_type_proto.ElemType.TensorType.Shape = tensor_value_info.Type.TensorType.Shape;

            return value_info_proto;
        }

        private static string _sanitize_str(string s)
        {
            var sanitized = s;

            if (sanitized.Length < 64)
            {
                return sanitized;
            }

            return sanitized.Substring(0, 64) + $"...<+len={sanitized.Length - 64}>";
        }

        private static string _sanitize_str(byte[] s)
        {
            return _sanitize_str(Encoding.UTF8.GetString(s));
        }

        public static (string, GraphProto[]) PrintableAttribute(AttributeProto attr, bool subgraphs= false)
        {
            var content = new List<string>();
            content.Add(attr.Name);
            content.Add("=");
            
            
            Func<string[], string> str_list = (arr) => {
                // type: (Callable[[_T], Text], Sequence[_T]) -> Text
                return "[" + string.Join(", ", arr) + "]";
            };
                       

            // for now, this logic should continue to work as long as we are running on a proto3
            // implementation. If/when we switch to proto3, we will need to use attr.type
            // To support printing subgraphs, if we find a graph attribute, print out
            // its name here and pass the graph itself up to the caller for later
            // printing.
            var graphs = new List<GraphProto>();
            if (attr.Type == AttributeProto.AttributeType.Float)
            {
                content.Add(attr.F.ToString());
            }
            else if (attr.Type == AttributeProto.AttributeType.Int)
            {
                content.Add(attr.I.ToString());
            }
            else if (attr.Type == AttributeProto.AttributeType.String)
            {
                // TODO: Bit nervous about Python 2 / Python 3 determinism implications
                content.Add(_sanitize_str(attr.S));
            }
            else if (attr.Type == AttributeProto.AttributeType.Tensor)
            {
                if (attr.T.Dims.Length > 0)
                {
                    content.Add("<Tensor>");
                }
                else
                {
                    // special case to print scalars
                    var field = Mapping.STORAGE_TENSOR_TYPE_TO_FIELD[attr.T.data_type];
                    var fieldVal = attr.T.GetType().GetProperty(field).GetValue(attr.T);
                    content.Add($"<Scalar Tensor {fieldVal.ToString()}>");
                }
            }
            else if (attr.Type == AttributeProto.AttributeType.Graph)
            {
                content.Add($"<graph {attr.G.Name}>");
                graphs.Add(attr.G);
            }
            else if (attr.Type == AttributeProto.AttributeType.Floats)
            {
                content.Add(str_list(attr.Floats.Select(x => x.ToString()).ToArray()));
            }
            else if (attr.Type == AttributeProto.AttributeType.Ints)
            {
                content.Add(str_list(attr.Ints.Select(x => x.ToString()).ToArray()));
            }
            else if (attr.Type == AttributeProto.AttributeType.Strings)
            {
                content.Add(str_list(attr.Strings.Select(x => _sanitize_str(x)).ToArray()));
            }
            else if (attr.Type == AttributeProto.AttributeType.Tensors)
            {
                content.Add("[<Tensor>, ...]");
            }
            else if (attr.Type == AttributeProto.AttributeType.Graphs)
            {
                content.Add("[");
                foreach (var _tup_1 in attr.Graphs.Select((_p_1, _p_2) => Tuple.Create(_p_2, _p_1)))
                {
                    var i = _tup_1.Item1;
                    var g = _tup_1.Item2;
                    var comma = i != attr.Graphs.Count - 1 ? "," : "";
                    content.Add($"<graph {g.Name}>{comma}");
                }

                content.Add("]");
                graphs.AddRange(attr.Graphs);
            }
            else
            {
                content.Add("<Unknown>");
            }

            if (subgraphs)
            {
                return (string.Join(" ", content), graphs.ToArray());
            }
            else
            {
                return (string.Join(" ", content), null);
            }
        }

        public static string PrintableDim(TensorShapeProto.Dimension dim)
        {
            return dim.DimParam != null ? dim.DimParam : dim.DimValue.ToString();
        }

        public static string PrintableType(TypeProto t)
        {
            if (t.TensorType != null)
            {
                var s = Enum.GetName(typeof(TensorProto.DataType), t.TensorType.ElemType);
                if (t.TensorType.Shape != null)
                {
                    if (t.TensorType.Shape.Dims.Count > 0)
                    {
                        s += string.Join("x", t.TensorType.Shape.Dims.Select(x => PrintableDim(x)));
                    }
                    else
                    {
                        s += ", scalar".ToString();
                    }
                }
                return s;
            }

            return "";
        }

        public static string PrintableValueInfo(ValueInfoProto v)
        {
            var s = $"%{v.Name}";
            if (v.Type != null)
            {
                s = $"{s}[{PrintableType(v.Type)}]";
            }

            return s;
        }

        public static string PrintableTensorProto(TensorProto t)
        {
            var s = $"%{t.Name}[";
            s += Enum.GetName(typeof(TensorProto.DataType), t.data_type);
            if (t.Dims != null)
            {
                if (t.Dims.Length > 0)
                {
                    s += (", " + string.Join("x", t.Dims));
                }
                else
                {
                    s += ", scalar".ToString();
                }
            }

            s += "]";
            return s;
        }

        public static (string, GraphProto[]) PrintableNode(NodeProto node, string prefix= "", bool subgraphs= false)
        {
            var content = new List<string>();
            if (node.Outputs.Count > 0)
            {
                content.Add(string.Join(", ", (from name in node.Outputs
                                               select $"%{name}")));
                content.Add("=");
            }
            // To deal with nested graphs
            var graphs = new List<GraphProto>();
            var printed_attrs = new List<string>();
            foreach (var attr in node.Attributes)
            {
                if (subgraphs)
                {
                    var (printed_attr, gs) = PrintableAttribute(attr, subgraphs);
                    graphs.AddRange(gs);
                    printed_attrs.Add(printed_attr);
                }
                else
                {
                    var (printed, _) = PrintableAttribute(attr);
                    printed_attrs.Add(printed);
                }
            }

            var printed_attributes = string.Join(", ", printed_attrs.OrderBy(_p_1 => _p_1));
            var printed_inputs = string.Join(", ", (from name in node.Inputs
                                                    select $"%{name}"));
            if (node.Attributes != null)
            {
                content.Add($"{node.OpType}[{printed_attributes}]({printed_inputs})");
            }
            else
            {
                content.Add($"{node.OpType}({printed_inputs})");
            }

            if (subgraphs)
            {
                return (prefix + string.Join(" ", content), graphs.ToArray());
            }
            else
            {
                return (prefix + string.Join(" ", content), null);
            }
        }

        public static string PrintableGraph(GraphProto graph, string prefix = "")
        {
            var content = new List<string>();
            var indent = prefix + "  ";
            // header
            var header = new List<string> {
                                "graph",
                                graph.Name
                            };
            var initializers = (from t in graph.Initializers
                                select t.Name).ToHashSet().ToList();
            if (graph.Inputs.Count > 0)
            {
                header.Add("(");
                var in_strs = new List<string>();
                var in_with_init_strs = new List<string>();
                foreach (var inp in graph.Inputs)
                {
                    if (!initializers.Contains(inp.Name))
                    {
                        in_strs.Add(PrintableValueInfo(inp));
                    }
                    else
                    {
                        in_with_init_strs.Add(PrintableValueInfo(inp));
                    }
                }
                if (in_strs.Count > 0)
                {
                    content.Add(prefix + string.Join(" ", header));
                    header = new List<string>();
                    foreach (var line in in_strs)
                    {
                        content.Add(prefix + "  " + line);
                    }
                }

                header.Add(")");
                if (in_with_init_strs.Count > 0)
                {
                    header.Add("optional inputs with matching initializers (");
                    content.Add(prefix + string.Join(" ", header));
                    header = new List<string>();
                    foreach (var line in in_with_init_strs)
                    {
                        content.Add(prefix + "  " + line);
                    }
                    header.Add(")");
                }
                // from IR 4 onwards an initializer is not required to have a matching graph input
                // so output the name, type and shape of those as well
                if (in_with_init_strs.Count < initializers.Count)
                {
                    var graph_inputs = (from i in graph.Inputs
                                        select i.Name).ToHashSet().ToList();
                    var init_strs = (from i in graph.Initializers
                                     where !graph_inputs.Contains(i.Name)
                                     select PrintableTensorProto(i)).ToList();
                    header.Add("initializers (");
                    content.Add(prefix + string.Join(" ", header));
                    header = new List<string>();
                    foreach (var line in init_strs)
                    {
                        content.Add(prefix + "  " + line);
                    }

                    header.Add(")");
                }
            }
            header.Add("{");
            content.Add(prefix + string.Join(" ", header));
            var graphs = new List<GraphProto>();
            // body
            foreach (var node in graph.Nodes)
            {
                var (pn, gs) = PrintableNode(node, indent, subgraphs: true);
                content.Add(pn);
                graphs.AddRange(gs);
            }
            // tail
            var tail = new List<string> {
                            "return"
                        };

            if (graph.Outputs.Count > 0)
            {
                tail.Add(string.Join(", ", (from @out in graph.Outputs
                                       select $"%{@out.Name}")));
            }

            content.Add(indent + string.Join(" ", tail));
            // closing bracket
            content.Add(prefix + "}");
            foreach (var g in graphs)
            {
                content.Add("\n" + PrintableGraph(g));
            }

            return string.Join("\n", content);
        }

        public static void StripDocDtring(Google.Protobuf.IMessage proto)
        {
            foreach (var descriptor in proto.Descriptor.Fields.InDeclarationOrder())
            {
                if (descriptor.Name == "doc_string")
                {
                    descriptor.Accessor.Clear(proto);
                    //proto.ClearField(descriptor.name);
                }
                else if (descriptor.FieldType == FieldType.Message)
                {
                    if (descriptor.IsRepeated)
                    {
                        var protos = (IList<IMessage>)descriptor.Accessor.GetValue(proto);
                        foreach (var x in protos)
                        {
                            StripDocDtring(x);
                        }
                    }
                    else if (proto.Descriptor.Fields.InDeclarationOrder().FirstOrDefault(x=>x.Name == descriptor.Name) != null)
                    {
                        StripDocDtring((IMessage)descriptor.Accessor.GetValue(proto));
                    }
                }
            }
        }

        public static TrainingInfoProto make_training_info(GraphProto algorithm, AssignmentBindingType algorithm_bindings,
                GraphProto initialization, AssignmentBindingType initialization_bindings)
        {
            // type: (GraphProto, AssignmentBindingType,  Optional[GraphProto], Optional[AssignmentBindingType]) -> TrainingInfoProto
            var training_info = new TrainingInfoProto();
            training_info.Algorithm = algorithm;
            foreach (var _tup_1 in algorithm_bindings)
            {
                var k = _tup_1.Item1;
                var v = _tup_1.Item2;
                training_info.UpdateBindings.Add(new StringStringEntryProto() { Key = k,Value = v });
            }
            if (initialization != null)
            {
                training_info.Initialization = initialization;
            }
            if (initialization_bindings != null)
            {
                foreach (var _tup_2 in initialization_bindings)
                {
                    var k = _tup_2.Item1;
                    var v = _tup_2.Item2;
                    training_info.UpdateBindings.Add(new StringStringEntryProto() { Key = k, Value = v });
                }
            }

            return training_info;
        }
    }
}
