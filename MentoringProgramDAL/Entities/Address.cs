using System;
using System.Collections.Generic;
using System.Text;

namespace MentoringProgramDAL.Entities
{
    public class Address
    {
        public int Id { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public string PostCode { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
