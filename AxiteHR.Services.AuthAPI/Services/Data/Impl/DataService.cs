using AxiteHR.Services.AuthAPI.Data;
using AxiteHR.Services.AuthAPI.Models.DataModels.Dto;
using Microsoft.EntityFrameworkCore;

namespace AxiteHR.Services.AuthAPI.Services.Data.Impl
{
	public class DataService(AppDbContext dbContext) : IDataService
	{
		public IEnumerable<UserDataListViewDto> GetUserDataListViewDtoList(IList<Guid> userIds)
		{
			return dbContext.Users
				.Where(x => x.Id.Equals(userIds))
				.AsNoTracking()
				.Select(x => new UserDataListViewDto
				{
					UserId = x.Id,
					UserEmail = x.Email,
					UserName = x.UserName ?? string.Empty,
					FirstName = x.FirstName,
					LastName = x.LastName
				});
		}
	}
}
