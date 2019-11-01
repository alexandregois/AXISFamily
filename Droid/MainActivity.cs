using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.Locations;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Telephony;
using Android.Widget;
using Com.OneSignal;
using family.Domain.Realm;
using family.Droid.CrossPlataform;
using family.Droid.CustomClass;
using family.Droid.Services;
using family.Services.ServiceRealm;
using Firebase;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.LocalNotifications;
using Plugin.Permissions;
using Plugin.Toasts;
using System;
using Xamarin.Forms;
using Notification = Android.App.Notification;

namespace family.Droid
{
    [Activity(
        Label = "SSX Family"
        , Theme = "@style/MyTheme"
        , MainLauncher = true
        , ConfigurationChanges = ConfigChanges.ScreenSize
        | ConfigChanges.Orientation

        , ScreenOrientation = ScreenOrientation.Portrait
    )]
    [IntentFilter(
        new[] { Android.Content.Intent.ActionView },
        Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable, Android.Content.Intent.CategoryAlternative, Android.Content.Intent.CategoryLauncher },
        DataScheme = "ssxfamily",
        DataHosts = new String[] { "family.systemsatx.com.br", "familyhomolog.systemsatx.com.br", "10.10.3.174:26038",
        "200.152.54.164", "200.152.54.164:83"},
        DataPathPrefixes = new String[] { "/tracking" }

    )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, ActivityCompat.IOnRequestPermissionsResultCallback
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

        public int MY_PERMISSIONS_REQUEST_READ_CONTACTS { get; private set; }

        //[Obsolete]
        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);

            OneSignal.Current.StartInit("dbe55559-99e9-487c-9b98-2f93fcff459f")
                  .EndInit();

            global::Xamarin.Forms.Forms.Init(this, bundle);

            global::Xamarin.FormsGoogleMaps.Init(this, bundle);


            AppCenter.Start("ddea3d2d-538f-4994-8793-e2b6417e393c",
                   typeof(Analytics), typeof(Crashes));


            try
            {
                CrossPlataformUtil util = new CrossPlataformUtil();
                util.TrackKeepAlive(true);

                FirebaseOptions options = new FirebaseOptions.Builder()
                            .SetApplicationId("br.com.systemsat.family")
                            .SetDatabaseUrl("https://ssx-family-f47b6.firebaseio.com")
                            .SetGcmSenderId("310951902575")
                            .Build();

                //var apps = Firebase.FirebaseApp.GetApps(this);
                //if (apps.Count == 0)
                //{
                    FirebaseApp.InitializeApp(this, options);
                //}

                //Firebase.FirebaseApp.InitializeApp(this);
                //Firebase.Iid.FirebaseInstanceId.GetInstance(Firebase.FirebaseApp.InitializeApp(this));

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            LocalNotificationsImplementation.NotificationIconId = Resource.Drawable.ic_launcher;

            IsPlayServicesAvailable();

            LoadApplication(new App(false, "family"));

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

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
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

        protected override void OnStart()
        {
            base.OnStart();

            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation,
                    Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage, Manifest.Permission.ReadPhoneState}, 0);
            }

        }

    }
}
