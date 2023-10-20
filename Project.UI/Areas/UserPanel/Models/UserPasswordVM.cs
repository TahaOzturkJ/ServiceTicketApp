using System.ComponentModel.DataAnnotations;

namespace Project.UI.Areas.UserPanel.Models
{
    public class UserPasswordVM
    {
        public string? FullName { get; set; }

        public string? Mail { get; set; }

        public string? Phone { get; set; }

        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Mevcut şifrenizi giriniz")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Yeni şifrenizi giriniz")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Yeni şifrenizi tekrar giriniz")]
        [Compare("Password", ErrorMessage = "Şifreler uyumlu değil")]
        public string ConfirmPassword { get; set; }
    }
}
