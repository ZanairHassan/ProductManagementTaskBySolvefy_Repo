using DAL.Models;
using DAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IProductService
    {
        Task<Product> CreateProduct(ProductVM productVM);
        Task<Product> GetProductById(int id);
        Task<List<Product>> GetAllProduct();
        Task<Product> DeleteProduct(int id);
        Task<Product> UpdateProduct(int id, ProductVM productVM);
        Task<List<Product>> SearchProducts(string searchString);
    }
}
