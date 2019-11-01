using System;
using Android.App;
using Android.Content;
using Android.OS;
using family.Domain.Enum;
using Microsoft.AppCenter.Crashes;

namespace family.Droid.Services
{
	[Service(
		Enabled = true
		, Exported = true
		, IsolatedProcess = true
		, Name = "br.com.alltechseguranca.PanicoService"
    )]
	public class PanicoService : IntentService
	{
		IBinder binder;

		protected override void OnHandleIntent(Intent intent)
		{
			try
			{

				Context contexto = Android.App.Application.Context;

				contexto.StopService(new Intent(
					contexto
					, typeof(GetBestPosition)
				));

				Intent startPosicaoService = 
					new Intent(
						contexto
						, typeof(GetBestPosition)
					);
				startPosicaoService.PutExtra(
					"evento"
					, ((int)EnumPosicaoEvento.BotaoPanicoAtivado).ToString()
				);

				contexto.StartService(startPosicaoService);

			}
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }
		}

		public override IBinder OnBind(Intent intent)
		{
			binder = new PanicoServiceBinder(this);
			return binder;
		}
	}

	public class PanicoServiceBinder : Binder
	{
		readonly PanicoService service;

		public PanicoServiceBinder(PanicoService service)
		{
			this.service = service;
		}

		public PanicoService GetPanicoService()
		{
			return service;
		}
	}

}
