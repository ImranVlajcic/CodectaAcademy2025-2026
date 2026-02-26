using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ExpenseTracker.Application.AccountFolders.Interface.Application;
using ExpenseTracker.Domain.AccountData;
using ExpenseTracker.Application.AccountFolders.Services;
using Microsoft.IdentityModel.Tokens;

namespace ExpenseTracker.Infrastructure.Authentication.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions _jwtOptions;

        public JwtTokenGenerator(JwtOptions jwtOptions)
        {
            _jwtOptions = jwtOptions ?? throw new ArgumentNullException(nameof(jwtOptions));
        }

        public string GenerateAccessToken(Account account)
        {
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret)),
                SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, account.userID.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, account.email),
                new Claim(JwtRegisteredClaimNames.GivenName, account.realName),
                new Claim(JwtRegisteredClaimNames.FamilyName, account.realSurname),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userId", account.userID.ToString()),
                new Claim("username", account.username)
            };

            var securityToken = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        /*public int? ValidateRefreshToken(string token)
        {
            try
            {
                Convert.FromBase64String(token);
                return null; 
            }
            catch
            {
                return null;
            }
        }*/
    }
}
