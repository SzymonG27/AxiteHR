using AutoMapper;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels.Dto;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Request;

namespace AxiteHr.Services.CompanyAPI
{
	public static class MapperConfig
	{
		public static MapperConfiguration RegisterMaps()
		{
			return new MapperConfiguration(config =>
			{
				config.CreateMap<Company, CompanyDto>().ReverseMap();
				config.CreateMap<CompanyLevel, CompanyLevelDto>().ReverseMap();
				config.CreateMap<CompanyPermission, CompanyPermissionDto>().ReverseMap();
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
