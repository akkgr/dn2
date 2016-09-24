using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace cinnamon.api.Models
{
    [Validator(typeof(RequestValidator))]
    public class Request
    {
        public Request()
        {
            History = new List<RequestHistory>();
            Products = new List<Product>();
        }

        public string Id { get; set; }

        [Required]
        public string RequestTypeId { get; set; }

        [Required]
        public DateTime Inserted { get; set; }

        public DateTime? Scheduled { get; set; }

        [Required]
        public Person Customer { get; set; }

        public string Representative { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Description { get; set; }

        public IList<RequestHistory> History { get; set; }

        public IList<Product> Products { get; set; }

        public string IdCode { get; set; }
        
        public string AllProducts { get { return string.Join(", ", Products.Select(t => t.Title)); } }

        public decimal NetValue { get { return Products.Sum(t => t.NetValue); } }

        public decimal TaxValue { get { return Products.Sum(t => t.TaxValue); } }

        public decimal FinalValue { get { return Products.Sum(t => t.FinalValue); } }

        public string Status { get { return History.OrderBy(t => t.Occurred).Last().Status.ToString(); } }

        public virtual string RequestType { get; set; }
    }

    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(x => x.Scheduled).GreaterThanOrEqualTo(x => x.Inserted);
        }
    }
}