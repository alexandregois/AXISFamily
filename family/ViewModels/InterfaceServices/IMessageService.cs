using System;
using System.Threading.Tasks;

namespace family.ViewModels.InterfaceServices
{
	public interface IMessageService
	{
		Task ShowAlertAsync (
			String paramMessage
			, String paramTitulo
			, String paramOK = "OK"
		);

		Task<Boolean> ShowAlertChooseAsync (
			String paramMessage
			, String paramCancel
			, String paramOK
			, String paramTitulo
		);

		Task<String> ShowMessageAsync(
			string titulo
			, string paramCancel
			, string paramDestruction
			, string[] paramButtons
		);
	}
}
