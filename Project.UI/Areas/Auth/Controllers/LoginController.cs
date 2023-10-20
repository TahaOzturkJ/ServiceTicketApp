using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.BLL.EmailSender.IEmail;
using Project.ENTITY.Models;
using Project.UI.Areas.Auth.Models;
using System;
using System.Text;

namespace Project.UI.Areas.Auth.Controllers
{
    [AllowAnonymous]
    [Area("Auth")]
    public class LoginController : Controller
    {
        UserRepository _uRep;

        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public LoginController(SignInManager<User> signInManager, UserManager<User> userManager, IEmailSender emailSender, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _configuration = configuration;

            _uRep = new UserRepository();
        }

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

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(UserPasswordResetModel model, [FromServices] IToastNotification toast)
        {
            if (!ModelState.IsValid)
            {
                toast.AddErrorToastMessage("Şifre yenileme talebi oluşturulamadı.", new ToastrOptions { Title = "Başarısız!" });
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                toast.AddErrorToastMessage("Bu mail adresine kayıtlı kullanıcı bulunamadı.", new ToastrOptions { Title = "Başarısız!" });
                return View(model);
            }

            var appUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var resetUrl = $"{appUrl}/Auth/Login/ResetPasswordConfirm?uid={user.Id}&token={token}";

            var receiver = model.Email;

            var subject = "BBS Ticket sistemindeki üyeliğiniz için şifre sıfırlama talebi";

            var htmlBody = new StringBuilder();
            htmlBody.AppendLine("<html><body>");
            htmlBody.AppendLine("<p>Şifrenizi sıfırlamak için aşağıdaki linke tıklayın:</p>");
            htmlBody.AppendLine($"<p><a href=\"{resetUrl}\">Şifreyi Sıfırla</a></p>");
            htmlBody.AppendLine("</body></html>");

            var message = htmlBody.ToString();

            await _emailSender.SendEmailAsync(receiver, subject, message);

            toast.AddSuccessToastMessage("Şifre yenileme talebi iletildi.", new ToastrOptions { Title = "Başarılı!" });
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirm(string uid, string token)
        {
            UserPasswordResetConfirmModel uprcm = new UserPasswordResetConfirmModel
            {
                Token = token,
                UserId = uid,
            };

            return View(uprcm);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordConfirm(UserPasswordResetConfirmModel model, [FromServices] IToastNotification toast)
        {
            model.Token = model.Token.Replace(' ', '+');

            var result = await _userManager.ResetPasswordAsync(await _userManager.FindByIdAsync(model.UserId), model.Token, model.NewPassword);

            if (result.Succeeded)
            {
                toast.AddSuccessToastMessage("Şifre başarıyla güncellendi.", new ToastrOptions { Title = "Başarılı!" });
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Login", "Auth");
        }
    }
}
