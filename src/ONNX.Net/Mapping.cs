using NumpyDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Onnx
{
    public class Mapping
    {
        public static Dictionary<int, dtype> TENSOR_TYPE_TO_NP_TYPE = new Dictionary<int, dtype> {
        {
            Convert.ToInt32(TensorProto.Types.DataType.Float),
            np.Float32
            },
        {
            Convert.ToInt32(TensorProto.Types.DataType.Uint8),
            np.UInt8},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Int8),
            np.Int8},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Uint16),
            np.UInt16},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Int16),
            np.Int16},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Int32),
            np.Int32},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Int64),
            np.Int64},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Bool),
            np.Bool},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Float16),
            np.Float32},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Double),
            np.Float64},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Complex64),
            np.Complex},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Complex128),
            np.Complex},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Uint32),
            np.UInt32},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Uint64),
            np.UInt64},
        {
            Convert.ToInt32(TensorProto.Types.DataType.String),
            np.Object
            }
        };

        public static Dictionary<dtype, int> NP_TYPE_TO_TENSOR_TYPE = TENSOR_TYPE_TO_NP_TYPE.ToDictionary(_tup_2 => _tup_2.Value, _tup_2 => _tup_2.Key);

        public static Dictionary<int, int> TENSOR_TYPE_TO_STORAGE_TENSOR_TYPE = new Dictionary<int, int> {
        {
            Convert.ToInt32(TensorProto.Types.DataType.Float),
            Convert.ToInt32(TensorProto.Types.DataType.Float)},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Uint8),
            Convert.ToInt32(TensorProto.Types.DataType.Int32)},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Int8),
            Convert.ToInt32(TensorProto.Types.DataType.Int32)},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Uint16),
            Convert.ToInt32(TensorProto.Types.DataType.Int32)},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Int16),
            Convert.ToInt32(TensorProto.Types.DataType.Int32)},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Int32),
            Convert.ToInt32(TensorProto.Types.DataType.Int32)},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Int64),
            Convert.ToInt32(TensorProto.Types.DataType.Int64)},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Bool),
            Convert.ToInt32(TensorProto.Types.DataType.Int32)},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Float16),
            Convert.ToInt32(TensorProto.Types.DataType.Uint16)},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Bfloat16),
            Convert.ToInt32(TensorProto.Types.DataType.Uint16)},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Double),
            Convert.ToInt32(TensorProto.Types.DataType.Double)},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Complex64),
            Convert.ToInt32(TensorProto.Types.DataType.Float)},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Complex128),
            Convert.ToInt32(TensorProto.Types.DataType.Double)},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Uint32),
            Convert.ToInt32(TensorProto.Types.DataType.Uint32)},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Uint64),
            Convert.ToInt32(TensorProto.Types.DataType.Uint64)},
        {
            Convert.ToInt32(TensorProto.Types.DataType.String),
            Convert.ToInt32(TensorProto.Types.DataType.String)}};

        public static Dictionary<int, string> STORAGE_TENSOR_TYPE_TO_FIELD = new Dictionary<int, string> {
        {
            Convert.ToInt32(TensorProto.Types.DataType.Float),
            "float_data"},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Int32),
            "int32_data"},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Int64),
            "int64_data"},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Uint16),
            "int32_data"},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Double),
            "double_data"},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Complex64),
            "float_data"},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Complex128),
            "double_data"},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Uint32),
            "uint64_data"},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Uint64),
            "uint64_data"},
        {
            Convert.ToInt32(TensorProto.Types.DataType.String),
            "string_data"},
        {
            Convert.ToInt32(TensorProto.Types.DataType.Bool),
            "int32_data"}};

#if ONNX_ML
        public static Dictionary<int, string> STORAGE_ELEMENT_TYPE_TO_FIELD = new Dictionary<int, string> {
        {
            Convert.ToInt32(SequenceProto.Types.DataType.Tensor),
            "tensor_values"},
        {
            Convert.ToInt32(SequenceProto.Types.DataType.SparseTensor),
            "sparse_tensor_values"},
        {
            Convert.ToInt32(SequenceProto.Types.DataType.Sequence),
            "sequence_values"},
        {
            Convert.ToInt32(SequenceProto.Types.DataType.Map),
            "map_values"
            }
        };
#endif
    }

}
