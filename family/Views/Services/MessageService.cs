using System;
using System.Threading.Tasks;
using family.ViewModels.InterfaceServices;
using family.Views.Services;
using Xamarin.Forms;


[assembly: Dependency(typeof(MessageService))]
namespace family.Views.Services
{
	public class MessageService: IMessageService
	{

		public async Task ShowAlertAsync (
			String paramMessage
			, String paramTitulo
			, String paramOK = "OK"
		)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				await Application
					.Current.MainPage.DisplayAlert
					(
						paramTitulo
						, paramMessage
						, paramOK
					);
			});
		}

		public async Task<Boolean> ShowAlertChooseAsync (
			String paramMessage
			, String paramCancel
			, String paramOK
			, String paramTitulo
		)
		{
			TaskCompletionSource<bool> ret = new TaskCompletionSource<bool>();
			Device.BeginInvokeOnMainThread(async () =>
			{
				Boolean alertRet = await Application
					.Current.MainPage.DisplayAlert
					(
						paramTitulo
						, paramMessage
						, paramOK
						, paramCancel
					);
				ret.SetResult(alertRet);
			});

			return await ret.Task;
		}

		public async Task<String> ShowMessageAsync(
			string titulo
			, string paramCancel
			, string paramDestruction
			, string[] paramButtons
		)
		{

			TaskCompletionSource<String> ret = new TaskCompletionSource<String>();
			Device.BeginInvokeOnMainThread(async () => 
			{
				String alertRet = await Application
					.Current.MainPage.DisplayActionSheet
					(
						titulo
						, paramCancel
						, paramDestruction
						, paramButtons
					);
				ret.SetResult(alertRet);
			});
			return await ret.Task;
		}
	}
}

