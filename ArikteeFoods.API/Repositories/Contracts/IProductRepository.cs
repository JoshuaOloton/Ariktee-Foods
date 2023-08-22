using ArikteeFoods.API.Entities;
using ArikteeFoods.Models.DTOs;

namespace ArikteeFoods.API.Repositories.Contracts
{
    public interface IProductRepository
    {
        Task<Product?> GetProduct(int Id);

        Task<IEnumerable<Product>> GetProducts();

        Task<Product> AddProduct(ProductToAddDto productToAddDto);
    }
}
