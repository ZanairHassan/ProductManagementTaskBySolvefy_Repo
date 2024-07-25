using BLL.Interfaces;
using DAL.DBContext;
using Microsoft.AspNetCore.Mvc;

namespace ProductManagement.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public IActionResult Index()
        {
            return View();
        }
    }
}
