using System;
using System.Runtime.InteropServices;

namespace Onnx
{
    public class NativeMethods
    {
        private const string onnx_dll = "onnx.dll";

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SchemaVersionMap(string key, int value1, int value2);

        [DllImport(onnx_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CS_has_schema(string op_type, string domain);

        [DllImport(onnx_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int CS_Add(int a, int b, int c);

        [DllImport(onnx_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int CS_schema_version_map(SchemaVersionMap map);

        [DllImport(onnx_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern OpSchemaStruct CS_get_schema(string op_type, int max_inclusive_version, string domain);
    }
}
