using GKill.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GKill.Controllers
{
    public class ProcessesController: Controller
    {
        public ActionResult Index(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = string.Empty;
            }

            var runningProcesses = Process.GetProcesses();
            var processesViews = runningProcesses.Select(p => new ProcessViewModel
                    {
                        Name = p.ProcessName.ToUpper(),
                        Id = p.Id
                    })
                .OrderBy(p => p.Id)
                .Where(p => p.Name.Contains(name));

            return View(processesViews);
        }
    }
}
