using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using family.Droid.CrossPlataform;
using family.Services.ServiceRealm;
using family.Domain.Realm;
using family.Domain.Enum;
using Android.Graphics;
using System.Globalization;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using family.ViewModels;
using Microsoft.AppCenter.Crashes;

namespace family.Droid.Services
{
    [Service(
        Enabled = true
        , Name = "br.com.alltechseguranca.KeepAliveService"
    )]
    #pragma warning disable CS4014
    #pragma warning disable RECS0022
    #pragma warning disable CS1998
    public class KeepAliveService : Service
    {
        //private bool _isRunning;
        //private Context _context;
        //private Task _task;

        //private Timer timer;

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

        #region overrides

        public override void OnCreate()
        {
            base.OnCreate();
        }

        public override IBinder OnBind(Intent intent)
        {
            //binder = new KeepAliveService(this);
            return null;
        }        

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            DoWork();
            //return base.OnStartCommand(intent, flags, startId);

            return StartCommandResult.NotSticky;
        }

        public override bool StopService(Intent name)
        {
            return base.StopService(name);
        }

        #endregion

        private void DoWork()
        {
            Task.Run(async () =>
            {
                try
                {
                    if (PropConf.IsLogado == true)
                    {
                        //CrossPlataformUtil util = new CrossPlataformUtil();
                        //util.TrackService(true);

                        ViewModelConfiguracao viewModelConfiguracao = new ViewModelConfiguracao();
                        viewModelConfiguracao.CheckKeepAlive();
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }

            });

        }
        
    }

#pragma warning restore CS1998
#pragma warning restore RECS0022
#pragma warning restore CS4014
}
