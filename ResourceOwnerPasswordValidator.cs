
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace cinnamon.api
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public ResourceOwnerPasswordValidator()
        {

        }

        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {            
            if (context.UserName != context.Password)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Wrong username or password.");                
            }
            else
            {
                List<Claim> c = new List<Claim>();
                c.Add(new Claim("Role","Administrator"));
                context.Result = new GrantValidationResult(subject: "tasos", authenticationMethod: "custom", claims: c);
            }

            return Task.FromResult(0);
        }
    }
}