using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TestPointer
{
    class Program
    {
        const int PROCESS_WM_READ = 0x0010;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess,
          int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);
        static void Main()
        {
            Process pbp = Process.GetProcessesByName("penumbra")[0];
            IntPtr processHandle = OpenProcess(PROCESS_WM_READ, false, pbp.Id);
            int bytesRead = 0;
            byte[] buffer = new byte[200];

            int baseAddress = pbp.MainModule.BaseAddress.ToInt32() + 0x2DCAF0;
            ReadProcessMemory((int)processHandle, baseAddress, buffer, buffer.Length, ref bytesRead);
            int baseValue = BitConverter.ToInt32(buffer, 0);

            int firstAddress = baseValue + 0x174;
            ReadProcessMemory((int)processHandle, baseAddress + 0x174, buffer, buffer.Length, ref bytesRead);
            int firstValue = BitConverter.ToInt32(buffer, 0);

            int secondAddress = firstValue + 0x24;
            ReadProcessMemory((int)processHandle, secondAddress, buffer, buffer.Length, ref bytesRead);
            int secondValue = BitConverter.ToInt32(buffer, 0);

            int thirdAddress = secondValue + 0xF4;
            ReadProcessMemory((int)processHandle, thirdAddress, buffer, buffer.Length, ref bytesRead);
            int thirdValue = BitConverter.ToInt32(buffer, 0);

            int fourthAddress = thirdValue + 0x0;
            ReadProcessMemory((int)processHandle, fourthAddress, buffer, buffer.Length, ref bytesRead);

            Console.WriteLine(Encoding.UTF8.GetString(buffer) +
               " (" + bytesRead.ToString() + "bytes)");
            Console.ReadLine();
        }

    }
}
