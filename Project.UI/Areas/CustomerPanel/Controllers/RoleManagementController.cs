
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Project.UI.Areas.CustomerPanel.Controllers
{
    [Authorize(Roles = "Üye")]
    [Area("CustomerPanel")]
    public class RoleManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
