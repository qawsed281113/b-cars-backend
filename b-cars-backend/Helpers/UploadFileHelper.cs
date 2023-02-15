using System.Text;
using b_cars_backend.Models;

namespace b_cars_backend.Helpers;

public static class UploadFileHelper
{
    private static string GetRandomString(int stringLength)
    {
        var sb = new StringBuilder();
        var numGuidsToConcat = (stringLength - 1) / 32 + 1;
        for (var i = 1; i <= numGuidsToConcat; i++)
        {
            sb.Append(Guid.NewGuid().ToString("N"));
        }

        return sb.ToString(0, stringLength);
    }

    public static string UploadRoot { get; set; } = "E:\\diplom\\b-cars-frontend\\public\\uploads";


    public static string GetRandomFilePath(string ext)
    {
        //TODO CONFIG
        var randomFileName = GetRandomString(15);
        string dir1 = randomFileName[0].ToString();
        string dir2 = randomFileName[1].ToString();

        var directoryPath = Path.Combine(UploadRoot, dir1, dir2);
        Directory.CreateDirectory(directoryPath);
        randomFileName += ext;
        return Path.Combine(directoryPath, randomFileName);
    }

    public static string GetUrl(string filename)
    {
        string dir1 = filename[0].ToString();
        string dir2 = filename[1].ToString();
        return $"/uploads/{dir1}/{dir2}/{filename}";
    }

    public static void Delete(string filename)
    {
        var dir1 = filename[0].ToString();
        var dir2 = filename[1].ToString();
        var fullFilename = Path.Combine(UploadRoot, dir1, dir2, filename);
        File.Delete(fullFilename);
    }
}