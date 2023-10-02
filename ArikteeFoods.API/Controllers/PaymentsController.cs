using ArikteeFoods.API.Exceptions;
using ArikteeFoods.API.Repositories.Contracts;
using ArikteeFoods.Models.DTOs;
using ArikteeFoods.Models.PaystackModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArikteeFoods.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public PaymentsController(IShoppingCartRepository shoppingCartRepository)
        {
            this._shoppingCartRepository = shoppingCartRepository;
        }

        [HttpPost("{cartId:int}")]
        public async Task<ActionResult<InitializeCheckoutDto>> InitializeCheckout(int cartId, [FromBody] TotalPayInfo payInfo)
        {
            try
            {
                var userExists = await _shoppingCartRepository.VerifyUserExists(payInfo.PayInfo.Email);
                if (!userExists)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "This user does not exist.");
                }
                var initializeCheckoutDto = await _shoppingCartRepository.InitializeCheckout(cartId, payInfo);
                return Ok(initializeCheckoutDto);
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

        [HttpPost]
        [Route("VerifyCheckout")]
        public async Task<ActionResult<VerifyCheckoutDto>> VerifyCheckout(CartCheckoutToVerifyDto cartCheckoutToVerifyDto)
        {
            try
            {
                var verifyResult = await _shoppingCartRepository.VerifyCheckout(cartCheckoutToVerifyDto);
                return Ok(verifyResult);
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
    }
}
