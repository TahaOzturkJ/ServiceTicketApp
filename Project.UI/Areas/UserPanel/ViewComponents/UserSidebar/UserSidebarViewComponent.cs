using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.ENTITY.Models;
using Project.UI.Areas.UserPanel.Models;

namespace Project.UI.Areas.UserPanel.ViewComponents.UserSidebar
{
    [ViewComponent]
    public class UserSidebarViewComponent : ViewComponent
    {
        private readonly UserManager<User> _userManager;

        public UserSidebarViewComponent(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            UserViewModel model = new UserViewModel();
            model.FullName = values.FullName;
            model.ImageUrl = values.ImageUrl;

            return View(model);
        }
    }
}
