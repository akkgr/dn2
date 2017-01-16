using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.MongoDB;
using MongoDB.Bson.Serialization.Attributes;

namespace cinnamon.api.Models
{
    public class User : IdentityUser
    {
        [BsonIgnore]
        public string Password { get; set; }
        [BsonIgnore]
        public string OldPassword { get; set; }
        [BsonIgnore]
        public string NewPassword { get; set; }
        [BsonIgnore]
        public string ConfirmPassword { get; set; }
    }
}