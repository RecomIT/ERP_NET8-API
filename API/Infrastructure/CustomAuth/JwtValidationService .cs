using Microsoft.IdentityModel.Tokens;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Infrastructure.CustomAuth
{
    public class JwtValidationService : IJwtValidationService
    {
        private readonly string _validIssuer; // Set this to the expected issuer of your API

        public JwtValidationService(string validIssuer)
        {
            _validIssuer = validIssuer;
        }

        public async Task<JwtValidationResult> ValidateTokenAsync(string token)
        {
            var result = new JwtValidationResult();

            try {
                var tokenHandler = new JwtSecurityTokenHandler();

                // Token validation parameters
                var validationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidIssuer = _validIssuer, // Set this to the expected issuer of your API
                    ValidateAudience = false,   // You may set this to true if you have a specific audience
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,  // Adjust as needed
                    RequireSignedTokens = true,
                    RequireExpirationTime = true,
                    IssuerSigningKey = GetSigningKey() // Replace with your signing key
                };

                // Validate the token
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                result.IsValid = true;
                result.Claims = principal.Claims;
            }
            catch (SecurityTokenException) {
                // Token validation failed
                result.IsValid = false;
                // Log or handle the exception as needed
            }

            return result;
        }

        private SecurityKey GetSigningKey()
        {
            // Implement logic to retrieve or generate your signing key
            // This could involve loading a key from a secure storage, etc.
            // Replace this with your actual signing key retrieval logic

            // Example: use a symmetric key
            var secret = AppSettings.SymmetricSecurityKey;
            var keyBytes = Encoding.UTF8.GetBytes(secret);
            return new SymmetricSecurityKey(keyBytes);
        }
    }
}
