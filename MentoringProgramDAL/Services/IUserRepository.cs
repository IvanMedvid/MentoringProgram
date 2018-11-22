using MentoringProgramDAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MentoringProgramDAL.Services
{
    public interface IUserRepository
    {
        User GetUserById(int id);

        IEnumerable<User> GetUserList();

        void AddUser(User user);

        void UpdateUser(User user);

        void DeleteUser(int id);

        bool Save();
    }
}
