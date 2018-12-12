using MentoringProgramDAL;
using MentoringProgramDAL.Entities;
using MentoringProgramDAL.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MentoringProgramTest.Services
{
    [TestFixture]
    public class UserRepository_Tests
    {

        private static object[] _userListDataSource =
        {
            new object[] { new List<User>() },
            new object[] { new List<User> {
                new User
                {
                    Id = 1,
                    FirstName = "FirstName",
                    LastName = "LastName",
                    MiddleName = "MiddleName",
                    Email = "someemail@test.com",
                    AddressId = 1
                }
            } }
        };

        [Test, TestCaseSource("_userListDataSource")]
        public void GetUserList_ReturnList(List<User> dataSource)
        {
            var data = dataSource.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<MentoringProgramContext>();
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            var service = new UserRepository(mockContext.Object);
            var users = service.GetUserList();

            Assert.AreEqual(dataSource.Count(), users.Count());
        }

        [TestCase(1)]
        public void GetUserById_ReturnUserInstance(int id)
        {
            var dataSource = new List<User> {
                new User
                {
                    Id = 1,
                    FirstName = "FirstName",
                    LastName = "LastName",
                    MiddleName = "MiddleName",
                    Email = "someemail@test.com",
                    AddressId = 1
                }
            };

            var data = dataSource.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<MentoringProgramContext>();
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            var service = new UserRepository(mockContext.Object);
            var user = service.GetUserById(id);

            Assert.NotNull(user);
            Assert.IsInstanceOf<User>(user);
            Assert.AreEqual(mockSet.Object.First().Id, user.Id);
        }

        [TestCase(2)]
        [TestCase(3)]
        public void GetUserById_GetNotExistingItem_ReturnNull(int id)
        {
            var dataSource = new List<User> {
                new User
                {
                    Id = 1,
                    FirstName = "FirstName",
                    LastName = "LastName",
                    MiddleName = "MiddleName",
                    Email = "someemail@test.com",
                    AddressId = 1
                }
            };

            var data = dataSource.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<MentoringProgramContext>();
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            var service = new UserRepository(mockContext.Object);
            var user = service.GetUserById(id);

            Assert.Null(user);
        }

        [Test, TestCaseSource("_userListDataSource")]
        public void AddUser_SuccessCase(List<User> dataSource)
        {
            var userToInsert = new User
            {
                Id = 2,
                FirstName = "NewUser",
                LastName = "NewUserLastName",
                MiddleName = "NewUserMiddleName",
                Email = "someemail@test.com",
                AddressId = 2
            };

            var data = dataSource.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(d => d.Add(It.IsAny<User>())).Callback<User>((s) => dataSource.Add(s));

            var mockContext = new Mock<MentoringProgramContext>();
            mockContext.Setup(m => m.Users).Returns(mockSet.Object);
            var usersCountBeforeInsert = mockSet.Object.Count();

            var service = new UserRepository(mockContext.Object);
            service.AddUser(userToInsert);
            var usersCountAfterInsert = mockSet.Object.Count();

            mockSet.Verify(m => m.Add(It.IsAny<User>()), Times.Once());
            Assert.AreNotEqual(usersCountAfterInsert, usersCountBeforeInsert);
            Assert.AreEqual(1, usersCountAfterInsert - usersCountBeforeInsert);
        }

        [TestCase(1)]
        public void DeleteUser_SuccessCase(int id)
        {
            var dataSource = new List<User> {
                new User
                {
                    Id = 1,
                    FirstName = "FirstName",
                    LastName = "LastName",
                    MiddleName = "MiddleName",
                    Email = "someemail@test.com",
                    AddressId = 1
                }
            };

            var data = dataSource.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(d => d.Remove(It.IsAny<User>())).Callback<User>((s) => dataSource.Remove(s));


            var mockContext = new Mock<MentoringProgramContext>();
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);
            var userCoutBeforeDelete = mockContext.Object.Users.Count();

            var service = new UserRepository(mockContext.Object);
            service.DeleteUser(id);
            var userCoutAfterDelete = mockContext.Object.Users.Count();

            mockSet.Verify(m => m.Remove(It.IsAny<User>()), Times.Once());
            Assert.AreEqual(0, userCoutAfterDelete);
            Assert.AreNotEqual(userCoutBeforeDelete, userCoutAfterDelete);
        }

        [TestCase(2)]
        [TestCase(10)]
        public void DeleteUser_RemoveNotExistingUser(int id)
        {
            var dataSource = new List<User> {
                new User
                {
                    Id = 1,
                    FirstName = "FirstName",
                    LastName = "LastName",
                    MiddleName = "MiddleName",
                    Email = "someemail@test.com",
                    AddressId = 1
                }
            };

            var data = dataSource.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(d => d.Remove(It.IsAny<User>())).Callback<User>((s) => dataSource.Remove(s));


            var mockContext = new Mock<MentoringProgramContext>();
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);
            var userCoutBeforeDelete = mockContext.Object.Users.Count();

            var service = new UserRepository(mockContext.Object);
            service.DeleteUser(id);
            var userCoutAfterDelete = mockContext.Object.Users.Count();

            mockSet.Verify(m => m.Remove(It.IsAny<User>()), Times.Never());
            Assert.AreEqual(1, userCoutAfterDelete);
            Assert.AreEqual(userCoutBeforeDelete, userCoutAfterDelete);
        }

        [Test, TestCaseSource("_userListDataSource")]
        public void Save_SuccessCase(List<User> dataSource)
        {
            var userToInsert = new User
            {
                Id = 2,
                FirstName = "NewUser",
                LastName = "NewUserLastName",
                MiddleName = "NewUserMiddleName",
                Email = "someemail@test.com",
                AddressId = 2
            };

            var data = dataSource.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(d => d.Add(It.IsAny<User>())).Callback<User>((s) => dataSource.Add(s));

            var mockContext = new Mock<MentoringProgramContext>();
            mockContext.Setup(m => m.Users).Returns(mockSet.Object);
            
            var service = new UserRepository(mockContext.Object);
            service.AddUser(userToInsert);
            var result = service.Save();
            
            mockSet.Verify(m => m.Add(It.IsAny<User>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.AreEqual(true, result);
        }
    }
}