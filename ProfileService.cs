
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.MongoDB;

namespace cinnamon.api
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<IdentityUser> userManager;

        public ProfileService(UserManager<IdentityUser> uctx)
        {
            userManager = uctx;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            string subject = context.Subject.Claims.ToList().Find(s => s.Type == "sub").Value;
            try
            {
                var user = userManager.FindByNameAsync(subject).Result;
                List<Claim> claims = new List<Claim>();
                foreach (var r in user.Roles)
                {
                    claims.Add(new Claim("role", r));
                }
                foreach (var c in user.Claims)
                {
                    claims.Add(new Claim(c.Type, c.Value));
                }
                context.IssuedClaims = claims;
                return Task.FromResult(0);
            }
            catch
            {
                return Task.FromResult(0);
            }
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.FromResult(0);
        }
    }
}