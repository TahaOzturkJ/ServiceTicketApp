using System.ComponentModel.DataAnnotations;

namespace Project.UI.Areas.Auth.Models
{
    public class UserPasswordResetModel
    {
        [Display(Name = "Mail Adresi")]
        [Required]
        public string Email { get; set; }
    }
}
