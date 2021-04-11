using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TestPointer
{
    class LevelName
    {
        const int PROCESS_WM_READ = 0x0010;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        private static int ADDRESS_POINTER = 0x2DCAF0;
        private static int[] LEVELNAME1_OFFSETS = { 0x174, 0x24, 0xF4 };
        private static int[] LEVELNAME2_OFFSETS = { 0x174, 0x24, 0xF4, 0x0 };

        static void Main()
        {
            Process pbp = Process.GetProcessesByName("penumbra")[0];
            IntPtr processHandle = OpenProcess(PROCESS_WM_READ, false, pbp.Id);

            IntPtr pointer = IntPtr.Add(pbp.Modules[0].BaseAddress, ADDRESS_POINTER);
            Console.WriteLine("Pointer Address is 0x" + Convert.ToString((int)pointer, 16));

            int newAddr = (int)pointer;


            int bytesRead = 0;
            byte[] buffer = new byte[7];
            ReadProcessMemory((int)processHandle, newAddr, buffer, buffer.Length, ref bytesRead);
            int baseAddr = BitConverter.ToInt32(buffer, 0);
            Console.WriteLine("Base Address is 0x" + Convert.ToString(baseAddr, 16));

            IntPtr Offset1 = IntPtr.Add((IntPtr)baseAddr, LEVELNAME2_OFFSETS[0]);
            ReadProcessMemory((int)processHandle, (int)Offset1, buffer, buffer.Length, ref bytesRead);
            int Off1 = BitConverter.ToInt32(buffer, 0);
            Console.WriteLine("Offset address 1 is 0x" + Convert.ToString(Off1, 16));

            IntPtr Offset2 = IntPtr.Add((IntPtr)Off1, LEVELNAME2_OFFSETS[1]);
            ReadProcessMemory((int)processHandle, (int)Offset2, buffer, buffer.Length, ref bytesRead);
            int Off2 = BitConverter.ToInt32(buffer, 0);
            Console.WriteLine("Offset address 2 is 0x" + Convert.ToString(Off2, 16));

            IntPtr Offset3 = IntPtr.Add((IntPtr)Off2, LEVELNAME2_OFFSETS[2]);
            ReadProcessMemory((int)processHandle, (int)Offset3, buffer, buffer.Length, ref bytesRead);
            int Off3 = BitConverter.ToInt32(buffer, 0);
            Console.WriteLine("Offset address 3 is 0x" + Convert.ToString(Off3, 16));

            Console.WriteLine("");

            Level:
                ReadProcessMemory((int)processHandle, Off3, buffer, buffer.Length, ref bytesRead);
                Console.Write("\rMap name is " + Encoding.UTF8.GetString(buffer));
                System.Threading.Thread.Sleep(30);
            goto Level;
        }

    }
}
