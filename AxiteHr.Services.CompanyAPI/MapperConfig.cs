using AutoMapper;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Request;
using AxiteHR.Services.CompanyAPI.Models.Permissions;
using AxiteHR.Services.CompanyAPI.Models.Roles;

namespace AxiteHR.Services.CompanyAPI
{
	public static class MapperConfig
	{
		public static MapperConfiguration RegisterMaps()
		{
			return new MapperConfiguration(config =>
			{
				config.CreateMap<Company, CompanyDto>().ReverseMap();
				config.CreateMap<CompanyLevel, CompanyLevelDto>().ReverseMap();
				config.CreateMap<CompanyRole, CompanyRoleDto>().ReverseMap();
				config.CreateMap<CompanyUser, CompanyUserDto>().ReverseMap();

				config.CreateMap<NewCompanyRequestDto, Company>()
					.ForMember(dto => dto.InsUserId, opt => opt.MapFrom(domain => domain.CreatorId))
					.ForMember(dto => dto.UpdUserId, opt => opt.MapFrom(domain => domain.CreatorId))
					.ReverseMap();
			});
		}
	}
}
