using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication2.Models;
using WebApplication2.Models.Services.Implementation;
using WebApplication2.Models.Services.InterFace;
using WebApplication2.Models.ViewModels;

namespace WebApplication2.Controllers
{
    public class ProductController : Controller
    {
        #region Filed
        private readonly IProductServices _productServices;
        private readonly IFileServices _fileServices;
        private readonly ICategoriesServices _Categories;
        private readonly IImagesServices _imagesServices;
        private readonly IMapper _Mapper; 
        #endregion

        #region Constrator
        public ProductController(IProductServices productServices,
            IFileServices fileServices,
            ICategoriesServices categories,
            IImagesServices imagesServices,
            IMapper mapper)
        {
            _productServices = productServices;
            _fileServices = fileServices;
            _Categories = categories;
            _imagesServices = imagesServices;
            _Mapper = mapper;
        }
        #endregion

        #region Index
        [Route("ProductController/Index")]
        public async Task<IActionResult> Index()
        {
            var products = await _productServices.GetProductsAsync();

            return View(products);
        }
        #endregion

        #region Function
        public async Task GetCategory<T>(T? p = null)where T :class
        {
            if (p == null)
            {
                ViewBag.Gategories = new SelectList(await _Categories.GetCategoriesAsync(), "id", "Name");
            }
            else
            {
                ViewBag.Gategories = new SelectList(await _Categories.GetCategoriesAsync(), "id", "Name",p);
            }
           
        }
        public async Task<IActionResult> IsProductExits(string Name)
        {
            return true == await _productServices.IsProductNameExitsAsync(Name) ? Json(false) : Json(true);
        }
        #endregion

        #region Create
        [HttpGet]
        [Route("ProductController/Create")]
        public async Task <IActionResult> Create()
        {
            await GetCategory<Product>();
            return View();
        }

        [HttpPost]
        [Route("ProductController/Create")]
        public async Task<IActionResult> Create(AddProductViewMoldes product)
        {

            if (product == null)
            {
                ModelState.AddModelError("", "Product data is null.");
                await GetCategory<Product>();
                return View(new AddProductViewMoldes());
            }
            var prod = _Mapper.Map<Product>(product);
           

            if (!ModelState.IsValid)
            {
                await GetCategory(prod);
                return View(product);
            }

            try
            {

                // Add Image In File Server
                var imageResult = await _fileServices.AddImageAsync(product.Files, "Product");
                if (imageResult is null)
                {
                    return BadRequest("Image upload failed.");
                }
              

                var result = await _productServices.AddProductAsync(prod, imageResult);
                if (result == "Success")
                {
                    return RedirectToAction(nameof(Index));
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return View(product);
            }
        }
        #endregion


        #region Edit

        [HttpGet]
        [Route("ProductController/Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productServices.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Map the Product to EditProductViewMoldes
            var editProductViewModel = new EditProductViewMoldes
            {
                id = product.id,
                Name = product.Name,
                Price = product.Price,
                CategoryId = product.CategoryId,
            };
            await GetCategory<Product>();
            return View(editProductViewModel);
        }



        [HttpPost]
        [Route("ProductController/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditProductViewMoldes NewProduct)
        {
            if (id != NewProduct.id)
            {
                return NotFound("Invalid id");
            }

            if (NewProduct == null)
            {
                ModelState.AddModelError("", "Product data is null.");
                return View(new EditProductViewMoldes());
            }
            

            try
            {


                if (ModelState.IsValid)
                { 
                    var result = await _productServices.UpdateProduct(NewProduct);

                    if (result == "Success")
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                       ModelState.AddModelError("", "Failed to update product");
                        await GetCategory(NewProduct);
                        return View(NewProduct);
                    }


                }

                 await GetCategory(NewProduct);
                return View(NewProduct);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error updating product: {ex.Message}");
                await GetCategory(NewProduct);
                return View(NewProduct);
            }
        }

        #endregion


        #region Delete
        [HttpGet]
        [Route("ProductController/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid product ID.");
            }

            try
            {
                // Retrieve the product by its ID
                var product = await _productServices.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound("Product not found.");
                }

                // Remove associated images
                if (product.Images != null && product.Images.Any())
                {
                    var r = product.Images.Select(img => img.Images).ToList();
                    if (!_fileServices.RemoveImageAsync(r)) ;
                    {
                        // Log warning if image removal failed
                        Console.WriteLine($"Failed to remove some or all images for product ID: {id}");
                    }
                }

                // Delete the product
                await _productServices.DeleteProductAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex+ "An error occurred while deleting the product.");
                return StatusCode(500, $"Error deleting product: {ex.Message}");
            }
        }

        #endregion

        #region Details
        [Route("ProductController/Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productServices.GetProductByIdAsync(id);
            return product == null ? NotFound() : View(product);
        }
        #endregion






    }
}
