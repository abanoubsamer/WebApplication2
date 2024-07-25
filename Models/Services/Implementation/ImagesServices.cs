using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models.Services.InterFace;

namespace WebApplication2.Models.Services.Implementation
{
    public class ImagesServices : IImagesServices
    {
        #region Fields
        private readonly AppDbContext _db;
 
        #endregion

        #region Constructors
        public ImagesServices(AppDbContext db)
        {
            _db = db;
    
        }
        #endregion

        #region Methods

        #region AddImageAsync
        public async Task<string> AddImageAsync(Product product, List<string>? imagePaths)
        {
            if (product == null || imagePaths == null || !imagePaths.Any())
            {
                return "No images provided for addition.";
            }

            try
            {
                var productImages = imagePaths.Select(path => new ProductImages
                {
                    ProductId = product.id,
                    Images = path
                }).ToList();

                await _db.Images.AddRangeAsync(productImages);
                await _db.SaveChangesAsync();

                return "Success";
            }
            catch (Exception e)
            {
                // Consider logging the exception here
                return $"Error adding images: {e.Message}";
            }
        }
        #endregion

        #region RemoveImageAsync
        public async Task<string> RemoveImageAsync(List<ProductImages> imagesToRemove)
        {
            if (imagesToRemove == null || !imagesToRemove.Any())
            {
                return "No images found for removal.";
            }

            try
            {
                _db.Images.RemoveRange(imagesToRemove);
                await _db.SaveChangesAsync();

                return "Images removed successfully.";
            }
            catch (Exception e)
            {
                // Consider logging the exception here
                return $"Error removing images: {e.Message}";
            }
        }

        public Task<string> RemoveImageAsync(int ProductId)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region UpdateImageAsync
        public async Task<string> UpdateImageAsync(Product Product, List<string>? newImagePaths)
        {
            if (newImagePaths == null || !newImagePaths.Any())
            {
                return "No images provided for update.";
            }

            try
            {

                if (Product == null)
                {
                    return "Product not found.";
                }

                // Remove existing images
                if (Product.Images != null && Product.Images.Any())
                {
                    var existingImages = Product.Images.ToList();
                    var r = await RemoveImageAsync(existingImages);
                    if (r.StartsWith("Error"))
                    {
                        return r;
                    }
                }

                // Process and add new images
                var result = await AddImageAsync(Product, newImagePaths);
                if (result.StartsWith("Error"))
                {
                    return result;
                }
                return "Success";
            }
            catch (Exception ex)
            {
                // Consider logging the exception here
                return $"Error updating images: {ex.Message}";
            }
        }
        #endregion

        #endregion
    }
}
