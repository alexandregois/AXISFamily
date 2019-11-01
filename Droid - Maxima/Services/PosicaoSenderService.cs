using System;
using Android.App;
using Android.Content;
using Android.OS;
using family.Domain.Realm;
using family.Droid.CustomClass.Position;
using family.Services.ServiceRealm;

namespace family.Droid.Services
{
	[Service(
		Enabled = true
		, Exported = true
		, IsolatedProcess = true
		, Name = "br.com.maxima.PosicaoSenderService"
    )]
	#pragma warning disable CS4014
	#pragma warning disable RECS0022
	#pragma warning disable CS1998
	public class PosicaoSenderService : IntentService
	{
		private static PositionSender _positionSender { get; set; }

		private static DateTime? _dataInicio { get; set; }

		#region Intent
		public override StartCommandResult OnStartCommand(
			Intent intent,
			StartCommandFlags flags,
			int startId
		)
		{
			if(_positionSender != null)
			{
				if(PositionSender.udpClient != null)
				{
					TokenDataStore tokenDataStore = new TokenDataStore();
					TokenRealm token = tokenDataStore.Get(1);
					if(_dataInicio.HasValue)
					{
						Int64 tempo = (long)DateTime.UtcNow.ToUniversalTime()
						                            .Subtract(_dataInicio.Value)
						                            .TotalSeconds;

						if(
							tempo >= token.TempoTransmissao
						)
						{
							PositionSender.udpClient.Close();
							PositionSender.StarSender();
						}
					}
				}
			}
			else
			{
				_positionSender = new PositionSender(this);
				_dataInicio = DateTime.UtcNow;
			}

			return StartCommandResult.Sticky;
		}

		public override void OnCreate()
		{
			base.OnCreate();
			_positionSender = new PositionSender(this);
			_dataInicio = DateTime.UtcNow;
		}

		public override void OnDestroy()
		{
			base.OnDestroy();
		}

		protected override void OnHandleIntent(Intent intent)
		{
		}
		#endregion

		#region Service
		IBinder _binder;
		public override IBinder OnBind(Intent intent)
		{
			_binder = new PosicaoSenderServiceBinder(this);
			return _binder;
		}

		public class PosicaoSenderServiceBinder : Binder
		{
			readonly PosicaoSenderService service;

			public PosicaoSenderServiceBinder(PosicaoSenderService service)
			{
				this.service = service;
			}

			public PosicaoSenderService GetPosicaoSenderService()
			{
				return service;
			}
		}
		#endregion

		public void Stop()
		{
			_positionSender = null;
			_dataInicio = null;
			StopService(
				new Intent (
					Android.App.Application.Context
					, typeof(PosicaoSenderService)
				)
			);
		}
	}
	#pragma warning restore CS1998
	#pragma warning restore RECS0022
	#pragma warning restore CS4014
}
