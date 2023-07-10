using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.ENTITY.Models;
using Project.UI.Areas.Login.Models;

namespace Project.UI.Areas.Login.Controllers
{
    [AllowAnonymous]
    [Area("Login")]
    public class RegisterController : Controller
    {
        CompanyRepository _cRep = new CompanyRepository();

        private readonly UserManager<User> _userManager;

        public RegisterController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var Companies = new SelectList(_cRep.GetActives(), "ID", "CompanyName");

            if (Companies != null)
            {
                ViewBag.Companies = Companies;
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserRegisterViewModel urvm)
        {
            var Companies = new SelectList(_cRep.GetActives(), "ID", "CompanyName");

            if (Companies != null)
            {
                ViewBag.Companies = Companies;
            }

            if (ModelState.IsValid)
            {
                User u = new User()
                {
                    FullName = urvm.FullName,
                    Email = urvm.Mail,
                    UserName = urvm.UserName,
                    ImageUrl = urvm.ImageUrl,
                    CompanyID = urvm.CompanyID
                };


                if (urvm.ConfirmPassword == urvm.Password)
                {
                    var result = await _userManager.CreateAsync(u, urvm.Password);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Login", "Login");
                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                        return View();
                    }
                }
                return View();
            }
            return View();
        }
    }
}
