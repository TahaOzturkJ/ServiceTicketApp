using System.ComponentModel.DataAnnotations;

namespace Project.UI.Areas.Auth.Models
{
    public class UserLoginViewModel
    {
        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "Kullanıcı adını giriniz")]
        public string UserName { get; set; }

        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Şifreyi giriniz")]
        public string Password { get; set; }
    }
}
