using Xamarin.Forms;
using System;

namespace family.CrossPlataform
{
	public interface ICrossPlataformUtil
	{
		void changeColorStatusBar(
			Color paramColor
			, ContentPage paramPage
		);

		string GetIdentifier();
		Int32? RetornaNivelBateria();
		Boolean IsInternetOnline();
		String GetPushKey();
		void DeletePushKey();
		String GetEmailLogadoFromPrefs();
		void SaveEmail(String paramEmail);
		void CheckPermissions();
		void EnviaPanico();
		void DeslogarRest();

		#region Sizes
		Double GetScreenWidth();
		Double GetHeightStatusBar();
		Double GetScreenHeight();
		#endregion

		#region SequencialPosicao
		string GetSequencialPosicao();
		void SaveSequencialPosicao(String paramOrdem);
		#endregion

		String GetAppName();

		double GetDeviceFontSize();

		void TrackService(Boolean paramIsLocator);

		//void KeepAlive();

        void ChangeTempoTransmissao();

        void OpenWaze(string paramUrl);

        void OpenStreet(String paramLatitude, String paramLongitude);

        void OpenActivity(Boolean paramIsOpen);

        //void Contratar();

    }
}
