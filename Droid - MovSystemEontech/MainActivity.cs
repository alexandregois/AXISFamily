using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.OS;
using family.Domain.Realm;
using family.Droid.CrossPlataform;
using family.Droid.CustomClass;
using family.Droid.Services;
using family.Services.ServiceRealm;
using Plugin.LocalNotifications;
using Plugin.Toasts;
using System;

namespace family.Droid
{
    [Activity(
        Label = "MovSystem"
        , Theme = "@style/MyTheme"
        , ConfigurationChanges = ConfigChanges.ScreenSize
        | ConfigChanges.Orientation
        , ScreenOrientation = ScreenOrientation.Portrait
    )]
    [IntentFilter(
        new[] { Android.Content.Intent.ActionView },
        Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable, Android.Content.Intent.CategoryAlternative},

        DataScheme = "ssxfamily",
        //DataHost = "family.systemsatx.com.br",
        DataHosts = new String[] { "family.systemsatx.com.br",
		"200.152.54.164", "200.152.54.164:83"},
        DataPathPrefixes = new String[] { "/Tracking" }

    )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public string text { get; set; }

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

        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            global::Xamarin.FormsGoogleMaps.Init(this, bundle);

            


            //ServicoKeepAlive();


            CrossPlataformUtil util = new CrossPlataformUtil();
            util.TrackKeepAlive(true);

            LocalNotificationsImplementation.NotificationIconId = Resource.Drawable.ic_launcher;
            

            IsPlayServicesAvailable();

            LoadApplication(new App(true, ""));
        }

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    text = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                else
                {
                    text = "This device is not supported";
                    Finish();
                }
                return false;
            }
            else
            {
                text = "Google Play Services is available.";
                return true;
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions
                                                        , Permission[] grantResults)
        {
            PermissionsHandler.CheckPermissions(
                Android.App.Application.Context
                , this
            );
        }

        public void ServicoKeepAlive()
        {
            TokenDataStore tokenDataSource = new TokenDataStore();

            TokenRealm token = tokenDataSource.Get(1);

            try
            {

                Int64 totalMilliseconds = (long)TimeSpan
                    .FromSeconds(PropConf.TempoKeepAlive)
                    .TotalSeconds;
                Context contexto = Android.App.Application.Context;

                AlarmManager alarmMgrKeepAlive =
                    (AlarmManager)contexto.GetSystemService(Context.AlarmService);

                Intent intent = new Intent(
                    contexto
                    , typeof(KeepAliveService)
                );

                PendingIntent alarmIntentKeepAlive =
                    PendingIntent.GetService(
                        contexto
                        , PropConf.RequestCodeAlarm
                        , intent
                        , PendingIntentFlags.CancelCurrent
                    );

                alarmMgrKeepAlive.Cancel(alarmIntentKeepAlive);

                alarmMgrKeepAlive.SetRepeating(
                    AlarmType.ElapsedRealtimeWakeup
                    , 0
                    , totalMilliseconds * 1000
                    , alarmIntentKeepAlive
                );

            }
            catch (Exception ex)
            {
                ServicoKeepAlive();
            }
        }

    }
}
