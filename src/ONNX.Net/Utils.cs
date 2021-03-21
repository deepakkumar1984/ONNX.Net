using System;
using System.IO;

namespace Onnx
{
    public class Utils
    {
        public static void ExtractModel(string input_path, string output_path, string[] input_names,
            string[] output_names)
        {
            /*Extracts sub-model from an ONNX model.
            The sub-model is defined by the names of the input and output tensors *exactly *.
            Note: For control-flow operators, e.g.If and Loop, the _boundary of sub-model_,
            which is defined by the input and output tensors, should not _cut through_ the
            subgraph that is connected to the _main graph_ as attributes of these operators.
            Arguments:
                input_path(string): The path to original ONNX model.
                output_path(string): The path to save the extracted ONNX model.
                input_names(list of string): The names of the input tensors that to be extracted.
                output_names(list of string): The names of the output tensors that to be extracted.
            */

            if (!File.Exists(input_path))
                throw new FileNotFoundException("Invalid input model path: " + input_path);

            if (string.IsNullOrWhiteSpace(output_path))
                throw new ArgumentException("Output model path shall not be empty!");

            if (output_names == null)
                throw new ArgumentException("Output tensor names shall not be empty!");

            Checker.CheckModel(input_path);
            var model = onnx.Load(input_path);

            var e = new Extractor(model);
            var extracted = e.ExtractModel(input_names, output_names);

            onnx.SaveModel(extracted, output_path);
            Checker.CheckModel(output_path);
        }
    }
}