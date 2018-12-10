using AutoMapper;
using MentoringProgram.Models;
using MentoringProgramDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentoringProgram
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Address, AddressDto>();

            CreateMap<AddressToManipulateDto, Address>();

            CreateMap<User, UserDto>();

            CreateMap<UserToManipulateDto, User>();
        }
    }
}
