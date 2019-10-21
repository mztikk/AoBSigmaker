namespace AoBSigmaker
{
    public enum MemoryType
    {
        None,
        Byte,
        SByte,
        Short,
        UShort,
        Int,
        Uint,
        Long,
        ULong,
        Float,
        Double,
        IntPtr,
        String
    }

    public static class MemoryTypeHelper
    {
        public static string ToString(MemoryType t) => t switch
        {
            MemoryType.Byte => "Byte",
            MemoryType.SByte => $"Byte Signed ({t})",
            MemoryType.Short => $"2 Bytes Signed ({t})",
            MemoryType.UShort => $"2 Bytes Unsigned ({t})",
            MemoryType.Int => $"4 Bytes Signed ({t})",
            MemoryType.Uint => $"4 Bytes Unsigned ({t})",
            MemoryType.Long => $"8 Bytes Signed ({t})",
            MemoryType.ULong => $"8 Bytes Unsigned ({t})",
            MemoryType.Float => $"4 Bytes Float ({t})",
            MemoryType.Double => $"8 Bytes Float ({t})",
            MemoryType.IntPtr => $"Pointer ({t})",
            MemoryType.String => "String",
            _ => t.ToString(),
        };
    }
}
