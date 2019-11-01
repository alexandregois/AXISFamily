using System;
using System.Collections.Generic;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using family.Domain.Enum;
using family.Domain.Realm;
using family.Droid.CustomClass.Position;
using family.Services.ServiceRealm;
using family.Droid.CrossPlataform;
using System.Threading.Tasks;
using family.ViewModels.InterfaceServices;
using Android.Widget;
using Android.Support.V4.App;
using Xamarin.Forms;
using Microsoft.AppCenter.Crashes;

namespace family.Droid.Services
{
    [Service(
        Enabled = true
        , Exported = true
        , IsolatedProcess = true
        , Name = "br.com.maxima.GetBestPosition"
    )]
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
    public class GetBestPosition : Service, ILocationListener
    {
        private IMessageService _messageService;

        private LocationManager _locationManager;
        private PositionServiceHelper _positionServiceHelper;
        private List<Location> _lstLocation;
        private Int32 _loopCount;

        private CancellationTokenSource _cancellationToken;

        private String NumEvento;
        private TokenRealm _token;
        private Int32 _tempoTransmicao;
        private Boolean _startBest;

        public App _app => (Xamarin.Forms.Application.Current as App);

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


        #region Intent
        public override StartCommandResult OnStartCommand(
            Intent intent
            , StartCommandFlags flags
            , int startId
        )
        {

            //_messageService =
            //       DependencyService.Get<IMessageService>();

            base.OnStartCommand(intent, flags, startId);

            try
            {
                NumEvento = intent.GetStringExtra("evento");
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }
            finally
            {
                if (String.IsNullOrWhiteSpace(NumEvento))
                {
                    NumEvento = ((int)EnumPosicaoEvento.PosicaoTemporizada).ToString();
                }
            }

            StartLocation();

            return StartCommandResult.Sticky;
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
            public GetBestPosition Service { get { return this.LocService; } }
            protected GetBestPosition LocService;
            public bool IsBound { get; set; }
            public ServiceTestBinder(GetBestPosition service) { this.LocService = service; }
        }
        #endregion

        public void StartLocation()
        {

            try
            {

                LocationManager manager = (LocationManager)GetSystemService(Context.LocationService);

                if (manager.IsProviderEnabled(LocationManager.GpsProvider) == false)
                {
                    if (manager.IsProviderEnabled(LocationManager.NetworkProvider) == false)
                    {
                        if (manager.IsProviderEnabled(LocationManager.PassiveProvider) == false)
                            // Call your Alert message
                            OpenSettingGps(1);
                    }
                }



                TokenDataStore tokenDataSource = new TokenDataStore();

                _token = tokenDataSource.Get(1);

                if (_cancellationToken != null)
                    _cancellationToken.Cancel();

                if (_token != null)
                {
                    _tempoTransmicao = _token.TempoTransmissao;

                    _startBest = false;

                    _positionServiceHelper = new PositionServiceHelper();
                    _lstLocation = new List<Location>();
                    _loopCount = 0;
                    _locationManager = (LocationManager)GetSystemService(LocationService);
                    _cancellationToken = new CancellationTokenSource();


                    Location ultimaPosicaoAparelho = GetLastKnownLocationAsync();


                    //CrossPlataformUtil _util = new CrossPlataformUtil();
                    //_util.CheckPermissions();


                    if (ultimaPosicaoAparelho == null)
                    {
                        StartRequestLocaticon(LocationManager.GpsProvider);
                        StartRequestLocaticon(LocationManager.NetworkProvider);
                        StartRequestLocaticon(LocationManager.PassiveProvider);

                        LimiteMaximoTempo();
                    }
                    else
                    {
                        FinalizaServico(ultimaPosicaoAparelho);
                    }

                }
                else
                {
                    _tempoTransmicao = PropConf.TempoTransmissaoPadrao;

                    _startBest = false;

                    _positionServiceHelper = new PositionServiceHelper();
                    _lstLocation = new List<Location>();
                    _loopCount = 0;
                    _locationManager = (LocationManager)GetSystemService(LocationService);
                    _cancellationToken = new CancellationTokenSource();


                    Location ultimaPosicaoAparelho = GetLastKnownLocationAsync();


                    if (ultimaPosicaoAparelho == null)
                    {
                        StartRequestLocaticon(LocationManager.GpsProvider);
                        StartRequestLocaticon(LocationManager.NetworkProvider);
                        StartRequestLocaticon(LocationManager.PassiveProvider);

                        LimiteMaximoTempo();
                    }
                    else
                    {
                        FinalizaServico(ultimaPosicaoAparelho);
                    }

                }

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

                System.Diagnostics.Debug.WriteLine("FA&)#2 IniciaLocation " +
                                                   "PosiçãoService: InnerException: ", ex.InnerException);
                System.Diagnostics.Debug.WriteLine("FA&)#2 IniciaLocation " +
                                                   "PosiçãoService: Message: ", ex.Message);
            }
        }

        private void StartRequestLocaticon(String provider)
        {
            try
            {

                //_locationManager
                //    .RequestLocationUpdates(
                //        provider
                //        , 0
                //        , 0
                //        , this
                //    );


                _locationManager
                .RequestLocationUpdates(
                    provider
                    , 2000
                    , 1
                    , this
                );

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

        }

        #region Timer
        public void LimiteMaximoTempo()
        {
            try
            {
                System.Threading.Tasks.Task.Run(async () =>
                {
                    await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(PropConf.TempoTransmissaoPadrao));
                    CancelaLocationUpdates();

                }, _cancellationToken.Token);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }
        }
        #endregion

        #region LocationManager
        public void OnLocationChanged(Location location)
        {
            try
            {
                if (location != null)
                {
                    _lstLocation.Add(location);
                    _loopCount++;
                }
                if (_loopCount >= 10)
                {
                    CancelaLocationUpdates();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

        }

        public async void OnProviderDisabled(string provider)
        {

            //    Intent intent = new Intent("android.location.GPS_ENABLED_CHANGE");
            //    intent.PutExtra("enabled", true);
            //    SendBroadcast(intent);

            //    OpenSettingGps();

        }

        public void OnProviderEnabled(string provider) { }

        public void OnStatusChanged(string provider, Availability status, Bundle extras) { }
        #endregion

        public void CancelaLocationUpdates()
        {
            try
            {

                if (!_startBest)
                {
                    _startBest = true;
                    _locationManager.RemoveUpdates(this);

                    _cancellationToken.Cancel();

                    Location bestLocation = null;

                    foreach (Location actualLocation in _lstLocation)
                    {
                        if (IsBetterLocation(actualLocation, bestLocation))
                        {
                            bestLocation = actualLocation;
                        }
                    }

                    FinalizaServico(bestLocation);
                }

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }

        }

        #region BestLocation
        protected Boolean IsBetterLocation(Location location, Location currentBestLocation)
        {

            if (currentBestLocation == null)
            {
                // A new location is always better than no location
                return true;
            }

            // Check whether the new location fix is newer or older
            long timeDelta = location.Time - currentBestLocation.Time;
            Boolean isSignificantlyNewer = timeDelta > _tempoTransmicao;
            Boolean isSignificantlyOlder = timeDelta < -_tempoTransmicao;
            Boolean isNewer = timeDelta > 0;

            // If it's been more than two minutes since the current location, use the new location
            // because the user has likely moved
            if (isSignificantlyNewer)
            {
                return true;
                // If the new location is more than two minutes older, it must be worse
            }
            else if (isSignificantlyOlder)
            {
                return false;
            }

            // Check whether the new location fix is more or less accurate
            int accuracyDelta = (int)(location.Accuracy - currentBestLocation.Accuracy);
            Boolean isLessAccurate = accuracyDelta > 0;
            Boolean isMoreAccurate = accuracyDelta < 0;
            Boolean isSignificantlyLessAccurate = accuracyDelta > 100;

            // Check if the old and new location are from the same provider
            Boolean isFromSameProvider = IsSameProvider(location.Provider,
                                                        currentBestLocation.Provider);

            // Determine location quality using a combination of timeliness and accuracy
            if (isMoreAccurate)
            {
                return true;
            }
            else if (isNewer && !isLessAccurate)
            {
                return true;
            }
            else if (isNewer && !isSignificantlyLessAccurate && isFromSameProvider)
            {
                return true;
            }
            return false;
        }

        /** Checks whether two providers are the same */
        private Boolean IsSameProvider(String provider1, String provider2)
        {
            if (provider1 == null)
            {
                return provider2 == null;
            }
            return provider1.Equals(provider2);
        }
        #endregion

        private Location GetLastKnownLocationAsync()
        {
            Location ret = null;
            try
            {
                Location lastGps =
                    _locationManager.GetLastKnownLocation(LocationManager.GpsProvider);
                Location best =
                    _locationManager.GetLastKnownLocation(LocationManager.NetworkProvider);
                Location passive =
                    _locationManager.GetLastKnownLocation(LocationManager.PassiveProvider);

                if (lastGps != null)
                {
                    if (IsBetterLocation(lastGps, best))
                    {
                        best = lastGps;
                    }
                }
                else
                {
                    OpenSettingGps(2);
                }


                if (best != null)
                {
                    CrossPlataformUtil _util = new CrossPlataformUtil();

                    DateTime dataPos = _util.Epoch2DateTime(best.Time);

                    DateTime dataAtual = DateTime.UtcNow;

                    double dataDiff = dataAtual.ToUniversalTime()
                                              .Subtract(dataPos)
                                               .TotalSeconds;

                    System.Diagnostics.Debug.WriteLine(
                        String.Format("FA&)#2 Data Posição: {0}", dataPos)
                    );

                    System.Diagnostics.Debug.WriteLine(
                        String.Format("FA&)#2 Data Atual: {0}", dataAtual)
                    );

                    System.Diagnostics.Debug.WriteLine(
                        String.Format("FA&)#2 Data Diff: {0}", dataDiff)
                    );

                    if (dataDiff < 11)
                    {
                        ret = best;

                        System.Diagnostics.Debug.WriteLine(
                            String.Format("FA&)#2 Usou a última")
                        );
                    }
                }


            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

                System.Diagnostics
                      .Debug.WriteLine(
                          "FA&)#2 GetLastKnownLocation() " +
                          "InnerException: "
                          , ex.InnerException
                         );
                System.Diagnostics
                      .Debug.WriteLine(
                          "FA&)#2 GetLastKnownLocation() " +
                          "Message: "
                          , ex.Message
                         );
            }

            return ret;

        }

        private async void OpenSettingGps(byte paramId)
        {
            try
            {
                Boolean envia = false;

                if (paramId == 1)
                {
                    //envia = await _messageService.ShowAlertChooseAsync(
                    //                _app.MessageGps("GpsSettings")
                    //                , _app.MessageGps("No")
                    //                , _app.MessageGps("GpsOpenSettings")
                    //                , _app.MessageGps("Alert")
                    //            );
                }
                else
                {
                    //envia = await _messageService.ShowAlertChooseAsync(
                    //            _app.MessageGps("Gps")
                    //            , _app.MessageGps("No")
                    //            , _app.MessageGps("GpsOpenSettings")
                    //            , _app.MessageGps("Alert")
                    //        );

                    Toast.MakeText(this, _app.MessageGps("Gps"), ToastLength.Long).Show();
                    Task.Delay(1000);
                }


                if (envia == true)
                    StartActivity(new Intent(Android.Provider.Settings.ActionLocationSourceSettings));


            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

        }

        private void FinalizaServico(Location paramLocation)
        {

            if (paramLocation != null)
            {
                _positionServiceHelper.MakeMessage(
                    paramLocation
                    , Convert.ToInt32(NumEvento)
                    , _locationManager
                    , Android.App.Application.Context
                );
            }

            //StopService (
            //	new Intent (
            //		Android.App.Application.Context
            //		, typeof(GetBestPosition))
            //);
        }

    }

#pragma warning restore CS1998
#pragma warning restore RECS0022
#pragma warning restore CS4014
}