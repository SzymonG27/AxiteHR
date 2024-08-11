using AxiteHR.Services.AuthAPI.Models.DataModels.Dto;

namespace AxiteHR.Services.AuthAPI.Services.Data.Impl
{
	public class DataService(IDataRepository dataRepository) : IDataService
	{
		public IEnumerable<UserDataListViewDto> GetUserDataListViewDtoList(IList<string> userIds)
		{
			return dataRepository.GetUserDataListViewDtoList(userIds);
		}
	}
}
