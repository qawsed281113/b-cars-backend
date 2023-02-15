using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace b_cars_backend.Models;

public class CarDbContext : IdentityDbContext<IdentityUser<int>,IdentityRole<int>,int>
{
    public CarDbContext(DbContextOptions options) : base(options)
    {
        //Database.EnsureCreated();
    }

    public virtual DbSet<Car> Cars { get; set; } = null!;

    

    public virtual DbSet<Image> Images { get; set; } = null!;
}