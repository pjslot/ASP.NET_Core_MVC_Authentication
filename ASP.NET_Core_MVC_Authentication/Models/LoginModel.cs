using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Core_MVC_Authentication.Models
{
    public class LoginModel
    {
        [Required]
        [UIHint("email@@@")]
        public string Email { get; set; }
        [Required]
        [UIHint("password***")]
        public string Password { get; set; }
    }
}
