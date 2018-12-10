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
    class UserController_Tests
    {
        [Test]
        public void GetUsers_GetExistedUser()
        {
            var usersSource = new List<User>{
                new User
                {
                    Id = 1,
                    FirstName = "FirstName",
                    MiddleName = "MiddleName",
                    LastName = "LasrName",
                    Email = "SomeEmail@test.com"
                }
            };

            var userDtoList = new List<UserDto>{
                new UserDto
                {
                    Id = 1,
                    FirstName = "FirstName",
                    MiddleName = "MiddleName",
                    LastName = "LasrName",
                    Email = "SomeEmail@test.com"
                }
            };


            var mockRepository = new Mock<IUserRepository>();
            mockRepository.Setup(repo => repo.GetUserList()).Returns(usersSource);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<IEnumerable<UserDto>>(It.IsAny<IEnumerable<User>>())).Returns(userDtoList);

            var controller = new UserController(mockRepository.Object, mockMapper.Object);

            IActionResult result = controller.GetUsers();

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okObject = result as OkObjectResult;
            Assert.IsInstanceOf<List<UserDto>>(okObject.Value);
            var resultValue = okObject.Value as List<UserDto>;
            Assert.AreEqual(resultValue.Count(), userDtoList.Count());
        }

        [TestCase(1)]
        public void GetUsersById_GetExistedUser(int id)
        {
            var usersSource = new List<User>{
                new User
                {
                    Id = 1,
                    FirstName = "FirstName",
                    MiddleName = "MiddleName",
                    LastName = "LasrName",
                    Email = "SomeEmail@test.com"
                }
            };

            var userDto = new UserDto
            {
                Id = 1,
                FirstName = "FirstName",
                MiddleName = "MiddleName",
                LastName = "LasrName",
                Email = "SomeEmail@test.com"
            };


            var mockRepository = new Mock<IUserRepository>();
            mockRepository.Setup(repo => repo.GetUserById(id)).Returns(usersSource.FirstOrDefault(x => x.Id == id));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<UserDto>(It.IsAny<User>())).Returns(userDto);

            var controller = new UserController(mockRepository.Object, mockMapper.Object);

            IActionResult result = controller.GetUsersById(id);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okObject = result as OkObjectResult;
            Assert.IsInstanceOf<UserDto>(okObject.Value);
        }

        [TestCase(2)]
        [TestCase(4)]
        public void GetUsersById_GetNotExistedUser(int id)
        {
            var usersSource = new List<User>{
                new User
                {
                    Id = 1,
                    FirstName = "FirstName",
                    MiddleName = "MiddleName",
                    LastName = "LasrName",
                    Email = "SomeEmail@test.com"
                }
            };

            var mockRepository = new Mock<IUserRepository>();
            mockRepository.Setup(repo => repo.GetUserById(id)).Returns(usersSource.FirstOrDefault(x => x.Id == id));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<UserDto>(It.IsAny<User>())).Returns(new UserDto());

            var controller = new UserController(mockRepository.Object, mockMapper.Object);

            IActionResult result = controller.GetUsersById(id);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}
