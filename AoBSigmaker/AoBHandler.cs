/** 
 * sigScan C# Implementation - Written by atom0s [aka Wiccaan] 
 *          
 * [ CREDITS ] ---------------------------------------------------------------------------- 
 * 
 *      sigScan is based on the FindPattern code written by 
 *      dom1n1k and Patrick at GameDeception.net 
 *      
 *      Full credit to them for the purpose of this code. I, atom0s, simply 
 *      take credit for converting it to C#. 
 * 
 */

namespace AoBSigmaker
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;

    public class AoBHandler
    {
        #region Fields

        /// <summary>
        ///     m_vAddress
        ///     The starting address we want to begin reading at.
        /// </summary>
        private IntPtr mVAddress;

        /// <summary>
        ///     m_vDumpedRegion
        ///     The memory dumped from the external process.
        /// </summary>
        private byte[] mVDumpedRegion;

        /// <summary>
        ///     m_vProcess
        ///     The process we want to read the memory of.
        /// </summary>
        private Process mVProcess;

        /// <summary>
        ///     m_vSize
        ///     The number of bytes we wish to read from the process.
        /// </summary>
        private int mVSize;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     AoBHandler
        ///     class constructor that sets the class
        ///     properties during construction.
        /// </summary>
        /// <param name="proc">The process to dump the memory from.</param>
        /// <param name="addr">The started address to begin the dump.</param>
        /// <param name="size">The size of the dump.</param>
        public AoBHandler(Process proc, IntPtr addr, int size)
        {
            this.mVProcess = proc;
            this.mVAddress = addr;
            this.mVSize = size;
        }

        #endregion

        #region Public Properties

        public IntPtr Address
        {
            get
            {
                return this.mVAddress;
            }

            set
            {
                this.mVAddress = value;
            }
        }

        public Process Process
        {
            get
            {
                return this.mVProcess;
            }

            set
            {
                this.mVProcess = value;
            }
        }

        public int Size
        {
            get
            {
                return this.mVSize;
            }

            set
            {
                this.mVSize = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     FindPattern
        ///     Attempts to locate the given pattern inside the dumped memory region
        ///     compared against the given mask. If the pattern is found, the offset
        ///     is added to the located address and returned to the user.
        /// </summary>
        /// <param name="btPattern">Byte pattern to look for in the dumped region.</param>
        /// <param name="strMask">The mask string to compare against.</param>
        /// <param name="nOffset">The offset added to the result address.</param>
        /// <returns>IntPtr - zero if not found, address if found.</returns>
        public IntPtr FindPattern(byte[] btPattern, string strMask, int nOffset)
        {
            try
            {
                // Dump the memory region if we have not dumped it yet. 
                if (this.mVDumpedRegion == null || this.mVDumpedRegion.Length == 0)
                {
                    if (!this.DumpMemory()) return IntPtr.Zero;
                }

                // Ensure the mask and pattern lengths match. 
                if (strMask.Length != btPattern.Length) return IntPtr.Zero;

                // Loop the region and look for the pattern. 
                for (var x = 0; x < this.mVDumpedRegion.Length; x++)
                {
                    if (this.MaskCheck(x, btPattern, strMask))
                    {
                        // The pattern was found, return it. 
                        return new IntPtr((int)this.mVAddress + (x + nOffset));
                    }
                }

                // Pattern was not found. 
                return IntPtr.Zero;
            }
            catch (Exception)
            {
                return IntPtr.Zero;
            }
        }

        public IntPtr FindPattern(string pattern)
        {
            var sig = pattern;
            var byteSig = GetBytePattern(sig);
            var byteMask = GetMaskFromPattern(sig);

            return this.FindPattern(byteSig, byteMask, 0);
        }

        public static string GenerateSigFromAoBs(string[] aobs)
        {
            var patternWorking = new List<string>();
            var lastPattern = new List<string>();
            var temp =
                (from aob in aobs
                 where !aob.StartsWith("##") && aob != string.Empty
                 select Regex.Replace(aob, @"\s+", string.Empty)).ToList();

            var checkedAoBs =
                temp.Select(aob => aob.Length > temp[0].Length ? aob.Remove(temp[0].Length) : aob).ToList();

            foreach (var pattern in checkedAoBs)
            {
                var loopPattern =
                    Enumerable.Range(0, pattern.Length / 2).Select(i => pattern.Substring(i * 2, 2)).ToArray();

                for (var i = 0; i < loopPattern.Length; i++)
                {
                    if (i + 1 > lastPattern.Count || !lastPattern.Any() || loopPattern[i] == string.Empty)
                    {
                        continue;
                    }

                    if (i + 1 <= patternWorking.Count && patternWorking[i] == "??")
                    {
                        continue;
                    }

                    var currentByte = loopPattern[i].ToLower();
                    var lastByte = lastPattern[i].ToLower();
                    var newByte = i + 1 <= patternWorking.Count ? patternWorking[i] : string.Empty;

                    if (currentByte == lastByte)
                    {
                        if (i + 1 <= patternWorking.Count)
                        {
                            patternWorking.RemoveAt(i);
                            patternWorking.Insert(i, loopPattern[i]);
                        }
                        else
                        {
                            patternWorking.Add(loopPattern[i]);
                        }
                    }

                    if (currentByte != lastByte && i + 1 > patternWorking.Count)
                    {
                        patternWorking.Add("??");
                    }

                    if (i + 1 <= patternWorking.Count)
                    {
                        if (currentByte != lastByte && currentByte != newByte)
                        {
                            patternWorking.RemoveAt(i);
                            patternWorking.Insert(i, "??");
                        }
                    }
                }

                lastPattern = loopPattern.ToList();
            }

            return patternWorking.Aggregate<string, string>(null, (current, @by) => current + (@by.ToUpper() + " "));
        }

        public static byte[] GetBytePattern(string pattern, string wildcard = "??", string replaceTo = "00")
        {
            var reg = Regex.Replace(pattern, @"\s+", string.Empty).Replace(wildcard, replaceTo);
            return
                Enumerable.Range(0, reg.Length)
                    .Where(x => x % 2 == 0)
                    .Select(x => Convert.ToByte(reg.Substring(x, 2), 16))
                    .ToArray();
        }

        public static string GetMaskFromPattern(string pattern, string wildcard = "??")
        {
            var reg = Regex.Replace(pattern, @"\s+", string.Empty);
            var splitThis = Enumerable.Range(0, reg.Length / 2).Select(i => reg.Substring(i * 2, 2)).ToArray();
            var mask = string.Empty;
            foreach (var by in splitThis)
            {
                if (by == string.Empty)
                {
                    continue;
                }

                if (by == wildcard)
                {
                    mask += "?";
                }
                else
                {
                    mask += "x";
                }
            }

            return mask;
        }

        /// <summary>
        ///     ResetRegion
        ///     Resets the memory dump array to nothing to allow
        ///     the class to redump the memory.
        /// </summary>
        public void ResetRegion()
        {
            this.mVDumpedRegion = null;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     ReadProcessMemory
        ///     API import definition for ReadProcessMemory.
        /// </summary>
        /// <param name="hProcess">Handle to the process we want to read from.</param>
        /// <param name="lpBaseAddress">The base address to start reading from.</param>
        /// <param name="lpBuffer">The return buffer to write the read data to.</param>
        /// <param name="dwSize">The size of data we wish to read.</param>
        /// <param name="lpNumberOfBytesRead">The number of bytes successfully read.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            [Out] byte[] lpBuffer,
            int dwSize,
            out int lpNumberOfBytesRead);

        /// <summary>
        ///     DumpMemory
        ///     Internal memory dump function that uses the set class
        ///     properties to dump a memory region.
        /// </summary>
        /// <returns>Boolean based on RPM results and valid properties.</returns>
        private bool DumpMemory()
        {
            try
            {
                // Checks to ensure we have valid data. 
                if (this.mVProcess == null) return false;
                if (this.mVProcess.HasExited) return false;
                if (this.mVAddress == IntPtr.Zero) return false;
                if (this.mVSize == 0) return false;

                // Create the region space to dump into. 
                this.mVDumpedRegion = new byte[this.mVSize];

                int nBytesRead;

                // Dump the memory. 
                var ret = ReadProcessMemory(
                    this.mVProcess.Handle,
                    this.mVAddress,
                    this.mVDumpedRegion,
                    this.mVSize,
                    out nBytesRead);

                // Validation checks. 
                return ret && nBytesRead == this.mVSize;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     MaskCheck
        ///     Compares the current pattern byte to the current memory dump
        ///     byte to check for a match. Uses wildcards to skip bytes that
        ///     are deemed unneeded in the compares.
        /// </summary>
        /// <param name="nOffset">Offset in the dump to start at.</param>
        /// <param name="btPattern">Pattern to scan for.</param>
        /// <param name="strMask">Mask to compare against.</param>
        /// <returns>Boolean depending on if the pattern was found.</returns>
        private bool MaskCheck(int nOffset, IEnumerable<byte> btPattern, string strMask)
        {
            // Loop the pattern and compare to the mask and dump. 
            return
                !btPattern.Where(
                    (t, x) => strMask[x] != '?' && ((strMask[x] == 'x') && (t != this.mVDumpedRegion[nOffset + x])))
                     .Any();

            // The loop was successful so we found the pattern. 
        }

        #endregion
    }
}