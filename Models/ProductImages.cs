using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class ProductImages
    {

        [Key]
        public int id  { get; set; }
        public string Images { get; set; }
        [ForeignKey(nameof(ProductId))]
        public int ProductId { get; set; }

        public Product? product { get; set; }
       
    }
}
