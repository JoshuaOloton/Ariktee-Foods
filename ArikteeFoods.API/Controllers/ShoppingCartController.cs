using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ArikteeFoods.Models.DTOs;
using ArikteeFoods.API.Repositories.Contracts;
using ArikteeFoods.API.Extensions;
using ArikteeFoods.API.Exceptions;
using ArikteeFoods.API.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace ArikteeFoods.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository)
        {
            this._shoppingCartRepository = shoppingCartRepository;
        }

        [HttpGet("{userID:int}")]
        public async Task<ActionResult<IEnumerable<ShoppingCartDto>>> GetAllCarts(int userID)
        {
            try
            {
                var carts = await _shoppingCartRepository.GetAllCarts(userID);
                if (carts == null)
                {
                    return NotFound();
                }
                var cartsDto = carts.ConvertToDto();
                return Ok(cartsDto);
            }
            catch(HttpResponseException ex)
            {
                return StatusCode(ex.StatusCode, ex.Value);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetItemsByCart/{cartID:int}")]
        public async Task<ActionResult<IEnumerable<ShoppingCartDto>>> GetCartItems(int cartID)
        {
            try
            {
                var cartItems = await _shoppingCartRepository.GetCartItems(cartID);
                if (cartItems == null)
                {
                    return NotFound();
                }
                var cartItemsDto = await cartItems.ConvertToDto();
                return Ok(cartItemsDto);
            }
            catch (HttpResponseException ex)
            {
                return StatusCode(ex.StatusCode, ex.Value);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{userId:int}/GetAllItems")]
        public async Task<ActionResult<IEnumerable<ShoppingCartItemDto>>> GetAllShoppingCartItems(int userId)
        {
            try
            {
                var items = await _shoppingCartRepository.GetAllItems(userId);
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

        [HttpGet("{userId:int}/GetItems")]
        public async Task<ActionResult<IEnumerable<ShoppingCartItemDto>>> GetShoppingCartItems(int userId)
        {
            try
            {
                var currentCart = await _shoppingCartRepository.GetCurrentCart(userId);
                if (currentCart is null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Your cart could not be found");
                }
                var items = await _shoppingCartRepository.GetItems(userId, currentCart.Id);
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

        [HttpGet("{Id:int}/GetItem")]
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
                {
                    return NoContent();
                }
                var newVwCartItem = await _shoppingCartRepository.GetItem(newCartItem.Id);
                if (newVwCartItem is null)
                {
                    return NoContent();
                }
                var newVwCartItemDto = newVwCartItem.ConvertToDto();
                return CreatedAtAction(nameof(GetShoppingCartItem), new { Id = newVwCartItemDto.Id }, newVwCartItemDto);
            }
            catch (HttpResponseException ex)
            {
                return StatusCode(ex.StatusCode, ex.Value);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{userId:int}/GetCart")]
        public async Task<ActionResult<ShoppingCartDto>> GetCurrentCart(int userId)
        {
            try
            {
                var currentCart = await _shoppingCartRepository.GetCurrentCart(userId);
                if (currentCart is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "User not found.");
                }
                var currentCartDto = currentCart.ConvertToDto();
                return Ok(currentCartDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("{userId:int}/GetLastCart")]
        public async Task<ActionResult<ShoppingCartDto>> GetLastCart(int userId)
        {
            try
            {
                var currentCart = await _shoppingCartRepository.GetLastCart(userId);
                if (currentCart is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "User not found.");
                }
                var currentCartDto = currentCart.ConvertToDto();
                return Ok(currentCartDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPatch("{Id:int}")]
        public async Task<ActionResult<ShoppingCartItemDto>> UpdateQty(int Id, [FromBody] CartItemQtyUpdateDto updateCartItemQtyUpdateDto)
        {
            try
            {
                var cartItem = await _shoppingCartRepository.UpdateQty(Id, updateCartItemQtyUpdateDto);
                if (cartItem is null)
                {
                    return NotFound();
                }
                var vwCartItem = await _shoppingCartRepository.GetItem(cartItem.Id);
                if (vwCartItem is null)
                {
                    return NotFound();
                }
                var cartItemDto = vwCartItem.ConvertToDto();
                return Ok(cartItemDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CartItemToAddDto>> DeleteItem(int id)
        {
            try
            {
                var vwCartItem = await _shoppingCartRepository.GetItem(id);
                if (vwCartItem is null)
                {
                    return NotFound();
                }
                var cartItem = await _shoppingCartRepository.DeleteItem(id);
                if (cartItem == null)
                {
                    return NotFound();
                }
                var cartItemDto = vwCartItem.ConvertToDto();
                return Ok(cartItemDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
