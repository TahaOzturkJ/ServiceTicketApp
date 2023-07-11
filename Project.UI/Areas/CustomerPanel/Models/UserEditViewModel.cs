namespace Project.UI.Areas.CustomerPanel.Models
{
    public class UserEditViewModel
    {
        public string FullName { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile Image { get; set; }

    }
}
