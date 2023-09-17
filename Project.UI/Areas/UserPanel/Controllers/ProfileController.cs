using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.UI.Areas.UserPanel.Models;
using System.Data;
using System.Security.Claims;

namespace Project.UI.Areas.UserPanel.Controllers
{
    [Authorize(Roles = "Üst Yönetici")]
    [Area("UserPanel")]
    public class ProfileController : Controller
    {
        UserRepository _uRep = new UserRepository();


        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userInfo = _uRep.Find(Convert.ToInt32(userId));

            string[] nameParts = userInfo.FullName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            UserEditVM vm = new UserEditVM
            {
                Surname = nameParts[nameParts.Length - 1],
                Name = nameParts[0],
                Mail = userInfo.Email,
                Phone = userInfo.PhoneNumber

            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult UpdateFields(string Name, string Surname, string Mail, string Phone)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userInfo = _uRep.Find(Convert.ToInt32(userId));

            userInfo.FullName = Name + " " + Surname;
            userInfo.Email = Mail;
            userInfo.PhoneNumber = Phone;

            _uRep.Update(userInfo);

            return RedirectToAction("Index");
        }
    }
}
