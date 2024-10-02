using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Shared.OtherModels.User;
using Shared.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;

namespace Shared.Helpers
{
    public static class UserObjects
    {
        public static AppUser UserData(HttpRequest httpRequest)
        {
            AppUser request = new AppUser();
            if (httpRequest == null)
                throw new Exception("Http Request is empty");
            if (httpRequest != null) {

                //StringValues AccessToken = new StringValues();
                //httpRequest.Headers.TryGetValue("access_token", out AccessToken);
                //request.AccessToken = AccessToken;

                //StringValues x_user_info = new StringValues();
                //httpRequest.Headers.TryGetValue("x_site_info", out x_user_info);

                //var userInfo = Utility.JsonToObject<UserHeader>(x_user_info);

                // Check if HttpContext is available
                if (httpRequest == null)
                    throw new Exception("HttpContext is null");

                // Retrieve the JWT token from the request headers
                var token = httpRequest.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
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
                var decryptedUserInfo = "";
                try
                {
                    decryptedUserInfo = Decryptor.DecryptStringAES(userInfoClaim.Value);
                }
                catch (Exception)
                {
                     decryptedUserInfo = userInfoClaim.Value;
                }
                
                var userInfo = Utility.JsonToObject<UserHeader>(decryptedUserInfo);
                request.UserId = userInfo.userId;
                request.Username = userInfo.username;
                request.BranchId = userInfo.branchId;
                request.DivisionId = userInfo.divisionId;
                request.CompanyId = userInfo.companyId;
                request.OrganizationId = userInfo.organizationId;
                request.DepartmentId = userInfo.departmentId;
                request.DesignationId = userInfo.designationId;
                request.RoleId = userInfo.roleId;
                request.RoleName = userInfo.roleName;
                request.EmployeeId = userInfo.employeeId;
                request.EmployeeCode = userInfo.employeeCode;
            }
            return request;
        }
    }
    

}
