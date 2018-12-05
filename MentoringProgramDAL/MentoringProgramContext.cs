using MentoringProgramDAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MentoringProgramDAL
{
    public class MentoringProgramContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Address> Addresses { get; set; }

        public MentoringProgramContext()
        {
        }

        public MentoringProgramContext(DbContextOptions<MentoringProgramContext> options) : base(options)
        {
        }
    }
}
