using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorIdentity.Models.Identity
{
    public class LoginModel
    {
        [Required(ErrorMessage = "{0} boş geçilemez."), DisplayName("Email ")]
        [UIHint("email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez."), DisplayName("Password ")]
        [UIHint("password")]
        public string Password { get; set; }
    }
}
