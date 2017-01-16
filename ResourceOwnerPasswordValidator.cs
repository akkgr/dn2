
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.MongoDB;

namespace cinnamon.api
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;

        public ResourceOwnerPasswordValidator(SignInManager<IdentityUser> sctx, UserManager<IdentityUser> uctx)
        {
            signInManager = sctx;
            userManager = uctx;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var result = await signInManager.PasswordSignInAsync(context.UserName, context.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = await userManager.FindByNameAsync(context.UserName);
                List<Claim> c = new List<Claim>();
                context.Result = new GrantValidationResult(subject: "tasos", authenticationMethod: "custom", claims: c);
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid login attempt.");
            }
        }
    }
}