using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebApplication2.Models.Services.InterFace;
using WebApplication2.Models;

public class FileServices : IFileServices
{
    private readonly ILogger<FileServices> _logger;

    public FileServices(ILogger<FileServices> logger)
    {
        _logger = logger;
    }

    #region AddImage
    public async Task<List<string>> AddImageAsync(List<IFormFile>? images, string location)
    {
       
        var imagePaths = new List<string>();

        if (images == null || images.Count == 0)
        {
            return imagePaths;
        }

        try
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", location);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            foreach (var file in images)
            {
                if (file.Length > 0) // Check if the file is not empty
                {
                   
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    imagePaths.Add(Path.Combine("images", location, uniqueFileName));
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding image: {ex.Message}", ex);
            
        }

        return imagePaths;
    }


    #endregion

    #region RemoveImage
    public bool RemoveImageAsync(List<string> imagePaths)
    {
        if (imagePaths == null || imagePaths.Count == 0)
        {
            return false;
        }

        try
        {
            foreach (var relativePath in imagePaths)
            {
                // Combine the relative path with the root path
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);

                // Check if the file exists before attempting to delete it
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
                else
                {
                    _logger.LogWarning($"Image not found: {imagePath}");
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            // Log the exception with a more detailed message
            _logger.LogError(ex, "An error occurred while removing images.");
            return false;
        }
    }



    #endregion

    #region UpdateImage
    /// <summary>
    /// This function to Update image in File Server 
    /// </summary>
    /// <param name="imagePaths"></param>
    /// This Old Image To Need Remove 
    /// <param name="newImage"></param>
    /// This New Image To Need Save In File
    /// <param name="location"></param>
    /// This File Loaction To Need Save Image In Image File  
    /// <returns></returns>
    public async Task<List<string>> UpdateImageAsync(List<string> imagePaths, List<IFormFile>? newImage, string location)
    {
        RemoveImageAsync(imagePaths);
        return await AddImageAsync(newImage, location);
    }
    #endregion
}
