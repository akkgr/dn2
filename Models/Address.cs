using System.ComponentModel.DataAnnotations;

namespace cinnamon.api.Models
{
    public class Address
    {
        [Required]
        public AddressType AddressType { get; set; }

        [Required]
        public string Area { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string StreetNumber { get; set; }

        [Required]
        public string PostalCode { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }

        public string Title
        {
            get
            {
                return string.Format("{1} {2}, {0} {3}", Area, Street, StreetNumber, PostalCode);
            }
        }
    }
}