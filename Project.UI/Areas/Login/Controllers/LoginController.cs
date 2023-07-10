using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.ENTITY.Models;
using Project.UI.Areas.Login.Models;
using System.Runtime.CompilerServices;

namespace Project.UI.Areas.Login.Controllers
{
    [AllowAnonymous]
    [Area("Login")]
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
                    var values = _uRep.Where(x => x.UserName == ulvm.UserName).ToArray();


                    return RedirectToAction("Index", "Dashboard", new {area = "UserPanel"});
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
            return RedirectToAction("Index", "Login", "Login");
        }
    }
}
