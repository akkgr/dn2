using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentValidation;
using FluentValidation.Attributes;

namespace cinnamon.api.Models
{
    [Validator(typeof(RepairValidator))]
    public class Repair
    {
        public Repair()
        {
            History = new List<RepairHistory>();
            Products = new List<Product>();
        }

        public string Id { get; set; }

        [Required]
        public Person Customer { get; set; }

        [Required]
        public Product Product { get; set; }

        public IList<RepairHistory> History { get; set; }
        
        public string Description { get; set; }

        public string Auth { get; set; }

        public string Remarks { get; set; }

        public bool Bag { get; set; }

        public bool Mouse { get; set; }

        public bool PowerSupply { get; set; }

        public string Other { get; set; }

        public string IdCode { get; set; }

        public string Actions { get; set; }

        public IList<Product> Products { get; set; }

        public DateTime Issued { get { return History.OrderBy(t => t.Occurred).First().Occurred; } }

        public string Status { get { return History.OrderBy(t=>t.Occurred).Last().Status.ToString(); } }

        public string AllProducts { get { return string.Join(", ", Products.Select(t => t.Title)); } }
                
        public decimal NetValue { get { return Products.Sum(t => t.NetValue); } }

        public decimal TaxValue { get { return Products.Sum(t => t.TaxValue); } }

        public decimal FinalValue { get { return Products.Sum(t => t.FinalValue); } }
    }

    public class RepairValidator : AbstractValidator<Repair>
    {
        public RepairValidator()
        {
            RuleFor(x => x.History).Must(history => history.Count > 0);
        }
    }
}