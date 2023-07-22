using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.ENTITY.Models;
using Project.UI.Areas.Auth.Models;

namespace Project.UI.Areas.Auth.Controllers
{
    [AllowAnonymous]
    [Area("Auth")]
    public class LoginController : Controller
    {
        UserRepository _uRep;

        private readonly SignInManager<User> _signInManager;

        public LoginController(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;

            _uRep = new UserRepository();
        }   

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserLoginViewModel ulvm)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(ulvm.UserName, ulvm.Password, true, true);
                if (result.Succeeded)
                {
                    var user = _uRep.Where(x => x.UserName == ulvm.UserName).FirstOrDefault();

                    var userId = user.Id;

                    var userManager = HttpContext.RequestServices.GetService<UserManager<User>>();
                    var currentuser = await userManager.FindByIdAsync(userId.ToString());
                    var userRoles = await userManager.GetRolesAsync(currentuser);

                    if (userRoles.Any(x=> x == "Üst Yönetici" || x == "Yönetici"))
                    {
                        return RedirectToAction("Index", "Dashboard", new { area = "UserPanel" });
                    }
                    else if (userRoles.Any(x=> x == "Üye"))
                    {
                        return RedirectToAction("Index", "Dashboard", new { area = "CustomerPanel" });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Lütfen rol için yöneticiniz ile görüşün");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı");
                }
            }
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Login", "Auth");
        }
    }
}
