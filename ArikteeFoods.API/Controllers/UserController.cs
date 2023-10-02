using ArikteeFoods.API.Entities;
using ArikteeFoods.API.Exceptions;
using ArikteeFoods.API.Extensions;
using ArikteeFoods.API.Repositories.Contracts;
using ArikteeFoods.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;
using System.Diagnostics;

namespace ArikteeFoods.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;

        public UserController(IAuthRepository authRepository, IUserRepository userRepository)
        {
            this._authRepository = authRepository;
            this._userRepository = userRepository;
        }

        [HttpGet("{userId:int}")]
        public async Task<ActionResult<UserDto>> GetUser(int userId)
        {
            try
            {
                var user = await _authRepository.GetUserById(userId);
                if (user is null)
                {
                    return NotFound();
                }
                var userDto = user.ConvertToDto();
                return Ok(userDto);    
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("address/{userId:int}")]
        public async Task<ActionResult<UserDto>> AddAddress(int userId, [FromBody] AddressToAddDto addressToAddDto)
        {
            try
            {
                var user = await _userRepository.AddAddress(userId, addressToAddDto);
                var userDto = user.ConvertToDto();
                return Ok(userDto);
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

        [HttpPost("login")]
        public async Task<ActionResult<LoggedInUserDto>> Login(UserToLoginDto userToLoginDto)
        {
            try
            {
                var user = await _authRepository.GetUser(userToLoginDto);
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "This user does not exist.");
                }
                var token = _authRepository.GenerateToken(user.Id, user.Fullname, user.Email);
                var refreshToken = _authRepository.GenerateRefreshToken();
                await _authRepository.SetRefreshToken(user.Id, refreshToken);

                var cookieOptions = new CookieOptions()
                {
                    IsEssential = true,
                    SameSite = SameSiteMode.None,
                    Expires = refreshToken.ExpiresAt,
                    Path = "/",
                    //Secure = true
                };
                Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

                var loggedInUserDto = new LoggedInUserDto
                {
                    Id = user.Id,
                    Status = true,
                    AccessToken = token,
                    RefreshToken = refreshToken.Token,
                    Email = user.Email,
                    Fullname = user.Fullname
                };

                return Ok(loggedInUserDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(UserToRegisterDto userToRegisterDto)
        {
            try
            {
                var newUser = await _authRepository.RegisterUser(userToRegisterDto);
                if (newUser == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred adding this user.");
                }
                var newUserDto = newUser.ConvertToDto();
                return Ok(newUserDto);
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

        [HttpPost("refresh-token")]
        public async Task<ActionResult<RefreshTokenDto>> RefreshToken()
        {
            try
            {
                 var refreshToken = Request.Cookies["refreshToken"];
                if (refreshToken is null)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "You are not authorized to access this resource.");
                }
                var user = await _authRepository.GetUserByRefreshToken(refreshToken);
                if (user is null || user.TokenExpires < DateTime.Now || user.RefreshToken != refreshToken)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "You are not authorized to access this resource.");
                }
                var token = _authRepository.GenerateToken(user.Id, user.Fullname, user.Email);
                var newRefreshToken = _authRepository.GenerateRefreshToken();
                await _authRepository.SetRefreshToken(user.Id, newRefreshToken);

                var cookieOptions = new CookieOptions()
                {
                    IsEssential = true,
                    SameSite = SameSiteMode.None,
                    Expires = newRefreshToken.ExpiresAt,
                    Path = "/",
                    //Secure = true
                };
                Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

                var tokenDto = new RefreshTokenDto
                {
                    AccessToken = token,
                    RefeshToken = newRefreshToken.Token,
                    UserId = user.Id
                };
                return Ok(tokenDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("revoke/{userId:int}")]
        public async Task<ActionResult> Revoke(int userId)
        {
            try
            {
                var user = await _authRepository.GetUserById(userId);
                if (user is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "The requested user does not exist.");
                }
                await _authRepository.SetRefreshToken(user.Id, null);
                Response.Cookies.Delete("refreshToken");
                return Ok();    
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
