using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using family.Droid.CrossPlataform;
using family.Services.ServiceRealm;
using family.Domain.Realm;
using family.Domain.Enum;
using Android.Graphics;
using System.Globalization;

namespace family.Droid.Services
{
	[Service(
		Enabled = true
		, Exported = true
		, IsolatedProcess = true
		, Name = "br.com.systemsat.PosicaoService"
	)]
	#pragma warning disable CS4014
	#pragma warning disable RECS0022
	#pragma warning disable CS1998
	public class PosicaoService : Service
	{
        //protected App _app => (Application.Current as App);

        private Configuracao _configuracao;
		public Configuracao PropConf
		{
			get
			{
				if(_configuracao == null)
					_configuracao = new Configuracao();
				return _configuracao;
			}
		}

		public bool passOnCreate = false;

		#region Intent
		public override StartCommandResult OnStartCommand(
			Intent intent
			, StartCommandFlags flags
			, int startId
		)
		{
			if(intent != null)
			{
				if(!String.IsNullOrWhiteSpace(intent.Action))
				{
					if(intent.Action.Equals(PropConf.PosicaoStartAction))
					{
						if(!passOnCreate)
							StartPeriodicLocationTask();
					}
					else if(intent.Action.Equals(PropConf.TimerRestart))
					{
						StartPeriodicLocationTask();
					}
				}
			}

			return StartCommandResult.Sticky;
		}

		public override void OnCreate()
		{
			base.OnCreate();
			passOnCreate = true;
			StartPeriodicLocationTask();
		}

		public override void OnDestroy()
		{
			base.OnDestroy();

			CancelGetPosition();

			StopForeground(true);

			string deslogar = "";

			try
			{
				ISharedPreferences preferences = PreferenceManager
					.GetDefaultSharedPreferences(Android.App.Application.Context);
				deslogar = preferences.GetString("deslogar", null);
			}
			catch (Exception){ }
			finally
			{
				if (String.IsNullOrEmpty(deslogar))
				{
					CrossPlataformUtil posicao = new CrossPlataformUtil();
					posicao.TrackService(true);
				}
			}

		}
		#endregion

		#region Service
		IBinder _binder;
		public override IBinder OnBind(Intent intent)
		{
			_binder = new ServiceTestBinder(this);
			return _binder;
		}
		public class ServiceTestBinder : Binder
		{
			public PosicaoService Service { get { return this.LocService; } }
			protected PosicaoService LocService;
			public bool IsBound { get; set; }
			public ServiceTestBinder(PosicaoService service) { this.LocService = service; }
		}
		#endregion

		#region Foreground
		public void RegisterForegroundService()
		{
			CrossPlataformUtil util = 
				new CrossPlataformUtil();

			String text = "Tap to see your family members.";

			Localize loc = new Localize();
			CultureInfo cultura = loc.GetCurrentCultureInfo();

			if(cultura.Name.ToLower() == "pt-br")
			{
				text = "Toque para ver seus familiares.";
			}

			var notification = 
				new Notification
					.Builder(this)
					.SetContentTitle(util.GetAppName())
					.SetContentIntent(BuildIntentToShowMainActivity())
					.SetContentText(text)
					.SetSmallIcon(Resource.Drawable.ic_stat_onesignal_default)
					.SetLargeIcon(
						BitmapFactory.DecodeResource(
							Resources
							, Resource.Drawable.ic_onesignal_large_icon_default
						)
					)
					.SetOngoing(true)
					//.AddAction(BuildRestartTimerAction())
					//.AddAction(BuildStopServiceAction())
					.Build();


			// Enlist this instance of the service as a foreground service
			StartForeground(
				PropConf.PosicaoForegroundId
				, notification
			);
		}

		public PendingIntent BuildIntentToShowMainActivity()
		{
			var notificationIntent = new Intent(
				this
				, typeof(SplashActivity)
			);

			var pendingIntent = PendingIntent.GetActivity(
				this
				, 0
				, notificationIntent
				, PendingIntentFlags.UpdateCurrent
			);
			return pendingIntent;
		}

		#endregion


		public void StartPeriodicLocationTask() 
		{
			try
			{

                TokenDataStore tokenDataSource = new TokenDataStore();

				TokenRealm token = tokenDataSource.Get(1);

				if(token != null)
				{
					if(token.IsLocator)
					{
						RegisterForegroundService();
						Servico(token);                        
					}

                    //ServicoKeepAlive(token);

                }

			} 
			catch(Exception ex) 
			{
				System.Diagnostics
				      .Debug.WriteLine(
					      "FA&)#2 StartPeriodicLocationTask() " +
					      "IniciaLocation PosiçãoService: InnerException: "
					      , ex.InnerException
					     );
				System.Diagnostics
				      .Debug.WriteLine(
					      "FA&)#2 StartPeriodicLocationTask() " +
					      "IniciaLocation PosiçãoService: Message: "
					      , ex.Message
					     );
			}
		}

		public void Servico(TokenRealm paramToken)
		{

			try
			{

				Int64 totalMilliseconds = (long)TimeSpan
					.FromSeconds(paramToken.TempoTransmissao)
					.TotalMilliseconds;
				Context contexto = Android.App.Application.Context;

				AlarmManager alarmMgr = 
					(AlarmManager)contexto.GetSystemService(Context.AlarmService);

				Intent intent = new Intent(
					contexto
					, typeof(GetBestPosition)
				);
					
				intent.PutExtra(
					"evento"
					, ((int)EnumPosicaoEvento.PosicaoTemporizada).ToString()
				);

				PendingIntent alarmIntent = 
					PendingIntent.GetService(
						contexto
						, PropConf.RequestCodeAlarm
						, intent
						, PendingIntentFlags.CancelCurrent
					);

				alarmMgr.Cancel(alarmIntent);
					
				alarmMgr.SetRepeating(
					AlarmType.ElapsedRealtimeWakeup
					, 0
					, totalMilliseconds
					, alarmIntent
				);

			}
			catch(Exception ex)
			{
				StartPeriodicLocationTask();
			}
		}

        private void CancelGetPosition()
		{
			try
			{

				Context contexto = Android.App.Application.Context;

				AlarmManager alarmMgr = 
					(AlarmManager)contexto.GetSystemService(Context.AlarmService);

				Intent intent = new Intent(
					contexto
					, typeof(GetBestPosition)
				);

				PendingIntent alarmIntent = 
					PendingIntent.GetService(
						contexto
						, PropConf.RequestCodeAlarm
						, intent
						, PendingIntentFlags.CancelCurrent
					);

				alarmMgr.Cancel(alarmIntent);

			}
			catch
			{
			}
		}

	}
	#pragma warning restore CS1998
	#pragma warning restore RECS0022
	#pragma warning restore CS4014
}
