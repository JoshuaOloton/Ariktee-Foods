using ArikteeFoods.Models.DTOs;

namespace ArikteeFoods.Web.Services.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
        Task<ProductDto?> GetProduct(int Id);
    }
}
