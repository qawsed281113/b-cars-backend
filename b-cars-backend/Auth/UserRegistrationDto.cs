using System.ComponentModel.DataAnnotations;

namespace b_cars_backend.Auth
{
    public class UserRegistration
    {
        [Required] public string UserName { get; set; }
        [Required] public string Phone { get; set; }
        [Required] [EmailAddress] public string Email { get; set; }
        [Required] public string Password { get; set; }
    }
}