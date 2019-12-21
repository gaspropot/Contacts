using Contacts.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace Contacts.DAL
{
    public class HelperForSeed
    {
        public static void SeedData(ContactsContext context)
        {
            SeedContacts(context);
            SeedPhoneNumbers(context);
            context.SaveChanges();
        }

        private static void SeedContacts(ContactsContext context)
        {
            var contacts = new List<Contact>
            {
                new Contact{Id = 1, Name="George White", Email = "something@something.com", Address = "Some Address 21"},
                new Contact{Id = 2, Name="Jack Black", Email = "something2@something2.com", Address = "Some other Address 22"},
                new Contact{Id = 3, Name="Becky Green", Email = "something3@something3.com", Address = "Some random Address 23"},
                new Contact{Id = 4, Name="Samantha Red", Email = "something4@something4.com", Address = "Some strange Address 24"},

            };

            contacts.ForEach(c => context.Contacts.AddOrUpdate(p => p.Name, c));
            context.SaveChanges();
        }

        private static void SeedPhoneNumbers(ContactsContext context)
        {
            var phoneNumbers = new List<PhoneNumber>
            {
                new PhoneNumber{Id = 1, Number = "12345789011", Contact = context.Contacts.FirstOrDefault(x => x.Name == "George White")},
                new PhoneNumber{Id = 2, Number = "22222111112", Contact = context.Contacts.FirstOrDefault(x => x.Name == "George White")},
                new PhoneNumber{Id = 3, Number = "3333322222", Contact = context.Contacts.FirstOrDefault(x => x.Name == "Jack Black")},
                new PhoneNumber{Id = 4, Number = "4444433333", Contact = context.Contacts.FirstOrDefault(x => x.Name == "Becky Green")},
            };

            phoneNumbers.ForEach(c => context.PhoneNumbers.AddOrUpdate(p => p.Number, c));
            context.SaveChanges();
        }
    }
}