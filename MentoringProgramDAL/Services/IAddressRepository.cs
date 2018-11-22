using MentoringProgramDAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MentoringProgramDAL.Services
{
    public interface IAddressRepository
    {
        IEnumerable<Address> GetAddressList();

        Address GetAddressById(int id);

        void AddAddress(Address address);
        
        void UpdateAddress(Address address);

        void DeleteAddress(int id);

        bool IsAddressExists(Address address);

        bool Save();
    }
}
