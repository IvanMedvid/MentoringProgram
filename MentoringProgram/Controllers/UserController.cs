using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MentoringProgram.Models;
using MentoringProgramDAL.Entities;
using MentoringProgramDAL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MentoringProgram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet()]
        public IActionResult GetUsers()
        {
            var usersFromRepo = _userRepository.GetUserList();

            var users = Mapper.Map<IEnumerable<UserDto>>(usersFromRepo);
            return Ok(users);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult GetUsersById(int id)
        {
            var userFromRepo = _userRepository.GetUserById(id);

            if (userFromRepo == null)
            {
                return NotFound();
            }

            var user = Mapper.Map<UserDto>(userFromRepo);
            return Ok(user);
        }

        [HttpPost()]
        public IActionResult AddUser([FromBody]UserToManipulateDto user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            var userEntity = Mapper.Map<User>(user);

            _userRepository.AddUser(userEntity);

            if (!_userRepository.Save())
            {
                throw new Exception("Creating a user failed on save.");
            }

            var userToReturn = Mapper.Map<UserDto>(userEntity);

            return CreatedAtRoute("GetUser",
                new { id = userToReturn.Id },
                userToReturn);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, UserToManipulateDto user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            var userFromRepo = _userRepository.GetUserById(id);
            if (userFromRepo == null)
            {
                return NotFound();
            }

            Mapper.Map(user, userFromRepo);

            _userRepository.UpdateUser(userFromRepo);

            if (!_userRepository.Save())
            {
                throw new Exception($"Updating user {id} failed on save.");
            }

            return CreatedAtRoute("GetUser",
                new { id = userFromRepo.Id },
                user);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var userFromRepo = _userRepository.GetUserById(id);
            if (userFromRepo == null)
            {
                return NotFound();
            }

            _userRepository.DeleteUser(id);

            if (!_userRepository.Save())
            {
                throw new Exception($"Deleting user {id} failed on save.");
            }

            return NoContent();
        }
    }
}