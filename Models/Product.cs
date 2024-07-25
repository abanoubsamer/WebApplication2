using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class Product
    {
      
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
        [ForeignKey("Category")]
        public int? CategoryId { get; set; }
        public Category? Categorys { get; set; }
        public virtual ICollection<ProductImages>? Images { get; set; }



    }
}
