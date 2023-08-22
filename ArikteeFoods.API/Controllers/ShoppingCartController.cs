using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ArikteeFoods.Models.DTOs;
using ArikteeFoods.API.Repositories.Contracts;
using ArikteeFoods.API.Extensions;

namespace ArikteeFoods.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository)
        {
            this._shoppingCartRepository = shoppingCartRepository;
        }

        [HttpGet("{userId:int}/GetItems")]
        public async Task<ActionResult<IEnumerable<ShoppingCartItemDto>>> GetShoppingCartItems(int userId)
        {
            try
            {
                var items = await _shoppingCartRepository.GetItems(userId);
                if (items is null)
                {
                    return NotFound();
                }
                var itemsDto = await items.ConvertToDto();
                return Ok(itemsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<IEnumerable<ShoppingCartItemDto>>> GetShoppingCartItem(int Id)
        {
            try
            {
                var item = await _shoppingCartRepository.GetItem(Id);
                if (item is null)
                {
                    return NotFound();
                }
                var itemDto = item.ConvertToDto();
                return Ok(itemDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<ShoppingCartItemDto>>> AddShoppingCartItem([FromBody] CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                var newCartItem = await _shoppingCartRepository.AddItem(cartItemToAddDto);
                if (newCartItem is null)
                    return NoContent();
                var newVwCartItem = await _shoppingCartRepository.GetItem(newCartItem.Id);
                if (newVwCartItem is null)
                    return NoContent();
                var newVwCartItemDto = newVwCartItem.ConvertToDto();
                return CreatedAtAction(nameof(GetShoppingCartItem), new { Id = newVwCartItemDto.Id }, newVwCartItemDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
