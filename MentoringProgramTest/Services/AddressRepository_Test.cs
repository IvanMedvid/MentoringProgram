using MentoringProgramDAL;
using MentoringProgramDAL.Entities;
using MentoringProgramDAL.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MentoringProgramTest.Services
{
    [TestFixture]
    class AddressRepository_Test
    {

        private static object[] _addressListDataSource =
        {
            new object[] { new List<Address>() },
            new object[] { new List<Address> {
                new Address
                {
                    Id = 1,
                    AddressLine1 = "AddressLine1",
                    AddressLine2 = "AddressLine2",
                    AddressLine3 = "AddressLine3",
                    PostCode = "PostCode"
                }
            } }
        };

        [Test, TestCaseSource("_addressListDataSource")]
        public void GetAddressList_ReturnList(List<Address> dataSource)
        {
            var data = dataSource.AsQueryable();

            var mockSet = new Mock<DbSet<Address>>();
            mockSet.As<IQueryable<Address>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Address>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Address>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Address>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<MentoringProgramContext>();
            mockContext.Setup(c => c.Addresses).Returns(mockSet.Object);

            var service = new AddressRepository(mockContext.Object);
            var addresses = service.GetAddressList();

            Assert.AreEqual(dataSource.Count(), addresses.Count());
        }

        [TestCase(1)]
        public void GetAddressById_ReturnAddressInstance(int id)
        {
            var dataSource = new List<Address> {
                new Address
                {
                    Id = 1,
                    AddressLine1 = "AddressLine1",
                    AddressLine2 = "AddressLine2",
                    AddressLine3 = "AddressLine3",
                    PostCode = "PostCode"
                }
            };

            var data = dataSource.AsQueryable();

            var mockSet = new Mock<DbSet<Address>>();
            mockSet.As<IQueryable<Address>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Address>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Address>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Address>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<MentoringProgramContext>();
            mockContext.Setup(c => c.Addresses).Returns(mockSet.Object);

            var service = new AddressRepository(mockContext.Object);
            var address = service.GetAddressById(id);

            Assert.NotNull(address);
            Assert.IsInstanceOf<Address>(address);
            Assert.AreEqual(mockSet.Object.First().Id, address.Id);
        }

        [TestCase(2)]
        [TestCase(3)]
        public void GetAddressById_GetNotExistingItem_ReturnNull(int id)
        {
            var dataSource = new List<Address> {
                new Address
                {
                    Id = 1,
                    AddressLine1 = "AddressLine1",
                    AddressLine2 = "AddressLine2",
                    AddressLine3 = "AddressLine3",
                    PostCode = "PostCode"
                }
            };

            var data = dataSource.AsQueryable();

            var mockSet = new Mock<DbSet<Address>>();
            mockSet.As<IQueryable<Address>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Address>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Address>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Address>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<MentoringProgramContext>();
            mockContext.Setup(c => c.Addresses).Returns(mockSet.Object);

            var service = new AddressRepository(mockContext.Object);
            var address = service.GetAddressById(id);

            Assert.Null(address);
        }

        [Test, TestCaseSource("_addressListDataSource")]
        public void AddAddress_SuccessCase(List<Address> dataSource)
        {
            var addressToInsert = new Address
            {
                Id = 2,
                AddressLine1 = "NewAddressLine1",
                AddressLine2 = "NewAddressLine2",
                AddressLine3 = "NewAddressLine3",
                PostCode = "NewPostCode"
            };

            var data = dataSource.AsQueryable();

            var mockSet = new Mock<DbSet<Address>>();
            mockSet.As<IQueryable<Address>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Address>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Address>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Address>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(d => d.Add(It.IsAny<Address>())).Callback<Address>((s) => dataSource.Add(s));

            var mockContext = new Mock<MentoringProgramContext>();
            mockContext.Setup(c => c.Addresses).Returns(mockSet.Object);

            var service = new AddressRepository(mockContext.Object);
            var addressesCountBeforeInsert = mockSet.Object.Count();

            service.AddAddress(addressToInsert);
            var addressesCountAfterInsert = mockSet.Object.Count();

            mockSet.Verify(m => m.Add(It.IsAny<Address>()), Times.Once());
            Assert.AreNotEqual(addressesCountAfterInsert, addressesCountBeforeInsert);
            Assert.AreEqual(1, addressesCountAfterInsert - addressesCountBeforeInsert);
        }

        [Test]
        public void AddAddress_AddExistingAddress()
        {
            var dataSource = new List<Address> {
                new Address
                {
                    Id = 1,
                    AddressLine1 = "AddressLine1",
                    AddressLine2 = "AddressLine2",
                    AddressLine3 = "AddressLine3",
                    PostCode = "PostCode"
                }
            };

            var addressToInsert = new Address
            {
                Id = 2,
                AddressLine1 = "AddressLine1",
                AddressLine2 = "AddressLine2",
                AddressLine3 = "AddressLine3",
                PostCode = "PostCode"
            };

            var data = dataSource.AsQueryable();

            var mockSet = new Mock<DbSet<Address>>();
            mockSet.As<IQueryable<Address>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Address>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Address>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Address>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(d => d.Add(It.IsAny<Address>())).Callback<Address>((s) => dataSource.Add(s));

            var mockContext = new Mock<MentoringProgramContext>();
            mockContext.Setup(c => c.Addresses).Returns(mockSet.Object);

            var service = new AddressRepository(mockContext.Object);
            var addressesCountBeforeInsert = mockSet.Object.Count();

            service.AddAddress(addressToInsert);
            var addressesCountAfterInsert = mockSet.Object.Count();

            mockSet.Verify(m => m.Add(It.IsAny<Address>()), Times.Never());
            Assert.AreEqual(addressesCountAfterInsert, addressesCountBeforeInsert);
        }

        [TestCase(1)]
        public void DeleteAddress_SuccessCase(int id)
        {
            var dataSource = new List<Address> {
                new Address
                {
                    Id = 1,
                    AddressLine1 = "AddressLine1",
                    AddressLine2 = "AddressLine2",
                    AddressLine3 = "AddressLine3",
                    PostCode = "PostCode"
                }
            };

            var data = dataSource.AsQueryable();

            var mockSet = new Mock<DbSet<Address>>();
            mockSet.As<IQueryable<Address>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Address>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Address>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Address>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(d => d.Remove(It.IsAny<Address>())).Callback<Address>((s) => dataSource.Remove(s));


            var mockContext = new Mock<MentoringProgramContext>();
            mockContext.Setup(c => c.Addresses).Returns(mockSet.Object);
            var addressesCoutBeforeDelete = mockContext.Object.Addresses.Count();

            var service = new AddressRepository(mockContext.Object);
            service.DeleteAddress(id);
            var addressesCoutAfterDelete = mockContext.Object.Addresses.Count();

            mockSet.Verify(m => m.Remove(It.IsAny<Address>()), Times.Once());
            Assert.AreEqual(0, addressesCoutAfterDelete);
            Assert.AreNotEqual(addressesCoutBeforeDelete, addressesCoutAfterDelete);
        }

        [TestCase(2)]
        [TestCase(10)]
        public void DeleteAddress_RemoveNotExistingAddress(int id)
        {
            var dataSource = new List<Address> {
                new Address
                {
                    Id = 1,
                    AddressLine1 = "AddressLine1",
                    AddressLine2 = "AddressLine2",
                    AddressLine3 = "AddressLine3",
                    PostCode = "PostCode"
                }
            };

            var data = dataSource.AsQueryable();

            var mockSet = new Mock<DbSet<Address>>();
            mockSet.As<IQueryable<Address>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Address>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Address>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Address>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(d => d.Remove(It.IsAny<Address>())).Callback<Address>((s) => dataSource.Remove(s));


            var mockContext = new Mock<MentoringProgramContext>();
            mockContext.Setup(c => c.Addresses).Returns(mockSet.Object);
            var addressesCoutBeforeDelete = mockContext.Object.Addresses.Count();

            var service = new AddressRepository(mockContext.Object);
            service.DeleteAddress(id);
            var addressesCoutAfterDelete = mockContext.Object.Addresses.Count();

            mockSet.Verify(m => m.Remove(It.IsAny<Address>()), Times.Never());
            Assert.AreEqual(1, addressesCoutAfterDelete);
            Assert.AreEqual(addressesCoutBeforeDelete, addressesCoutAfterDelete);
        }

        [Test]
        public void Save_SuccessCase()
        {
            var addressToInsert = new Address
            {
                Id = 2,
                AddressLine1 = "NewAddressLine1",
                AddressLine2 = "NewAddressLine2",
                AddressLine3 = "NewAddressLine3",
                PostCode = "NewPostCode"
            };

            List<Address> dataSource = new List<Address>();
            var data = dataSource.AsQueryable();

            var mockSet = new Mock<DbSet<Address>>();
            mockSet.As<IQueryable<Address>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Address>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Address>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Address>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(d => d.Add(It.IsAny<Address>())).Callback<Address>((s) => dataSource.Add(s));

            var mockContext = new Mock<MentoringProgramContext>();
            mockContext.Setup(c => c.Addresses).Returns(mockSet.Object);

            var service = new AddressRepository(mockContext.Object);

            service.AddAddress(addressToInsert);
            var result = service.Save();

            mockSet.Verify(m => m.Add(It.IsAny<Address>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.AreEqual(true, result);
        }


        [Test]
        public void IsAddressExists_ExistedAddress()
        {
            var dataSource = new List<Address> {
                new Address
                {
                    Id = 1,
                    AddressLine1 = "AddressLine1",
                    AddressLine2 = "AddressLine2",
                    AddressLine3 = "AddressLine3",
                    PostCode = "PostCode"
                }
            };

            var address = new Address
            {
                Id = 2,
                AddressLine1 = "AddressLine1",
                AddressLine2 = "AddressLine2",
                AddressLine3 = "AddressLine3",
                PostCode = "PostCode"
            };

            var data = dataSource.AsQueryable();

            var mockSet = new Mock<DbSet<Address>>();
            mockSet.As<IQueryable<Address>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Address>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Address>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Address>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<MentoringProgramContext>();
            mockContext.Setup(c => c.Addresses).Returns(mockSet.Object);

            var service = new AddressRepository(mockContext.Object);
            var result = service.IsAddressExists(address);

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [Test]
        public void IsAddressExists_NotExistedAddress()
        {
            var dataSource = new List<Address> {
                new Address
                {
                    Id = 1,
                    AddressLine1 = "AddressLine1",
                    AddressLine2 = "AddressLine2",
                    AddressLine3 = "AddressLine3",
                    PostCode = "PostCode"
                }
            };

            var address = new Address
            {
                Id = 2,
                AddressLine1 = "NewAddressLine1",
                AddressLine2 = "NewAddressLine2",
                AddressLine3 = "NewAddressLine3",
                PostCode = "PostCode"
            };

            var data = dataSource.AsQueryable();

            var mockSet = new Mock<DbSet<Address>>();
            mockSet.As<IQueryable<Address>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Address>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Address>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Address>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<MentoringProgramContext>();
            mockContext.Setup(c => c.Addresses).Returns(mockSet.Object);

            var service = new AddressRepository(mockContext.Object);
            var result = service.IsAddressExists(address);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
        }
    }
}
