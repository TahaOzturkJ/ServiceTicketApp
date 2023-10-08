using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.ENTITY.Models;
using Project.UI.Areas.UserPanel.Models;
using System.Security.Claims;

namespace Project.UI.Areas.UserPanel.Controllers
{
    [Authorize(Roles = "Üst Yönetici")]
    [Area("UserPanel")]
    public class RoleManagementController : Controller
    {
        UserRepository _uRep = new UserRepository();
        IdentityRoleRepository _rRep = new IdentityRoleRepository();
        IdentityUserRoleRepository _urRep = new IdentityUserRoleRepository();
        ServiceTicketRepository _stRep = new ServiceTicketRepository();

        private readonly UserManager<User> userManager;

        public RoleManagementController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {

            var stVM = new ServiceTicketVM();
            stVM.Users = _uRep.GetActives();

            foreach (var user in stVM.Users)
            {
                var roles = await userManager.GetRolesAsync(user);
                var userRoleInfo = new UserRoleInfo
                {
                    UserId = user.Id,
                    Roles = roles.ToList()
                };
                stVM.UserRoles.Add(userRoleInfo);
            }

            return View(stVM);
        }

        [HttpPost]
        public IActionResult ChangeRole(int userId, string roleName)
        {
            var user = _uRep.FirstOrDefault(x=>x.Id == userId);

            if (_urRep.Any(x => x.UserId == user.Id))
            {
                var currentRoles = _urRep.FirstOrDefault(x => x.UserId == user.Id);

                _urRep.Destroy(currentRoles);
            }

            var role = _rRep.Where(x => x.Name == roleName);

            IdentityUserRole iuRole = new IdentityUserRole()
            {
                UserId = userId,
                RoleId = role.First().Id
            };

            _urRep.Add(iuRole);

            return Json(new { success = true, message = "Kullanıcı rolü başarıyla güncellendi" });
        }


        public IActionResult DeleteUser(int id, [FromServices] IToastNotification toast)
        {
            var userValues = _uRep.Find(id);
            _uRep.Delete(userValues);
            var userTickets = _stRep.GetActives().Where(x => x.User.Id == id);
            _stRep.DeleteRange(userTickets);
            toast.AddErrorToastMessage("Kullanıcı silindi", new ToastrOptions { Title = "Başarılı!" });
            return RedirectToAction("Index");
        }

    }
}
