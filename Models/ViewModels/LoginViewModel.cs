using System.ComponentModel.DataAnnotations;

namespace WebsiteTour.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email hoặc Username là bắt buộc")]
        public string EmailOrUsername { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
