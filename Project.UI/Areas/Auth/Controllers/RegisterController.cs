using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using NToastNotify;
using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.BLL.EmailSender.Email;
using Project.BLL.EmailSender.IEmail;
using Project.ENTITY.Models;
using Project.UI.Areas.Auth.Models;

namespace Project.UI.Areas.Auth.Controllers
{
    [AllowAnonymous]
    [Area("Auth")]
    public class RegisterController : Controller
    {
        CompanyRepository _cRep = new CompanyRepository();

        private readonly UserManager<User> _userManager;

        public RegisterController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserRegisterViewModel urvm, [FromServices] IToastNotification toast)
        {

            if (ModelState.IsValid)
            {
                User u = new User();

                u.FullName = urvm.FullName;
                u.Email = urvm.Mail;
                u.UserName = urvm.UserName;

                int startIndex = urvm.Mail.IndexOf('@') + 1;
                int endIndex = urvm.Mail.IndexOf('.', startIndex);
                string domain = urvm.Mail.Substring(startIndex, endIndex - startIndex).ToUpperInvariant();

                var companies = _cRep.GetActives();
                var matchedCompany = companies.FirstOrDefault(x => x.CompanyName.Replace(" ", "").ToUpperInvariant() == domain);

                if (matchedCompany != null)
                {
                    u.CompanyID = matchedCompany.ID;

                    if (urvm.ConfirmPassword == urvm.Password)
                    {
                        var result = await _userManager.CreateAsync(u, urvm.Password);

                        if (result.Succeeded)
                        {
                            toast.AddSuccessToastMessage("Kullanıcı Oluşturuldu!.", new ToastrOptions { Title = "Başarılı!" });
                            return RedirectToAction("Index", "Login", "Auth");
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
                }
                else {

                    ModelState.AddModelError("", "Maalesef size hizmet verememekteyiz");

                }

                return View();

            }

            return View();

        }
    }
}
