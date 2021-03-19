using System;
using System.Collections.Generic;

namespace Onnx
{
    public class Extractor
    {
        public Extractor(ModelProto model)
        {
            throw new NotImplementedException();
        }
        
        internal static Dictionary<string, ValueInfoProto> _build_name2obj_dict(params ValueInfoProto[] objs)
        {
            throw new NotImplementedException();
        }

        internal ValueInfoProto[] _collect_new_io_core(ValueInfoProto[] original_io, string[] io_names_to_extract)
        {
            throw new NotImplementedException();
        }

        internal ValueInfoProto[] _collect_new_inputs(string[] names)
        {
            throw new NotImplementedException();
        }
        
        internal ValueInfoProto[] _collect_new_outputs(string[] names)
        {
            throw new NotImplementedException();
        }

        internal NodeProto _dfs_search_reachable_nodes(string node_output_name, string[] graph_input_names,
            NodeProto[] reachable_nodes)
        {
            throw new NotImplementedException();
        }

        internal ModelProto _make_model(NodeProto[] nodes, ValueInfoProto[] inputs, ValueInfoProto[] outputs,
            TensorProto[] initializer, ValueInfoProto[] value_info)
        {
            throw new NotImplementedException();
        }

        public ModelProto ExtractModel(string[] input_names, string[] output_names)
        {
            throw new NotImplementedException();
        }
    }
}