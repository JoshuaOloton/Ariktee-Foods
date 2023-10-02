using ArikteeFoods.API.Data;
using ArikteeFoods.API.Entities;
using ArikteeFoods.API.Exceptions;
using ArikteeFoods.API.Repositories.Contracts;
using ArikteeFoods.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Validations;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ArikteeFoods.API.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ArikteeDbContext _arikteeDbContext;

        public AuthRepository(IConfiguration configuration, ArikteeDbContext arikteeDbContext)
        {
            this._configuration = configuration;
            this._arikteeDbContext = arikteeDbContext;
        }

        public async Task<User?> GetUserById(int userId) => await _arikteeDbContext.Users.Where(e => e.Id == userId).Include(e => e.DeliveryAddresses).FirstOrDefaultAsync();

        public String GenerateToken(int userId, String fullName, String email)
        {
            var jwtAudience = _configuration.GetSection("JWTSettings:Audience").Value!;
            var jwtIssuer = _configuration.GetSection("JWTSettings:Issuer").Value!;
            List<Claim> claims = new()
            {
                new Claim(JwtRegisteredClaimNames.NameId, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, fullName),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Aud, jwtAudience),
                new Claim(JwtRegisteredClaimNames.Iss, jwtIssuer),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            SymmetricSecurityKey key = new (Encoding.UTF8.GetBytes(_configuration.GetSection("JWTSettings:JWTToken").Value!));

            SigningCredentials credentials = new (key: key, algorithm: SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new(
                claims: claims,
                expires: DateTime.Now.AddSeconds(20),
                signingCredentials: credentials
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt; // token string
        }

        public RefreshToken GenerateRefreshToken()
        {
            var refreshTokenModel = new RefreshToken()
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                CreatedAt = DateTime.Now,
                ExpiresAt = DateTime.Now.AddDays(7)
            };
            return refreshTokenModel;
        }

        public async Task SetRefreshToken(int userId, RefreshToken? refreshToken)
        {
            var user = await _arikteeDbContext.Users.Where(e => e.Id == userId).FirstOrDefaultAsync();
            if (user != null)
            {
                user.RefreshToken = refreshToken?.Token;
                user.TokenCreated = refreshToken?.CreatedAt;
                user.TokenExpires = refreshToken?.ExpiresAt;
                await _arikteeDbContext.SaveChangesAsync();
            }
        }

        public async Task<User?> GetUser(UserToLoginDto userToLoginDto)
        {
            var user = await _arikteeDbContext.Users.Where(e => e.Email == userToLoginDto.Email).Include(e => e.DeliveryAddresses).FirstOrDefaultAsync();
            if (user is null || !BCrypt.Net.BCrypt.Verify(userToLoginDto.Password, user.Passwordhash)) return null;
            return user;
        }

        public async Task<User> RegisterUser(UserToRegisterDto userToRegisterDto)
        {
            var user = await _arikteeDbContext.Users.Where(e => e.Email == userToRegisterDto.Email).Include(e => e.DeliveryAddresses).FirstOrDefaultAsync();
            if (user is not null)
            {
                throw new HttpResponseException(400, "A user with this email already exists.");
            }
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(userToRegisterDto.Password);
            var newuser = new User
            {
                Passwordhash = passwordHash, // use newly generated hash value
                Fullname = userToRegisterDto.FullName,
                Email = userToRegisterDto.Email,
                PhoneNo = userToRegisterDto.PhoneNo,
                Password = userToRegisterDto.Password,
            };
            await _arikteeDbContext.Users.AddAsync(newuser);
            await _arikteeDbContext.SaveChangesAsync();
            return newuser;
        }

        public async Task<User?> GetUserByRefreshToken(String refreshToken) => await _arikteeDbContext.Users.Where(e => e.RefreshToken == refreshToken).FirstOrDefaultAsync();
    }
}
