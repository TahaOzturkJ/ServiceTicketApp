namespace Project.UI.Areas.UserPanel.Models
{
    public class UserEditVM
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile Image { get; set; }
    }
}
