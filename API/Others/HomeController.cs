using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Others
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        //[Authorize]
        //public string Secret()
        //{

        //    return "Secret Message from ApiOne";
        //}
    }
}
