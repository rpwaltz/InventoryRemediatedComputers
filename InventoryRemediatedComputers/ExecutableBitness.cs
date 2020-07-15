using System;
using System.Runtime.InteropServices;


// https://stackoverflow.com/questions/3782191/how-do-i-determine-if-a-net-application-is-32-or-64-bit
// http://zuga.net/articles/cs-how-to-determine-if-a-program-process-or-file-is-32-bit-or-64-bit/#getbinarytype

namespace InventoryRemediatedComputers
    {
    class ExecutableBitness
        {


        public Boolean isInstalledProgramX64(string filePath)
            {


            BinaryType bt;
            if (!GetBinaryType(filePath, out bt))
                return false;

            if (bt.Equals(BinaryType.SCS_64BIT_BINARY))
                {
                return true;
                }
            else
                {
                return false;
                }
            }
        public Boolean isInstalledProgramX32(string filePath)
            {


            BinaryType bt;
            if (!GetBinaryType(filePath, out bt))
                return false;
            if (bt.Equals(BinaryType.SCS_32BIT_BINARY))
                {
                return true;
                }
            else
                {
                return false;
                }
            }

        [DllImport("kernel32.dll")]
        static extern bool GetBinaryType(string lpApplicationName, out BinaryType lpBinaryType);

        public enum BinaryType : uint
            {
            SCS_32BIT_BINARY = 0,   // A 32-bit Windows-based application
            SCS_64BIT_BINARY = 6,   // A 64-bit Windows-based application.
            SCS_DOS_BINARY = 1,     // An MS-DOS � based application
            SCS_OS216_BINARY = 5,   // A 16-bit OS/2-based application
            SCS_PIF_BINARY = 3,     // A PIF file that executes an MS-DOS � based application
            SCS_POSIX_BINARY = 4,   // A POSIX � based application
            SCS_WOW_BINARY = 2      // A 16-bit Windows-based application
            }
        }
    }
