namespace WebApplication2.Models.Services.InterFace
{
    public interface IImagesServices
    {
        public Task<string> AddImageAsync(Product product, List<string>? images);
        public Task<string> RemoveImageAsync(List<ProductImages> imagesToRemove);
        public Task<string> UpdateImageAsync(Product product, List<string>? NewImagePaths);

    }
}
