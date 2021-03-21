using System;
using System.Collections.Generic;

namespace Onnx
{
    public class C
    {
        public static void check_value_info(byte[] bytes, CheckerContext checker_context)
        {
            throw new NotImplementedException();
        }

        public static void check_tensor(byte[] bytes, CheckerContext checker_context)
        {
            throw new NotImplementedException();
        }

        public static void check_sparse_tensor(byte[] bytes, CheckerContext checker_context)
        {
            throw new NotImplementedException();
        }

        public static void check_attribute(byte[] bytes, CheckerContext checker_context)
        {
            throw new NotImplementedException();
        }

        public static void check_node(byte[] bytes, CheckerContext checker_context)
        {
            throw new NotImplementedException();
        }

        public static void check_graph(byte[] bytes, CheckerContext checker_context)
        {
            throw new NotImplementedException();
        }

        public static void check_model(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public static void check_model_path(string path)
        {
            throw new NotImplementedException();
        }

        public static bool has_schema(string op_type)
        {
            throw new NotImplementedException();
        }

        public static Dictionary<string, (int, int)> schema_version_map()
        {
            throw new NotImplementedException();
        }

        public static OpSchema get_schema(string op_type, int max_inclusive_version, string domain = "")
        {
            throw new NotImplementedException();
        }

        public static OpSchema get_schema(string op_type, string domain = "")
        {
            throw new NotImplementedException();
        }

        public static OpSchema[] get_all_schemas()
        {
            throw new NotImplementedException();
        }

        public static OpSchema[] get_all_schemas_with_history()
        {
            throw new NotImplementedException();
        }

        public static byte[] infer_shapes(byte[] bytes, bool check_type, bool strict_mode)
        {
            throw new NotImplementedException();
        }

        public static void infer_shapes_path(string model_path, string output_path, bool check_type, bool strict_mode)
        {
            throw new NotImplementedException();
        }

        public static byte[] convert_version(byte[] bytes, int target)
        {
            throw new NotImplementedException();
        }
    }
}
