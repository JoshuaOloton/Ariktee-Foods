using ArikteeFoods.API.Entities;
using ArikteeFoods.API.Extensions;
using ArikteeFoods.API.Repositories.Contracts;
using ArikteeFoods.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ArikteeFoods.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public UserController(IAuthRepository authRepository)
        {
            this._authRepository = authRepository;
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

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(UserToLoginDto userToLoginDto)
        {
            try
            {
                var user = await _authRepository.GetUser(userToLoginDto);
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "This user does not exist.");
                }
                String fullName = $"{user.Surname} {user.Firstname}";
                await Console.Out.WriteLineAsync($"----- FULLNAME: {fullName} ----");
                Debug.WriteLine($"----- FULLNAME: {fullName} ----");
                var token = _authRepository.GenerateToken(user.Id, fullName, user.Email);
                await Console.Out.WriteLineAsync($"----- TOKEN: {token} ----");

                var userDto = user.ConvertToDto(token);
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //[HttpPost]
        //public async Task<ActionResult<UserDto>> Register(LoginUserDto loginUserDto)
        //{
        //    return null;
        //}
    }
}
