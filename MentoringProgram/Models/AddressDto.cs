using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentoringProgram.Models
{
    public class AddressDto
    {
        public int Id { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public string PostCode { get; set; }

    }
}
