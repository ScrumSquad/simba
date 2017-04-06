using System.Diagnostics;
using System.Collections.Generic;

namespace GKill.Models{
    public static class ProcessModel{
        public static Process[] GetProcesses(){
            return Process.GetProcesses();

        }
    }
}