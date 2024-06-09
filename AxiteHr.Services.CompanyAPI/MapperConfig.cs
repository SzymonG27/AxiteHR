using AutoMapper;
using AxiteHr.Services.CompanyAPI.Models;
using AxiteHr.Services.CompanyAPI.Models.Dto;

namespace AxiteHr.Services.CompanyAPI
{
	public static class MapperConfig
	{
		public static MapperConfiguration RegisterMaps()
		{
			var mappingConfig = new MapperConfiguration(config =>
			{
				config.CreateMap<Company, CompanyDto>().ReverseMap();
				config.CreateMap<CompanyLevel, CompanyLevelDto>().ReverseMap();
				config.CreateMap<CompanyPermission, CompanyPermissionDto>().ReverseMap();
				config.CreateMap<CompanyRole, CompanyRoleDto>().ReverseMap();
				config.CreateMap<CompanyUser, CompanyUserDto>().ReverseMap();
			});
			return mappingConfig;
		}
	}
}
