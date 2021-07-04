using ContactInformationCore.Interface;
using ContactInformationCore.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ContactInformationCore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private IContactInformation _IContact;

        public ContactsController(IContactInformation IContact)
        {
            _IContact = IContact;
        }

        // GET: api/Contacts
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                IEnumerable<ContactInformation> contact = _IContact.GetAll();
                if (contact == null)
                {
                    return NotFound("Records not found");
                }

                return Ok(contact);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        // GET: api/Contacts/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int? id)
        {
            try
            {
                if (id == null) { return BadRequest(); }

                ContactInformation contact = _IContact.GetByID(id);

                if (contact == null)
                {
                    return NotFound("Record not found for Id:" + id);
                }

                return Ok(contact);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // PUT: api/Contacts/5
        [HttpPut("{id}")]
        public IActionResult Put(int? id, ContactInformation contactToUpdate)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ContactInformation contact = _IContact.GetByID(id);

                    if (contact == null)
                    {
                        return NotFound("Record not found for Update");
                    }

                    _IContact.Update(contactToUpdate, contact);

                    return Ok("Record Updated");
                }
                catch (Exception ex)
                {

                    return BadRequest(ex);
                }
            }
            return BadRequest();
        }

        // POST: api/Contacts
        [HttpPost]
        public IActionResult Post(ContactInformation contact)
        {
            try
            {
                _IContact.Save(contact);

                if (contact.Id > 0)
                    return CreatedAtRoute("Get", new { Id = contact.Id }, contact);
                else
                    return BadRequest("Record not saved");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int? id)
        {
            try
            {
                if (id == null) { return BadRequest("Id should not be null"); }

                ContactInformation contact = _IContact.GetByID(id);

                if (contact == null)
                {
                    return NotFound("Record not found for Delete");
                }

                var result = _IContact.Delete(id);

                return Ok("Record Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
