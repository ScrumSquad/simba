using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Gkill.API.Controllers{

    [Route("[controller]")]
    public class HomeController : Controller{
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(IHostingEnvironment hostingEnvironment){
            _hostingEnvironment = hostingEnvironment;
        }
        
        [HttpGet]
        public string Index()
        {
            /*var path = "\\Public\\index.html";
            var response = new HttpResponseMessage();
            response.Content = new StringContent(System.IO.File.ReadAllText(path));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;*/
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            var filePath = Path.GetFullPath(Path.Combine(contentRootPath, "Public\\index.html"));
            Response.Headers.Add("Content-Type", "text/html");

            return System.IO.File.ReadAllText(filePath);
        }
    }
}