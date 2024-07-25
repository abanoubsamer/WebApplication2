using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Category
    {
        [Key]
        public int id { get; set; }
        
        public string ?Name { get; set; }

        //HashSet btmn3 ale tkrar
        public ICollection<Product>Products { get; set; }=new HashSet<Product>();


    }
}
