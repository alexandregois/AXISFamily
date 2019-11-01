using System;
using System.Threading.Tasks;
using Com.OneSignal;
using family.iOS.Services;
using family.ViewModels;
using Foundation;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;
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
		public static PosicaoSenderService PosicaoSenderService
		{
			get
			{
				if (_posicaoSenderService == null)
				{
					_posicaoSenderService = new PosicaoSenderService();
				}
				return _posicaoSenderService;
			}
		}
		public static PosicaoService PosicaoService { get; set; }

		public static GetBestPosition LocationManager { get; set; }

		public static Boolean StopLocation { get; set; }
		#endregion

		public override bool FinishedLaunching(
			UIApplication app
			, NSDictionary options
		)
		{

			global::Xamarin.Forms.Forms.Init();


            //iOS Family SSAT
            AppCenter.Start("6fe8e8b4-8ba4-44da-ad77-0ec70508b062",
				typeof(Analytics), typeof(Crashes), typeof(Push));



            //OneSignal.Current.StartInit("eb3e8927-71b4-497d-b4d6-3366944d4812").EndInit();
            OneSignal.Current.StartInit("dbe55559-99e9-487c-9b98-2f93fcff459f").EndInit();


            #if DEBUG

			    Xamarin.Calabash.Start();
			    Xamarin.FormsGoogleMaps.Init("AIzaSyBzIBGYnbFo3XOE6fJGYRegaQ3hn1vaBbk");
            
            #else

                Xamarin.FormsGoogleMaps.Init("AIzaSyBw3Voldg8_kywqtlXmqoqxF_3VbUXi2ws");
            #endif


            //UINavigationBar.Appearance.BarTintColor = UIColor.White;
            //UINavigationBar.Appearance.TintColor = UIColor.White;
            UIApplication.SharedApplication
						 .SetStatusBarStyle(UIStatusBarStyle.LightContent, true);


			LoadApplication(new App(true, "2minutos"));

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

			var taskID = UIApplication.SharedApplication.BeginBackgroundTask(() => { });
			new Task(() =>
			{
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