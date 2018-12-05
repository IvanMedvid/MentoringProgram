using MentoringProgramDAL;
using MentoringProgramDAL.Entities;
using MentoringProgramDAL.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var data = new List<User>().AsQueryable();

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<MentoringProgramContext>();
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            var service = new UserRepository(mockContext.Object);
            var users = service.GetUserList();

            Assert.AreEqual(0, users.Count());
        }

        [Test]
        public void Test2()
        {
            var item = new User
            {
                Id =1,
                FirstName = "User",
                LastName = "LastName",
                MiddleName = "MiddleName",
                Email = "someemail@test.com",
                AddressId = 1
            };

            var source = new List<User>();
            var data = source.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(d => d.Add(It.IsAny<User>())).Callback<User>((s) => source.Add(s));

            var mockContext = new Mock<MentoringProgramContext>();
            mockContext.Setup(m => m.Users).Returns(mockSet.Object);

            var service = new UserRepository(mockContext.Object);
            service.AddUser(item);

            mockSet.Verify(m => m.Add(It.IsAny<User>()), Times.Once());          
        }
    }
}