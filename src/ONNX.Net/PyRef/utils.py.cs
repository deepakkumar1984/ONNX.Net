
using @absolute_import = @@__future__.absolute_import;

using @division = @@__future__.division;

using @print_function = @@__future__.print_function;

using @unicode_literals = @@__future__.unicode_literals;

using os;

using List = typing.List;

using Tuple = typing.Tuple;

using Text = typing.Text;

using onnx.checker;

using onnx.helper;

using onnx.shape_inference;

using ModelProto = onnx.ModelProto;

using NodeProto = onnx.NodeProto;

using TensorProto = onnx.TensorProto;

using ValueInfoProto = onnx.ValueInfoProto;

using System.Collections.Generic;

using System.Linq;

public static class utils {
    
    public class Extractor {
        
        public object graph;
        
        public object model;
        
        public List<object> nodes;
        
        public object vimap;
        
        public object wmap;
        
        public Extractor(object model) {
            // type: (ModelProto) -> None
            this.model = onnx.shape_inference.infer_shapes(model);
            this.graph = this.model.graph;
            this.wmap = this._build_name2obj_dict(this.graph.initializer);
            this.vimap = this._build_name2obj_dict(this.graph.value_info);
        }
        
        [staticmethod]
        public static Dictionary<object, object> _build_name2obj_dict(object objs) {
            // type: ignore
            return objs.ToDictionary(obj => obj.name, obj => obj);
        }
        
        public virtual List<object> _collect_new_io_core(object original_io, object io_names_to_extract) {
            // type: ignore
            var original_io_map = this._build_name2obj_dict(original_io);
            var original_io_names = new HashSet<object>(original_io_map.keys());
            var s_io_names_to_extract = new HashSet<object>(io_names_to_extract);
            var io_names_to_keep = s_io_names_to_extract & original_io_names;
            var new_io_names_to_add = s_io_names_to_extract - original_io_names;
            var new_io_tensors = new List<object>();
            foreach (var name in io_names_to_keep) {
                new_io_tensors.append(original_io_map[name]);
            }
            foreach (var name in new_io_names_to_add) {
                // activation become input or output
                new_io_tensors.append(this.vimap[name]);
            }
            // adjust sequence
            var new_io_tensors_map = this._build_name2obj_dict(new_io_tensors);
            return (from name in io_names_to_extract
                select new_io_tensors_map[name]).ToList();
        }
        
        public virtual void _collect_new_inputs(object names) {
            // type: (List[Text]) -> List[ValueInfoProto]
            return this._collect_new_io_core(this.graph.input, names);
        }
        
        public virtual void _collect_new_outputs(object names) {
            // type: (List[Text]) -> List[ValueInfoProto]
            return this._collect_new_io_core(this.graph.output, names);
        }
        
        static Extractor() {
            this._dfs_search_reachable_nodes(name, input_names, reachable_nodes);
        }
        
        public List<object> nodes = (from n in this.graph.node
            where reachable_nodes.Contains(n)
            select n).ToList();
    }
}
