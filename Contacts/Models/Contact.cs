using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Contacts.Models
{
    public class Contact
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "You can only use letters, as well as spaces.")]
        [MinLength(2, ErrorMessage = "Name may be between 2 and 100 characters.")]
        [MaxLength(100, ErrorMessage = "Name may be between 2 and 100 characters.")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(254)]
        public string Email { get; set; }

        [MaxLength(100, ErrorMessage = "Address may be between 2 and 100 characters.")]
        public string Address { get; set; }

        public virtual ICollection<PhoneNumber> PhoneNumbers {get; set;}
    }
}