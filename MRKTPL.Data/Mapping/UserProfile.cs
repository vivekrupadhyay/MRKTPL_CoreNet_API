using AutoMapper;
using MRKTPL.Data.Entities;
using MRKTPL.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MRKTPL.Data.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserMaster, UserViewModel>().ReverseMap();
        }
    }
}
