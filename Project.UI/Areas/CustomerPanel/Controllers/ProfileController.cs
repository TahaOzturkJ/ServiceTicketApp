using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Project.UI.Areas.CustomerPanel.Controllers
{
    [Authorize(Roles = "Üye")]
    [Area("CustomerPanel")]
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
