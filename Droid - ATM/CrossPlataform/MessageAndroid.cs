using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using family.CrossPlataform;
using family.Droid.CrossPlataform;

[assembly: Xamarin.Forms.Dependency(typeof(MessageAndroid))]
namespace family.Droid.CrossPlataform
{
    public class MessageAndroid : IMessage
    {
        public void ShowToast(string toastString)
        {
            Vibration(500);
            //Toast.MakeText(Android.App.Application.Context, toastString, ToastLength.Short).Show();
            Toast.MakeText(Android.App.Application.Context, toastString, ToastLength.Long).Show();
        }

        /// <summary>
        /// Vibrate device for specified amount of time
        /// </summary>
        /// <param name="milliseconds">Time in MS (500ms is default).</param>
        public void Vibration(int milliseconds = 500)
        {
            using (var v = (Vibrator)Android.App.Application.Context.GetSystemService(Context.VibratorService))
            {
                if ((int)Build.VERSION.SdkInt >= 11)
                {
                #if __ANDROID_11__
                    if (!v.HasVibrator)
                    {
                        Console.WriteLine("Android device does not have vibrator.");
                        return;
                    }
                #endif
                }

                if (milliseconds < 0)
                    milliseconds = 0;

                try
                {
                    v.Vibrate(milliseconds);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unable to vibrate Android device, ensure VIBRATE permission is set. Error: {ex.Message}");
                }
            }
        }
    }
}