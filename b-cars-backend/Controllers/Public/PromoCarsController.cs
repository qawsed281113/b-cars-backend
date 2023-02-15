using b_cars_backend.Helpers;
using Microsoft.AspNetCore.Mvc;
using b_cars_backend.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace b_cars_backend.Controllers.Public;

[ApiController]
[Route("api/public/promo")]
public class PromoCarsController : ControllerBase
{
    private readonly ILogger<PromoCarsController> _logger;
    private readonly CarDbContext _db;

    public PromoCarsController(ILogger<PromoCarsController> logger, CarDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    [HttpGet]
    [Route("cars")]
    public IEnumerable<object> GetPromoCars()
    {
        _logger.Log(LogLevel.Debug, "Method GetPromoCars");

        return _db.Cars
            .Include(c => c.User)
            .Include(c => c.Images)
            .OrderByDescending(c => c.UpdatedAt)
            .Take(6)
            .Select(car => ResponseModelHelper.ToResponse(car));
    }
}