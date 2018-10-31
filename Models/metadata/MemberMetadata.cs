using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GCDGameStore.Models
{

    [ModelMetadataType(typeof(MemberMetadata))]

    public partial class Member : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MailingCity == null && MailingPostalCode == null && MailingProvince == null&& MailingStreetAddress == null)
            {

            }
            else if (MailingCity != null && MailingPostalCode != null && MailingProvince != null && MailingStreetAddress != null)
            {

            }
            else
            {
                if (MailingCity == null)
                {
                    yield return new ValidationResult("The mailing city can not be empty", new[] { nameof(MailingCity) });
                }
                if (MailingPostalCode == null)
                {
                    yield return new ValidationResult("The mailing postal code can not be empty", new[] { nameof(MailingPostalCode) });
                }
                if (MailingProvince == null)
                {
                    yield return new ValidationResult("The mailing province can not be empty", new[] { nameof(MailingProvince) });
                }
                if (MailingStreetAddress == null)
                {
                    yield return new ValidationResult("The mailing street address can not be empty", new[] { nameof(MailingStreetAddress) });
                }
            }

            if (ShippingCity == null && ShippingPostalCode == null && ShippingProvince == null && ShippingStreetAddress == null)
            {

            }
            else if (ShippingCity != null && ShippingCity != null && ShippingProvince != null && ShippingStreetAddress != null)
            {

            }
            else
            {
                if (ShippingCity == null)
                {
                    yield return new ValidationResult("The shipping city can not be empty", new[] { nameof(ShippingCity) });
                }
                if (ShippingPostalCode == null)
                {
                    yield return new ValidationResult("The shipping postal code can not be empty", new[] { nameof(ShippingPostalCode) });
                }
                if (ShippingProvince == null)
                {
                    yield return new ValidationResult("The shipping province can not be empty", new[] { nameof(ShippingProvince) });
                }
                if (ShippingStreetAddress == null)
                {
                    yield return new ValidationResult("The shipping street address can not be empty", new[] { nameof(ShippingStreetAddress) });
                }
            }

            yield return ValidationResult.Success;

        }
    }

    public partial class MemberMetadata
    {
        public int MemberId { get; set; }

        [Required]
        [MinLength(5)]
        [StringLength(15)]
        public string Username { get; set; }

        [Required]
        public string PwHash { get; set; }

        public string PwSalt { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        public string MailingStreetAddress { get; set; }
        [RegularExpression(@"^[A-Za-z]\d[A-Za-z] ?\d[A-Za-z]\d$", ErrorMessage = "postal(zip) code is invalid")]
        public string MailingPostalCode { get; set; }
        public string MailingCity { get; set; }
        public string MailingProvince { get; set; }

        public string ShippingStreetAddress { get; set; }
        [RegularExpression(@"^[A-Za-z]\d[A-Za-z] ?\d[A-Za-z]\d$", ErrorMessage = "postal(zip) code is is invalid")]
        public string ShippingPostalCode { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingProvince { get; set; }

        public virtual ICollection<Friend> MyFriends { get; set; }
        public virtual ICollection<Friend> FriendsOf { get; set; }
    }

   

}
