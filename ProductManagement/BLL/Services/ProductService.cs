using BLL.Interfaces;
using DAL.DBContext;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Product> CreateProduct(ProductVM productVM)
        {
            var product = new Product
            {
                ProductName = productVM.ProductName,
                ProductDescription= productVM.ProductDescription,
                ProductPrice= productVM.ProductPrice,
                ProductType = productVM.ProductType,
                ProductCreatedDate=DateTime.Now,
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
            
        }

        public async Task<Product> DeleteProduct(int id)
        {
            var product=await _context.Products.FirstOrDefaultAsync(x=> x.ProductId==id && !x.IsDeleted);
            if (product == null)
            {
                return null;
            }
            product.IsDeleted = true;
            product.ProductDeletedDate = DateTime.Now;
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<List<Product>> GetAllProduct()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(x=> x.ProductId==id);
        }

        public async Task<List<Product>> SearchProducts(string searchString)
        {
            var products = await _context.Products
                .Where(p => !p.IsDeleted && p.ProductType.ToLower().Contains(searchString.ToLower()))
                .ToListAsync();

            return products;
        }



        public async Task<Product> UpdateProduct(int id, ProductVM productVM) 
        {
            var product= await _context.Products.FirstOrDefaultAsync(x=>x.ProductId==id);
            if (product == null)
            {
                return null ;
            }
            product.ProductName = productVM.ProductName;
            product.ProductDescription = productVM.ProductDescription;
            product.ProductPrice = productVM.ProductPrice;
            product.ProductType = productVM.ProductType;
            product.ProductModifiedDate= DateTime.Now;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }
    }
}
