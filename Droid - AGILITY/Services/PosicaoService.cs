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
using Microsoft.AppCenter.Crashes;

namespace family.Droid.Services
{
    [Service(
        Enabled = true
        , Exported = true
        , IsolatedProcess = true
        , Name = "br.com.PosicaoService"
    )]
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
    public class PosicaoService : Service
    {
        //protected App _app => (Application.Current as App);

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

        public bool passOnCreate = false;

        #region Intent
        [Obsolete]
        public override StartCommandResult OnStartCommand(
            Intent intent
            , StartCommandFlags flags
            , int startId
        )
        {
            if (intent != null)
            {
                if (!String.IsNullOrWhiteSpace(intent.Action))
                {
                    if (intent.Action.Equals(PropConf.PosicaoStartAction))
                    {
                        if (!passOnCreate)
                            StartPeriodicLocationTask();
                    }
                    else if (intent.Action.Equals(PropConf.TimerRestart))
                    {
                        StartPeriodicLocationTask();
                    }
                }
            }

            return StartCommandResult.Sticky;
        }

        [Obsolete]
        public override void OnCreate()
        {
            base.OnCreate();
            passOnCreate = true;
            StartPeriodicLocationTask();
        }

        public override void OnDestroy()
        {
            string deslogar = "";
            try
            {

                try
                {
                    base.OnDestroy();

                    CancelGetPosition();

                    StopForeground(true);

                    ISharedPreferences preferences = PreferenceManager
                        .GetDefaultSharedPreferences(Android.App.Application.Context);
                    deslogar = preferences.GetString("deslogar", null);
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
                finally
                {
                    if (String.IsNullOrEmpty(deslogar))
                    {
                        CrossPlataformUtil posicao = new CrossPlataformUtil();
                        posicao.TrackService(true);
                        posicao.TrackKeepAlive(true);

                    }
                }


            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

        }
        #endregion

        #region Service
        IBinder _binder;
        public override IBinder OnBind(Intent intent)
        {
            _binder = new ServiceTestBinder(this);
            return _binder;
        }
        public class ServiceTestBinder : Binder
        {
            public PosicaoService Service { get { return this.LocService; } }
            protected PosicaoService LocService;
            public bool IsBound { get; set; }
            public ServiceTestBinder(PosicaoService service) { this.LocService = service; }
        }
        #endregion

        #region Foreground
        [Obsolete]
        public void RegisterForegroundService()
        {
            try
            {

                CrossPlataformUtil util =
                    new CrossPlataformUtil();

                String text = "Tap to see your family members.";

                Localize loc = new Localize();
                CultureInfo cultura = loc.GetCurrentCultureInfo();

                if (cultura.Name.ToLower() == "pt-br")
                {
                    text = "Toque para ver seus familiares.";
                }

                Notification notification = null;

                if (Build.VERSION.SdkInt >= Build.VERSION_CODES.O)
                {
                    notification = new Notification
                    .Builder(this, CreateNotificationChannel())
                    .SetContentTitle(util.GetAppName())
                    .SetContentIntent(BuildIntentToShowMainActivity())
                    .SetContentText(text)
                    .SetSmallIcon(Resource.Drawable.ic_stat_onesignal_default)
                    .SetLargeIcon(
                        BitmapFactory.DecodeResource(
                            Resources
                            , Resource.Drawable.ic_onesignal_large_icon_default
                        )
                    )
                    .SetOngoing(true)
                    //.AddAction(BuildRestartTimerAction())
                    //.AddAction(BuildStopServiceAction())
                    .Build();
                }
                else
                {

                    notification = new Notification
                        .Builder(this)
                        .SetContentTitle(util.GetAppName())
                        .SetContentIntent(BuildIntentToShowMainActivity())
                        .SetContentText(text)
                        .SetSmallIcon(Resource.Drawable.ic_stat_onesignal_default)
                        .SetLargeIcon(
                            BitmapFactory.DecodeResource(
                                Resources
                                , Resource.Drawable.ic_onesignal_large_icon_default
                            )
                        )
                        .SetOngoing(true)
                        //.AddAction(BuildRestartTimerAction())
                        //.AddAction(BuildStopServiceAction())
                        .Build();

                }


                // Enlist this instance of the service as a foreground service
                StartForeground(
                    PropConf.PosicaoForegroundId
                    , notification
                );

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private string CreateNotificationChannel()
        {

            CrossPlataformUtil util =
                    new CrossPlataformUtil();

            string channelId = $"CHANNEL_LOCAL_NOTIFICATION.general";
            NotificationChannel channel = new NotificationChannel(channelId, util.GetAppName(), NotificationImportance.Default);

            NotificationManager manager = (NotificationManager)Android.App.Application.Context.GetSystemService(Context.NotificationService);

            manager.CreateNotificationChannel(channel);

            return channelId;
        }

        public PendingIntent BuildIntentToShowMainActivity()
        {

            var notificationIntent = new Intent(
                this
                , typeof(MainActivity)
            );

            var pendingIntent = PendingIntent.GetActivity(
                this
                , 0
                , notificationIntent
                , PendingIntentFlags.UpdateCurrent
            );
            return pendingIntent;
        }

        #endregion


        [Obsolete]
        public void StartPeriodicLocationTask()
        {
            try
            {

                TokenDataStore tokenDataSource = new TokenDataStore();

                TokenRealm token = null;

                if (tokenDataSource.Get(1) != null)
                    token = tokenDataSource.Get(1);

                if (token.IsLocator)
                {
                    RegisterForegroundService();
                    Servico(token);
                }


            }
            catch (Exception ex)
            {
                System.Diagnostics
                      .Debug.WriteLine(
                          "FA&)#2 StartPeriodicLocationTask() " +
                          "IniciaLocation PosiçãoService: InnerException: "
                          , ex.InnerException
                         );
                System.Diagnostics
                      .Debug.WriteLine(
                          "FA&)#2 StartPeriodicLocationTask() " +
                          "IniciaLocation PosiçãoService: Message: "
                          , ex.Message
                         );

                Crashes.TrackError(ex);
            }
        }

        [Obsolete]
        public void Servico(TokenRealm paramToken)
        {

            try
            {

                Int32 _tempoTransmissao = 0;


                if (paramToken != null)
                    _tempoTransmissao = paramToken.TempoTransmissao;
                else
                    _tempoTransmissao = PropConf.TempoTransmissaoPadrao;


                Int64 totalMilliseconds = (long)TimeSpan
                    .FromSeconds(_tempoTransmissao)
                    .TotalMilliseconds;

                Context contexto = Android.App.Application.Context;

                AlarmManager alarmMgr =
                    (AlarmManager)contexto.GetSystemService(Context.AlarmService);

                Intent intent = new Intent(
                    contexto
                    , typeof(GetBestPosition)
                );

                intent.PutExtra(
                    "evento"
                    , ((int)EnumPosicaoEvento.PosicaoTemporizada).ToString()
                );

                PendingIntent alarmIntent =
                    PendingIntent.GetService(
                        contexto
                        , PropConf.RequestCodeAlarm
                        , intent
                        , PendingIntentFlags.CancelCurrent
                    );

                alarmMgr.Cancel(alarmIntent);

                alarmMgr.SetRepeating(
                    AlarmType.ElapsedRealtimeWakeup
                    , 0
                    , totalMilliseconds
                    , alarmIntent
                );

            }
            catch (Exception ex)
            {
                StartPeriodicLocationTask();
                Crashes.TrackError(ex);
            }
        }

        private void CancelGetPosition()
        {
            try
            {

                Context contexto = Android.App.Application.Context;

                AlarmManager alarmMgr =
                    (AlarmManager)contexto.GetSystemService(Context.AlarmService);

                Intent intent = new Intent(
                    contexto
                    , typeof(GetBestPosition)
                );

                PendingIntent alarmIntent =
                    PendingIntent.GetService(
                        contexto
                        , PropConf.RequestCodeAlarm
                        , intent
                        , PendingIntentFlags.CancelCurrent
                    );

                alarmMgr.Cancel(alarmIntent);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

    }
#pragma warning restore CS1998
#pragma warning restore RECS0022
#pragma warning restore CS4014
}
