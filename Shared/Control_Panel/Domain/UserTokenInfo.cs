using Microsoft.AspNetCore.Identity;
using System;

namespace Shared.Control_Panel.Domain
{
    public class UserTokenInfo : IdentityUserToken<Guid>
    {
        public string BrowserName { get; set; }
        public string BrowserVersion { get; set; }
    }
}
