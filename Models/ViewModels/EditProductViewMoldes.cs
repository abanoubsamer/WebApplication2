using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.ViewModels
{
    public class EditProductViewMoldes
    {
        [Key]
        public int id { get; set; }
        [Remote("IsProductExits", "Product", HttpMethod = "Post", ErrorMessage = "Is Name Is Aready Exits")]
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(1000, 9000)]
        public int Price { get; set; }

        public List<IFormFile>? Files { get; set; }

        public int? CategoryId { get; set; }
    }
}
