
using @absolute_import = @@__future__.absolute_import;

using @division = @@__future__.division;

using @print_function = @@__future__.print_function;

using @unicode_literals = @@__future__.unicode_literals;

using collections;

using numbers;

using text_type = six.text_type;

using integer_types = six.integer_types;

using binary_type = six.binary_type;

using google.protobuf.message;

using TensorProto = onnx.TensorProto;

using SparseTensorProto = onnx.SparseTensorProto;

using AttributeProto = onnx.AttributeProto;

using ValueInfoProto = onnx.ValueInfoProto;

using TensorShapeProto = onnx.TensorShapeProto;

using NodeProto = onnx.NodeProto;

using ModelProto = onnx.ModelProto;

using GraphProto = onnx.GraphProto;

using OperatorSetIdProto = onnx.OperatorSetIdProto;

using TypeProto = onnx.TypeProto;

using SequenceProto = onnx.SequenceProto;

using MapProto = onnx.MapProto;

using IR_VERSION = onnx.IR_VERSION;

using TrainingInfoProto = onnx.TrainingInfoProto;

using defs = onnx.defs;

using mapping = onnx.mapping;

using STORAGE_TENSOR_TYPE_TO_FIELD = onnx.mapping.STORAGE_TENSOR_TYPE_TO_FIELD;

using Text = typing.Text;

using Sequence = typing.Sequence;

using Any = typing.Any;

using Optional = typing.Optional;

using Dict = typing.Dict;

using Union = typing.Union;

using TypeVar = typing.TypeVar;

using Callable = typing.Callable;

using Tuple = typing.Tuple;

using List = typing.List;

using cast = typing.cast;

using np = numpy;

using System.Collections.Generic;

using System;

using System.Linq;

using System.Collections;

using System.Diagnostics;

public static class helper {
    
    public static object VersionRowType = Union[Tuple[Text,@int,@int,@int],Tuple[Text,@int,@int,@int,@int]];
    
    public static object VersionTableType = List[VersionRowType];
    
    public static object AssignmentBindingType = List[Tuple[Text,Text]];
    
    public static List<object> VERSION_TABLE = new List<object> {
        ("1.0", 3, 1, 1),
        ("1.1", 3, 5, 1),
        ("1.1.2", 3, 6, 1),
        ("1.2", 3, 7, 1),
        ("1.3", 3, 8, 1),
        ("1.4.1", 4, 9, 1),
        ("1.5.0", 5, 10, 1),
        ("1.6.0", 6, 11, 2),
        ("1.7.0", 7, 12, 2, 1),
        ("1.8.0", 7, 13, 2, 1),
        ("1.8.1", 7, 13, 2, 1)
    };
    
    public static object VersionMapType = Dict[Tuple[Text,@int],@int];
    
    // create a map from (opset-domain, opset-version) to ir-version from above table
    public static dict create_op_set_id_version_map(List<object> table) {
        // type: (VersionTableType) -> VersionMapType
        var result = new dict();
        Func<object, object, object, object> process = (release_version,ir_version,args) => {
            // type: (Text, int, Any) -> None
            foreach (var pair in zip(new List<string> {
                "ai.onnx",
                "ai.onnx.ml",
                "ai.onnx.training"
            }, args)) {
                if (!result.Contains(pair)) {
                    result[pair] = ir_version;
                }
            }
        };
        foreach (var row in table) {
            process(row);
        }
        return result;
    }
    
    public static dict OP_SET_ID_VERSION_MAP = create_op_set_id_version_map(VERSION_TABLE);
    
    // Given list of opset ids, determine minimum IR version required
    public static int find_min_ir_version_for(List<object> opsetidlist) {
        // type: (List[OperatorSetIdProto]) -> int
        var default_min_version = 3;
        Func<object, object, object> find_min = (domain,version) => {
            // type: (Union[Text, None], int) -> int
            var key = (domain ? domain : "ai.onnx", version);
            if (OP_SET_ID_VERSION_MAP.Contains(key)) {
                return OP_SET_ID_VERSION_MAP[key];
            } else {
                throw new ValueError("Unsupported opset-version.");
            }
        };
        if (opsetidlist) {
            return max((from x in opsetidlist
                select find_min(x.domain, x.version)).ToList());
        }
        return default_min_version;
    }
    
    //<parser-error>
    //<parser-error>
    // type: (...) -> NodeProto
    //<parser-error>
    //<parser-error>
    // type: (...) -> OperatorSetIdProto
    //<parser-error>
    //<parser-error>
    // type: (...) -> GraphProto
    //<parser-error>
    public static void make_opsetid(object domain, object version) {
        // type: (Text, int) -> OperatorSetIdProto
        var opsetid = OperatorSetIdProto();
        opsetid.domain = domain;
        opsetid.version = version;
        return opsetid;
    }
    
    public static void make_model(object graph, Hashtable kwargs) {
        // type: (GraphProto, **Any) -> ModelProto
        var model = ModelProto();
        // Touch model.ir_version so it is stored as the version from which it is
        // generated.
        model.ir_version = IR_VERSION;
        model.graph.CopyFrom(graph);
        object opset_imports = null;
        opset_imports = kwargs.pop("opset_imports", null);
        if (opset_imports != null) {
            model.opset_import.extend(opset_imports);
        } else {
            // Default import
            var imp = model.opset_import.add();
            imp.version = defs.onnx_opset_version();
        }
        foreach (var _tup_1 in kwargs.items()) {
            var k = _tup_1.Item1;
            var v = _tup_1.Item2;
            // TODO: Does this work with repeated fields?
            setattr(model, k, v);
        }
        return model;
    }
    
    // An extension of make_model that infers an IR_VERSION for the model,
    // if not specified, using a best-effort-basis.
    public static void make_model_gen_version(object graph, Hashtable kwargs) {
        // type: (GraphProto, **Any) -> ModelProto
        var ir_version_field = "ir_version".ToString();
        if (!kwargs.Contains(ir_version_field)) {
            var opset_imports_field = "opset_imports".ToString();
            var imports = kwargs.Contains(opset_imports_field) ? kwargs[opset_imports_field] : new List<object>();
            kwargs[ir_version_field] = find_min_ir_version_for(imports);
        }
        return make_model(graph, kwargs);
    }
    
    public static void set_model_props(object model, object dict_value) {
        // type: (ModelProto, Dict[Text, Text]) -> None
        model.metadata_props.Remove(":");
        foreach (var _tup_1 in dict_value.items()) {
            var k = _tup_1.Item1;
            var v = _tup_1.Item2;
            var entry = model.metadata_props.add();
            entry.key = k;
            entry.value = v;
            // model.metadata_properties.append(entry)
        }
    }
    
    public static List<object> split_complex_to_pairs(object ca) {
        // type: (Sequence[np.complex64]) -> Sequence[int]
        return (from i in Enumerable.Range(0, ca.Count * 2)
            select i % 2 == 0 ? ca[i / 2].real : ca[i / 2].imag).ToList();
    }
    
    // 
    //     Make a TensorProto with specified arguments.  If raw is False, this
    //     function will choose the corresponding proto field to store the
    //     values based on data_type. If raw is True, use "raw_data" proto
    //     field to store the values, and values should be of type bytes in
    //     this case.
    //     
    public static void make_tensor(
        object name,
        object data_type,
        object dims,
        object vals,
        bool raw = false) {
        // type: (...) -> TensorProto
        var tensor = TensorProto();
        tensor.data_type = data_type;
        tensor.name = name;
        if (data_type == TensorProto.STRING) {
            Debug.Assert(!raw);
            Debug.Assert("Can not use raw_data to store string type");
        }
        // Check number of vals specified equals tensor size
        var size = !raw ? 1 : mapping.TENSOR_TYPE_TO_NP_TYPE[data_type].itemsize;
        foreach (var d in dims) {
            size = size * d;
        }
        if (vals.Count != size) {
            throw new ValueError("Number of values does not match tensor's size.");
        }
        if (data_type == TensorProto.COMPLEX64 || data_type == TensorProto.COMPLEX128) {
            vals = split_complex_to_pairs(vals);
        }
        if (raw) {
            tensor.raw_data = vals;
        } else {
            var field = mapping.STORAGE_TENSOR_TYPE_TO_FIELD[mapping.TENSOR_TYPE_TO_STORAGE_TENSOR_TYPE[data_type]];
            getattr(tensor, field).extend(vals);
        }
        tensor.dims.extend(dims);
        return tensor;
    }
    
    //<parser-error>
    //<parser-error>
    // type: (...) -> SparseTensorProto
    //<parser-error>
    //<parser-error>
    // type: (...) -> SequenceProto
    //<parser-error>
    //<parser-error>
    //<parser-error>
    // type: (...) -> MapProto
    //<parser-error>
    // An internal graph to convert the input to a bytes or to False.
    // 
    //     The criteria for conversion is as follows and should be python 2 and 3
    //     compatible:
    //     - If val is py2 str or py3 bytes: return bytes
    //     - If val is py2 unicode or py3 str: return val.decode('utf-8')
    //     - Otherwise, return False
    //     
    public static bool _to_bytes_or_false(object val) {
        // type: (Union[Text, bytes]) -> Union[bytes, bool]
        if (val is bytes) {
            return val;
        }
        try {
            return val.encode("utf-8");
        } catch (AttributeError) {
            return false;
        }
    }
    
    // Makes an AttributeProto based on the value type.
    public static void make_attribute(object key, object value, void doc_string = null) {
        // type: (...) -> AttributeProto
        var attr = AttributeProto();
        attr.name = key;
        if (doc_string) {
            attr.doc_string = doc_string;
        }
        var is_iterable = value is collections.Iterable;
        var bytes_or_false = _to_bytes_or_false(value);
        // First, singular cases
        // float
        if (value is float) {
            attr.f = value;
            attr.type = AttributeProto.FLOAT;
        } else if (value is numbers.Integral) {
            // integer
            attr.i = cast(@int, value);
            attr.type = AttributeProto.INT;
        } else if (bytes_or_false != false) {
            // string
            Debug.Assert(bytes_or_false is bytes);
            attr.s = bytes_or_false;
            attr.type = AttributeProto.STRING;
        } else if (value is TensorProto) {
            attr.t.CopyFrom(value);
            attr.type = AttributeProto.TENSOR;
        } else if (value is SparseTensorProto) {
            attr.sparse_tensor.CopyFrom(value);
            attr.type = AttributeProto.SPARSE_TENSOR;
        } else if (value is GraphProto) {
            attr.g.CopyFrom(value);
            attr.type = AttributeProto.GRAPH;
        } else if (is_iterable) {
            // third, iterable cases
            var byte_array = (from v in value
                select _to_bytes_or_false(v)).ToList();
            if (all(from v in value
                select v is numbers.Integral)) {
                // Turn np.int32/64 into Python built-in int.
                attr.ints.extend(from v in value
                    select Convert.ToInt32(v));
                attr.type = AttributeProto.INTS;
            } else if (all(from v in value
                select v is numbers.Real)) {
                // Since ints and floats are members of Real, this allows a mix of ints and floats
                // (and converts the ints to floats).
                attr.floats.extend(from v in value
                    select float(v));
                attr.type = AttributeProto.FLOATS;
            } else if (all(map(bytes_or_false => bytes_or_false != false, byte_array))) {
                attr.strings.extend(cast(List[bytes], byte_array));
                attr.type = AttributeProto.STRINGS;
            } else if (all(from v in value
                select v is TensorProto)) {
                attr.tensors.extend(value);
                attr.type = AttributeProto.TENSORS;
            } else if (all(from v in value
                select v is SparseTensorProto)) {
                attr.sparse_tensors.extend(value);
                attr.type = AttributeProto.SPARSE_TENSORS;
            } else if (all(from v in value
                select v is GraphProto)) {
                attr.graphs.extend(value);
                attr.type = AttributeProto.GRAPHS;
            } else {
                throw new ValueError("You passed in an iterable attribute but I cannot figure out its applicable type.");
            }
        } else {
            throw new TypeError("value \"{}\" is not valid attribute data type.".format(value));
        }
        return attr;
    }
    
    public static list get_attribute_value(object attr) {
        // type: (AttributeProto) -> Any
        if (attr.type == AttributeProto.FLOAT) {
            return attr.f;
        }
        if (attr.type == AttributeProto.INT) {
            return attr.i;
        }
        if (attr.type == AttributeProto.STRING) {
            return attr.s;
        }
        if (attr.type == AttributeProto.TENSOR) {
            return attr.t;
        }
        if (attr.type == AttributeProto.GRAPH) {
            return attr.g;
        }
        if (attr.type == AttributeProto.FLOATS) {
            return attr.floats.ToList();
        }
        if (attr.type == AttributeProto.INTS) {
            return attr.ints.ToList();
        }
        if (attr.type == AttributeProto.STRINGS) {
            return attr.strings.ToList();
        }
        if (attr.type == AttributeProto.TENSORS) {
            return attr.tensors.ToList();
        }
        if (attr.type == AttributeProto.GRAPHS) {
            return attr.graphs.ToList();
        }
        throw new ValueError("Unsupported ONNX attribute: {}".format(attr));
    }
    
    public static void make_empty_tensor_value_info(object name) {
        // type: (Text) -> ValueInfoProto
        var value_info_proto = ValueInfoProto();
        value_info_proto.name = name;
        return value_info_proto;
    }
    
    //<parser-error>
    // type: (...) -> ValueInfoProto
    //<parser-error>
    // You might think this is a no-op (extending a normal Python
    // list by [] certainly is), but protobuf lists work a little
    // differently; if a field is never set, it is omitted from the
    // resulting protobuf; a list that is explicitly set to be
    // empty will get an (empty) entry in the protobuf. This
    // difference is visible to our consumers, so make sure we emit
    // an empty shape!
    //<parser-error>
    //<parser-error>
    // type: (...) -> ValueInfoProto
    //<parser-error>
    public static object _sanitize_str(object s) {
        // type: (Union[Text, bytes]) -> Text
        if (s is text_type) {
            var sanitized = s;
        } else if (s is binary_type) {
            sanitized = s.decode("utf-8", errors: "ignore");
        } else {
            sanitized = s.ToString();
        }
        if (sanitized.Count < 64) {
            return sanitized;
        }
        return sanitized[::64] + String.Format("...<+len=%d>", sanitized.Count - 64);
    }
    
    public static object printable_attribute(object attr, bool subgraphs = false) {
        // type: (AttributeProto, bool) -> Union[Text, Tuple[Text, List[GraphProto]]]
        var content = new List<object>();
        content.append(attr.name);
        content.append("=");
        Func<object, object> str_float = f => {
            // type: (float) -> Text
            // NB: Different Python versions print different numbers of trailing
            // decimals, specifying this explicitly keeps it consistent for all
            // versions
            return "{:.15g}".format(f);
        };
        Func<object, object> str_int = i => {
            // type: (int) -> Text
            // NB: In Python 2, longs will repr() as '2L', which is ugly and
            // unnecessary.  Explicitly format it to keep it consistent.
            return "{:d}".format(i);
        };
        Func<object, object> str_str = s => {
            // type: (Text) -> Text
            return repr(s);
        };
        var _T = TypeVar("_T");
        Func<object, object, object> str_list = (str_elem,xs) => {
            // type: (Callable[[_T], Text], Sequence[_T]) -> Text
            return "[" + ", ".join(map(str_elem, xs)) + "]";
        };
        // for now, this logic should continue to work as long as we are running on a proto3
        // implementation. If/when we switch to proto3, we will need to use attr.type
        // To support printing subgraphs, if we find a graph attribute, print out
        // its name here and pass the graph itself up to the caller for later
        // printing.
        var graphs = new List<object>();
        if (attr.HasField("f")) {
            content.append(str_float(attr.f));
        } else if (attr.HasField("i")) {
            content.append(str_int(attr.i));
        } else if (attr.HasField("s")) {
            // TODO: Bit nervous about Python 2 / Python 3 determinism implications
            content.append(repr(_sanitize_str(attr.s)));
        } else if (attr.HasField("t")) {
            if (attr.t.dims.Count > 0) {
                content.append("<Tensor>");
            } else {
                // special case to print scalars
                var field = STORAGE_TENSOR_TYPE_TO_FIELD[attr.t.data_type];
                content.append("<Scalar Tensor {}>".format(getattr(attr.t, field).ToString()));
            }
        } else if (attr.HasField("g")) {
            content.append("<graph {}>".format(attr.g.name));
            graphs.append(attr.g);
        } else if (attr.floats) {
            content.append(str_list(str_float, attr.floats));
        } else if (attr.ints) {
            content.append(str_list(str_int, attr.ints));
        } else if (attr.strings) {
            // TODO: Bit nervous about Python 2 / Python 3 determinism implications
            content.append(map(_sanitize_str, attr.strings).ToList().ToString());
        } else if (attr.tensors) {
            content.append("[<Tensor>, ...]");
        } else if (attr.graphs) {
            content.append("[");
            foreach (var _tup_1 in attr.graphs.Select((_p_1,_p_2) => Tuple.Create(_p_2, _p_1))) {
                var i = _tup_1.Item1;
                var g = _tup_1.Item2;
                var comma = i != attr.graphs.Count - 1 ? "," : "";
                content.append("<graph {}>{}".format(g.name, comma));
            }
            content.append("]");
            graphs.extend(attr.graphs);
        } else {
            content.append("<Unknown>");
        }
        if (subgraphs) {
            return Tuple.Create(" ".join(content), graphs);
        } else {
            return " ".join(content);
        }
    }
    
    public static string printable_dim(object dim) {
        // type: (TensorShapeProto.Dimension) -> Text
        var which = dim.WhichOneof("value");
        Debug.Assert(which != null);
        return getattr(dim, which).ToString();
    }
    
    public static string printable_type(object t) {
        // type: (TypeProto) -> Text
        if (t.WhichOneof("value") == "tensor_type") {
            var s = TensorProto.DataType.Name(t.tensor_type.elem_type);
            if (t.tensor_type.HasField("shape")) {
                if (t.tensor_type.shape.dim.Count) {
                    s += (", " + "x".join(map(printable_dim, t.tensor_type.shape.dim))).ToString();
                } else {
                    s += ", scalar".ToString();
                }
            }
            return s;
        }
        if (t.WhichOneof("value") == null) {
            return "";
        }
        return "Unknown type {}".format(t.WhichOneof("value"));
    }
    
    public static string printable_value_info(object v) {
        // type: (ValueInfoProto) -> Text
        var s = "%{}".format(v.name);
        if (v.type) {
            s = "{}[{}]".format(s, printable_type(v.type));
        }
        return s;
    }
    
    public static string printable_tensor_proto(object t) {
        // type: (TensorProto) -> Text
        var s = "%{}[".format(t.name);
        s += TensorProto.DataType.Name(t.data_type);
        if (t.dims != null) {
            if (t.dims.Count) {
                s += (", " + "x".join(map(str, t.dims))).ToString();
            } else {
                s += ", scalar".ToString();
            }
        }
        s += "]";
        return s;
    }
    
    public static object printable_node(object node, string prefix = "", bool subgraphs = false) {
        // type: (NodeProto, Text, bool) -> Union[Text, Tuple[Text, List[GraphProto]]]
        var content = new List<object>();
        if (node.output.Count) {
            content.append(", ".join((from name in node.output
                select "%{}".format(name)).ToList()));
            content.append("=");
        }
        // To deal with nested graphs
        var graphs = new List<object>();
        var printed_attrs = new List<object>();
        foreach (var attr in node.attribute) {
            if (subgraphs) {
                var _tup_1 = printable_attribute(attr, subgraphs);
                var printed_attr = _tup_1.Item1;
                var gs = _tup_1.Item2;
                Debug.Assert(gs is list);
                graphs.extend(gs);
                printed_attrs.append(printed_attr);
            } else {
                var printed = printable_attribute(attr);
                Debug.Assert(printed is Text);
                printed_attrs.append(printed);
            }
        }
        var printed_attributes = ", ".join(printed_attrs.OrderBy(_p_1 => _p_1).ToList());
        var printed_inputs = ", ".join((from name in node.input
            select "%{}".format(name)).ToList());
        if (node.attribute) {
            content.append("{}[{}]({})".format(node.op_type, printed_attributes, printed_inputs));
        } else {
            content.append("{}({})".format(node.op_type, printed_inputs));
        }
        if (subgraphs) {
            return Tuple.Create(prefix + " ".join(content), graphs);
        } else {
            return prefix + " ".join(content);
        }
    }
    
    public static string printable_graph(object graph, string prefix = "") {
        // type: (GraphProto, Text) -> Text
        var content = new List<object>();
        var indent = prefix + "  ";
        // header
        var header = new List<string> {
            "graph",
            graph.name
        };
        var initializers = (from t in graph.initializer
            select t.name).ToHashSet();
        if (graph.input.Count) {
            header.append("(");
            var in_strs = new List<object>();
            var in_with_init_strs = new List<object>();
            foreach (var inp in graph.input) {
                if (!initializers.Contains(inp.name)) {
                    in_strs.append(printable_value_info(inp));
                } else {
                    in_with_init_strs.append(printable_value_info(inp));
                }
            }
            if (in_strs) {
                content.append(prefix + " ".join(header));
                header = new List<object>();
                foreach (var line in in_strs) {
                    content.append(prefix + "  " + line);
                }
            }
            header.append(")");
            if (in_with_init_strs) {
                header.append("optional inputs with matching initializers (");
                content.append(prefix + " ".join(header));
                header = new List<object>();
                foreach (var line in in_with_init_strs) {
                    content.append(prefix + "  " + line);
                }
                header.append(")");
            }
            // from IR 4 onwards an initializer is not required to have a matching graph input
            // so output the name, type and shape of those as well
            if (in_with_init_strs.Count < initializers.Count) {
                var graph_inputs = (from i in graph.input
                    select i.name).ToHashSet();
                var init_strs = (from i in graph.initializer
                    where !graph_inputs.Contains(i.name)
                    select printable_tensor_proto(i)).ToList();
                header.append("initializers (");
                content.append(prefix + " ".join(header));
                header = new List<object>();
                foreach (var line in init_strs) {
                    content.append(prefix + "  " + line);
                }
                header.append(")");
            }
        }
        header.append("{");
        content.append(prefix + " ".join(header));
        var graphs = new List<object>();
        // body
        foreach (var node in graph.node) {
            var _tup_1 = printable_node(node, indent, subgraphs: true);
            var pn = _tup_1.Item1;
            var gs = _tup_1.Item2;
            Debug.Assert(gs is list);
            content.append(pn);
            graphs.extend(gs);
        }
        // tail
        var tail = new List<string> {
            "return"
        };
        if (graph.output.Count) {
            tail.append(", ".join((from @out in graph.output
                select "%{}".format(@out.name)).ToList()));
        }
        content.append(indent + " ".join(tail));
        // closing bracket
        content.append(prefix + "}");
        foreach (var g in graphs) {
            content.append("\n" + printable_graph(g));
        }
        return "\n".join(content);
    }
    
    // 
    //     Empties `doc_string` field on any nested protobuf messages
    //     
    public static void strip_doc_string(object proto) {
        // type: (google.protobuf.message.Message) -> None
        Debug.Assert(proto is google.protobuf.message.Message);
        foreach (var descriptor in proto.DESCRIPTOR.fields) {
            if (descriptor.name == "doc_string") {
                proto.ClearField(descriptor.name);
            } else if (descriptor.type == descriptor.TYPE_MESSAGE) {
                if (descriptor.label == descriptor.LABEL_REPEATED) {
                    foreach (var x in getattr(proto, descriptor.name)) {
                        strip_doc_string(x);
                    }
                } else if (proto.HasField(descriptor.name)) {
                    strip_doc_string(getattr(proto, descriptor.name));
                }
            }
        }
    }
    
    public static void make_training_info(object algorithm, object algorithm_bindings, object initialization, object initialization_bindings) {
        object binding;
        object v;
        object k;
        // type: (GraphProto, AssignmentBindingType,  Optional[GraphProto], Optional[AssignmentBindingType]) -> TrainingInfoProto
        var training_info = TrainingInfoProto();
        training_info.algorithm.CopyFrom(algorithm);
        foreach (var _tup_1 in algorithm_bindings) {
            k = _tup_1.Item1;
            v = _tup_1.Item2;
            binding = training_info.update_binding.add();
            binding.key = k;
            binding.value = v;
        }
        if (initialization) {
            training_info.initialization.CopyFrom(initialization);
        }
        if (initialization_bindings) {
            foreach (var _tup_2 in initialization_bindings) {
                k = _tup_2.Item1;
                v = _tup_2.Item2;
                binding = training_info.initialization_binding.add();
                binding.key = k;
                binding.value = v;
            }
        }
        return training_info;
    }
}
