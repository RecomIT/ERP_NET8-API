using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Shared.Services;
using System.IdentityModel.Tokens.Jwt;

namespace Shared.Helpers.Token
{
    public static class TokenHelper
    {

        public static string GetDatabaseNameFromToken(IHttpContextAccessor httpContextAccessor, IClientDatabase clientDatabase)
        {
            // Retrieve the current HttpContext
            var httpRequest = httpContextAccessor.HttpContext;

            // Check if HttpContext is available
            if (httpRequest == null)
                throw new Exception("HttpContext is null");

            // Retrieve the JWT token from the request headers
            var token = httpRequest.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token == null)
                throw new Exception("Authorization token not found");

            // Parse the JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var parsedToken = tokenHandler.ReadJwtToken(token);

            // Retrieve the user info claim from the token
            var userInfoClaim = parsedToken.Claims.FirstOrDefault(c => c.Type == "userinfo");
            if (userInfoClaim == null)
                throw new Exception("User info claim not found in token");

            // Decrypt the user info claim
            var decryptedUserInfo = Decryptor.DecryptStringAES(userInfoClaim.Value);

            // Deserialize the JSON string into a JObject
            JObject userInfoObject = JObject.Parse(decryptedUserInfo);

            // Extract the username and userId
            string username = (string)userInfoObject["username"];

            // Get the database name using the username
            return clientDatabase.GetDatabaseName(username);
        }
    }
}
