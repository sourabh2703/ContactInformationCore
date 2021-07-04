using ContactInformationCore.Interface;
using ContactInformationCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ContactInformationCore.WebAPI
{
    public class ContactService : IContactInformation
    {
        readonly DatabaseContaxt _dataContext;

        public ContactService(DatabaseContaxt context)
        {
            _dataContext = context;
        }

        public void Save(ContactInformation Contact)
        {
            try
            {
                _dataContext.Contacts.Add(Contact);
              var response=  _dataContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Delete(int? id)
        {
            try
            {
                ContactInformation contact = _dataContext.Contacts.Find(id);
                _dataContext.Contacts.Remove(contact);

                return _dataContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ContactInformation GetByID(int? id)
        {
            try
            {
                return _dataContext.Contacts.Find(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(ContactInformation ContacttoUpdate, ContactInformation Contact)
        {
            try
            {
                if (ContacttoUpdate != null)
                {
                    Contact.First_Name = ContacttoUpdate.First_Name;
                    Contact.Last_Name = ContacttoUpdate.Last_Name;
                    Contact.Email = ContacttoUpdate.Email;
                    Contact.Phone_Number = ContacttoUpdate.Phone_Number;
                    Contact.Status = ContacttoUpdate.Status;

                    //Commit the transaction
                    _dataContext.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<ContactInformation> GetAll()
        {
            try
            {
                var ContactList = (from contact in _dataContext.Contacts
                                   select new ContactInformation
                                   {
                                       Id = contact.Id,
                                       First_Name = contact.First_Name,
                                       Last_Name = contact.Last_Name,
                                       Email = contact.Email,
                                       Phone_Number = contact.Phone_Number,
                                       Status = contact.Status
                                   }).ToList();

                return ContactList;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
