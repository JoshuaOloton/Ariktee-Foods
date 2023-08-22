using ArikteeFoods.API.Data;
using ArikteeFoods.API.Entities;
using ArikteeFoods.API.Repositories.Contracts;
using ArikteeFoods.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

        public async Task<User?> GetUserById(int userId) => await _arikteeDbContext.Users.FindAsync(userId);

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
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt; // token string
        }

        public async Task<User?> GetUser(UserToLoginDto userToLoginDto)
        {
            var user = await _arikteeDbContext.Users.Where(e => e.Email == userToLoginDto.Email).FirstOrDefaultAsync();
            if (user is null || !BCrypt.Net.BCrypt.Verify(userToLoginDto.Password, user.Passwordhash)) return null;
            return user;
        }
    }
}
