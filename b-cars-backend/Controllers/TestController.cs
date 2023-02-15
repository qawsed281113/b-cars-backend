using b_cars_backend.Helpers;
using Microsoft.AspNetCore.Mvc;
using b_cars_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace b_cars_backend.Controllers;

[ApiController]
//[Route("[controller]")]
[Route("api/public/test")]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;
    private readonly CarDbContext _db;
    private readonly IServiceProvider _serviceProvider;

    public TestController(ILogger<TestController> logger, CarDbContext db, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _db = db;
        _serviceProvider = serviceProvider;
    }

    protected async Task<User> GetCurrentUser(UserManager<User> userManager)
    {
        return await userManager.FindByIdAsync(HttpContext.User.Claims.First(x => x.Type == "Id").Value);
    }
    
    [HttpGet(Name = "Test")]
    [AllowAnonymous]
    //[Authorize]
    public async Task<IActionResult> Test()
    {
        _logger.LogInformation("Test method start");
        var userManager = _serviceProvider.GetService<UserManager<User>>();
        //var user = await GetCurrentUser(userManager);
        
        var user = await userManager.FindByIdAsync("2");
        string code = await userManager.GeneratePasswordResetTokenAsync(user);
        await userManager.ResetPasswordAsync(user, code, "Pa$$word123");
        
        
        //var identity = user.Identity;
        return Ok(new
        {
            user
        });
        
        // var userManager = _serviceProvider.GetService<UserManager<User>>();
        // var roleManager = _serviceProvider.GetService<RoleManager<IdentityRole<int>>>();
        // // var user = new User
        // // {
        // //     UserName = "user",
        // //     EmailConfirmed = true,
        // // };
        // // await userManager.CreateAsync(user, "Pa$$word123");
        //
        // if (!await roleManager.RoleExistsAsync(Constants.AdminRole))
        // {
        //      await roleManager.CreateAsync(new IdentityRole<int>(Constants.AdminRole));
        // }
        // if (!await roleManager.RoleExistsAsync(Constants.UserRole))
        // {
        //     await roleManager.CreateAsync(new IdentityRole<int>(Constants.UserRole));
        // }
        //
        // //var user = await userManager.FindByIdAsync("1");
        // var user = await userManager.FindByNameAsync("user");
        // if (!await userManager.IsInRoleAsync(user, Constants.AdminRole))
        // {
        //     await userManager.AddToRoleAsync(user, Constants.AdminRole);
        // }
        //
        // var passwordValidator = new PasswordValidator<User>();
        // var valid = await passwordValidator.ValidateAsync(userManager, user, "Pa$$word123");
        
        //_logger.LogInformation();        
        // return Ok(new
        // {
        //     user,
        //     valid
        // });

        // if (user != null)
        // {
        //     return Ok(new { message = "Exist", user});
        // }
        // // if (user == null)
        // // {
        // //     user = new IdentityUser
        // //     {
        // //         UserName = UserName,
        // //         EmailConfirmed = true
        // //     };
        // //     await userManager.CreateAsync(user, testUserPw);
        // // }
        //
        // return Ok(new { message = "Created"}); 
    }
}