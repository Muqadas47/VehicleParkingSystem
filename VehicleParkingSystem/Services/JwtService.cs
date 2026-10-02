using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VehicleParkingSystem.Models;

namespace VehicleParkingSystem.Services
{
    public class JwtService
    {
        private readonly string secretKey =
            "VehicleParkingSystem_SuperSecretKey_2026_123456789";

        public string GenerateToken(User user)
        {
            var securityKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(secretKey)
                );

            var credentials =
                new SigningCredentials(
                    securityKey,
                    SecurityAlgorithms.HmacSha256
                );

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.UserId.ToString()
                ),

                new Claim(
                    ClaimTypes.Email,
                    user.Email
                ),

                new Claim(
                    ClaimTypes.Role,
                    user.Role
                )
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }


        public ClaimsPrincipal ValidateToken(string token)
        {
            var securityKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(secretKey)
                );

            var validationParameters =
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,

                    IssuerSigningKey = securityKey,

                    ValidateIssuer = false,

                    ValidateAudience = false,

                    ValidateLifetime = true,

                    ClockSkew = TimeSpan.Zero
                };


            var tokenHandler =
                new JwtSecurityTokenHandler();


            SecurityToken validatedToken;


            var principal =
                tokenHandler.ValidateToken(
                    token,
                    validationParameters,
                    out validatedToken
                );


            return principal;
        }
    }
}