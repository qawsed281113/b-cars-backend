using b_cars_backend.Models;

namespace b_cars_backend.Helpers;

public static class ResponseModelHelper
{
    private static object ToResponse(Image image)
    {
        return image.Id != 0
            ? new
            {
                image.Id,
                src = UploadFileHelper.GetUrl(image.FileName),
                image.IsMain
            }
            : new
            {
                Id = 0,
                src = image.FileName,
                image.IsMain
            };
    }

    public static object ToResponse(Car car)
    {
        var mainImage = car.Images.FirstOrDefault(x => x.IsMain) ?? new Image
        {
            Id = 0,
            FileName = "/images/no_image.jpg",
            IsMain = true
        };

        return new
        {
            car.Id,
            car.Title,
            car.Mileage,
            car.Transmission,
            car.Description,
            car.City,
            car.Year,
            car.Fuel,
            car.PriceUah,
            car.PriceUsd,
            car.CreatedAt,
            car.UpdatedAt,
            mainImage = ToResponse(mainImage),
            User = new
            {
                car.User.Id,
                car.User.UserName,
                car.User.PhoneNumber
            },

            Images = car.Images.OrderByDescending(x => x.IsMain).Select(ToResponse)
        };
    }
}