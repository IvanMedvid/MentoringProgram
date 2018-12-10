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
        public void GetUsers_GetExistedUsers()
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

        [Test]
        public void AddUser_SuccessCase()
        {
            var users = new List<User>();
            var userToAddManipulateDto = new UserToManipulateDto
            {
                FirstName = "NewUserFirstName",
                MiddleName = "NewUserMiddleName",
                LastName = "NewUserLastName",
                AddressId = 2
            };
            var userToAdd = new User
            {
                Id = 1,
                FirstName = "NewUserFirstName",
                MiddleName = "NewUserMiddleName",
                LastName = "NewUserLastName",
                AddressId = 2
            };

            var userDto = new UserDto
            {
                Id = 1,
                FirstName = "NewUserFirstName",
                MiddleName = "NewUserMiddleName",
                LastName = "NewUserLastName",
                AddressId = 2
            };

            var mockRepository = new Mock<IUserRepository>();
            mockRepository.Setup(repo => repo.AddUser(It.IsAny<User>())).Callback<User>((s) => users.Add(s));
            mockRepository.Setup(repo => repo.Save()).Returns(true);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<User>(It.IsAny<UserToManipulateDto>())).Returns(userToAdd);
            mockMapper.Setup(x => x.Map<UserDto>(It.IsAny<User>())).Returns(userDto);

            var controller = new UserController(mockRepository.Object, mockMapper.Object);

            IActionResult result = controller.AddUser(userToAddManipulateDto);

            Assert.IsInstanceOf<CreatedAtRouteResult>(result);
            var resultValue = result as CreatedAtRouteResult;
            Assert.AreEqual(201, resultValue.StatusCode);
            Assert.AreEqual("GetUser", resultValue.RouteName);
            Assert.IsInstanceOf<UserDto>(resultValue.Value);
            Assert.AreEqual(1, users.Count());
        }

        [Test]
        public void AddUser_TryToAddNull()
        {
            var users = new List<User>();
            UserToManipulateDto userToAddManipulateDto = null;
            User userToAdd = null;
            UserDto userDto = null;

            var mockRepository = new Mock<IUserRepository>();

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<User>(It.IsAny<UserToManipulateDto>())).Returns(userToAdd);
            mockMapper.Setup(x => x.Map<UserDto>(It.IsAny<User>())).Returns(userDto);

            var controller = new UserController(mockRepository.Object, mockMapper.Object);

            IActionResult result = controller.AddUser(userToAddManipulateDto);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void AddUser_SaveUserError()
        {
            var users = new List<User>();
            var userToAddManipulateDto = new UserToManipulateDto
            {
                FirstName = "NewUserFirstName",
                MiddleName = "NewUserMiddleName",
                LastName = "NewUserLastName",
                AddressId = 2
            };
            var userToAdd = new User
            {
                Id = 1,
                FirstName = "NewUserFirstName",
                MiddleName = "NewUserMiddleName",
                LastName = "NewUserLastName",
                AddressId = 2
            };

            var userDto = new UserDto
            {
                Id = 1,
                FirstName = "NewUserFirstName",
                MiddleName = "NewUserMiddleName",
                LastName = "NewUserLastName",
                AddressId = 2
            };

            var mockRepository = new Mock<IUserRepository>();
            mockRepository.Setup(repo => repo.AddUser(It.IsAny<User>())).Callback<User>((s) => users.Add(s)); ;
            mockRepository.Setup(repo => repo.Save()).Returns(false);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<User>(It.IsAny<UserToManipulateDto>())).Returns(userToAdd);
            mockMapper.Setup(x => x.Map<UserDto>(It.IsAny<User>())).Returns(userDto);

            var controller = new UserController(mockRepository.Object, mockMapper.Object);

            Assert.Throws<Exception>(() => controller.AddUser(userToAddManipulateDto));
        }

        [TestCase(1)]
        public void UpdateUser_SuccessCase(int id)
        {
            var users = new List<User> {
                new User
                {
                    Id = 1,
                    FirstName = "FirstName",
                    MiddleName = "MiddleName",
                    LastName = "LastName",
                    AddressId = 2
                }
            };
            var userToUpdateManipulateDto = new UserToManipulateDto
            {
                FirstName = "NewUserFirstName",
                MiddleName = "NewUserMiddleName",
                LastName = "NewUserLastName",
                AddressId = 2
            };
            var user = new User
            {
                Id = 1,
                FirstName = "NewUserFirstName",
                MiddleName = "NewUserMiddleName",
                LastName = "NewUserLastName",
                AddressId = 2
            };
            

            var mockRepository = new Mock<IUserRepository>();
            mockRepository.Setup(repo => repo.GetUserById(id)).Returns(users.FirstOrDefault(x => x.Id == id));
            mockRepository.Setup(repo => repo.UpdateUser(It.IsAny<User>())).Callback<User>((s) => {
                users.Remove(users.FirstOrDefault(x => x.Id == id));
                users.Add(s); });
            mockRepository.Setup(repo => repo.Save()).Returns(true);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map(It.IsAny<UserToManipulateDto>(), It.IsAny<User>())).Returns(user);
            
            var controller = new UserController(mockRepository.Object, mockMapper.Object);

            IActionResult result = controller.UpdateUser(id, userToUpdateManipulateDto);

            Assert.IsInstanceOf<CreatedAtRouteResult>(result);
            var resultValue = result as CreatedAtRouteResult;
            Assert.AreEqual(201, resultValue.StatusCode);
            Assert.AreEqual("GetUser", resultValue.RouteName);
            Assert.IsInstanceOf<UserToManipulateDto>(resultValue.Value);
            var resulUser = resultValue.Value as UserToManipulateDto;
            Assert.AreEqual(userToUpdateManipulateDto.FirstName, resulUser.FirstName);
        }

        [TestCase(1)]
        public void UpdateUser_TryToUpdateUserWhereModelNull(int id)
        {
            var users = new List<User> {
                new User
                {
                    Id = 1,
                    FirstName = "FirstName",
                    MiddleName = "MiddleName",
                    LastName = "LastName",
                    AddressId = 2
                }
            };
            UserToManipulateDto userToUpdateManipulateDto = null;
            var mockRepository = new Mock<IUserRepository>();

            var mockMapper = new Mock<IMapper>();

            var controller = new UserController(mockRepository.Object, mockMapper.Object);

            IActionResult result = controller.UpdateUser(id, userToUpdateManipulateDto);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [TestCase(4)]
        [TestCase(6)]
        public void UpdateUser_TryToUpdateNotExistedUser(int id)
        {
            var users = new List<User> {
                new User
                {
                    Id = 1,
                    FirstName = "FirstName",
                    MiddleName = "MiddleName",
                    LastName = "LastName",
                    AddressId = 2
                }
            };
            var userToUpdateManipulateDto = new UserToManipulateDto
            {
                FirstName = "NewUserFirstName",
                MiddleName = "NewUserMiddleName",
                LastName = "NewUserLastName",
                AddressId = 2
            };
            var user = new User
            {
                Id = 1,
                FirstName = "NewUserFirstName",
                MiddleName = "NewUserMiddleName",
                LastName = "NewUserLastName",
                AddressId = 2
            };


            var mockRepository = new Mock<IUserRepository>();
            mockRepository.Setup(repo => repo.GetUserById(id)).Returns(users.FirstOrDefault(x => x.Id == id));
            mockRepository.Setup(repo => repo.UpdateUser(It.IsAny<User>())).Callback<User>((s) => {
                users.Remove(users.FirstOrDefault(x => x.Id == id));
                users.Add(s);
            });
            mockRepository.Setup(repo => repo.Save()).Returns(true);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map(It.IsAny<UserToManipulateDto>(), It.IsAny<User>())).Returns(user);

            var controller = new UserController(mockRepository.Object, mockMapper.Object);

            IActionResult result = controller.UpdateUser(id, userToUpdateManipulateDto);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [TestCase(1)]
        public void UpdateUser_SaveError(int id)
        {
            var users = new List<User> {
                new User
                {
                    Id = 1,
                    FirstName = "FirstName",
                    MiddleName = "MiddleName",
                    LastName = "LastName",
                    AddressId = 2
                }
            };
            var userToUpdateManipulateDto = new UserToManipulateDto
            {
                FirstName = "NewUserFirstName",
                MiddleName = "NewUserMiddleName",
                LastName = "NewUserLastName",
                AddressId = 2
            };
            var user = new User
            {
                Id = 1,
                FirstName = "NewUserFirstName",
                MiddleName = "NewUserMiddleName",
                LastName = "NewUserLastName",
                AddressId = 2
            };


            var mockRepository = new Mock<IUserRepository>();
            mockRepository.Setup(repo => repo.GetUserById(id)).Returns(users.FirstOrDefault(x => x.Id == id));
            mockRepository.Setup(repo => repo.UpdateUser(It.IsAny<User>())).Callback<User>((s) => {
                users.Remove(users.FirstOrDefault(x => x.Id == id));
                users.Add(s);
            });
            mockRepository.Setup(repo => repo.Save()).Returns(false);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map(It.IsAny<UserToManipulateDto>(), It.IsAny<User>())).Returns(user);

            var controller = new UserController(mockRepository.Object, mockMapper.Object);

            Assert.Throws<Exception>(() => controller.UpdateUser(id, userToUpdateManipulateDto));
        }
        
        [TestCase(1)]
        public void DeleteUser_SuccessCase(int id)
        {
            var users = new List<User> {
                new User
                {
                    Id = 1,
                    FirstName = "FirstName",
                    MiddleName = "MiddleName",
                    LastName = "LastName",
                    AddressId = 2
                }
            };
          
            var mockRepository = new Mock<IUserRepository>();
            mockRepository.Setup(repo => repo.GetUserById(id)).Returns(users.FirstOrDefault(x => x.Id == id));
            mockRepository.Setup(repo => repo.DeleteUser(id)).Callback<int>((s) => {
                users.Remove(users.FirstOrDefault(x => x.Id == id));
            });
            mockRepository.Setup(repo => repo.Save()).Returns(true);
            var mockMapper = new Mock<IMapper>();

            var controller = new UserController(mockRepository.Object, mockMapper.Object);

            IActionResult result = controller.DeleteUser(id);

            Assert.IsInstanceOf<NoContentResult>(result);
            Assert.AreEqual(0, users.Count());
        }

        [TestCase(2)]
        [TestCase(90)]
        public void DeleteUser_TryToDeleteNotexistedUser(int id)
        {
            var users = new List<User> {
                new User
                {
                    Id = 1,
                    FirstName = "FirstName",
                    MiddleName = "MiddleName",
                    LastName = "LastName",
                    AddressId = 2
                }
            };

            var mockRepository = new Mock<IUserRepository>();
            mockRepository.Setup(repo => repo.GetUserById(id)).Returns(users.FirstOrDefault(x => x.Id == id));
            mockRepository.Setup(repo => repo.DeleteUser(id)).Callback<int>((s) => {
                users.Remove(users.FirstOrDefault(x => x.Id == id));
            });
            mockRepository.Setup(repo => repo.Save()).Returns(true);
            var mockMapper = new Mock<IMapper>();

            var controller = new UserController(mockRepository.Object, mockMapper.Object);

            IActionResult result = controller.DeleteUser(id);

            Assert.IsInstanceOf<NotFoundResult>(result);
            Assert.AreEqual(1, users.Count());
        }

        [TestCase(1)]
        public void DeleteUser_SaveError(int id)
        {
            var users = new List<User> {
                new User
                {
                    Id = 1,
                    FirstName = "FirstName",
                    MiddleName = "MiddleName",
                    LastName = "LastName",
                    AddressId = 2
                }
            };

            var mockRepository = new Mock<IUserRepository>();
            mockRepository.Setup(repo => repo.GetUserById(id)).Returns(users.FirstOrDefault(x => x.Id == id));
            mockRepository.Setup(repo => repo.DeleteUser(id)).Callback<int>((s) => {
                users.Remove(users.FirstOrDefault(x => x.Id == id));
            });
            mockRepository.Setup(repo => repo.Save()).Returns(false);
            var mockMapper = new Mock<IMapper>();

            var controller = new UserController(mockRepository.Object, mockMapper.Object);

            Assert.Throws<Exception>(() => controller.DeleteUser(id));
        }
    }
}
