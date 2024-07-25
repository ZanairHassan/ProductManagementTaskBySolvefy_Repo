using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class TokenUtility
    {
        private readonly IConfiguration _configuration;
        public TokenUtility(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GenerateToken(string email)
        {
            //byte[] keyBytes = new byte[32]; // 32 bytes = 256 bits
            //using (var rng = new RNGCryptoServiceProvider())
            //{
            //    rng.GetBytes(keyBytes);
            //}
            var key = _configuration["Jwt:SecretKey"];
            var keyBytes = Encoding.UTF8.GetBytes(key);

            var key1 = new SymmetricSecurityKey(keyBytes);
            var signingCredentials = new SigningCredentials(key1, SecurityAlgorithms.HmacSha256);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Email, email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = signingCredentials
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
