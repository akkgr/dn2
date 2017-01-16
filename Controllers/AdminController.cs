using cinnamon.api.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.MongoDB;

namespace cinnamon.api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;

        public AdminController(UserManager<IdentityUser> ctx)
        {
            this.userManager = ctx;
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]User value)
        {
            var user = await userManager.FindByNameAsync(value.UserName);
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, token, value.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors);
                return this.BadRequest(errors);
            }

            return this.Ok();
        }
        
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]User value)
        {
            if(value.NewPassword != value.ConfirmPassword)
            {
                return this.BadRequest("New Password mismatch.");
            }
            
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var result = await userManager.ChangePasswordAsync(user, value.OldPassword, value.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors);
                return this.BadRequest(errors);
            }

            return this.Ok();
        }
    }
}