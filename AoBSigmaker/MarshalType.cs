/*
https://github.com/lolp1/Process.NET/blob/master/src/Process.NET/Marshaling/MarshalType.cs
https://github.com/ZenLulz/MemorySharp/blob/master/src/MemorySharp/Internals/MarshalType.cs
https://github.com/ZenLulz/MemorySharp/blob/master/src/MemorySharp/Memory/LocalUnmanagedMemory.cs
*/

namespace AoBSigmaker
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    ///     Static class providing tools for extracting information related to types.
    /// </summary>
    /// <typeparam name="T">Type to analyse.</typeparam>
    public static class MarshalType<T>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static information related to the specified type.
        /// </summary>
        static MarshalType()
        {
            // Gather information related to the provided type
            IsIntPtr = typeof(T) == typeof(IntPtr);
            RealType = typeof(T);
            Size = TypeCode == TypeCode.Boolean ? 1 : Marshal.SizeOf(RealType);
            TypeCode = Type.GetTypeCode(RealType);
            // Check if the type can be stored in registers
            if (IntPtr.Size == 8)
            {
                CanBeStoredInRegisters = Test();
            }
            else
            {
                CanBeStoredInRegisters = IsIntPtr || TypeCode == TypeCode.Boolean || TypeCode == TypeCode.Byte
                                         || TypeCode == TypeCode.Char || TypeCode == TypeCode.Int16
                                         || TypeCode == TypeCode.Int32 || TypeCode == TypeCode.Int64
                                         || TypeCode == TypeCode.SByte || TypeCode == TypeCode.Single
                                         || TypeCode == TypeCode.UInt16 || TypeCode == TypeCode.UInt32;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets if the type can be stored in a registers (for example ACX, ECX, ...).
        /// </summary>
        public static bool CanBeStoredInRegisters { get; }

        /// <summary>
        ///     State if the type is <see cref="IntPtr" />.
        /// </summary>
        public static bool IsIntPtr { get; }

        /// <summary>
        ///     The real type.
        /// </summary>
        public static Type RealType { get; }

        /// <summary>
        ///     The size of the type.
        /// </summary>
        public static int Size { get; }

        /// <summary>
        ///     The typecode of the type.
        /// </summary>
        public static TypeCode TypeCode { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Marshals an array of byte to a managed object.
        /// </summary>
        /// <param name="byteArray">The array of bytes corresponding to a managed object.</param>
        /// <returns>A managed object.</returns>
        public static T ByteArrayToObject(byte[] byteArray)
        {
            // We'll tried to avoid marshalling as it really slows the process
            // First, check if the type can be converted without marhsalling
            switch (TypeCode)
            {
                case TypeCode.Object:
                    if (IsIntPtr)
                    {
                        switch (byteArray.Length)
                        {
                            case 1:
                                return (T)(object)new IntPtr(
                                    BitConverter.ToInt32(new byte[] { byteArray[0], 0x0, 0x0, 0x0 }, 0));
                            case 2:
                                return (T)(object)new IntPtr(
                                    BitConverter.ToInt32(new byte[] { byteArray[0], byteArray[1], 0x0, 0x0 }, 0));
                            case 4:
                                return (T)(object)new IntPtr(BitConverter.ToInt32(byteArray, 0));
                            case 8:
                                return (T)(object)new IntPtr(BitConverter.ToInt64(byteArray, 0));
                        }
                    }
                    break;
                case TypeCode.Boolean:
                    return (T)(object)BitConverter.ToBoolean(byteArray, 0);
                case TypeCode.Byte:
                    return (T)(object)byteArray[0];
                case TypeCode.Char:
                    return (T)(object)Encoding.UTF8.GetChars(byteArray)[0]; //BitConverter.ToChar(byteArray, 0);
                case TypeCode.Double:
                    return (T)(object)BitConverter.ToDouble(byteArray, 0);
                case TypeCode.Int16:
                    return (T)(object)BitConverter.ToInt16(byteArray, 0);
                case TypeCode.Int32:
                    return (T)(object)BitConverter.ToInt32(byteArray, 0);
                case TypeCode.Int64:
                    return (T)(object)BitConverter.ToInt64(byteArray, 0);
                case TypeCode.Single:
                    return (T)(object)BitConverter.ToSingle(byteArray, 0);
                case TypeCode.String:
                    throw new InvalidCastException("This method doesn't support string conversion.");
                case TypeCode.UInt16:
                    return (T)(object)BitConverter.ToUInt16(byteArray, 0);
                case TypeCode.UInt32:
                    return (T)(object)BitConverter.ToUInt32(byteArray, 0);
                case TypeCode.UInt64:
                    return (T)(object)BitConverter.ToUInt64(byteArray, 0);
            }
            // Check if it's not a common type
            // Allocate a block of unmanaged memory
            using (var unmanaged = new LocalUnmanagedMemory(Size))
            {
                // Write the array of bytes inside the unmanaged memory
                unmanaged.Write(byteArray);
                // Return a managed object created from the block of unmanaged memory
                return unmanaged.Read<T>();
            }
        }

        /// <summary>
        ///     Marshals a managed object to an array of bytes.
        /// </summary>
        /// <param name="obj">The object to marshal.</param>
        /// <returns>A array of bytes corresponding to the managed object.</returns>
        public static byte[] ObjectToByteArray(T obj)
        {
            // We'll tried to avoid marshalling as it really slows the process
            // First, check if the type can be converted without marhsalling
            switch (TypeCode)
            {
                case TypeCode.Object:
                    if (IsIntPtr)
                    {
                        switch (Size)
                        {
                            case 4:
                                return BitConverter.GetBytes(((IntPtr)(object)obj).ToInt32());
                            case 8:
                                return BitConverter.GetBytes(((IntPtr)(object)obj).ToInt64());
                        }
                    }
                    break;
                case TypeCode.Boolean:
                    return BitConverter.GetBytes((bool)(object)obj);
                case TypeCode.Char:
                    return Encoding.UTF8.GetBytes(new[] { (char)(object)obj });
                case TypeCode.Double:
                    return BitConverter.GetBytes((double)(object)obj);
                case TypeCode.Int16:
                    return BitConverter.GetBytes((short)(object)obj);
                case TypeCode.Int32:
                    return BitConverter.GetBytes((int)(object)obj);
                case TypeCode.Int64:
                    return BitConverter.GetBytes((long)(object)obj);
                case TypeCode.Single:
                    return BitConverter.GetBytes((float)(object)obj);
                case TypeCode.String:
                    throw new InvalidCastException("This method doesn't support string conversion.");
                case TypeCode.UInt16:
                    return BitConverter.GetBytes((ushort)(object)obj);
                case TypeCode.UInt32:
                    return BitConverter.GetBytes((uint)(object)obj);
                case TypeCode.UInt64:
                    return BitConverter.GetBytes((ulong)(object)obj);
            }
            // Check if it's not a common type
            // Allocate a block of unmanaged memory
            using (var unmanaged = new LocalUnmanagedMemory(Size))
            {
                // Write the object inside the unmanaged memory
                unmanaged.Write(obj);
                // Return the content of the block of unmanaged memory
                return unmanaged.Read();
            }
        }

        /// <summary>
        ///     Converts a pointer to a given type. This function converts the value of the pointer or the pointed value,
        ///     according if the data type is primitive or reference.
        /// </summary>
        /// <param name="memorySharp">The concerned process.</param>
        /// <param name="pointer">The pointer to convert.</param>
        /// <returns>The return value is the pointer converted to the given data type.</returns>
        public static T PtrToObject(RemoteMemory memorySharp, IntPtr pointer)
        {
            return ByteArrayToObject(
                CanBeStoredInRegisters ? BitConverter.GetBytes(pointer.ToInt64()) : memorySharp.Read<byte>(pointer, Size));
        }

        #endregion

        #region Methods

        private static bool Test()
        {
            return IsIntPtr || TypeCode == TypeCode.Int64 || TypeCode == TypeCode.UInt64 || TypeCode == TypeCode.Boolean
                   || TypeCode == TypeCode.Byte || TypeCode == TypeCode.Char || TypeCode == TypeCode.Int16
                   || TypeCode == TypeCode.Int32 || TypeCode == TypeCode.Int64 || TypeCode == TypeCode.SByte
                   || TypeCode == TypeCode.Single || TypeCode == TypeCode.UInt16 || TypeCode == TypeCode.UInt32;
        }

        #endregion
    }

    public class LocalUnmanagedMemory : IDisposable
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LocalUnmanagedMemory" /> class, allocating a block of memory in the
        ///     local process.
        /// </summary>
        /// <param name="size">The size to allocate.</param>
        public LocalUnmanagedMemory(int size)
        {
            // Allocate the memory
            this.Size = size;
            this.Address = Marshal.AllocHGlobal(this.Size);
        }

        /// <summary>
        ///     Frees resources and perform other cleanup operations before it is reclaimed by garbage collection.
        /// </summary>
        ~LocalUnmanagedMemory()
        {
            this.Dispose();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     The address where the data is allocated.
        /// </summary>
        public IntPtr Address { get; private set; }

        /// <summary>
        ///     The size of the allocated memory.
        /// </summary>
        public int Size { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Releases the memory held by the <see cref="LocalUnmanagedMemory" /> object.
        /// </summary>
        public virtual void Dispose()
        {
            // Free the allocated memory
            Marshal.FreeHGlobal(this.Address);
            // Remove the pointer
            this.Address = IntPtr.Zero;
            // Avoid the finalizer
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Reads data from the unmanaged block of memory.
        /// </summary>
        /// <typeparam name="T">The type of data to return.</typeparam>
        /// <returns>The return value is the block of memory casted in the specified type.</returns>
        public T Read<T>()
        {
            // Marshal data from the block of memory to a new allocated managed object
            return (T)Marshal.PtrToStructure(this.Address, typeof(T));
        }

        /// <summary>
        ///     Reads an array of bytes from the unmanaged block of memory.
        /// </summary>
        /// <returns>The return value is the block of memory.</returns>
        public byte[] Read()
        {
            // Allocate an array to store data
            var bytes = new byte[this.Size];
            // Copy the block of memory to the array
            Marshal.Copy(this.Address, bytes, 0, this.Size);
            // Return the array
            return bytes;
        }

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return string.Format("Size = {0:X}", this.Size);
        }

        /// <summary>
        ///     Writes an array of bytes to the unmanaged block of memory.
        /// </summary>
        /// <param name="byteArray">The array of bytes to write.</param>
        /// <param name="index">The start position to copy bytes from.</param>
        public void Write(byte[] byteArray, int index = 0)
        {
            // Copy the array of bytes into the block of memory
            Marshal.Copy(byteArray, index, this.Address, this.Size);
        }

        /// <summary>
        ///     Write data to the unmanaged block of memory.
        /// </summary>
        /// <typeparam name="T">The type of data to write.</typeparam>
        /// <param name="data">The data to write.</param>
        public void Write<T>(T data)
        {
            // Marshal data from the managed object to the block of memory
            Marshal.StructureToPtr(data, this.Address, false);
        }

        #endregion
    }
}