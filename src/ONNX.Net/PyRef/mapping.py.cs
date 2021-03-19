
using @absolute_import = @@__future__.absolute_import;

using @division = @@__future__.division;

using @print_function = @@__future__.print_function;

using @unicode_literals = @@__future__.unicode_literals;

using TensorProto = onnx.TensorProto;

using SequenceProto = onnx.SequenceProto;

using Text = typing.Text;

using Any = typing.Any;

using np = numpy;

using System.Collections.Generic;

using System;

public static class mapping {
    
    public static Dictionary<int, object> TENSOR_TYPE_TO_NP_TYPE = new Dictionary<object, object> {
        {
            Convert.ToInt32(TensorProto.FLOAT),
            np.dtype("float32")},
        {
            Convert.ToInt32(TensorProto.UINT8),
            np.dtype("uint8")},
        {
            Convert.ToInt32(TensorProto.INT8),
            np.dtype("int8")},
        {
            Convert.ToInt32(TensorProto.UINT16),
            np.dtype("uint16")},
        {
            Convert.ToInt32(TensorProto.INT16),
            np.dtype("int16")},
        {
            Convert.ToInt32(TensorProto.INT32),
            np.dtype("int32")},
        {
            Convert.ToInt32(TensorProto.INT64),
            np.dtype("int64")},
        {
            Convert.ToInt32(TensorProto.BOOL),
            np.dtype("bool")},
        {
            Convert.ToInt32(TensorProto.FLOAT16),
            np.dtype("float16")},
        {
            Convert.ToInt32(TensorProto.DOUBLE),
            np.dtype("float64")},
        {
            Convert.ToInt32(TensorProto.COMPLEX64),
            np.dtype("complex64")},
        {
            Convert.ToInt32(TensorProto.COMPLEX128),
            np.dtype("complex128")},
        {
            Convert.ToInt32(TensorProto.UINT32),
            np.dtype("uint32")},
        {
            Convert.ToInt32(TensorProto.UINT64),
            np.dtype("uint64")},
        {
            Convert.ToInt32(TensorProto.STRING),
            np.dtype(np.object)}};
    
    public static Dictionary<object, object> NP_TYPE_TO_TENSOR_TYPE = TENSOR_TYPE_TO_NP_TYPE.items().ToDictionary(_tup_2 => _tup_2.Item2, _tup_2 => _tup_2.Item1);
    
    public static Dictionary<int, int> TENSOR_TYPE_TO_STORAGE_TENSOR_TYPE = new Dictionary<object, object> {
        {
            Convert.ToInt32(TensorProto.FLOAT),
            Convert.ToInt32(TensorProto.FLOAT)},
        {
            Convert.ToInt32(TensorProto.UINT8),
            Convert.ToInt32(TensorProto.INT32)},
        {
            Convert.ToInt32(TensorProto.INT8),
            Convert.ToInt32(TensorProto.INT32)},
        {
            Convert.ToInt32(TensorProto.UINT16),
            Convert.ToInt32(TensorProto.INT32)},
        {
            Convert.ToInt32(TensorProto.INT16),
            Convert.ToInt32(TensorProto.INT32)},
        {
            Convert.ToInt32(TensorProto.INT32),
            Convert.ToInt32(TensorProto.INT32)},
        {
            Convert.ToInt32(TensorProto.INT64),
            Convert.ToInt32(TensorProto.INT64)},
        {
            Convert.ToInt32(TensorProto.BOOL),
            Convert.ToInt32(TensorProto.INT32)},
        {
            Convert.ToInt32(TensorProto.FLOAT16),
            Convert.ToInt32(TensorProto.UINT16)},
        {
            Convert.ToInt32(TensorProto.BFLOAT16),
            Convert.ToInt32(TensorProto.UINT16)},
        {
            Convert.ToInt32(TensorProto.DOUBLE),
            Convert.ToInt32(TensorProto.DOUBLE)},
        {
            Convert.ToInt32(TensorProto.COMPLEX64),
            Convert.ToInt32(TensorProto.FLOAT)},
        {
            Convert.ToInt32(TensorProto.COMPLEX128),
            Convert.ToInt32(TensorProto.DOUBLE)},
        {
            Convert.ToInt32(TensorProto.UINT32),
            Convert.ToInt32(TensorProto.UINT32)},
        {
            Convert.ToInt32(TensorProto.UINT64),
            Convert.ToInt32(TensorProto.UINT64)},
        {
            Convert.ToInt32(TensorProto.STRING),
            Convert.ToInt32(TensorProto.STRING)}};
    
    public static Dictionary<int, string> STORAGE_TENSOR_TYPE_TO_FIELD = new Dictionary<object, object> {
        {
            Convert.ToInt32(TensorProto.FLOAT),
            "float_data"},
        {
            Convert.ToInt32(TensorProto.INT32),
            "int32_data"},
        {
            Convert.ToInt32(TensorProto.INT64),
            "int64_data"},
        {
            Convert.ToInt32(TensorProto.UINT16),
            "int32_data"},
        {
            Convert.ToInt32(TensorProto.DOUBLE),
            "double_data"},
        {
            Convert.ToInt32(TensorProto.COMPLEX64),
            "float_data"},
        {
            Convert.ToInt32(TensorProto.COMPLEX128),
            "double_data"},
        {
            Convert.ToInt32(TensorProto.UINT32),
            "uint64_data"},
        {
            Convert.ToInt32(TensorProto.UINT64),
            "uint64_data"},
        {
            Convert.ToInt32(TensorProto.STRING),
            "string_data"},
        {
            Convert.ToInt32(TensorProto.BOOL),
            "int32_data"}};
    
    public static Dictionary<int, string> STORAGE_ELEMENT_TYPE_TO_FIELD = new Dictionary<object, object> {
        {
            Convert.ToInt32(SequenceProto.TENSOR),
            "tensor_values"},
        {
            Convert.ToInt32(SequenceProto.SPARSE_TENSOR),
            "sparse_tensor_values"},
        {
            Convert.ToInt32(SequenceProto.SEQUENCE),
            "sequence_values"},
        {
            Convert.ToInt32(SequenceProto.MAP),
            "map_values"}};
}
