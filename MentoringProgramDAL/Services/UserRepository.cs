using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MentoringProgramDAL.Entities;

namespace MentoringProgramDAL.Services
{
    public class UserRepository : IUserRepository
    {

        private MentoringProgramContext _context;

        public UserRepository(MentoringProgramContext context)
        {
            _context = context;
        }

        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<User> GetUserList()
        {
            return _context.Users.ToList();
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
        }

        public void DeleteUser(int id)
        {
            var user = GetUserById(id);
            if(user != null)
            {
                _context.Users.Remove(user);
            }
        }
 
        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateUser(User user)
        {
            // no code in this implementation
        }
    }
}
