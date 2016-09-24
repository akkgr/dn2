using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using FluentValidation.Attributes;

namespace cinnamon.api.Models
{
    [Validator(typeof(ProductValidator))]
    public class Product
    {
        public Product()
        {
            Categories = new List<string>();
        }

        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Code { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }

        public string Description { get; set; }

        public string PartNo { get; set; }

        public string SerialNo { get; set; }
        
        public IList<string> Categories { get; set; }

        public string BarCode { get; set; }

        public decimal NetValue { get; set; }

        public decimal Tax { get; set; }

        public decimal TaxValue { get { return Math.Round( (NetValue * Tax / 100M) , 2); } }

        public decimal FinalValue { get { return NetValue + TaxValue; } }

        public virtual string AllCategories { get; set; }

        public virtual bool Service { get; set; }
    }

    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Categories).Must(categories => categories.Count > 0);
        }
    }
}