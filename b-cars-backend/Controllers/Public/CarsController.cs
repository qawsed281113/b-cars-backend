using b_cars_backend.Helpers;
using Microsoft.AspNetCore.Mvc;
using b_cars_backend.Models;
using Microsoft.EntityFrameworkCore;
using b_cars_backend.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace b_cars_backend.Controllers.Public;

[ApiController]
[Route("api/public/car")]
public class CarsController : ControllerBase
{
    private readonly ILogger<CarsController> _logger;
    private readonly CarDbContext _db;

    public CarsController(ILogger<CarsController> logger, CarDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    [HttpGet]
    public object GetCar(int id)
    {
        _logger.Log(LogLevel.Debug, "Method Get Car");
        return ResponseModelHelper.ToResponse(_db.Cars
            .Include(c => c.User)
            .Include(c => c.Images)
            .First(c => c.Id == id));
    }

    [HttpGet]
    [Route("search")]
    public IEnumerable<object> SearchCars([FromQuery] CarSearchModel searchModel)
    {
        _logger.Log(LogLevel.Debug, "Method Car search");

        var q = _db
            .Cars
            .Include(c => c.User)
            .Include(c => c.Images)
            .AsQueryable();

        if (searchModel.Title != null)
        {
            q = q.Where(c => c.Title.Contains(searchModel.Title));
        }
        
        if (searchModel.Year != null)
        {
            q = q.Where(c => c.Year.Equals(searchModel.Year));
        }

        if (searchModel.Transmission != null)
        {
            q = q.Where(c => c.Transmission.Equals(searchModel.Transmission));
        }

        if (searchModel.Fuel != null)
        {
            q = q.Where(c => c.Fuel.Equals(searchModel.Fuel));
        }
        
        if (searchModel.MaxPrice != null)
        {
            q = q.Where(c => c.PriceUsd < (decimal)searchModel.MaxPrice);
        }


        return
            q
                .AsEnumerable()
                .Select(ResponseModelHelper.ToResponse);
    }
}