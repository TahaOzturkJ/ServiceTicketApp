namespace Project.UI.Areas.UserPanel.Models
{
    public class UserViewModel
    {
        public string FullName { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile Image { get; set; }

    }
}
