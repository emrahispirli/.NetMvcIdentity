using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorIdentity.Models.Identity
{
    public class RegisterModel
    {

        [Required(ErrorMessage = "{0} boş geçilemez."), DisplayName("Kullanıcı Adı :")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez."), DisplayName("Şifre :")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "{0} boş geçilemez."), DisplayName("Email :")]
        public string? Email { get; set; }
    }
}
