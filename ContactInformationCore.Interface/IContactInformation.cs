using ContactInformationCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContactInformationCore.Interface
{
    public interface IContactInformation
    {
        void Save(ContactInformation Contact);
        int Delete(int? id);
        ContactInformation GetByID(int? id);
        void Update(ContactInformation ContacttoUpdate, ContactInformation Contact);
        IEnumerable<ContactInformation> GetAll();
    }
}
