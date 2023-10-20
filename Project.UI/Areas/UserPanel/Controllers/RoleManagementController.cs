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
    [Authorize(Roles = "Üst Yönetici,Yönetici")]
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
            var loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var loggedUserRole = _urRep.FirstOrDefault(x=>x.UserId == Convert.ToInt32(loggedUserId));

            var user = _uRep.FirstOrDefault(x => x.Id == userId);

            var userRole = _urRep.FirstOrDefault(x => x.UserId == userId);

            var role = _rRep.Where(x => x.Name == roleName);

            if (Convert.ToInt32(loggedUserId) == userId)
            {
                return Json(new { success = false, message = "Kendi rolünüzü güncelleyemezsiniz." });
            }

            if (userRole == null)
            {
                if (loggedUserRole.RoleId < role.First().Id)
                {
                    IdentityUserRole iuRole = new IdentityUserRole()
                    {
                        UserId = userId,
                        RoleId = role.First().Id
                    };

                    _urRep.Add(iuRole);

                    return Json(new { success = true, message = "Kullanıcı rolü başarıyla güncellendi" });
                }
                return Json(new { success = false, message = "Kullanıcı rolünü güncellemek için yetkiniz yetersiz" });
            }

            if (loggedUserRole.RoleId < userRole.RoleId && loggedUserRole.RoleId < role.First().Id)
            {
                if (_urRep.Any(x => x.UserId == user.Id))
                {
                    var currentRoles = _urRep.FirstOrDefault(x => x.UserId == user.Id);

                    _urRep.Destroy(currentRoles);
                }

                IdentityUserRole iuRole = new IdentityUserRole()
                {
                    UserId = userId,
                    RoleId = role.First().Id
                };

                _urRep.Add(iuRole);

                return Json(new { success = true, message = "Kullanıcı rolü başarıyla güncellendi" });
            }

            return Json(new { success = false, message = "Kullanıcının rolünü güncellemek için yetkiniz yetersiz" });

        }


        public IActionResult DeleteUser(int id, [FromServices] IToastNotification toast)
        {
            var loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var loggedUserRole = _urRep.FirstOrDefault(x => x.UserId == Convert.ToInt32(loggedUserId));

            var user = _uRep.FirstOrDefault(x => x.Id == id);

            var userRole = _urRep.FirstOrDefault(x => x.UserId == id);

            if (userRole == null)
            {
                var userValues = _uRep.Find(id);
                _uRep.Delete(userValues);

                toast.AddErrorToastMessage("Kullanıcı silindi", new ToastrOptions { Title = "Başarılı!" });
                return RedirectToAction("Index");
            }

            if (loggedUserRole.RoleId < userRole.RoleId)
            {
                var userValues = _uRep.Find(id);
                _uRep.Delete(userValues);

                if (_stRep.Any(x => x.User.Id == id && x.DataStatus != ENTITY.Enums.DataStatus.Silinmiş))
                {
                    var userTickets = _stRep.GetActives().Where(x => x.User.Id == id);
                    _stRep.DeleteRange(userTickets);
                }

                toast.AddErrorToastMessage("Kullanıcı silindi", new ToastrOptions { Title = "Başarılı!" });
                return RedirectToAction("Index");
            }

            toast.AddErrorToastMessage("Yetkiniz bu kullanıcıyı silmek için yetersiz", new ToastrOptions { Title = "Başarısız!" });
            return RedirectToAction("Index");
        }

    }
}
