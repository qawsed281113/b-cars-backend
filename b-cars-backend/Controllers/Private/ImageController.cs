using System.Net;
using System.Text;
using b_cars_backend.Helpers;
using Microsoft.AspNetCore.Mvc;
using b_cars_backend.Models;
using Microsoft.EntityFrameworkCore;
using b_cars_backend.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace b_cars_backend.Controllers.Private;

[ApiController]
[Authorize]
[Route("api/private/image")]
public class ImageController : ControllerBase
{
    private readonly ILogger<ImageController> _logger;
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


    public ImageController(ILogger<ImageController> logger, CarDbContext db, UserManager<User> userManager)
    {
        _logger = logger;
        _db = db;
        _userManager = userManager;
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteImage(int imageId)
    {
        var image = _db.Images.First(x => x.Id == imageId);

        _db.Images.Remove(image);
        await _db.SaveChangesAsync();
        UploadFileHelper.Delete(image.FileName);
        return Ok(new { message = "Deleted", id = imageId });
    }

    [HttpPost]
    [Route("set-main")]
    public async Task<IActionResult> SetMain(int imageId)
    {
        var image = _db.Images.Include(x => x.Car).First(x => x.Id == imageId);
        if (!await CanEdit(image.Car))
        {
            return BadRequest("Ви не маєте права редагувати зображення");
        }

        var allImages = _db.Images.Include(x => x.Car)
            .Where(x => x.Car.Id == image.Car.Id);

        allImages.ToList().ForEach(i => i.IsMain = false);
        image.IsMain = true;

        await _db.SaveChangesAsync();
        return Ok(new { message = "Set main", id = imageId });
    }


    [HttpPost]
    public async Task<IActionResult> UploadImages(int carId)
    {
        IFormFileCollection files = Request.Form.Files;
        _logger.Log(LogLevel.Debug, "Method UploadImages");
        long size = files.Sum(f => f.Length);

        var car = _db.Cars.First(x => x.Id == carId);
        if (!await CanEdit(car))
        {
            return BadRequest(new { error = "Ви не маєте права редагувати зображення" });
        }

        foreach (var formFile in files)
        {
            if (formFile.Length > 0)
            {
                var filename = UploadFileHelper.GetRandomFilePath(Path.GetExtension(formFile.FileName));
                using (var stream = System.IO.File.Create(filename))
                {
                    await formFile.CopyToAsync(stream);
                    Image image = new Image
                    {
                        Car = car,
                        FileName = Path.GetFileName(filename)
                    };
                    
                    _db.Images.Add(image);
                }
            }
        }

        await _db.SaveChangesAsync();
        // Process uploaded files
        // Don't rely on or trust the FileName property without validation.

        return Ok(new { count = files.Count, size });
    }
}