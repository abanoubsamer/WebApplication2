using AutoMapper;
using WebApplication2.Models;
using WebApplication2.Models.ViewModels;

namespace WebApplication2.Mapping
{
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            CreateMap<AddProductViewMoldes, Product>();
            CreateMap<EditProductViewMoldes, Product>();
        }

    }
}
