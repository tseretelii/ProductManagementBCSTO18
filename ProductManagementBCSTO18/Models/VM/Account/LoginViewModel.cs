using System.ComponentModel.DataAnnotations;

namespace ProductManagementBCSTO18.Models.VM.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "აუცილებელი ველი")]
        public string Email { get; set; }

        [Required(ErrorMessage = "აუცილებელი ველი")]
        public string Password { get; set; }
        public bool RememberMe {  get; set; }
    }
}
