using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.ENTITY.Models;
using Project.UI.Areas.CustomerPanel.Models;
using System.Data;
using System.Security.Claims;

namespace Project.UI.Areas.CustomerPanel.Controllers
{
    [Authorize(Roles = "Üye")]
    [Area("CustomerPanel")]
    public class ProfileController : Controller
    {
        UserRepository _uRep = new UserRepository();

        private readonly UserManager<User> _userManager;

        public ProfileController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userInfo = _uRep.Find(Convert.ToInt32(userId));

            UserEditVM vm = new UserEditVM
            {
                FullName = userInfo.FullName,
                Mail = userInfo.Email,
                Phone = userInfo.PhoneNumber
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Index(UserEditVM ueVM, [FromServices] IToastNotification toast)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userInfo = _uRep.Find(Convert.ToInt32(userId));

            if (ModelState.IsValid)
            {
                if (ueVM.FullName != userInfo.FullName || ueVM.Mail != userInfo.Email || ueVM.Phone != userInfo.PhoneNumber)
                {
                    userInfo.FullName = ueVM.FullName;
                    userInfo.Email = ueVM.Mail;
                    userInfo.PhoneNumber = ueVM.Phone;

                    _uRep.Update(userInfo);

                    toast.AddSuccessToastMessage("Kullanıcı bilgileri güncellendi.", new ToastrOptions { Title = "Başarılı!" });
                    return View(ueVM);
                }

                toast.AddInfoToastMessage("Kullanıcı bilgileri güncellenmedi.", new ToastrOptions { Title = "Bilgilendirme:" });
                return View(ueVM);
            }

            toast.AddErrorToastMessage("Kullanıcı bilgileri güncellenemedi.", new ToastrOptions { Title = "Başarısız!" });
            return View(ueVM);
        }

        public IActionResult Security()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userInfo = _uRep.Find(Convert.ToInt32(userId));

            UserPasswordVM upVM = new UserPasswordVM
            {
                FullName = userInfo.FullName,
                Mail = userInfo.Email,
                Phone = userInfo.PhoneNumber
            };

            return View(upVM);
        }

        [HttpPost]
        public async Task<IActionResult> Security(UserPasswordVM upVM, [FromServices] IToastNotification toast)
        {
            var user = await _userManager.FindByEmailAsync(upVM.Mail);

            if (ModelState.IsValid)
            {
                if (!await _userManager.CheckPasswordAsync(user, upVM.CurrentPassword))
                {
                    toast.AddErrorToastMessage("Mevcut şifrenizi yanlış girdiniz.", new ToastrOptions { Title = "Şifre Güncellenmedi." });
                    return RedirectToAction("Security");
                }

                var result = await _userManager.ChangePasswordAsync(user, upVM.CurrentPassword, upVM.Password);
                if (result.Succeeded)
                {
                    toast.AddSuccessToastMessage("Şifre Güncellendi.", new ToastrOptions { Title = "Başarılı!" });
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(upVM);
            }

            return View(upVM);
        }

    }
}
