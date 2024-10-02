using System.Collections.Generic;
using System.Security.Claims;

namespace API.Infrastructure.CustomAuth
{
    public class JwtValidationResult
    {
        public bool IsValid { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
    }
}
