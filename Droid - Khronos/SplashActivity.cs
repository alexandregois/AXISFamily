﻿using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using family.Droid.CustomClass;

namespace family.Droid
{
    [Activity(
        Theme = "@style/familySplash.Theme"
        , MainLauncher = true
        , NoHistory = true
        , ScreenOrientation = ScreenOrientation.Portrait
    )]
    //[Activity(
    //    Theme = "@style/familySplash.Theme"
    //    , MainLauncher = true
    //    , NoHistory = true
    //    , ScreenOrientation = ScreenOrientation.Portrait
    //)]
    public class SplashActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			//SetContentView(Resource.Layout.Main);
		}

		// Launches the startup task
		protected override void OnResume()
		{
			base.OnResume();
			Task startupWork = new Task(() => { SimulateStartup(); });
			startupWork.Start();
		}

		// Prevent the back button from canceling the startup process
		public override void OnBackPressed() { }

		// Simulates background work that happens behind the splash screen
		async void SimulateStartup()
		{
            StartActivity(new Intent(Android.App.Application.Context, typeof(MainActivity)));
        }

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions
		                                                , Permission[] grantResults)
		{
			PermissionsHandler.CheckPermissions(
				Android.App.Application.Context
				, this
			);
		}

	}
}