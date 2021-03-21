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
            Convert.ToInt32(TensorProto.DataType.Float),
            np.Float32
            },
        {
            Convert.ToInt32(TensorProto.DataType.Uint8),
            np.UInt8},
        {
            Convert.ToInt32(TensorProto.DataType.Int8),
            np.Int8},
        {
            Convert.ToInt32(TensorProto.DataType.Uint16),
            np.UInt16},
        {
            Convert.ToInt32(TensorProto.DataType.Int16),
            np.Int16},
        {
            Convert.ToInt32(TensorProto.DataType.Int32),
            np.Int32},
        {
            Convert.ToInt32(TensorProto.DataType.Int64),
            np.Int64},
        {
            Convert.ToInt32(TensorProto.DataType.Bool),
            np.Bool},
        {
            Convert.ToInt32(TensorProto.DataType.Float16),
            np.Float32},
        {
            Convert.ToInt32(TensorProto.DataType.Double),
            np.Float64},
        {
            Convert.ToInt32(TensorProto.DataType.Complex64),
            np.Complex},
        {
            Convert.ToInt32(TensorProto.DataType.Complex128),
            np.Complex},
        {
            Convert.ToInt32(TensorProto.DataType.Uint32),
            np.UInt32},
        {
            Convert.ToInt32(TensorProto.DataType.Uint64),
            np.UInt64},
        {
            Convert.ToInt32(TensorProto.DataType.String),
            np.Object
            }
        };

        public static Dictionary<dtype, int> NP_TYPE_TO_TENSOR_TYPE = TENSOR_TYPE_TO_NP_TYPE.ToDictionary(_tup_2 => _tup_2.Value, _tup_2 => _tup_2.Key);

        public static Dictionary<int, int> TENSOR_TYPE_TO_STORAGE_TENSOR_TYPE = new Dictionary<int, int> {
        {
            Convert.ToInt32(TensorProto.DataType.Float),
            Convert.ToInt32(TensorProto.DataType.Float)},
        {
            Convert.ToInt32(TensorProto.DataType.Uint8),
            Convert.ToInt32(TensorProto.DataType.Int32)},
        {
            Convert.ToInt32(TensorProto.DataType.Int8),
            Convert.ToInt32(TensorProto.DataType.Int32)},
        {
            Convert.ToInt32(TensorProto.DataType.Uint16),
            Convert.ToInt32(TensorProto.DataType.Int32)},
        {
            Convert.ToInt32(TensorProto.DataType.Int16),
            Convert.ToInt32(TensorProto.DataType.Int32)},
        {
            Convert.ToInt32(TensorProto.DataType.Int32),
            Convert.ToInt32(TensorProto.DataType.Int32)},
        {
            Convert.ToInt32(TensorProto.DataType.Int64),
            Convert.ToInt32(TensorProto.DataType.Int64)},
        {
            Convert.ToInt32(TensorProto.DataType.Bool),
            Convert.ToInt32(TensorProto.DataType.Int32)},
        {
            Convert.ToInt32(TensorProto.DataType.Float16),
            Convert.ToInt32(TensorProto.DataType.Uint16)},
        {
            Convert.ToInt32(TensorProto.DataType.Bfloat16),
            Convert.ToInt32(TensorProto.DataType.Uint16)},
        {
            Convert.ToInt32(TensorProto.DataType.Double),
            Convert.ToInt32(TensorProto.DataType.Double)},
        {
            Convert.ToInt32(TensorProto.DataType.Complex64),
            Convert.ToInt32(TensorProto.DataType.Float)},
        {
            Convert.ToInt32(TensorProto.DataType.Complex128),
            Convert.ToInt32(TensorProto.DataType.Double)},
        {
            Convert.ToInt32(TensorProto.DataType.Uint32),
            Convert.ToInt32(TensorProto.DataType.Uint32)},
        {
            Convert.ToInt32(TensorProto.DataType.Uint64),
            Convert.ToInt32(TensorProto.DataType.Uint64)},
        {
            Convert.ToInt32(TensorProto.DataType.String),
            Convert.ToInt32(TensorProto.DataType.String)}};

        public static Dictionary<int, string> STORAGE_TENSOR_TYPE_TO_FIELD = new Dictionary<int, string> {
        {
            Convert.ToInt32(TensorProto.DataType.Float),
            "floatdatas"},
        {
            Convert.ToInt32(TensorProto.DataType.Int32),
            "int32datas"},
        {
            Convert.ToInt32(TensorProto.DataType.Int64),
            "int64datas"},
        {
            Convert.ToInt32(TensorProto.DataType.Uint16),
            "int32datas"},
        {
            Convert.ToInt32(TensorProto.DataType.Double),
            "doubledatas"},
        {
            Convert.ToInt32(TensorProto.DataType.Complex64),
            "floatdatas"},
        {
            Convert.ToInt32(TensorProto.DataType.Complex128),
            "doubledatas"},
        {
            Convert.ToInt32(TensorProto.DataType.Uint32),
            "uint64datas"},
        {
            Convert.ToInt32(TensorProto.DataType.Uint64),
            "uint64datas"},
        {
            Convert.ToInt32(TensorProto.DataType.String),
            "stringdatas"},
        {
            Convert.ToInt32(TensorProto.DataType.Bool),
            "int32datas"}};

        public static Dictionary<int, string> STORAGE_ELEMENT_TYPE_TO_FIELD = new Dictionary<int, string> {
        {
            Convert.ToInt32(SequenceProto.DataType.Tensor),
            "tensorvalues"},
        {
            Convert.ToInt32(SequenceProto.DataType.SparseTensor),
            "sparsetensorvalues"},
        {
            Convert.ToInt32(SequenceProto.DataType.Sequence),
            "sequencevalues"},
        {
            Convert.ToInt32(SequenceProto.DataType.Map),
            "mapvalues"}};
    }
}
