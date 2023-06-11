using BlazorCore.Data.Models;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace BlazorCore.Areas.Services
{
    public abstract class CurrentUserService<T> where T : AppUserModel
    {
        public CurrentUserService(IConfiguration configuration)
        {
            var option = configuration.GetSection(nameof(RequireMFA)).Value;

            _ = bool.TryParse(option, out bool result);

            RequireMFA = result;
        }

        public T? CurrentUser { get; private set; }                               
        protected bool RequireMFA { get; private set; }                              

        public async Task AuthenticateUser(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal != null && claimsPrincipal.Identity != null)
            {
                var isAuthenticated = claimsPrincipal.Identity.IsAuthenticated;                            

                if (isAuthenticated && !RequireMFA ||
                    isAuthenticated && claimsPrincipal.HasClaim("amr", "mfa"))
                {
                    CurrentUser = await SetCurrentUser(claimsPrincipal);                    
                }
            }
        }

        protected abstract Task<T> SetCurrentUser(ClaimsPrincipal claimsPrincipal);
    }
}
