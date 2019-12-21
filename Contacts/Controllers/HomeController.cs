using Contacts.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Contacts.ViewModels;
using Contacts.Models;

namespace Contacts.Controllers
{
    public class HomeController : Controller
    {
        private ContactsContext _db = new ContactsContext();

        //Brings list of contacts to View. It is the only non-async controller Action.
        public ActionResult Index()
        {
            var contacts = _db.Contacts.ToList();

            return View(contacts);
        }

        //Asynchronously gets a new contact's details via ajax and writes it in the database. Id is assigned automatically.
        public JsonResult CreateContact(string name, string email, string address, List<string> phoneNumbers)
        {
            Contact contactToAdd = new Contact { Name = name, Email = email, Address = address };

            _db.Contacts.Add(contactToAdd);
            _db.SaveChanges();

            if (phoneNumbers != null)
            {
                foreach (var phone in phoneNumbers)
                {
                    PhoneNumber numberToAdd = new PhoneNumber { Number = phone, Contact = _db.Contacts.FirstOrDefault(x => x.Email == email) };
                    _db.PhoneNumbers.Add(numberToAdd);
                    _db.SaveChanges();
                }
            }
            
            return Json(JsonRequestBehavior.AllowGet);
        }

        //This method is called asynchronously via ajax when a contact is created OR edited, checking if email already exists(returns emailFound = true if email already exists, which won't let contact be added/edited). 
        //IF it is called for editing of already existing contact, it will exclude said contact's current email, thus returning false if the user decides not to change the email(edit will be completed without problems).
        public JsonResult EmailCheck(string email, int? Id)
        {
            bool emailFound;

            if (Id != 0)
            {
                emailFound = _db.Contacts.Any(x => x.Email == email && x.Id != Id);
            }
            else
            {
                emailFound = _db.Contacts.Any(x => x.Email == email);
            }
   
            return Json(emailFound, JsonRequestBehavior.AllowGet);
        }

        //This method is called when the user edits a contact. It brings forth the selected contact's details to pre-fill all input sections that already exist.
        public JsonResult GetById(int Id)
        {
            Contact selectedContact = _db.Contacts.FirstOrDefault(x => x.Id == Id);
            ContactIndexData VM = new ContactIndexData { Id = selectedContact.Id, Name = selectedContact.Name, Email = selectedContact.Email, Address = selectedContact.Address};


            if (selectedContact.PhoneNumbers != null)
            {
                foreach (var phone in selectedContact.PhoneNumbers)
                {
                    VM.PhoneNumbers.Add(new PhoneVM { Id = phone.Id, Number = phone.Number });
                }
            }

            return Json(VM, JsonRequestBehavior.AllowGet);
        }

        //Completes the editing of a pre-existing contact(when the user clicks "Update" button), assigning new values to the selected contact in the database. Method first deletes all pre-existing phone numbers, then
        //writes the new ones, if they exist. User leaving them blank or deleting them means the selected contact no longer has any phone numbers, hence they get deleted but no new ones will be added(since phoneNumbers 
        //list will be empty).
        public JsonResult UpdateContact(int Id, string name, string email, string address, List<string> phoneNumbers)
        {
            Contact contactToUpdate = _db.Contacts.FirstOrDefault(x => x.Id == Id);

            if (contactToUpdate != null)
            {
                contactToUpdate.Name = name;
                contactToUpdate.Email = email;
                contactToUpdate.Address = address;

                contactToUpdate.PhoneNumbers.Clear();

                if (phoneNumbers != null)
                {
                    foreach (var phone in phoneNumbers)
                    {
                        PhoneNumber numberToAdd = new PhoneNumber { Number = phone, Contact = _db.Contacts.FirstOrDefault(x => x.Id == Id) };
                        _db.PhoneNumbers.Add(numberToAdd);
                    }
                }
                _db.SaveChanges();
            }

            return Json(JsonRequestBehavior.AllowGet);
        }

        //First finds and deletes selected contact's phone numbers, if it has any, and then the selected contact itself(since there cannot be a phone number without an assigned contact to it).
        public JsonResult DeleteContact(int Id)
        {
            Contact contactToDelete = _db.Contacts.FirstOrDefault(x => x.Id == Id);

            var affectedPhoneNumbers = _db.PhoneNumbers.Where(x => x.Contact.Id == Id);

            if (contactToDelete != null)
            {
                if (affectedPhoneNumbers != null)
                {
                    foreach (var phone in affectedPhoneNumbers)
                    {
                        _db.PhoneNumbers.Remove(phone);
                    }
                }

                _db.Contacts.Remove(contactToDelete);
                _db.SaveChanges();
            }

            return Json(JsonRequestBehavior.AllowGet);
        }

    }
}