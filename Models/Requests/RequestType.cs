using System.ComponentModel.DataAnnotations;

namespace cinnamon.api.Models
{
    public class RequestType
    {
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }
    }
}