using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary
{
    public class MyLibrary
    {
        public static string updateRAM()
        {
            Process[] processes = Process.GetProcesses();
            long memoryUse = 0;

            foreach (Process p in processes)
            {
                memoryUse += p.PagedMemorySize64 / (1024 * 1024);
            }

            return memoryUse.ToString() + " Мб";
        }
    }
}
