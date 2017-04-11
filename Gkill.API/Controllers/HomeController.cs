using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;

namespace Gkill.API.Controllers{
    public class HomeController : Controller{
        
        [HttpGet]
        public HttpResponseMessage Index()
        {
            var path = "~/Public/index.html";
            var response = new HttpResponseMessage();
            response.Content = new StringContent(System.IO.File.ReadAllText(path));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
    }
}