using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using family.CrossPlataform;
using family.Droid.CrossPlataform;

[assembly: Xamarin.Forms.Dependency(typeof(NetworkConnection))]
namespace family.Droid.CrossPlataform
{
    public class NetworkConnection : INetworkConnection
    {
        public String SSIDName { get; set; }

        public bool IsConnected { get; set; }

        public void CheckNetworkConnection()
        {
            var connectivityManager = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
            var activeNetworkInfo = connectivityManager.ActiveNetworkInfo;

            if (activeNetworkInfo != null && activeNetworkInfo.IsConnectedOrConnecting)
            {
                IsConnected = true;

                if (activeNetworkInfo.Type == Android.Net.ConnectivityType.Wifi)
                    SSIDName = activeNetworkInfo.ExtraInfo;
                else
                    SSIDName = String.Empty;
            }
            else
            {
                IsConnected = false;
            }
        }
    }

}