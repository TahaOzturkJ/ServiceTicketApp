using Microsoft.AspNetCore.Mvc;

namespace Project.UI.Areas.Auth.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error404()
        {
            return View();
        }
    }
}
