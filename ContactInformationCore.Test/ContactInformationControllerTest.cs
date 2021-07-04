using ContactInformationCore.Interface;
using ContactInformationCore.Model;
using ContactInformationCore.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace ContactInformationCore.Test
{
    public class ContactInformationControllerTest
    {
        private readonly Mock<IContactInformation> mockContact;
        private readonly ContactsController controller;

        public ContactInformationControllerTest()
        {
            mockContact = new Mock<IContactInformation>();

            controller = new ContactsController(mockContact.Object);
        }

        #region GetAll
        [Fact]
        public void Should_Not_Get_ContactInformation()
        {
            //Arrange           
            mockContact.Setup(x => x.GetAll()).Returns(()=>null);
            //Act
            var data = controller.Get();
            var result = data as NotFoundObjectResult;
            //Assert          
            Assert.Equal("Records not found", result.Value);
        }

        [Fact]
        public void Should_GetAll_ContactInformation()
        {
            //Arrange
            var contactInfo1 = GetContactInformation();
            var contactInfo2 = GetContactInformation();
            contactInfo2.Id = 2;
            var contactList = new List<ContactInformation>() { contactInfo1, contactInfo2 };
            mockContact.Setup(x => x.GetAll()).Returns(contactList);
            //Act
            var data = controller.Get();
            var okObjectResult = data as OkObjectResult;
            var contactInfo = okObjectResult.Value as List<ContactInformation>;
            //Assert
            Assert.NotNull(contactInfo);
            Assert.Equal(2, contactInfo.Count);
        }
        #endregion

        #region Get By Id

        [Fact]
        public void Should_Get_ContactInformation_By_ID()
        {
            //Arrange      
            mockContact.Setup(x => x.GetByID(1)).Returns(GetContactInformation());
            //Act
            var data = controller.Get(1);
            var okObjectResult = data as OkObjectResult;
            var contactInfo = (ContactInformation)okObjectResult.Value;
            //Assert
            Assert.NotNull(contactInfo);
            Assert.Equal("Sameer", contactInfo.First_Name);
        }

        [Fact]
        public void Should_Not_Get_ContactInformation_By_ID()
        {
            //Arrange      
            mockContact.Setup(x => x.GetByID(1)).Returns(new ContactInformation());
            //Act
            var data = controller.Get(1);
            var okObjectResult = data as OkObjectResult;
            var contactInfo = (ContactInformation)okObjectResult.Value;
            //Assert
            Assert.Null(contactInfo.First_Name);
        }

        #endregion

        #region Add New Contact

        [Fact]
        public void Should_Save_ContactInformation()
        {
            //Arrange
            var contact = new ContactInformation()
            {
                Id = null,
                First_Name = "Same",
                Last_Name = "tie",
                Email = "same@outlook.com",
                Phone_Number = "9561154541",
                Status = Model.Status.Active
            };
            mockContact.Setup(x => x.Save(contact)).Callback(() => contact.Id = 1);

            //Act
            var response = controller.Post(contact);
            var createdRouteResult = response as CreatedAtRouteResult;
            //Assert
            mockContact.Verify(x => x.Save(contact), Times.Once);
            Assert.Equal(1, ((ContactInformation)createdRouteResult.Value).Id);
        }

        [Fact]
        public void Should_Not_Save_ContactInformation()
        {
            //Arrange
            var contact = new ContactInformation()
            {
                First_Name = "",
                Last_Name = "",
                Email = "",
                Phone_Number = "",
                Status = Model.Status.Active
            };
            mockContact.Setup(x => x.Save(contact));

            //Act
            var response = controller.Post(contact);
            var okObjectResult = response as BadRequestObjectResult;
            //Assert
            Assert.Equal(400, okObjectResult.StatusCode);
            Assert.Equal("Record not saved", okObjectResult.Value);
        }
        #endregion

        #region Edit Contact

        [Fact]
        public void Should_Update_ContactInformation()
        {
            //Arrange
            var oldContactInformation = GetContactInformation();
            var newContactInformation = new ContactInformation()
            {
                Id = 1,
                First_Name = "Sameer",
                Last_Name = "tie",
                Email = "same@outlook.com",
                Phone_Number = "9561154541",
                Status = Model.Status.Active
            };
            mockContact.Setup(x => x.GetByID(1)).Returns(oldContactInformation);
            mockContact.Setup(x => x.Update(newContactInformation, oldContactInformation)); ;

            //Act
            var response = controller.Put(1, newContactInformation);
            var createdRouteResult = response as OkObjectResult;
            //Assert
            mockContact.Verify(x => x.Update(newContactInformation, oldContactInformation), Times.Once);
            Assert.Equal("Record Updated", createdRouteResult.Value);
        }

        [Fact]
        public void Should_Not_Update_ContactInformation()
        {
            var newContactInformation = GetContactInformation();
            mockContact.Setup(x => x.GetByID(12)).Returns(() => null);

            //Act
            var response = controller.Put(1, newContactInformation);
            var result = response as NotFoundObjectResult;
            //Assert
            mockContact.Verify(x => x.Update(newContactInformation, null), Times.Never);
            Assert.Equal("Record not found for Update", result.Value);
        }


        #endregion

        #region Delete Contact

        [Fact]
        public void Should_Delete_ContactInformation()
        {
            //Arrange
            var oldContactInformation = GetContactInformation();

            mockContact.Setup(x => x.GetByID(1)).Returns(oldContactInformation);
            mockContact.Setup(x => x.Delete(1)); 

            //Act
            var response = controller.Delete(1);
            var createdRouteResult = response as OkObjectResult;
            //Assert
            mockContact.Verify(x => x.Delete(1), Times.Once);
            Assert.Equal("Record Deleted", createdRouteResult.Value);
        }

        [Fact]
        public void Should_Not_Delete_ContactInformation()
        {
            mockContact.Setup(x => x.GetByID(1)).Returns(() => null);

            //Act
            var response = controller.Delete(1);
            var result = response as NotFoundObjectResult;
            //Assert
            mockContact.Verify(x => x.Delete(1), Times.Never);
            Assert.Equal("Record not found for Delete", result.Value);
        }

        [Fact]
        public void Should_Not_Delete_Without_Id()
        {
            //Arrange
            //Act
            var response = controller.Delete(null);
            var result = response as BadRequestObjectResult;
            //Assert
            Assert.Equal("Id should not be null", result.Value);
        }
        #endregion

        #region private method
        private ContactInformation GetContactInformation()
        {
            //Arrange           
            return new ContactInformation()
            {
                Id = 1,
                First_Name = "Sameer",
                Last_Name = "tie",
                Email = "same@outlook.com",
                Phone_Number = "9561154541",
                Status = Model.Status.Active
            };
        }
        #endregion
    }
}

