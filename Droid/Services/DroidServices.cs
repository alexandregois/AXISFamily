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
using family.Droid.Services;
using family.Services.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(DroidService))]
namespace family.Droid.Services
{
    public class DroidService : IServices
    {
        public void StartService()
        {
            Context contexto = Android.App.Application.Context;
            contexto.StartService(new Intent(contexto, typeof(KeepAliveService)));
        }

        public void StopService()
        {
            Context contexto = Android.App.Application.Context;
            contexto.StopService(new Intent(contexto, typeof(KeepAliveService)));
        }
    }
}