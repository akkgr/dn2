
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using MongoDB.Driver;

namespace cinnamon.api
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private Models.Context db;

        public ResourceOwnerPasswordValidator(Models.Context ctx)
        {
            db = ctx;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await db.Users.Find(t => t.UserName == context.UserName).FirstOrDefaultAsync();
            if (user == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Wrong username.");
            }
            else
            {
                if (user.HashedPassword != context.Password)
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Wrong username or password.");
                }
                else
                {
                    List<Claim> c = new List<Claim>();
                    c.Add(new Claim("Role", "Administrator"));
                    context.Result = new GrantValidationResult(subject: "tasos", authenticationMethod: "custom", claims: c);
                }
            }
        }
    }
}