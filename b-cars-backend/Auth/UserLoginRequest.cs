using System.ComponentModel.DataAnnotations;

namespace b_cars_backend.Auth
{
    public class UserLoginRequest
    {
        [Required]
        [EmailAddress] 
        public string Email { get; set; }
        [Required] 
        public string Password { get; set; }
    }
}