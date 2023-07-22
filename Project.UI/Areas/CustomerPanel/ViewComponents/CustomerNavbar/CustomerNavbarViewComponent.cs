using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.ENTITY.Models;
using Project.UI.Areas.CustomerPanel.Models;

namespace Project.UI.Areas.CustomerPanel.ViewComponents.CustomerNavbar
{
    [ViewComponent]
    public class CustomerNavbarViewComponent : ViewComponent
    {
        private readonly UserManager<User> _userManager;

        public CustomerNavbarViewComponent(UserManager<User> userManager)
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
