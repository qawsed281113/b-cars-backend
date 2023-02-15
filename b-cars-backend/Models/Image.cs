using System.ComponentModel.DataAnnotations;

namespace b_cars_backend.Models;

public class Image
{
    public int Id { get; set; }
    public virtual Car Car { get; set; } = null!;
    [Required]
    public string FileName { get; set; }
    
    public bool IsMain { get; set; }
}