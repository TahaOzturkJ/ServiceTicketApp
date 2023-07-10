using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Project.UI.Areas.UserPanel.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    [Area("UserPanel")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
