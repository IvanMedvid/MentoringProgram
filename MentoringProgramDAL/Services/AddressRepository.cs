using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MentoringProgramDAL.Entities;

namespace MentoringProgramDAL.Services
{
    public class AddressRepository : IAddressRepository
    {
        private MentoringProgramContext _context;

        public AddressRepository(MentoringProgramContext context)
        {
            _context = context;
        }

        public Address GetAddressById(int id)
        {
            return _context.Addresses.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Address> GetAddressList()
        {
            return _context.Addresses.ToList();
        }

        public void AddAddress(Address address)
        {
            if (!IsAddressExists(address))
            {
                _context.Addresses.Add(address);
            }
        }

        public void DeleteAddress(int id)
        {
            var address = GetAddressById(id);
            if (address != null)
            {
                _context.Addresses.Remove(address);
            }
        }



        public bool IsAddressExists(Address address)
        {
            return _context.Addresses.Any(x => x.AddressLine1 == address.AddressLine1
                                            && x.AddressLine2 == address.AddressLine2
                                            && x.AddressLine3 == address.AddressLine3
                                            && x.PostCode == address.PostCode);
        }

        public void UpdateAddress(Address address)
        {
            // no code in this implementation
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
