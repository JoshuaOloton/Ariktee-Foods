using ArikteeFoods.API.Data;
using ArikteeFoods.API.Entities;
using ArikteeFoods.API.Repositories.Contracts;
using ArikteeFoods.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ArikteeFoods.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ArikteeDbContext _arikteeDbContext;

        public ProductRepository(ArikteeDbContext arikteeDbContext)
        {
            this._arikteeDbContext = arikteeDbContext;
        }
        public async Task<Product> AddProduct(ProductToAddDto productToAddDto)
        {
            var product = new Product
            {
                ProductName = productToAddDto.Name,
                ProductDescription = productToAddDto.Description,
                ProductImageUrl = productToAddDto.ImageURL,
                ProductPrice = productToAddDto.Price
            };
            var result = await _arikteeDbContext.Products.AddAsync(product);
            await _arikteeDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Product?> GetProduct(int Id) => await _arikteeDbContext.Products.FindAsync(Id);

        public async Task<IEnumerable<Product>> GetProducts() => await _arikteeDbContext.Products.ToListAsync();
    }
}
