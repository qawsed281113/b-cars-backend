using System.ComponentModel.DataAnnotations;
using b_cars_backend.Models;

namespace b_cars_backend.ViewModels;

public class CarViewModel
{
    public int? Id { get; set; }
    [Required] public string Title { get; set; }

    public string? Description { get; set; }
    [Required] public decimal PriceUsd { get; set; }
    [Required] public int Mileage { get; set; }
    [Required] public string City { get; set; }
    [Required] public string Transmission { get; set; }
    [Required] public string Fuel { get; set; }

    [Required] [Range(1900, 3000)] public int Year { get; set; }

    public void FillCar(Car car)
    {
        car.Title = Title;
        car.Description = Description ?? "";
        car.City = City;
        car.Mileage = Mileage;
        car.Transmission = Transmission;
        car.PriceUsd = PriceUsd;
        car.Fuel = Fuel;
        car.Year = Year;
        car.UpdatedAt = DateTime.Now;
    }
}