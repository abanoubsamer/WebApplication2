using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppDbContext _db;
        private int UserId = 1;
        private string UserName = "BiboSamer";
        public HomeController(AppDbContext context)
        {
            _db = context;
        }


        [HttpGet]
        public async Task <IActionResult> Index()
        {
            CookieOptions Cookiesoption = new CookieOptions();
            Cookiesoption.Expires = DateTime.UtcNow.AddSeconds(50);
            Response.Cookies.Append("UserId", UserId.ToString(),Cookiesoption);
            Response.Cookies.Append("UserName", UserName,Cookiesoption);


            var product = await _db.Category.Include(p => p.Products).ToListAsync();
            return View( product);
        }


        //[HttpGet("{controller}/d/{num}")]// hna ana bolh anta htst2bl fe ale route dh num 
        //[FromQuery]string n , [FromQuery] int p   https://localhost:5000/home/d?n=kokowaw&p=2500
        //[FromRoute] int id   https://localhost:5000/home/d/5
        //[FromForm] dh bth ale hya bydefult ale hwa byb3tha lma t3ml new product

        [HttpGet]
        public IActionResult d([FromRoute] int num)
        {
            
            return View();
        }

    }
}
