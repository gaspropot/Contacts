using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Contacts.Models
{
    public class PhoneNumber
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number can only contain numbers.")]
        [MaxLength(30, ErrorMessage = "Phone numbers may be between 10 and 30 characters.")]
        public string Number { get; set; }

        public virtual Contact Contact { get; set; }
    }
}