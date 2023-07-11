using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Project.UI.Areas.CustomerPanel.Controllers
{
    [Authorize(Roles = "Member")]
    [Area("CustomerPanel")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
