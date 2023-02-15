using b_cars_backend.Helpers;
using Microsoft.AspNetCore.Mvc;
using b_cars_backend.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace b_cars_backend.Controllers.Public;

[ApiController]
[Route("api/public/data")]
public class DataController : ControllerBase
{
    private readonly ILogger<DataController> _logger;
    private readonly CarDbContext _db;

    public DataController(ILogger<DataController> logger, CarDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    private readonly List<string> FuelTypes = new List<string>
    {
        "Бензин",
        "Дизель",
        "Газ",
        "Електро",
        "Бензин/Газ"
    };

    private readonly List<string> TransmissionTypes = new List<string>
    {
        "Автомат",
        "Механіка",
        "Типтронік",
        "Робот",
        "Варіатор",
    };

    [HttpGet]
    [Route("form")]
    public async Task<IActionResult> GetFormData()
    {
        _logger.LogInformation("GetFormData method");

        return Ok(new
        {
            TransmissionTypes = TransmissionTypes.OrderBy(t => t),
            FuelTypes = FuelTypes.OrderBy(t => t)
        });
    }
}