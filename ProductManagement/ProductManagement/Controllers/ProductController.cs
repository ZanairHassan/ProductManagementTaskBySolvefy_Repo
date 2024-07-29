using BLL.Interfaces;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;

public class ProductController : Controller
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    // GET: Products
    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetAllProduct();
        return View(products);
    }

    // GET: Product/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var product = await _productService.GetProductById(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    // GET: Product/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Product/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductVM productVM)
    {
        if (ModelState.IsValid)
        {
            await _productService.CreateProduct(productVM);
            return RedirectToAction(nameof(Index));
        }
        return View(productVM);
    }

    // POST: Product/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductVM productVM)
    {
        if (ModelState.IsValid)
        {
            var updatedProduct = await _productService.UpdateProduct(id, productVM);
            if (updatedProduct == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
        return View(productVM);
    }
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productService.GetProductById(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    // POST: Product/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var deletedProduct = await _productService.DeleteProduct(id);
        if (deletedProduct == null)
        {
            return NotFound();
        }
        return RedirectToAction(nameof(Index));
    }
}
 