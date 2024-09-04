
using Alb.AuthServer.Application.Contracts.Identity.Dto;
using Alb.AuthServer.Domain.Interfaces.Identity;
using Alb.AuthServer.Domain.Models.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Alb.Identity
{
    public class AlbAutoMapper : Profile
    {

        public AlbAutoMapper()
        {
            CreateMap<CreateUserDto, AuthCreateUserModel>();
            CreateMap<LoginUserDto, AuthLoginModel>();
        }
    }
}
