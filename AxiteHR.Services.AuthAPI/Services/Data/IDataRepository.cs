using AxiteHR.Services.AuthAPI.Models.DataModels.Dto;

namespace AxiteHR.Services.AuthAPI.Services.Data
{
	public interface IDataRepository
	{
		/// <summary>
		/// This method is used to populate the company's employee list view data
		/// </summary>
		/// <returns>List of company's employee list view data</returns>
		IEnumerable<UserDataListViewDto> GetUserDataListViewDtoList(IList<string> userIds);
	}
}
