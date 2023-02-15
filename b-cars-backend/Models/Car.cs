namespace b_cars_backend.Models;


public class Car
{
    public Car()
    {
        Images = new HashSet<Image>();
        //UpdatedAt = DateTime.Now;
        //CreatedAt = DateTime.Now;
    }
    public int Id { get; set; }
    public int Year { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal PriceUsd { get; set; }
    public decimal PriceUah => PriceUsd * 42;
    public int Mileage { get; set; }
    public string City { get; set; }
    public string Transmission { get; set; }
    public string Fuel { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual User User { get; set; } = null!;
    
    public virtual ICollection<Image> Images { get; set; }
}