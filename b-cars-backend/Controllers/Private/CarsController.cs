using System.Net;
using b_cars_backend.Helpers;
using Microsoft.AspNetCore.Mvc;
using b_cars_backend.Models;
using Microsoft.EntityFrameworkCore;
using b_cars_backend.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace b_cars_backend.Controllers.Private;

[Authorize]
[ApiController]
[Route("api/private/car")]
public class CarsController : ControllerBase
{
    private readonly ILogger<CarsController> _logger;
    private readonly CarDbContext _db;
    private readonly UserManager<User> _userManager;

    private async Task<User> GetCurrentUser()
    {
        return await _userManager
            .FindByIdAsync(HttpContext.User.Claims.First(x => x.Type == "Id").Value);
    }

    private async Task<bool> CanEdit(Car car)
    {
        //TODO ADMIN
        return (await GetCurrentUser()).Id == car.User.Id;
    }

    public CarsController(ILogger<CarsController> logger, CarDbContext db, UserManager<User> userManager)
    {
        _logger = logger;
        _db = db;
        _userManager = userManager;
    }

    [HttpGet(Name = "GetCar")]
    public async Task<object> GetCar(int id)
    {
        _logger.Log(LogLevel.Debug, "Method Get Car");
        var car = _db.Cars
            .Include(c => c.User)
            .Include(c => c.Images)
            .First(c => c.Id == id);
        
        if (!await CanEdit(car))
        {
            return StatusCode((int)HttpStatusCode.Forbidden, new
            {
                error = "Відсутні права на редагування" 
            });
        }
        
        return ResponseModelHelper.ToResponse(car);
        
        
        
    }

    [HttpGet]
    [Route("my-list")]
    public async Task<IEnumerable<object>> GetMyCars()
    {
        _logger.LogInformation("Method GetMyCars");

        var user = await GetCurrentUser();

        return _db.Cars
            .Include(c => c.User)
            .Include(c => c.Images)
            .OrderByDescending(c => c.UpdatedAt)
            .Where(c => c.User.Id == user.Id)
            .Select(car => ResponseModelHelper.ToResponse(car));
    }

    [HttpPut(Name = "PutCar")]
    public object PutCar([FromBody] CarViewModel car)
    {
        _logger.Log(LogLevel.Debug, "Method Put Car");
        var existedCar = _db.Cars
            .Include(x => x.Images)
            .Include(c => c.User)
            .First(x => x.Id == car.Id);
        car.FillCar(existedCar);

        _db.SaveChanges();

        return ResponseModelHelper.ToResponse(existedCar);
    }

    [HttpPost(Name = "PostCar")]
    public async Task<object> PostCar([FromBody] CarViewModel car)
    {
        _logger.LogInformation("Method Post Car");
        var newCar = new Car
        {
            User = await GetCurrentUser(),
            CreatedAt = DateTime.Now
        };
        car.FillCar(newCar);

        _db.Cars.Add(newCar);
        _db.SaveChanges();

        return ResponseModelHelper.ToResponse(newCar);
    }
}