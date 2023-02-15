using b_cars_backend.Configuration;

namespace b_cars_backend.Auth;

public class RegistrationResponse : AuthResult
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
}