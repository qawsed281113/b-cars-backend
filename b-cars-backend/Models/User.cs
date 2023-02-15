using Microsoft.AspNetCore.Identity;

namespace b_cars_backend.Models;

public class User: IdentityUser<int>
{
    public User()
    {
        Cars = new HashSet<Car>();
    }
    public virtual ICollection<Car> Cars { get; set; }
}