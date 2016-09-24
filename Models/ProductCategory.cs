using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cinnamon.api.Models
{
    public class ProductCategory
    {
        public ProductCategory()
        {
            Categories = new List<ProductCategory>();
        }

        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string ParentId { get; set; }

        public bool Service { get; set; }

        public virtual List<ProductCategory> Categories { get; set; }
    }
}