using ArikteeFoods.API.Entities;
using ArikteeFoods.API.Extensions;
using ArikteeFoods.API.Repositories.Contracts;
using ArikteeFoods.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArikteeFoods.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            try
            {
                var products = await _productRepository.GetProducts();
                if(products is null)
                {
                    return NotFound();
                }
                var productDtos = products.ConvertToDto();
                return Ok(productDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int Id)
        {
            try
            {
                var product = await _productRepository.GetProduct(Id);
                if (product is null)
                {
                    return NotFound();
                }
                var productDto = product.ConvertToDto();
                return Ok(productDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //[HttpPost]
        //public async Task<ActionResult<Product>> PostProduct([FromBody] ProductToAddDto productToAddDto)
        //{
        //    try
        //    {
        //        var newProduct = await _productRepository.AddProduct(productToAddDto);
        //        if (newProduct == null)
        //        {
        //            return NotFound();
        //        }
        //        var newProductDto = newProduct.ConvertToDto();
        //        return CreatedAtAction(nameof(GetProduct), new { Id = newProduct.Id }, newProduct);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        //}
    }
}
