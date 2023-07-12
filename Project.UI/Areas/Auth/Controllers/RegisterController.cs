using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
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

            if (ModelState.IsValid)
            {
                User u = new User();

                if (urvm.Image != null && Path.GetExtension(urvm.Image.FileName) != ".svg")
                {
                    var resource = Directory.GetCurrentDirectory();
                    var extension = Path.GetExtension(urvm.Image.FileName);
                    var imagename = Guid.NewGuid() + extension;
                    var savelocation = resource + "/wwwroot/UserImage/" + imagename;
                    var stream = new FileStream(savelocation, FileMode.Create);
                    await urvm.Image.CopyToAsync(stream);
                    u.ImageUrl = "/UserImage/" + imagename;
                }


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
                }
                else {
                    ModelState.AddModelError("", "Maalesef size hizmet verememekteyiz");
                }

                if (urvm.ConfirmPassword == urvm.Password)
                {
                    var result = await _userManager.CreateAsync(u, urvm.Password);

                    if (result.Succeeded)
                    {
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
                return View();
            }
            return View();
        }
    }
}
