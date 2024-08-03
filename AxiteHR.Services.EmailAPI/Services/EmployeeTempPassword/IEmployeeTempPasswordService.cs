using AxiteHR.Services.EmailAPI.Models;

namespace AxiteHR.Services.EmailAPI.Services.EmployeeTempPassword
{
	public interface IEmployeeTempPasswordService
	{
		Task EmailTempPasswordCreateAndLog(UserTempPasswordMessageBusDto messageBusDto);
	}
}
