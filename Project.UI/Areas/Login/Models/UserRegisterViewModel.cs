using Project.ENTITY.Enums;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace Project.UI.Areas.Login.Models
{
    public class UserRegisterViewModel
    {
        [Required(ErrorMessage = "Lütfen isminizi girin")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Lütfen kullanıcı adı girin")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Lütfen şifreyi girin")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Lütfen şifreyi tekrar girin")]
        [Compare("Password", ErrorMessage = "Şifreler uyumlu değil")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Lütfen şirket seçin")]
        public int CompanyID { get; set; }

        [Required(ErrorMessage = "Lütfen mail girin")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "Lütfen fotoğraf adresini girin")]
        public string ImageUrl { get; set; }
    }
}
