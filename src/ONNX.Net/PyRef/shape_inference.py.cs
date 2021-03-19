
using @absolute_import = @@__future__.absolute_import;

using @division = @@__future__.division;

using @print_function = @@__future__.print_function;

using @unicode_literals = @@__future__.unicode_literals;

using onnx;

using C = onnx.onnx_cpp2py_export.shape_inference;

using ModelProto = onnx.ModelProto;

using string_types = six.string_types;

using Text = typing.Text;

public static class shape_inference {
    
    static shape_inference() {
        @"onnx shape inference. Shape inference is not guaranteed to be
complete.

";
        @"Apply shape inference to the provided ModelProto.

Inferred shapes are added to the value_info field of the graph.

If the inferred values conflict with values already provided in the
graph, that means that the provided values are invalid (or there is a
bug in shape inference), and the result is unspecified.

Arguments:
    input (Union[ModelProto, Text], Text, bool) -> ModelProto

Return:
    return (ModelProto) model with inferred shape information
";
    }
    
    // SPDX-License-Identifier: Apache-2.0
    public static void infer_shapes(object model, bool check_type = false, bool strict_mode = false) {
        // type: (ModelProto, bool, bool) -> ModelProto
        if (model is ModelProto) {
            var model_str = model.SerializeToString();
            var inferred_model_str = C.infer_shapes(model_str, check_type, strict_mode);
            return onnx.load_from_string(inferred_model_str);
        } else if (model is string_types) {
            throw new TypeError("infer_shapes only accepts ModelProto,you can use infer_shapes_path for the model path (String).");
        } else {
            throw new TypeError("infer_shapes only accepts ModelProto, incorrect type: {}".format(type(model)));
        }
    }
    
    // 
    //     Take model path for shape_inference same as infer_shape; it support >2GB models
    //     Directly output the inferred model to the output_path; Default is the original model path
    //     
    public static void infer_shapes_path(object model_path, string output_path = "", bool check_type = false, bool strict_mode = false) {
        // type: (Text, Text, bool, bool) -> None
        if (model_path is ModelProto) {
            throw new TypeError("infer_shapes_path only accepts model Path (String),you can use infer_shapes for the ModelProto.");
        } else if (model_path is string_types) {
            // Directly output the inferred model into the specified path, return nothing
            // If output_path is not defined, default output_path would be the original model path
            if (output_path == "") {
                output_path = model_path;
            }
            C.infer_shapes_path(model_path, output_path, check_type, strict_mode);
        } else {
            throw new TypeError("infer_shapes_path only accepts model path (String), incorrect type: {}".format(type(model_path)));
        }
    }
    
    public static object InferenceError = C.InferenceError;
}
