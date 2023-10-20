using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Project.UI.Areas.Auth.Controllers
{
    [AllowAnonymous]
    [Area("Auth")]
    public class ErrorController : Controller
    {
        public IActionResult Error(string errCode)
        {
            if (errCode == "500" || errCode == "404" || errCode == "403")
            {
                return View($"/Error/{errCode}");
            }

            return View();
        }

    }
}
