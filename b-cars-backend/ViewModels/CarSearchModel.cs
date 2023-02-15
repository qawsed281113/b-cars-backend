namespace b_cars_backend.ViewModels;

public class CarSearchModel
{
    public string? Title { get; set; }
    public string? Transmission { get; set; }
    public string? Fuel { get; set; }
    public string? City { get; set; }
    public int? Year { get; set; }
    
    public double? MaxPrice { get; set; }
}