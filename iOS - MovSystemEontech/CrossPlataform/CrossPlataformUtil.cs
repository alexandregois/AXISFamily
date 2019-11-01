using System;
using System.Runtime.InteropServices;
using family.CrossPlataform;
using family.iOS.CrossPlataform;
using Foundation;
using UIKit;
using Xamarin.Forms;
using family.iOS.Services;

[assembly: Dependency(typeof(CrossPlataformUtil))]
namespace family.iOS.CrossPlataform
{
    public class CrossPlataformUtil : ICrossPlataformUtil
    {

        public void changeColorStatusBar(
            Color paramColor
            , Xamarin.Forms.ContentPage paramPage
        )
        {
            paramPage.BackgroundColor = paramColor;
            UINavigationBar.Appearance.BarTintColor = UIColor.White;
            UINavigationBar.Appearance.TintColor = UIColor.White;
            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, true);
        }

        #region Imei
        [DllImport("/System/Library/Frameworks/IOKit.framework/IOKit")]
        private static extern uint IOServiceGetMatchingService(uint masterPort, IntPtr matching);

        [DllImport("/System/Library/Frameworks/IOKit.framework/IOKit")]
        private static extern IntPtr IOServiceMatching(string s);

        [DllImport("/System/Library/Frameworks/IOKit.framework/IOKit")]
        private static extern IntPtr IORegistryEntryCreateCFProperty(uint entry, IntPtr key
                                                                     , IntPtr allocator, uint options);

        [DllImport("/System/Library/Frameworks/IOKit.framework/IOKit")]
        private static extern int IOObjectRelease(uint o);


        public string GetIdentifier()
        {
            string serial = string.Empty;
            try
            {
                NSUuid tempImei = UIDevice.CurrentDevice.IdentifierForVendor;
                String str = tempImei.AsString().Replace("-", "");

                if (str.Length > 50)
                {
                    serial = str.Substring(0, 50);
                }
                else
                {
                    serial = str;
                }

            }
            catch (Exception) { }

            return serial;
        }
        #endregion

        public DateTime NSDateToDateTime(Foundation.NSDate date)
        {
            DateTime reference = new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var utcDateTime = reference.AddSeconds(date.SecondsSinceReferenceDate);
            return utcDateTime.ToLocalTime();
        }

        public Int32? RetornaNivelBateria()
        {
            int bateria = 0;
            try
            {
                UIDevice temp = UIDevice.CurrentDevice;
                if (temp.BatteryState != UIDeviceBatteryState.Unknown)
                {
                    bateria = Convert.ToInt32(temp.BatteryLevel * 100);
                }
                //UIDevice tempBateria = UIDevice.CurrentDevice.BatteryLevel;
                //if(tempBateria >= 0){
                //	bateria = Convert.ToInt32(tempBateria * 100);
                //}
            }
            catch (Exception) { }

            return bateria;
        }

        public bool IsInternetOnline()
        {

            return true;
        }

        public string GetPushKey()
        {
            return "";
        }

        public void DeletePushKey()
        {

        }

        public string GetEmailLogadoFromPrefs()
        {
            var plist = NSUserDefaults.StandardUserDefaults;
            var useHeader = plist.StringForKey("email_logado");
            return useHeader;
        }

        public void SaveEmail(string paramEmail)
        {
            var plist = NSUserDefaults.StandardUserDefaults;
            plist.SetString(paramEmail, "email_logado");
        }

        public void CheckPermissions()
        {
        }

        public void EnviaPanico()
        {
            try
            {
                AppDelegate.PosicaoService.SendPanico();
            }
            catch (Exception ex)
            { }
        }

        public void DeslogarRest()
        {
            try
            {
                TrackService(false);
            }
            catch (Exception) { }
        }

        /*public void KeepAlive()
		{
			try
			{
				AppDelegate.PosicaoService = new PosicaoService();

			}
			catch(Exception) { }
		}*/

        public void TrackService(Boolean paramIsLocator)
        {

            try
            {

                if (paramIsLocator)
                {
                    AppDelegate.StopLocation = false;
                    PositionController temp = new PositionController();
                    AppDelegate.PosicaoService = new PosicaoService();
                }
                else
                {
                    AppDelegate.StopLocation = true;
                    if (GetBestPosition.LocationManager != null)
                    {
                        GetBestPosition.Stop();
                    }

                    if (AppDelegate.PosicaoService != null)
                    {
                        AppDelegate.PosicaoService.StopService();
                    }
                }
            }
            catch (Exception) { }
        }

        public void ChangeTempoTransmissao()
        {
            try
            {
                AppDelegate.PosicaoService.StopService();
                AppDelegate.PosicaoService.StartBackground();
            }
            catch (Exception) { }
        }

        #region SequencialPosicao
        public string GetSequencialPosicao()
        {
            var plist = NSUserDefaults.StandardUserDefaults;
            var useHeader = plist.StringForKey("posicao_ordem");
            return useHeader;
        }

        public void SaveSequencialPosicao(String paramOrdem)
        {
            var plist = NSUserDefaults.StandardUserDefaults;
            plist.SetString(paramOrdem, "posicao_ordem");
        }
        #endregion

        public string GetAppName()
        {
            return "SSX Family";
        }

        public double GetScreenWidth()
        {
            return UIScreen.MainScreen.Bounds.Width;
        }

        public double GetHeightStatusBar()
        {
            return UIApplication.SharedApplication.StatusBarFrame.Height;
        }

        public double GetScreenHeight()
        {
            return UIScreen.MainScreen.Bounds.Height;
        }

        public void OpenWaze(String paramUrl)
        {
            try
            {
                var wazeURL = paramUrl;
                Device.OpenUri(new Uri(wazeURL));
            }
            catch (Exception ex)
            {
                UIApplication.SharedApplication.OpenUrl(new NSUrl("itms-apps://itunes.apple.com/app/id323229106"));
            }
        }

        public double GetDeviceFontSize()
        {

            String scale = UIApplication.SharedApplication.PreferredContentSizeCategory;

            String scale2 = UIContentSizeCategory.ExtraLarge.ToString();

            System.Diagnostics.Debug.WriteLine(scale);
            System.Diagnostics.Debug.WriteLine(scale2);
            System.Diagnostics.Debug.WriteLine(UIFontDescriptor.PreferredBody);

            return 10;
        }

        public void OpenStreet(String paramLatitude, String paramLongitude)
        {

            try
            {
                //var wazeURL = paramUrl;
                //Device.OpenUri(new Uri(wazeURL));
                UIApplication.SharedApplication.OpenUrl(new NSUrl(@"comgooglemaps://?center=" + paramLatitude + "," + paramLongitude + "&mapmode=streetview"));

            }
            catch (Exception ex)
            {
                //// If Waze is not installed, open it in Google Play:
                //var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("market://details?id=com.google.android.street"));
                //Android.App.Application.Context.StartActivity(intent);
            }

        }

        public void OpenActivity(Boolean paramIsOpen)
        {
            try
            {
                /*
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



                */

            }
            catch (Exception) { }
        }
    }
}