using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Gkill.API.Models;

namespace Gkill.API.Controllers{

    [Route("api/[controller]")]
    public class ProcessesController : Controller{

        [HttpGet]
        public IEnumerable<ProcessModel> Index(){
            return Utils.ProccessManager.All();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id){
            int resultStatus = Utils.ProccessManager.KillProcess(id);
            if(resultStatus == Utils.ProccessManager.PROCESS_TERMINATED){
                return NoContent();
            }
            else if(resultStatus == Utils.ProccessManager.FAILED_TO_TERMINATE){
                return Forbid();
            }
            else{
                return NotFound();
            }
        }
    }
}