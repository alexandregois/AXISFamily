using System;
using System.Threading.Tasks;
using Com.OneSignal;
using family.iOS.Services;
using family.ViewModels;
using Foundation;
using Microsoft.AppCenter.Crashes;
using UIKit;
using UserNotifications;

namespace family.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{

		private Configuracao _configuracao;
		public Configuracao PropConf
		{
			get
			{
				if (_configuracao == null)
					_configuracao = new Configuracao();
				return _configuracao;
			}
		}

		#region LocationManager
		private static PosicaoSenderService _posicaoSenderService;
		public static PosicaoSenderService PosicaoSenderService { 
			get
			{
				if(_posicaoSenderService == null)
				{
					_posicaoSenderService = new PosicaoSenderService();
				}
				return _posicaoSenderService;
			}
		}
		public static PosicaoService PosicaoService { get; set; }

        //public static CLLocationManager _locationManager;
        //public static CLLocationManager LocationManager
        //{
        //	get
        //	{
        //		if(_locationManager == null)
        //		{
        //			_locationManager = new CLLocationManager ();
        //			_locationManager.PausesLocationUpdatesAutomatically = false;
        //			//set the desired accuracy, in meters
        //			_locationManager.DesiredAccuracy = CLLocation.AccuracyKilometer;
        //
        //			// iOS 8 has additional permissions requirements
        //			if (UIDevice.CurrentDevice.CheckSystemVersion (8, 0)) {
        //				_locationManager.RequestAlwaysAuthorization (); // works in background
        //				//locMgr.RequestWhenInUseAuthorization (); // only in foreground
        //			}
        //
        //			// iOS 9 requires the following for background location updates
        //			// By default this is set to false and will not allow background updates
        //			if (UIDevice.CurrentDevice.CheckSystemVersion (9, 0)) {
        //				_locationManager.AllowsBackgroundLocationUpdates = true;
        //			}
        //		}
        //		return _locationManager;
        //	}
        //}

        public static GetBestPosition LocationManager { get; set; }

		public static Boolean StopLocation { get; set; }
		#endregion

		public override bool FinishedLaunching(
			UIApplication app
			, NSDictionary options
		)
		{

			global::Xamarin.Forms.Forms.Init();

            OneSignal.Current.StartInit("eb3e8927-71b4-497d-b4d6-3366944d4812").EndInit();

			#if DEBUG
						Xamarin.Calabash.Start();
						Xamarin.FormsGoogleMaps.Init("AIzaSyBzIBGYnbFo3XOE6fJGYRegaQ3hn1vaBbk");
			#else
						Xamarin.FormsGoogleMaps.Init("AIzaSyBw3Voldg8_kywqtlXmqoqxF_3VbUXi2ws");
			#endif


			LoadApplication(new App(true, "maxima"));

            return base.FinishedLaunching(app, options);
		}

		// The following Exports are needed to run OneSignal in the iOS Simulator.
		//   The simulator doesn't support push however this prevents a crash due to a Xamarin bug
		//   https://bugzilla.xamarin.com/show_bug.cgi?id=52660
		[Export("oneSignalApplicationDidBecomeActive:")]
		public void OneSignalApplicationDidBecomeActive(UIApplication application)
		{
			// Remove line if you don't have a OnActivated method.
			OnActivated(application);
		}

		[Export("oneSignalApplicationWillResignActive:")]
		public void OneSignalApplicationWillResignActive(UIApplication application)
		{
			// Remove line if you don't have a OnResignActivation method.
			OnResignActivation(application);
		}

		[Export("oneSignalApplicationDidEnterBackground:")]
		public void OneSignalApplicationDidEnterBackground(UIApplication application)
		{

			var taskID = UIApplication.SharedApplication.BeginBackgroundTask( () => {});
			new Task ( () => {
				KeepAlive();
				UIApplication.SharedApplication.EndBackgroundTask(taskID);
			}).Start();


			// Remove line if you don't have a DidEnterBackground method.
			DidEnterBackground(application);
		}

		[Export("oneSignalApplicationWillTerminate:")]
		public void OneSignalApplicationWillTerminate(UIApplication application)
		{
			// Remove line if you don't have a WillTerminate method.
			WillTerminate(application);
		}

		public void KeepAlive()
		{
			Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(_configuracao.TempoKeepAlive), () =>
			{
				try
				{
					if (PropConf.IsLogado == true)
					{
						ViewModelConfiguracao viewModelConfiguracao = new ViewModelConfiguracao();
						viewModelConfiguracao.CheckKeepAlive();
					}

				}
				catch (Exception ex)
				{
					Crashes.TrackError(ex);
				}
                

                return true;
			});

		}


	}
}