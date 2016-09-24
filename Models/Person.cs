using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentValidation;
using FluentValidation.Attributes;

namespace cinnamon.api.Models
{
    [Validator(typeof(PersonValidator))]
    public class Person
    {
        public Person()
        {
            PersonTypes = new List<PersonType>();
            Addresses = new List<Address>();
            Phones = new List<Phone>();
            PersonInfos = new List<PersonInfo>();
        }

        public string Id { get; set; }

        public string Code { get; set; }

        public IList<PersonType> PersonTypes { get; set; }

        [Required]
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Company { get; set; }

        public string TaxId { get; set; }

        public IList<Address> Addresses { get; set; }
        
        public IList<Phone> Phones { get; set; }

        public IList<PersonInfo> PersonInfos { get; set; }

        public string FullName { get { return string.Format("{0} {1}", LastName, FirstName); } }
        public string AllPhones { get { return string.Join(", ", this.Phones.Select(t => t.Value)); } }

        public string AllPersonTypes
        {
            get
            {
                var s = string.Join(", ", this.PersonTypes);

                return s;
            }
        }
    }

    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(x => x.Phones).Must(phones => phones.Count > 0);
        }
    }
}