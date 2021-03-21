using System;
using System.Collections.Generic;
using System.Text;

namespace Onnx
{
    public class VersionRowType
    {
        public string ReleaseVersion { get; set; }
        public int IRVersion { get; set; }
        public int OnnxVersion { get; set; }
        public int OnnxMlVersion { get; set; }
        public int? TrainingVersion { get; set; }

        public VersionRowType(string release_version, int ir_version, int onnx_version, int onnx_ml_version, int? tranining_version = null)
        {
            ReleaseVersion = release_version;
            IRVersion = ir_version;
            OnnxVersion = onnx_version;
            OnnxMlVersion = onnx_ml_version;
            TrainingVersion = tranining_version;
        }
    }
}
