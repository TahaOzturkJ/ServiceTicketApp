using System.ComponentModel.DataAnnotations;

namespace Project.UI.Areas.CustomerPanel.Models
{
    public class UserEditVM
    {
        [Required(ErrorMessage = "Lütfen İsim Soyisim Girin")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Lütfen Mail Adresinizi Girin")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "Lütfen Telefon Numaranızı Girin")]
        public string Phone { get; set; }

        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
    }
}
