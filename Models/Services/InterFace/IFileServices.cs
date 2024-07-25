namespace WebApplication2.Models.Services.InterFace
{
    public interface IFileServices
    {
        public Task <List<string>> AddImageAsync(List<IFormFile>? Image,string Location);
        public bool RemoveImageAsync(List <string> Images);
        /// <summary>
        /// This function to Update image in File Server 
        /// </summary>
        /// <param name="imagePaths">This Old Image To Need Remove</param>
        /// <param name="newImage">This New Image To Need Save In File</param>
        /// <param name="location">This File Loaction To Need Save Image In Image File  </param>
        /// <returns>The New List To Image Path</returns>
        public Task<List<string>> UpdateImageAsync(List<string> imagePaths, List<IFormFile>? newImage, string location);

    }
}
