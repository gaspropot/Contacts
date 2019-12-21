using Contacts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contacts.ViewModels
{
    public class ContactIndexData
    {
        public ContactIndexData()
        {
            this.PhoneNumbers = new List<PhoneVM>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public List<PhoneVM> PhoneNumbers {get; set;}


    }
}