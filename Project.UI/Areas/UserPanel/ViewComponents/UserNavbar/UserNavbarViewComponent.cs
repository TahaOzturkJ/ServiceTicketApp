using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.ENTITY.Models;
using Project.UI.Areas.UserPanel.Models;

namespace Project.UI.Areas.UserPanel.ViewComponents.UserNavbar
{
    [ViewComponent]
    public class UserNavbarViewComponent : ViewComponent
    {
        private readonly UserManager<User> _userManager;

        public UserNavbarViewComponent(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            UserEditViewModel model = new UserEditViewModel();
            model.FullName = values.FullName;
            model.ImageUrl = values.ImageUrl;

            return View(model);
        }
    }
}
