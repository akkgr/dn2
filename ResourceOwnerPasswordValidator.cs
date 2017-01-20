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

        public ResourceOwnerPasswordValidator(SignInManager<IdentityUser> sctx)
        {
            signInManager = sctx;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var result = await signInManager.PasswordSignInAsync(context.UserName, context.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                context.Result = new GrantValidationResult(subject: context.UserName, authenticationMethod: "custom");
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid login attempt.");
            }
        }
    }
}