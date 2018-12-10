using AutoMapper;
using MentoringProgram.Controllers;
using MentoringProgram.Models;
using MentoringProgramDAL.Entities;
using MentoringProgramDAL.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MentoringProgramTest.Controllers
{
    [TestFixture]
    class AddressController_Tests
    {
        [Test]
        public void GetAddresses_GetExistedAddresses()
        {
            var addressesSource = new List<Address>{
                new Address
                {
                    Id = 1,
                    AddressLine1 = "AddressLine1",
                    AddressLine2 = "AddressLine2",
                    AddressLine3 = "AddressLine3",
                    PostCode = "PostCode"
                }
            };

            var addressDtoList = new List<AddressDto>{
                new AddressDto
                {
                    Id = 1,
                    AddressLine1 = "AddressLine1",
                    AddressLine2 = "AddressLine2",
                    AddressLine3 = "AddressLine3",
                    PostCode = "PostCode"
                }
            };


            var mockRepository = new Mock<IAddressRepository>();
            mockRepository.Setup(repo => repo.GetAddressList()).Returns(addressesSource);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<IEnumerable<AddressDto>>(It.IsAny<IEnumerable<Address>>())).Returns(addressDtoList);

            var controller = new AddressController(mockRepository.Object, mockMapper.Object);

            IActionResult result = controller.GetAdresses();

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okObject = result as OkObjectResult;
            Assert.IsInstanceOf<List<AddressDto>>(okObject.Value);
            var resultValue = okObject.Value as List<AddressDto>;
            Assert.AreEqual(resultValue.Count(), addressDtoList.Count());
        }

        [TestCase(1)]
        public void GetAddressById_GetExistedAddress(int id)
        {
            var addressesSource = new List<Address>{
                new Address
                {
                    Id = 1,
                    AddressLine1 = "AddressLine1",
                    AddressLine2 = "AddressLine2",
                    AddressLine3 = "AddressLine3",
                    PostCode = "PostCode"
                }
            };

            var addressDto = new AddressDto
            {
                Id = 1,
                AddressLine1 = "AddressLine1",
                AddressLine2 = "AddressLine2",
                AddressLine3 = "AddressLine3",
                PostCode = "PostCode"
            };


            var mockRepository = new Mock<IAddressRepository>();
            mockRepository.Setup(repo => repo.GetAddressById(id)).Returns(addressesSource.FirstOrDefault(x => x.Id == id));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<AddressDto>(It.IsAny<Address>())).Returns(addressDto);

            var controller = new AddressController(mockRepository.Object, mockMapper.Object);

            IActionResult result = controller.GetAddressById(id);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okObject = result as OkObjectResult;
            Assert.IsInstanceOf<AddressDto>(okObject.Value);
        }

        [TestCase(2)]
        [TestCase(4)]
        public void GetAddressById_GetNotExistedAddress(int id)
        {
            var addressesSource = new List<Address>{
                new Address
                {
                    Id = 1,
                    AddressLine1 = "AddressLine1",
                    AddressLine2 = "AddressLine2",
                    AddressLine3 = "AddressLine3",
                    PostCode = "PostCode"
                }
            };

            var mockRepository = new Mock<IAddressRepository>();
            mockRepository.Setup(repo => repo.GetAddressById(id)).Returns(addressesSource.FirstOrDefault(x => x.Id == id));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<AddressDto>(It.IsAny<Address>())).Returns(new AddressDto());

            var controller = new AddressController(mockRepository.Object, mockMapper.Object);

            IActionResult result = controller.GetAddressById(id);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void AddAddress_SuccessCase()
        {
            var addresses = new List<Address>();
            var addressToAddManipulateDto = new AddressToManipulateDto
            {
                AddressLine1 = "NewAddressLine1",
                AddressLine2 = "NewAddressLine1",
                AddressLine3 = "NewAddressLine1",
                PostCode = "NewPostCode"
            };
            var addressToAdd = new Address
            {
                Id = 1,
                AddressLine1 = "NewAAddressLine1",
                AddressLine2 = "NewAAddressLine1",
                AddressLine3 = "NewAAddressLine1",
                PostCode = "NewPostCode"
            };

            var addressDto = new AddressDto
            {
                Id = 1,
                AddressLine1 = "NewAAddressLine1",
                AddressLine2 = "NewAAddressLine1",
                AddressLine3 = "NewAAddressLine1",
                PostCode = "NewPostCode"
            };

            var mockRepository = new Mock<IAddressRepository>();
            mockRepository.Setup(repo => repo.AddAddress(It.IsAny<Address>())).Callback<Address>((s) => addresses.Add(s));
            mockRepository.Setup(repo => repo.Save()).Returns(true);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<Address>(It.IsAny<AddressToManipulateDto>())).Returns(addressToAdd);
            mockMapper.Setup(x => x.Map<AddressDto>(It.IsAny<Address>())).Returns(addressDto);

            var controller = new AddressController(mockRepository.Object, mockMapper.Object);

            IActionResult result = controller.AddAddress(addressToAddManipulateDto);

            Assert.IsInstanceOf<CreatedAtRouteResult>(result);
            var resultValue = result as CreatedAtRouteResult;
            Assert.AreEqual(201, resultValue.StatusCode);
            Assert.AreEqual("GetAddress", resultValue.RouteName);
            Assert.IsInstanceOf<AddressDto>(resultValue.Value);
            Assert.AreEqual(1, addresses.Count());
        }

        [Test]
        public void AddAddress_TryToAddNull()
        {
            var addresses = new List<Address>();
            AddressToManipulateDto addressToAddManipulateDto = null;
            Address addressToAdd = null;
            AddressDto addressDto = null;

            var mockRepository = new Mock<IAddressRepository>();

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<Address>(It.IsAny<AddressToManipulateDto>())).Returns(addressToAdd);
            mockMapper.Setup(x => x.Map<AddressDto>(It.IsAny<Address>())).Returns(addressDto);

            var controller = new AddressController(mockRepository.Object, mockMapper.Object);

            IActionResult result = controller.AddAddress(addressToAddManipulateDto);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void AddAddress_SaveAddressError()
        {
            var addresses = new List<Address>();
            var addressToAddManipulateDto = new AddressToManipulateDto
            {
                AddressLine1 = "NewAAddressLine1",
                AddressLine2 = "NewAAddressLine1",
                AddressLine3 = "NewAAddressLine1",
                PostCode = "NewPostCode"
            };
            var addressToAdd = new Address
            {
                Id = 1,
                AddressLine1 = "NewAAddressLine1",
                AddressLine2 = "NewAAddressLine1",
                AddressLine3 = "NewAAddressLine1",
                PostCode = "NewPostCode"
            };

            var addressDto = new AddressDto
            {
                Id = 1,
                AddressLine1 = "NewAAddressLine1",
                AddressLine2 = "NewAAddressLine1",
                AddressLine3 = "NewAAddressLine1",
                PostCode = "NewPostCode"
            };

            var mockRepository = new Mock<IAddressRepository>();
            mockRepository.Setup(repo => repo.AddAddress(It.IsAny<Address>())).Callback<Address>((s) => addresses.Add(s)); ;
            mockRepository.Setup(repo => repo.Save()).Returns(false);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<Address>(It.IsAny<AddressToManipulateDto>())).Returns(addressToAdd);
            mockMapper.Setup(x => x.Map<AddressDto>(It.IsAny<Address>())).Returns(addressDto);

            var controller = new AddressController(mockRepository.Object, mockMapper.Object);

            Assert.Throws<Exception>(() => controller.AddAddress(addressToAddManipulateDto));
        }

        [TestCase(1)]
        public void UpdateAddress_SuccessCase(int id)
        {
            var addresses = new List<Address> {
                new Address
                {
                    Id = 1,
                    AddressLine1 = "AddressLine1",
                    AddressLine2 = "AddressLine1",
                    AddressLine3 = "AddressLine1",
                    PostCode = "PostCode"
                }
            };
            var addressToUpdateManipulateDto = new AddressToManipulateDto
            {
                AddressLine1 = "NewAddressLine1",
                AddressLine2 = "NewAddressLine1",
                AddressLine3 = "NewAddressLine1",
                PostCode = "NewPostCode"
            };
            var address = new Address
            {
                Id = 1,
                AddressLine1 = "NewAddressLine1",
                AddressLine2 = "NewAddressLine1",
                AddressLine3 = "NewAddressLine1",
                PostCode = "NewPostCode"
            };


            var mockRepository = new Mock<IAddressRepository>();
            mockRepository.Setup(repo => repo.GetAddressById(id)).Returns(addresses.FirstOrDefault(x => x.Id == id));
            mockRepository.Setup(repo => repo.UpdateAddress(It.IsAny<Address>())).Callback<Address>((s) => {
                addresses.Remove(addresses.FirstOrDefault(x => x.Id == id));
                addresses.Add(s);
            });
            mockRepository.Setup(repo => repo.Save()).Returns(true);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map(It.IsAny<AddressToManipulateDto>(), It.IsAny<Address>())).Returns(address);

            var controller = new AddressController(mockRepository.Object, mockMapper.Object);

            IActionResult result = controller.UpdateAddress(id, addressToUpdateManipulateDto);

            Assert.IsInstanceOf<CreatedAtRouteResult>(result);
            var resultValue = result as CreatedAtRouteResult;
            Assert.AreEqual(201, resultValue.StatusCode);
            Assert.AreEqual("GetAddress", resultValue.RouteName);
            Assert.IsInstanceOf<AddressToManipulateDto>(resultValue.Value);
            var resulAddress = resultValue.Value as AddressToManipulateDto;
            Assert.AreEqual(addressToUpdateManipulateDto.AddressLine1, resulAddress.AddressLine1);
        }

        [TestCase(1)]
        public void UpdateAddress_TryToUpdateAddressWhereModelNull(int id)
        {
            var addresses = new List<Address> {
                new Address
                {
                    Id = 1,
                    AddressLine1 = "AddressLine1",
                    AddressLine2 = "AddressLine1",
                    AddressLine3 = "AddressLine1",
                    PostCode = "PostCode"
                }
            };
            AddressToManipulateDto addressToUpdateManipulateDto = null;
            var mockRepository = new Mock<IAddressRepository>();

            var mockMapper = new Mock<IMapper>();

            var controller = new AddressController(mockRepository.Object, mockMapper.Object);

            IActionResult result = controller.UpdateAddress(id, addressToUpdateManipulateDto);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [TestCase(4)]
        [TestCase(6)]
        public void UpdateAddress_TryToUpdateNotExistedAddress(int id)
        {
            var addresses = new List<Address> {
                new Address
                {
                    Id = 1,
                    AddressLine1 = "AddressLine1",
                    AddressLine2 = "AddressLine1",
                    AddressLine3 = "AddressLine1",
                    PostCode = "PostCode"
                }
            };
            var addressToUpdateManipulateDto = new AddressToManipulateDto
            {
                AddressLine1 = "NewAddressLine1",
                AddressLine2 = "NewAddressLine1",
                AddressLine3 = "NewAddressLine1",
                PostCode = "NewPostCode"
            };
            var address = new Address
            {
                Id = 1,
                AddressLine1 = "NewAddressLine1",
                AddressLine2 = "NewAddressLine1",
                AddressLine3 = "NewAddressLine1",
                PostCode = "NewPostCode"
            };


            var mockRepository = new Mock<IAddressRepository>();
            mockRepository.Setup(repo => repo.GetAddressById(id)).Returns(addresses.FirstOrDefault(x => x.Id == id));
            mockRepository.Setup(repo => repo.UpdateAddress(It.IsAny<Address>())).Callback<Address>((s) => {
                addresses.Remove(addresses.FirstOrDefault(x => x.Id == id));
                addresses.Add(s);
            });
            mockRepository.Setup(repo => repo.Save()).Returns(true);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map(It.IsAny<AddressToManipulateDto>(), It.IsAny<Address>())).Returns(address);

            var controller = new AddressController(mockRepository.Object, mockMapper.Object);

            IActionResult result = controller.UpdateAddress(id, addressToUpdateManipulateDto);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [TestCase(1)]
        public void UpdateAddress_SaveError(int id)
        {
            var addresses = new List<Address> {
                new Address
                {
                    Id = 1,
                    AddressLine1 = "AddressLine1",
                    AddressLine2 = "AddressLine1",
                    AddressLine3 = "AddressLine1",
                    PostCode = "PostCode"
                }
            };
            var addressToUpdateManipulateDto = new AddressToManipulateDto
            {
                AddressLine1 = "NewAddressLine1",
                AddressLine2 = "NewAddressLine1",
                AddressLine3 = "NewAddressLine1",
                PostCode = "NewPostCode"
            };
            var address = new Address
            {
                Id = 1,
                AddressLine1 = "NewAddressLine1",
                AddressLine2 = "NewAddressLine1",
                AddressLine3 = "NewAddressLine1",
                PostCode = "NewPostCode"
            };


            var mockRepository = new Mock<IAddressRepository>();
            mockRepository.Setup(repo => repo.GetAddressById(id)).Returns(addresses.FirstOrDefault(x => x.Id == id));
            mockRepository.Setup(repo => repo.UpdateAddress(It.IsAny<Address>())).Callback<Address>((s) => {
                addresses.Remove(addresses.FirstOrDefault(x => x.Id == id));
                addresses.Add(s);
            });
            mockRepository.Setup(repo => repo.Save()).Returns(false);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map(It.IsAny<AddressToManipulateDto>(), It.IsAny<Address>())).Returns(address);

            var controller = new AddressController(mockRepository.Object, mockMapper.Object);

            Assert.Throws<Exception>(() => controller.UpdateAddress(id, addressToUpdateManipulateDto));
        }

        [TestCase(1)]
        public void DeleteAddress_SuccessCase(int id)
        {
            var addresses = new List<Address> {
                new Address
                {
                    Id = 1,
                    AddressLine1 = "AddressLine1",
                    AddressLine2 = "AddressLine1",
                    AddressLine3 = "AddressLine1",
                    PostCode = "PostCode"
                }
            };

            var mockRepository = new Mock<IAddressRepository>();
            mockRepository.Setup(repo => repo.GetAddressById(id)).Returns(addresses.FirstOrDefault(x => x.Id == id));
            mockRepository.Setup(repo => repo.DeleteAddress(id)).Callback<int>((s) => {
                addresses.Remove(addresses.FirstOrDefault(x => x.Id == id));
            });
            mockRepository.Setup(repo => repo.Save()).Returns(true);
            var mockMapper = new Mock<IMapper>();

            var controller = new AddressController(mockRepository.Object, mockMapper.Object);

            IActionResult result = controller.DeleteAddress(id);

            Assert.IsInstanceOf<NoContentResult>(result);
            Assert.AreEqual(0, addresses.Count());
        }

        [TestCase(2)]
        [TestCase(90)]
        public void DeleteAddress_TryToDeleteNotexistedAddress(int id)
        {
            var addresses = new List<Address> {
                new Address
                {
                    Id = 1,
                    AddressLine1 = "AddressLine1",
                    AddressLine2 = "AddressLine1",
                    AddressLine3 = "AddressLine1",
                    PostCode = "PostCode"
                }
            };

            var mockRepository = new Mock<IAddressRepository>();
            mockRepository.Setup(repo => repo.GetAddressById(id)).Returns(addresses.FirstOrDefault(x => x.Id == id));
            mockRepository.Setup(repo => repo.DeleteAddress(id)).Callback<int>((s) => {
                addresses.Remove(addresses.FirstOrDefault(x => x.Id == id));
            });
            mockRepository.Setup(repo => repo.Save()).Returns(true);
            var mockMapper = new Mock<IMapper>();

            var controller = new AddressController(mockRepository.Object, mockMapper.Object);

            IActionResult result = controller.DeleteAddress(id);

            Assert.IsInstanceOf<NotFoundResult>(result);
            Assert.AreEqual(1, addresses.Count());
        }

        [TestCase(1)]
        public void DeleteAddress_SaveError(int id)
        {
            var addresses = new List<Address> {
                new Address
                {
                    Id = 1,
                    AddressLine1 = "AddressLine1",
                    AddressLine2 = "AddressLine1",
                    AddressLine3 = "AddressLine1",
                    PostCode = "PostCode"
                }
            };

            var mockRepository = new Mock<IAddressRepository>();
            mockRepository.Setup(repo => repo.GetAddressById(id)).Returns(addresses.FirstOrDefault(x => x.Id == id));
            mockRepository.Setup(repo => repo.DeleteAddress(id)).Callback<int>((s) => {
                addresses.Remove(addresses.FirstOrDefault(x => x.Id == id));
            });
            mockRepository.Setup(repo => repo.Save()).Returns(false);
            var mockMapper = new Mock<IMapper>();

            var controller = new AddressController(mockRepository.Object, mockMapper.Object);

            Assert.Throws<Exception>(() => controller.DeleteAddress(id));
        }
    }
}
