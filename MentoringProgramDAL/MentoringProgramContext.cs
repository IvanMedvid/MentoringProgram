using MentoringProgramDAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MentoringProgramDAL
{
    public class MentoringProgramContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public MentoringProgramContext(DbContextOptions<MentoringProgramContext> options) : base(options)
        {
        }
    }
}
