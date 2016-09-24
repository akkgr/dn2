using System.ComponentModel.DataAnnotations;

namespace cinnamon.api.Models
{
    public class PersonInfo
    {
        [Required]
        public PersonInfoType PersonInfoType { get; set; }

        [Required]
        public string Value { get; set; }
    }
}