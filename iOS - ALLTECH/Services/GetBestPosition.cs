using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoreLocation;
using family.Domain.Enum;
using family.iOS.CrossPlataform.Position;
using family.ViewModels.InterfaceServices;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace family.iOS.Services
{
	#pragma warning disable CS4014
	#pragma warning disable RECS0022
	#pragma warning disable CS1998
	public class GetBestPosition : CLLocationManagerDelegate
	{
		public static CLLocationManager LocationManager;

		public Boolean IsStopped = false;

		public App _app => (Xamarin.Forms.Application.Current as App);

		private IMessageService _messageService;

        public static CLLocation LastLocation;

		private static Configuracao _configuracao;
		public static Configuracao Configuracao
		{
			get
			{
				if(_configuracao == null)
					_configuracao = new Configuracao();
				return _configuracao;
			}
			set
			{
				_configuracao = value;
			}
		}

		private static EnumPosicaoEvento _eventoPosicao = EnumPosicaoEvento.PosicaoTemporizada;
		public static EnumPosicaoEvento EventoPosicao
		{
			get
			{
				return _eventoPosicao;
			}
			set
			{
				_eventoPosicao = value;
			}
		}

		protected GetBestPosition(NSObjectFlag t) : base(t)
		{
		}

		protected internal GetBestPosition(IntPtr handle) : base(handle)
		{
		}

		public GetBestPosition()
		{
            _messageService =
                   DependencyService.Get<IMessageService>();

            LocationManager = new CLLocationManager ();
			LocationManager.PausesLocationUpdatesAutomatically = true;
			//set the desired accuracy, in meters
			LocationManager.DesiredAccuracy = CLLocation.AccuracyHundredMeters;
			LocationManager.AllowsBackgroundLocationUpdates = true;
			LocationManager.RequestAlwaysAuthorization();

			LocationManager.Delegate = this;

			StartLocationUpdates();
		}

		public void StartLocationUpdates ()
		{
			switch(CLLocationManager.Status)
			{
				case CLAuthorizationStatus.NotDetermined:
				case CLAuthorizationStatus.Restricted:
				case CLAuthorizationStatus.Denied:
				case CLAuthorizationStatus.AuthorizedWhenInUse:
					LocationManager.RequestAlwaysAuthorization();
					break;
				case CLAuthorizationStatus.AuthorizedAlways:
					LocationManager.StartUpdatingLocation();
					//LocationManager.StartMonitoringSignificantLocationChanges();
					break;
			}
		}

		public static void Stop()
		{
			LocationManager.StopUpdatingLocation();
			LocationManager.StopMonitoringSignificantLocationChanges();
		}

		public override void AuthorizationChanged (CLLocationManager manager, CLAuthorizationStatus status)
		{
			switch(CLLocationManager.Status)
			{
				case CLAuthorizationStatus.Denied:
					OpenSettingGps();
					break;
				case CLAuthorizationStatus.NotDetermined:
				case CLAuthorizationStatus.Restricted:
				case CLAuthorizationStatus.AuthorizedWhenInUse:
					AlertaAuthorizationChanged();
					break;
				case CLAuthorizationStatus.AuthorizedAlways:
					//AppDelegate.PosicaoService.EnviaPosicao((int)EnumPosicaoEvento.PosicaoTemporizada);
					LocationManager.StartUpdatingLocation();
					//LocationManager.StartMonitoringSignificantLocationChanges();
					break;
			}

		}

		private void AlertaAuthorizationChanged()
		{
			TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

			String titulo = "Tracking Authorization";
			//String texto = "Please enable the authorization for always track.";
			String texto = "Authorization required for tracking.";
			String cancelText = "Close";

			if(Thread.CurrentThread.CurrentCulture.Name == "pt-BR")
			{
				titulo = "Autorização de rastreamento";
				//texto = "Por favor, ative a autorização para sempre rastrear.";
				texto = "Autorização necessária para rastreamento.";
				cancelText = "Fechar";
			}

			UIAlertController alert = UIAlertController.Create(
				titulo
				, texto
				, UIAlertControllerStyle.Alert
			);

			alert.AddAction(UIAlertAction.Create(cancelText, UIAlertActionStyle.Default, a => taskCompletionSource.SetResult(false)));

			UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert,true,null);

			//TODO: Deveria Exister um evento para Rastreamento Desligado
			AppDelegate.PosicaoService.EnviaPosicao((int)EnumPosicaoEvento.PosicaoTemporizada);
			IsStopped = true;
		}

		public override void DeferredUpdatesFinished (CLLocationManager manager, Foundation.NSError error)
		{
			System.Diagnostics.Debug.WriteLine(
				String.Format(
					"FA&)#2 DeferredUpdatesFinished() Data:D {0}"
					, DateTime.UtcNow
				)
			);
		}

		public override void DidRangeBeacons (CLLocationManager manager, CLBeacon[] beacons, CLBeaconRegion region)
		{
			System.Diagnostics.Debug.WriteLine(
				String.Format(
					"FA&)#2 DidRangeBeacons() Data:D {0}"
					, DateTime.UtcNow
				)
			);
		}

		public override void DidStartMonitoringForRegion (CLLocationManager manager, CLRegion region)
		{
			System.Diagnostics.Debug.WriteLine(
				String.Format(
					"FA&)#2 DidStartMonitoringForRegion() Data:D {0}"
					, DateTime.UtcNow
				)
			);
		}

		public override void DidVisit (CLLocationManager manager, CLVisit visit)
		{
			System.Diagnostics.Debug.WriteLine(
				String.Format(
					"FA&)#2 DidVisit() Data:D {0}"
					, DateTime.UtcNow
				)
			);
		}

		public override void Failed (CLLocationManager manager, Foundation.NSError error)
		{
			System.Diagnostics.Debug.WriteLine(
				String.Format(
					"FA&)#2 Failed() Data:D {0}"
					, DateTime.UtcNow
				)
			);
		}

		public override void LocationsUpdated (CLLocationManager manager, CLLocation[] locations)
		{

			if(locations != null)
			{
				CLLocation actualLocation = locations.Last();
				PositionServiceHelper positionServiceHelper = new PositionServiceHelper();

				if(EventoPosicao == EnumPosicaoEvento.BotaoPanicoAtivado)
				{
					positionServiceHelper.MakeMessage(
						actualLocation
						, (int)EventoPosicao
					);
					EventoPosicao = EnumPosicaoEvento.PosicaoTemporizada;
				}
				else if(CanSendLastPosition(GetBestPosition.LastLocation, actualLocation))
				{

					positionServiceHelper.MakeMessage(
						actualLocation
						, (int)EventoPosicao
					);
					LastLocation = actualLocation;
				}
			}
			else
			{

			}

		}

		public override void LocationUpdatesPaused (CLLocationManager manager)
		{
			System.Diagnostics.Debug.WriteLine(
				String.Format(
					"FA&)#2 LocationUpdatesPaused() Data:D {0}"
					, DateTime.UtcNow
				)
			);
		}

		public override void LocationUpdatesResumed (CLLocationManager manager)
		{
			System.Diagnostics.Debug.WriteLine(
				String.Format(
					"FA&)#2 LocationUpdatesResumed() Data:D {0}"
					, DateTime.UtcNow
				)
			);
		}

		public override void MonitoringFailed (CLLocationManager manager, CLRegion region, Foundation.NSError error)
		{
			System.Diagnostics.Debug.WriteLine(
				String.Format(
					"FA&)#2 MonitoringFailed() Data:D {0}"
					, DateTime.UtcNow
				)
			);
		}

		public override void RangingBeaconsDidFailForRegion (CLLocationManager manager, CLBeaconRegion region, Foundation.NSError error)
		{
			System.Diagnostics.Debug.WriteLine(
				String.Format(
					"FA&)#2 RangingBeaconsDidFailForRegion() Data:D {0}"
					, DateTime.UtcNow
				)
			);
		}

		public override void RegionEntered (CLLocationManager manager, CLRegion region)
		{
			System.Diagnostics.Debug.WriteLine(
				String.Format(
					"FA&)#2 RegionEntered() Data:D {0}"
					, DateTime.UtcNow
				)
			);
		}

		public override void RegionLeft (CLLocationManager manager, CLRegion region)
		{
			System.Diagnostics.Debug.WriteLine(
				String.Format(
					"FA&)#2 RegionLeft() Data:D {0}"
					, DateTime.UtcNow
				)
			);
		}

		public override void UpdatedHeading (CLLocationManager manager, CLHeading newHeading)
		{
			System.Diagnostics.Debug.WriteLine(
				String.Format(
					"FA&)#2 UpdatedHeading() Data:D {0}"
					, DateTime.UtcNow
				)
			);
		}

		public override void UpdatedLocation (CLLocationManager manager, CLLocation newLocation, CLLocation oldLocation)
		{
			System.Diagnostics.Debug.WriteLine(
				String.Format(
					"FA&)#2 UpdatedLocation() Data:D {0}"
					, DateTime.UtcNow
				)
			);
		}

        private async void OpenSettingGps()
        {
            try
            {

                Boolean envia = await _messageService.ShowAlertChooseAsync(
                                _app.MessageGps("GpsSettings")
                                , _app.MessageGps("No")
                                , _app.MessageGps("GpsOpenSettings")
                                , _app.MessageGps("Alert")
                            );

                if (envia == true)
                {
					//StartActivity(new Intent(Android.Provider.Settings.ActionLocationSourceSettings));
					NSUrl url = new NSUrl(UIKit.UIApplication.OpenSettingsUrlString);
					bool result = UIApplication.SharedApplication.OpenUrl(url);
				}

            }
            catch (Exception ex)
            {
            }

        }

        private Boolean CanSendLastPosition(CLLocation paramLastLocation, CLLocation paramActualLocation)
		{
			Boolean can = false;

			if(paramLastLocation != null)
			{
				DateTime dataPos = paramLastLocation.Timestamp.ToDateTime();

				DateTime dataAtual = paramActualLocation.Timestamp.ToDateTime();

				double dataDiff = dataAtual.Subtract(dataPos)
				                           .TotalSeconds;

				//System.Diagnostics.Debug.WriteLine(
				//	String.Format("FA&)#2 Data Posição: {0}", dataPos)
				//);

				//System.Diagnostics.Debug.WriteLine(
				//	String.Format("FA&)#2 Data Atual: {0}", dataAtual)
				//);

				//System.Diagnostics.Debug.WriteLine(
				//	String.Format("FA&)#2 Data Diff: {0}", dataDiff)
				//);

				can = (dataDiff >= Configuracao.TempoTransmissaoPadrao);

				if(can)
				{
					System.Diagnostics.Debug.WriteLine(
						String.Format("FA&)#2 Usou a última")
					);
				}
			}
			else
			{
				can = true;
			}

			return can;
		}

	}


	#pragma warning restore CS1998
	#pragma warning restore RECS0022
	#pragma warning restore CS4014
}
