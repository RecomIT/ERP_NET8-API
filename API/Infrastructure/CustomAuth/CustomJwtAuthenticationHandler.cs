using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace API.Infrastructure.CustomAuth
{
    public class CustomJwtAuthenticationHandler : AuthenticationHandler<JwtBearerOptions>
    {
        private readonly IJwtValidationService _jwtValidationService;

        public CustomJwtAuthenticationHandler(IOptionsMonitor<JwtBearerOptions> options,
                                              ILoggerFactory logger,
                                              UrlEncoder encoder,
                                              ISystemClock clock,
                                              IJwtValidationService jwtValidationService)
            : base(options, logger, encoder, clock)
        {
            _jwtValidationService = jwtValidationService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Get the token from the request
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token)) {
                return AuthenticateResult.Fail("No token provided");
            }

            // Validate the token using your custom service
            var validationResult = await _jwtValidationService.ValidateTokenAsync(token);

            if (!validationResult.IsValid) {
                return AuthenticateResult.Fail("Token validation failed");
            }

            // If validation is successful, create claims and build the principal
            var claims = validationResult.Claims;
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new CustomClaimsPrincipal(identity);

            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
