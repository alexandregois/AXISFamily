using System;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Net;
using Android.OS;
using Android.Preferences;
using Android.Telephony;
using Android.Views;
using family.CrossPlataform;
using family.Droid.CrossPlataform;
using family.Droid.CustomClass;
using family.Droid.Services;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(CrossPlataformUtil))]
namespace family.Droid.CrossPlataform
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
    public class CrossPlataformUtil : ICrossPlataformUtil
    {
        protected App _app => (Xamarin.Forms.Application.Current as App);

        public void changeColorStatusBar(
            Color paramColor
            , Xamarin.Forms.ContentPage paramPage
        )
        {
            try
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    Window _window = ((Activity)Forms.Context).Window;
                    //((Activity)paramPage.).Window
                    _window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                    _window.ClearFlags(WindowManagerFlags.TranslucentStatus);

                    _window.SetStatusBarColor(paramColor.ToAndroid());
                }

            }
            catch (Exception) { }
        }

        public string GetIdentifier()
        {

            String ret = "";

            try
            {

                String strDeviceId = Android.Provider.Settings.Secure.GetString(Forms.Context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);

                Context contexto = Android.App.Application.Context;
                TelephonyManager telephonyManager =
                    (TelephonyManager)contexto.GetSystemService(Context.TelephonyService);


                if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.O)
                {
                    try
                    {
                        ret = telephonyManager.GetDeviceId(0);
                    }
                    catch (Exception)
                    {
                        try
                        {
                            ret = telephonyManager.GetDeviceId(1);
                        }
                        catch (Exception)
                        {
                            //ret = telephonyManager.DeviceId;
                            //if (ret == null)
                            ret = strDeviceId;
                        }
                    };
                }
                else
                {
                    try
                    {
                        ret = telephonyManager.GetImei(0);
                        if (ret == null)
                            ret = strDeviceId;
                    }
                    catch (Exception)
                    {
                        try
                        {
                            //ret = telephonyManager.GetImei(1);
                            ret = strDeviceId;
                        }
                        catch (Exception)
                        {
                            ret = telephonyManager.Imei;
                        }
                    };
                }

            }
            catch (Exception) { }

            return ret;
        }

        public Int32? RetornaNivelBateria()
        {
            Int32? bateria = 0;

            try
            {
                Context contexto = Android.App.Application.Context;
                IntentFilter ifilter = new IntentFilter(Intent.ActionBatteryChanged);
                Intent batteryStatus = contexto.RegisterReceiver(null, ifilter);
                bateria = batteryStatus.GetIntExtra(BatteryManager.ExtraLevel, -1);
            }
            catch (Exception) { }

            return bateria;
        }

        public Boolean IsInternetOnline()
        {
            Boolean ret = false;
            try
            {
                Context contexto = Android.App.Application.Context;
                ConnectivityManager cm = (ConnectivityManager)contexto.GetSystemService(Context.ConnectivityService);
                NetworkInfo netInfo = cm.ActiveNetworkInfo;
                ret = (netInfo != null && netInfo.IsConnectedOrConnecting);
            }
            catch (Exception) { }

            return ret;
        }

        public String GetPushKey()
        {
            String ret = null;
            try
            {
                Context contexto = Android.App.Application.Context;
                ISharedPreferences preferences = PreferenceManager.GetDefaultSharedPreferences(contexto);
                ret = preferences.GetString("pushKey_id", null);
            }
            catch (Exception) { }
            return ret;
        }

        public void DeletePushKey()
        {
            try
            {
                //Context contexto = Android.App.Application.Context;
                //Intent startServiceIntent = new Intent(contexto, typeof(DeleteTokenService));
                //contexto.StartService(startServiceIntent);
            }
            catch (Exception) { }
        }

        public String GetEmailLogadoFromPrefs()
        {
            String ret = null;
            try
            {
                Context contexto = Android.App.Application.Context;
                ISharedPreferences preferences = PreferenceManager.GetDefaultSharedPreferences(contexto);
                ret = preferences.GetString("email_logado", null);
            }
            catch (Exception) { }
            return ret;
        }

        public void SaveEmail(String paramEmail)
        {
            try
            {
                Context contexto = Android.App.Application.Context;
                // Access Shared Preferences
                ISharedPreferences preferences = PreferenceManager.GetDefaultSharedPreferences(contexto);
                ISharedPreferencesEditor editor = preferences.Edit();

                // Save to SharedPreferences
                editor.PutString("email_logado", paramEmail);
                editor.Apply();
            }
            catch (Exception) { }
        }

        public void CheckPermissions()
        {
            PermissionsHandler.CheckPermissions(Android.App.Application.Context, ((Activity)Android.App.Application.Context));
        }

        public void EnviaPanico()
        {
            try
            {
                Context contexto = Android.App.Application.Context;
                Intent startServiceIntent = new Intent(contexto, typeof(PanicoService));
                contexto.StartService(startServiceIntent);
            }
            catch (Exception) { }
        }


        public void DeslogarRest()
        {
            try
            {
                Context contexto = Android.App.Application.Context;

                // Access Shared Preferences
                ISharedPreferences preferences = PreferenceManager.GetDefaultSharedPreferences(contexto);
                ISharedPreferencesEditor editor = preferences.Edit();

                editor.Clear();

                // Save to SharedPreferences
                editor.PutString("deslogar", "true");
                editor.Apply();

                TrackService(false);

                OpenActivity(false);


            }
            catch (Exception) { }
        }

        public void TrackService(Boolean paramIsLocator)
        {
            try
            {
                Context contexto = Android.App.Application.Context;
                Intent intent =
                    new Intent(
                        contexto
                        , typeof(PosicaoService)
                    );
                if (paramIsLocator)
                {
                    Configuracao conf = new Configuracao();
                    intent.SetAction(conf.PosicaoStartAction);
                    contexto.StartService(intent);
                }
                else
                {
                    contexto.StopService(intent);
                }
            }
            catch (Exception) { }
        }

        public void OpenActivity(Boolean paramIsOpen)
        {
            try
            {

                //Intent i = new Intent(context, typeof(MainActivity)); //Abre o App no reboot do celular
                //i.AddFlags(ActivityFlags.NewTask);
                //context.StartActivity(i);

                Context contexto = Android.App.Application.Context;
                Intent intent =
                    new Intent(
                        contexto
                        , typeof(MainActivity)
                    );
                if (paramIsOpen)
                {
                    contexto.StartService(intent);
                }
                else
                {
                    contexto.StopService(intent);
                }
            }
            catch (Exception) { }
        }

        public void TrackKeepAlive(Boolean isKeepAlive)
        {

            try
            {
                Configuracao conf = new Configuracao();

                Int64 totalMilliseconds = (long)TimeSpan
                    .FromSeconds(conf.TempoKeepAlive)
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
                        , conf.RequestCodeAlarm
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
                TrackKeepAlive(true);
            }
        }

        public void ChangeTempoTransmissao()
        {
            try
            {
                Context contexto = Android.App.Application.Context;
                Intent intent =
                    new Intent(
                        contexto
                        , typeof(PosicaoService)
                    );
                Configuracao conf = new Configuracao();
                intent.SetAction(conf.TimerRestart);
                contexto.StartService(intent);
            }
            catch (Exception) { }
        }


        #region SequencialPosicao
        public string GetSequencialPosicao()
        {
            String ret = null;
            try
            {
                Context contexto = Android.App.Application.Context;
                ISharedPreferences preferences = PreferenceManager.GetDefaultSharedPreferences(contexto);
                ret = preferences.GetString("posicao_ordem", null);
            }
            catch (Exception) { }
            return ret;
        }

        public void SaveSequencialPosicao(string paramSequencial)
        {
            try
            {
                Context contexto = Android.App.Application.Context;
                // Access Shared Preferences
                ISharedPreferences preferences = PreferenceManager.GetDefaultSharedPreferences(contexto);
                ISharedPreferencesEditor editor = preferences.Edit();

                // Save to SharedPreferences
                editor.PutString("posicao_ordem", paramSequencial);
                editor.Apply();
            }
            catch (Exception) { }
        }
        #endregion

        public String GetAppName()
        {
            String label = "SSX Family";
            try
            {
                Context contexto = Android.App.Application.Context;
                ApplicationInfo applicationInfo = contexto.ApplicationInfo;
                int stringId = applicationInfo.LabelRes;
                label = stringId == 0 ?
                    applicationInfo.NonLocalizedLabel.ToString()
                                   : contexto.GetString(stringId);
            }
            catch (Exception) { }

            return label;
        }

        public double GetScreenWidth()
        {

            return (Resources.System.DisplayMetrics.WidthPixels
                    / Resources.System.DisplayMetrics.Density);
        }

        public double GetHeightStatusBar()
        {
            int result = 0;
            int resourceId = Resources.System.GetIdentifier("status_bar_height", "dimen", "android");
            if (resourceId > 0)
            {
                result = Resources.System.GetDimensionPixelSize(resourceId);
            }
            return result;
        }

        public double GetScreenHeight()
        {
            return ((Resources.System.DisplayMetrics.HeightPixels
                     - GetHeightStatusBar()) / Resources.System.DisplayMetrics.Density);
        }

        public double GetDeviceFontSize()
        {
            //https://stackoverflow.com/questions/17228378/device-font-size
            double scale = Resources.System.Configuration.FontScale;
            //GetResources().getConfiguration().fontScale;
            return scale;
        }

        public DateTime Epoch2DateTime(long paramTime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(Math.Round(paramTime / 1000d))
                .ToUniversalTime();
        }

        public void OpenWaze(String paramUrl)
        {
            try
            {
                var wazeURL = paramUrl;
                var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(wazeURL));
                Android.App.Application.Context.StartActivity(intent);
            }
            catch (ActivityNotFoundException ex)
            {
                // If Waze is not installed, open it in Google Play:
                var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("market://details?id=com.waze"));
                Android.App.Application.Context.StartActivity(intent);
            }

        }

        public void OpenStreet(String paramLatitude, String paramLongitude)
        {
            try
            {
                Android.Net.Uri gmmIntentUri = Android.Net.Uri.Parse("google.streetview:cbll=" + paramLatitude + "," + paramLongitude);

                Intent mapIntent = new Intent(Intent.ActionView, gmmIntentUri);
                mapIntent.SetPackage("com.google.android.apps.maps");

                Android.App.Application.Context.StartActivity(mapIntent);

                //var streetViewUri = Android.Net.Uri.Parse("google.streetview:cbll=" + paramLatitude + ", " + paramLongitude + "&cbp=1,90,,0,1.0&mz=20");
                //var streetViewIntent = new Intent(Intent.ActionView, streetViewUri);
                //Android.App.Application.Context.StartActivity(streetViewIntent);

            }
            catch (ActivityNotFoundException ex)
            {
                // If Waze is not installed, open it in Google Play:
                var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("market://details?id=com.google.android.street"));
                Android.App.Application.Context.StartActivity(intent);
            }

        }
    }
#pragma warning restore RECS0022
#pragma warning restore CS4014
}