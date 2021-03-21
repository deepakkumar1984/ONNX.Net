// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: onnx-data.proto3
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Onnx {

  /// <summary>Holder for reflection information generated from onnx-data.proto3</summary>
  public static partial class OnnxDataReflection {

    #region Descriptor
    /// <summary>File descriptor for onnx-data.proto3</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static OnnxDataReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChBvbm54LWRhdGEucHJvdG8zEgRvbm54Gg5vbm54LW1sLnByb3RvMyK0AgoN",
            "U2VxdWVuY2VQcm90bxIMCgRuYW1lGAEgASgJEhEKCWVsZW1fdHlwZRgCIAEo",
            "BRIoCg10ZW5zb3JfdmFsdWVzGAMgAygLMhEub25ueC5UZW5zb3JQcm90bxI1",
            "ChRzcGFyc2VfdGVuc29yX3ZhbHVlcxgEIAMoCzIXLm9ubnguU3BhcnNlVGVu",
            "c29yUHJvdG8SLAoPc2VxdWVuY2VfdmFsdWVzGAUgAygLMhMub25ueC5TZXF1",
            "ZW5jZVByb3RvEiIKCm1hcF92YWx1ZXMYBiADKAsyDi5vbm54Lk1hcFByb3Rv",
            "Ik8KCERhdGFUeXBlEg0KCVVOREVGSU5FRBAAEgoKBlRFTlNPUhABEhEKDVNQ",
            "QVJTRV9URU5TT1IQAhIMCghTRVFVRU5DRRADEgcKA01BUBAEInIKCE1hcFBy",
            "b3RvEgwKBG5hbWUYASABKAkSEAoIa2V5X3R5cGUYAiABKAUSDAoEa2V5cxgD",
            "IAMoAxITCgtzdHJpbmdfa2V5cxgEIAMoDBIjCgZ2YWx1ZXMYBSABKAsyEy5v",
            "bm54LlNlcXVlbmNlUHJvdG9CAkgDYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Onnx.OnnxMlReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Onnx.SequenceProto), global::Onnx.SequenceProto.Parser, new[]{ "Name", "ElemType", "TensorValues", "SparseTensorValues", "SequenceValues", "MapValues" }, null, new[]{ typeof(global::Onnx.SequenceProto.Types.DataType) }, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Onnx.MapProto), global::Onnx.MapProto.Parser, new[]{ "Name", "KeyType", "Keys", "StringKeys", "Values" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  /// Sequences
  ///
  /// Defines a dense, ordered, collection of elements that are of homogeneous types.
  /// Sequences can be made out of tensors, maps, or sequences.
  ///
  /// If a sequence is made out of tensors, the tensors must have the same element
  /// type (i.e. int32). In some cases, the tensors in a sequence can have different
  /// shapes.  Whether the tensors can have different shapes or not depends on the
  /// type/shape associated with the corresponding "ValueInfo". For example,
  /// "Sequence&lt;Tensor&lt;float, [M,N]>" means that all tensors have same shape. However,
  /// "Sequence&lt;Tensor&lt;float, [omitted,omitted]>" means they can have different
  /// shapes (all of rank 2), where "omitted" means the corresponding dimension has
  /// no symbolic/constant value. Finally, "Sequence&lt;Tensor&lt;float, omitted>>" means
  /// that the different tensors can have different ranks, when the "shape" itself
  /// is omitted from the tensor-type. For a more complete description, refer to
  /// https://github.com/onnx/onnx/blob/master/docs/IR.md#static-tensor-shapes.
  /// </summary>
  public sealed partial class SequenceProto : pb::IMessage<SequenceProto> {
    private static readonly pb::MessageParser<SequenceProto> _parser = new pb::MessageParser<SequenceProto>(() => new SequenceProto());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<SequenceProto> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Onnx.OnnxDataReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SequenceProto() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SequenceProto(SequenceProto other) : this() {
      name_ = other.name_;
      elemType_ = other.elemType_;
      tensorValues_ = other.tensorValues_.Clone();
      sparseTensorValues_ = other.sparseTensorValues_.Clone();
      sequenceValues_ = other.sequenceValues_.Clone();
      mapValues_ = other.mapValues_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SequenceProto Clone() {
      return new SequenceProto(this);
    }

    /// <summary>Field number for the "name" field.</summary>
    public const int NameFieldNumber = 1;
    private string name_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Name {
      get { return name_; }
      set {
        name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "elem_type" field.</summary>
    public const int ElemTypeFieldNumber = 2;
    private int elemType_;
    /// <summary>
    /// The data type of the element.
    /// This field MUST have a valid SequenceProto.DataType value
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ElemType {
      get { return elemType_; }
      set {
        elemType_ = value;
      }
    }

    /// <summary>Field number for the "tensor_values" field.</summary>
    public const int TensorValuesFieldNumber = 3;
    private static readonly pb::FieldCodec<global::Onnx.TensorProto> _repeated_tensorValues_codec
        = pb::FieldCodec.ForMessage(26, global::Onnx.TensorProto.Parser);
    private readonly pbc::RepeatedField<global::Onnx.TensorProto> tensorValues_ = new pbc::RepeatedField<global::Onnx.TensorProto>();
    /// <summary>
    /// For TensorProto values.
    /// When this field is present, the elem_type field MUST be TENSOR.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Onnx.TensorProto> TensorValues {
      get { return tensorValues_; }
    }

    /// <summary>Field number for the "sparse_tensor_values" field.</summary>
    public const int SparseTensorValuesFieldNumber = 4;
    private static readonly pb::FieldCodec<global::Onnx.SparseTensorProto> _repeated_sparseTensorValues_codec
        = pb::FieldCodec.ForMessage(34, global::Onnx.SparseTensorProto.Parser);
    private readonly pbc::RepeatedField<global::Onnx.SparseTensorProto> sparseTensorValues_ = new pbc::RepeatedField<global::Onnx.SparseTensorProto>();
    /// <summary>
    /// For SparseTensorProto values.
    /// When this field is present, the elem_type field MUST be SPARSE_TENSOR.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Onnx.SparseTensorProto> SparseTensorValues {
      get { return sparseTensorValues_; }
    }

    /// <summary>Field number for the "sequence_values" field.</summary>
    public const int SequenceValuesFieldNumber = 5;
    private static readonly pb::FieldCodec<global::Onnx.SequenceProto> _repeated_sequenceValues_codec
        = pb::FieldCodec.ForMessage(42, global::Onnx.SequenceProto.Parser);
    private readonly pbc::RepeatedField<global::Onnx.SequenceProto> sequenceValues_ = new pbc::RepeatedField<global::Onnx.SequenceProto>();
    /// <summary>
    /// For SequenceProto values, allowing sequences to be of themselves.
    /// When this field is present, the elem_type field MUST be SEQUENCE.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Onnx.SequenceProto> SequenceValues {
      get { return sequenceValues_; }
    }

    /// <summary>Field number for the "map_values" field.</summary>
    public const int MapValuesFieldNumber = 6;
    private static readonly pb::FieldCodec<global::Onnx.MapProto> _repeated_mapValues_codec
        = pb::FieldCodec.ForMessage(50, global::Onnx.MapProto.Parser);
    private readonly pbc::RepeatedField<global::Onnx.MapProto> mapValues_ = new pbc::RepeatedField<global::Onnx.MapProto>();
    /// <summary>
    /// For MapProto values.
    /// When this field is present, the elem_type field MUST be MAP.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Onnx.MapProto> MapValues {
      get { return mapValues_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as SequenceProto);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(SequenceProto other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Name != other.Name) return false;
      if (ElemType != other.ElemType) return false;
      if(!tensorValues_.Equals(other.tensorValues_)) return false;
      if(!sparseTensorValues_.Equals(other.sparseTensorValues_)) return false;
      if(!sequenceValues_.Equals(other.sequenceValues_)) return false;
      if(!mapValues_.Equals(other.mapValues_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Name.Length != 0) hash ^= Name.GetHashCode();
      if (ElemType != 0) hash ^= ElemType.GetHashCode();
      hash ^= tensorValues_.GetHashCode();
      hash ^= sparseTensorValues_.GetHashCode();
      hash ^= sequenceValues_.GetHashCode();
      hash ^= mapValues_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Name.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Name);
      }
      if (ElemType != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(ElemType);
      }
      tensorValues_.WriteTo(output, _repeated_tensorValues_codec);
      sparseTensorValues_.WriteTo(output, _repeated_sparseTensorValues_codec);
      sequenceValues_.WriteTo(output, _repeated_sequenceValues_codec);
      mapValues_.WriteTo(output, _repeated_mapValues_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Name.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Name);
      }
      if (ElemType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(ElemType);
      }
      size += tensorValues_.CalculateSize(_repeated_tensorValues_codec);
      size += sparseTensorValues_.CalculateSize(_repeated_sparseTensorValues_codec);
      size += sequenceValues_.CalculateSize(_repeated_sequenceValues_codec);
      size += mapValues_.CalculateSize(_repeated_mapValues_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(SequenceProto other) {
      if (other == null) {
        return;
      }
      if (other.Name.Length != 0) {
        Name = other.Name;
      }
      if (other.ElemType != 0) {
        ElemType = other.ElemType;
      }
      tensorValues_.Add(other.tensorValues_);
      sparseTensorValues_.Add(other.sparseTensorValues_);
      sequenceValues_.Add(other.sequenceValues_);
      mapValues_.Add(other.mapValues_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            Name = input.ReadString();
            break;
          }
          case 16: {
            ElemType = input.ReadInt32();
            break;
          }
          case 26: {
            tensorValues_.AddEntriesFrom(input, _repeated_tensorValues_codec);
            break;
          }
          case 34: {
            sparseTensorValues_.AddEntriesFrom(input, _repeated_sparseTensorValues_codec);
            break;
          }
          case 42: {
            sequenceValues_.AddEntriesFrom(input, _repeated_sequenceValues_codec);
            break;
          }
          case 50: {
            mapValues_.AddEntriesFrom(input, _repeated_mapValues_codec);
            break;
          }
        }
      }
    }

    #region Nested types
    /// <summary>Container for nested types declared in the SequenceProto message type.</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static partial class Types {
      public enum DataType {
        [pbr::OriginalName("UNDEFINED")] Undefined = 0,
        [pbr::OriginalName("TENSOR")] Tensor = 1,
        [pbr::OriginalName("SPARSE_TENSOR")] SparseTensor = 2,
        [pbr::OriginalName("SEQUENCE")] Sequence = 3,
        [pbr::OriginalName("MAP")] Map = 4,
      }

    }
    #endregion

  }

  /// <summary>
  /// Maps
  ///
  /// Specifies an associative table, defined by keys and values.
  /// MapProto is formed with a repeated field of keys (of type INT8, INT16, INT32,
  /// INT64, UINT8, UINT16, UINT32, UINT64, or STRING) and values (of type TENSOR,
  /// SPARSE_TENSOR, SEQUENCE, or MAP). Key types and value types have to remain
  /// the same throughout the instantiation of the MapProto.
  /// </summary>
  public sealed partial class MapProto : pb::IMessage<MapProto> {
    private static readonly pb::MessageParser<MapProto> _parser = new pb::MessageParser<MapProto>(() => new MapProto());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<MapProto> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Onnx.OnnxDataReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MapProto() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MapProto(MapProto other) : this() {
      name_ = other.name_;
      keyType_ = other.keyType_;
      keys_ = other.keys_.Clone();
      stringKeys_ = other.stringKeys_.Clone();
      values_ = other.values_ != null ? other.values_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MapProto Clone() {
      return new MapProto(this);
    }

    /// <summary>Field number for the "name" field.</summary>
    public const int NameFieldNumber = 1;
    private string name_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Name {
      get { return name_; }
      set {
        name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "key_type" field.</summary>
    public const int KeyTypeFieldNumber = 2;
    private int keyType_;
    /// <summary>
    /// The data type of the key.
    /// This field MUST have a valid TensorProto.DataType value of
    /// INT8, INT16, INT32, INT64, UINT8, UINT16, UINT32, UINT64, or STRING
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int KeyType {
      get { return keyType_; }
      set {
        keyType_ = value;
      }
    }

    /// <summary>Field number for the "keys" field.</summary>
    public const int KeysFieldNumber = 3;
    private static readonly pb::FieldCodec<long> _repeated_keys_codec
        = pb::FieldCodec.ForInt64(26);
    private readonly pbc::RepeatedField<long> keys_ = new pbc::RepeatedField<long>();
    /// <summary>
    /// Every element of keys has to be one of the following data types
    /// INT8, INT16, INT32, INT64, UINT8, UINT16, UINT32, UINT64, or STRING.
    /// The integer cases are represented by the repeated int64 field keys below.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<long> Keys {
      get { return keys_; }
    }

    /// <summary>Field number for the "string_keys" field.</summary>
    public const int StringKeysFieldNumber = 4;
    private static readonly pb::FieldCodec<pb::ByteString> _repeated_stringKeys_codec
        = pb::FieldCodec.ForBytes(34);
    private readonly pbc::RepeatedField<pb::ByteString> stringKeys_ = new pbc::RepeatedField<pb::ByteString>();
    /// <summary>
    /// If keys are strings, they are represented by the repeated bytes field
    /// string_keys below.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<pb::ByteString> StringKeys {
      get { return stringKeys_; }
    }

    /// <summary>Field number for the "values" field.</summary>
    public const int ValuesFieldNumber = 5;
    private global::Onnx.SequenceProto values_;
    /// <summary>
    /// MapProto values are represented in a SequenceProto of the same length as the
    /// repeated keys field and have to be one of the following data types
    /// TENSOR, SPARSE_TENSOR, MAP, SEQUENCE.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Onnx.SequenceProto Values {
      get { return values_; }
      set {
        values_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as MapProto);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(MapProto other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Name != other.Name) return false;
      if (KeyType != other.KeyType) return false;
      if(!keys_.Equals(other.keys_)) return false;
      if(!stringKeys_.Equals(other.stringKeys_)) return false;
      if (!object.Equals(Values, other.Values)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Name.Length != 0) hash ^= Name.GetHashCode();
      if (KeyType != 0) hash ^= KeyType.GetHashCode();
      hash ^= keys_.GetHashCode();
      hash ^= stringKeys_.GetHashCode();
      if (values_ != null) hash ^= Values.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Name.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Name);
      }
      if (KeyType != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(KeyType);
      }
      keys_.WriteTo(output, _repeated_keys_codec);
      stringKeys_.WriteTo(output, _repeated_stringKeys_codec);
      if (values_ != null) {
        output.WriteRawTag(42);
        output.WriteMessage(Values);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Name.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Name);
      }
      if (KeyType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(KeyType);
      }
      size += keys_.CalculateSize(_repeated_keys_codec);
      size += stringKeys_.CalculateSize(_repeated_stringKeys_codec);
      if (values_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Values);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(MapProto other) {
      if (other == null) {
        return;
      }
      if (other.Name.Length != 0) {
        Name = other.Name;
      }
      if (other.KeyType != 0) {
        KeyType = other.KeyType;
      }
      keys_.Add(other.keys_);
      stringKeys_.Add(other.stringKeys_);
      if (other.values_ != null) {
        if (values_ == null) {
          Values = new global::Onnx.SequenceProto();
        }
        Values.MergeFrom(other.Values);
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            Name = input.ReadString();
            break;
          }
          case 16: {
            KeyType = input.ReadInt32();
            break;
          }
          case 26:
          case 24: {
            keys_.AddEntriesFrom(input, _repeated_keys_codec);
            break;
          }
          case 34: {
            stringKeys_.AddEntriesFrom(input, _repeated_stringKeys_codec);
            break;
          }
          case 42: {
            if (values_ == null) {
              Values = new global::Onnx.SequenceProto();
            }
            input.ReadMessage(Values);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
