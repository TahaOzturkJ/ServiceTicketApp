using System.ComponentModel.DataAnnotations;

namespace Project.UI.Areas.Auth.Models
{
    public class UserPasswordResetConfirmModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Token { get; set; }
        [Required, DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required, DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }
    }
}
