using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Gkill.API.Models;

namespace Gkill.API.Utils{
    public static class ProccessManager{


        public static int PROCESS_TERMINATED = 0;
        public static int PROCESS_NOT_RUNNING = 1;
        public static int FAILED_TO_TERMINATE = 2;

        public static IEnumerable<ProcessModel> All(){
            return Process.GetProcesses()
            .Select(p => new ProcessModel{
                ProcessID = p.Id,
                ProcessName = p.ProcessName,
                Memory = p.VirtualMemorySize64
            });
        }

        public static int KillProcess(int processId){
            try{
                var process = Process.GetProcessById(processId);
                process.Kill();
                return PROCESS_TERMINATED;
            }catch(ArgumentException){
                return PROCESS_NOT_RUNNING;
            }catch(Win32Exception){
                return FAILED_TO_TERMINATE;
            }
        }
    }
}