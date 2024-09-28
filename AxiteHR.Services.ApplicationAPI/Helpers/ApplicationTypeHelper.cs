using AxiteHR.Services.ApplicationAPI.Models.Application.Enums;

namespace AxiteHR.Services.ApplicationAPI.Helpers
{
	public static class ApplicationTypeHelper
	{
		public static bool IsTypeThatDontCountDaysOff(this ApplicationType type)
		{
			return type == ApplicationType.UnpaidBreak || type == ApplicationType.HomeWork;
		}
	}
}
