using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Onnx
{
    public class Extractor
    {
        public GraphProto graph;

        public ModelProto model;

        public List<NodeProto> nodes;

        public Dictionary<string, ValueInfoProto> vimap;

        public Dictionary<string, TensorProto> wmap;

        public Extractor(ModelProto model)
        {
            this.model = ShapeInference.infer_shapes(model);
            this.graph = this.model.Graph;
            this.wmap = this._build_name2obj_dict(this.graph.Initializer.ToArray());
            this.vimap = this._build_name2obj_dict(this.graph.ValueInfo.ToArray());
        }

        internal Dictionary<string, ValueInfoProto> _build_name2obj_dict(params ValueInfoProto[] objs)
        {
            return objs.ToDictionary(obj => obj.Name, obj => obj);
        }

        internal Dictionary<string, TensorProto> _build_name2obj_dict(params TensorProto[] objs)
        {
            return objs.ToDictionary(obj => obj.Name, obj => obj);
        }

        internal ValueInfoProto[] _collect_new_io_core(ValueInfoProto[] original_io, string[] io_names_to_extract)
        {
            var original_io_map = this._build_name2obj_dict(original_io);
            var original_io_names = new HashSet<string>(original_io_map.Keys).ToList();
            var s_io_names_to_extract = new HashSet<string>(io_names_to_extract).ToList();
            var io_names_to_keep = new List<string>();
            var new_io_names_to_add = new List<string>();

            foreach (var name in s_io_names_to_extract)
            {
                if (original_io_names.Contains(name))
                    io_names_to_keep.Add(name);

                if (!original_io_names.Contains(name))
                    new_io_names_to_add.Add(name);
            }

            var new_io_tensors = new List<ValueInfoProto>();
            foreach (var name in io_names_to_keep)
            {
                new_io_tensors.Add(original_io_map[name]);
            }

            foreach (var name in new_io_names_to_add)
            {
                // activation become input or output
                new_io_tensors.Add(this.vimap[name]);
            }
            // adjust sequence
            var new_io_tensors_map = this._build_name2obj_dict(new_io_tensors.ToArray());
            return (from name in io_names_to_extract
                    select new_io_tensors_map[name]).ToArray();
        }

        internal ValueInfoProto[] _collect_new_inputs(string[] names)
        {
            return this._collect_new_io_core(this.graph.Input.ToArray(), names);
        }

        internal ValueInfoProto[] _collect_new_outputs(string[] names)
        {
            return this._collect_new_io_core(this.graph.Output.ToArray(), names);
        }

        internal void _dfs_search_reachable_nodes(string node_output_name, string[] graph_input_names,
            List<NodeProto> reachable_nodes)
        {
            if (graph_input_names.Contains(node_output_name))
                return;
            foreach (var node in graph.Node)
            {
                if (reachable_nodes.FirstOrDefault(x => x.Name == node.Name) != null)
                    continue;
                if (node.Output.Contains(node_output_name))
                    continue;
                reachable_nodes.Add(node);
                foreach (var name in node.Input)
                {
                    this._dfs_search_reachable_nodes(name, graph_input_names, reachable_nodes);
                }
            }
        }

        internal NodeProto[] _collect_reachable_nodes(string[] input_names, string[] output_names)
        {
            var reachable_nodes = new List<NodeProto>();
            var nodes = new List<NodeProto>();
            foreach (var name in output_names)
            {
                this._dfs_search_reachable_nodes(name, input_names, reachable_nodes);
            }

            foreach (var n in graph.Node)
            {
                if (reachable_nodes.FirstOrDefault(x => x.Name == n.Name) != null)
                {
                    nodes.Add(n);
                }
            }

            return nodes.ToArray();
        }

        internal (TensorProto[], ValueInfoProto[]) _collect_reachable_tensors(NodeProto[] nodes)
        {
            var all_tensors_name = new HashSet<string>();
            foreach (var node in nodes)
            {
                foreach (var name in node.Input)
                {
                    all_tensors_name.Add(name);
                }

                foreach (var name in node.Output)
                {
                    all_tensors_name.Add(name);
                }
            }

            List<TensorProto> initializer = new List<TensorProto>();
            List<ValueInfoProto> value_info = new List<ValueInfoProto>();
            foreach (var (k, v) in wmap)
            {
                if (all_tensors_name.Contains(k))
                    initializer.Add(v);
            }

            foreach (var (k, v) in vimap)
            {
                if (all_tensors_name.Contains(k))
                    value_info.Add(v);
            }

            Debug.Assert(graph.SparseInitializer.Count == 0);
            Debug.Assert(graph.QuantizationAnnotation.Count == 0);
            return (initializer.ToArray(), value_info.ToArray());
        }



        internal ModelProto _make_model(NodeProto[] nodes, ValueInfoProto[] inputs, ValueInfoProto[] outputs,
            TensorProto[] initializer, ValueInfoProto[] value_info)
        {
            string name = "Extracted from {" + this.graph.Name + "}";
            graph = Helper.MakeGraph(nodes, name, inputs, outputs, initializer, value_info: value_info);

            var meta = new Dictionary<string, object>(){
                { "ir_version", model.IrVersion},
                {"opset_imports", model.OpsetImport },
                { "producer_name", "Utils.ExtractModel"}
            };

            return Helper.MakeModel(graph, meta);
        }

        public ModelProto ExtractModel(string[] input_names, string[] output_names)
        {
            var inputs = _collect_new_inputs(input_names);
            var outputs = _collect_new_outputs(output_names);
            var nodes = _collect_reachable_nodes(input_names, output_names);
            var (initializer, value_info) = _collect_reachable_tensors(nodes);
            var model = _make_model(nodes, inputs, outputs, initializer, value_info);

            return model;
        }
    }
}