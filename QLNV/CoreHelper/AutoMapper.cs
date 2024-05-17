
using AutoMapper;
using QLNV.Models.DTOs;
using QLNV.Models.Entities;

namespace QLNV.CoreHelper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => src.Sex))
            .ForMember(dest => dest.BirthDay, opt => opt.MapFrom(src => src.BirthDay))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.JobId, opt => opt.MapFrom(src => src.JobId))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId));

            CreateMap<UserDto, User>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => src.Sex))
            .ForMember(dest => dest.BirthDay, opt => opt.MapFrom(src => src.BirthDay))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.JobId, opt => opt.MapFrom(src => src.JobId))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId));

            CreateMap<Salary, SalaryDto>()
            //.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Month, opt => opt.MapFrom(src => src.Month))
            .ForMember(dest => dest.ContractSalary, opt => opt.MapFrom(src => src.ContractSalary))
            .ForMember(dest => dest.DayOff, opt => opt.MapFrom(src => src.DayOff));
            //.ForMember(dest => dest.TotalSalary, opt => opt.MapFrom(src => src.TotalSalary));

            CreateMap<SalaryDto, Salary>()
            //.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Month, opt => opt.MapFrom(src => src.Month))
            .ForMember(dest => dest.ContractSalary, opt => opt.MapFrom(src => src.ContractSalary))
            .ForMember(dest => dest.DayOff, opt => opt.MapFrom(src => src.DayOff));
            //.ForMember(dest => dest.TotalSalary, opt => opt.MapFrom(src => src.TotalSalary));
            CreateMap<UserRequest, UserRequestDtoGet>()
.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
.ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.Reason))
.ForMember(dest => dest.AttachmentName, opt => opt.MapFrom(src => src.AttachmentName))
.ForMember(dest => dest.DayTime, opt => opt.MapFrom(src => src.DayTime));
            CreateMap<UserRequestDtoGet, UserRequest>()
.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
.ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.Reason))
.ForMember(dest => dest.AttachmentName, opt => opt.MapFrom(src => src.AttachmentName))
.ForMember(dest => dest.DayTime, opt => opt.MapFrom(src => src.DayTime));


        }

    }
}
