using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using CoreLocation;
using family.Domain.Enum;
using family.Domain.Realm;
using family.iOS.CrossPlataform.Position;
using family.Services.ServiceRealm;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace family.iOS.Services
{
#pragma warning disable CS4014
    public class PosicaoService
    {

        private nint taskID;
        private TokenRealm _token;
        private Boolean isStop;
        private Int32 _tempoTransmissao;
        private CancellationTokenSource _cancellationToken;

        public App _app => (Xamarin.Forms.Application.Current as App);

        public PosicaoService()
        {
			//StartBackground();

            TokenDataStore dataStore = new TokenDataStore();
            _token = dataStore.Get(1);

            if (_token != null)
            {
                if (_token.IsLocator)
                {
                    _tempoTransmissao = _token.TempoTransmissao;
					StartBackground();
                }
            }
        }

        public void StartBackground()        {

            _cancellationToken = new CancellationTokenSource();
            TrackingNew();
        }

        public void TrackingNew()
        {
            /*Task.Factory.StartNew(async () =>
            {

                //expirationHandler only called if background time allowed exceeded
                taskID = UIApplication.SharedApplication.BeginBackgroundTask(() =>
                {
                    //Console.WriteLine("Exhausted time");
                    UIApplication.SharedApplication.EndBackgroundTask(taskID);
                    if (!_cancellationToken.IsCancellationRequested)
                        Task.Factory.StartNew(() => FinishLongRunningTask(taskID));
                });

                while (!AppDelegate.StopLocation)
                {
                    _app.KeepAlive();
                    await Task.Delay(
                        TimeSpan.FromSeconds(_app.Configuracao.TempoKeepAlive)
                        , _cancellationToken.Token
                    );
                }

                //Only called if loop terminated due to myFlag and not expiration of time
                UIApplication.SharedApplication.EndBackgroundTask(taskID);
            }, _cancellationToken.Token);*/

        }

        private void OnRunTask(
            object sender
            , ElapsedEventArgs e
        )
        {
            EnviaPosicao((int)EnumPosicaoEvento.PosicaoTemporizada);
        }

        private void FinishLongRunningTask(nint taskID)
        {
            if (!AppDelegate.StopLocation)
                AppDelegate.PosicaoService = new PosicaoService();
        }

        public void SendPanico()
        {
            GetBestPosition.EventoPosicao = EnumPosicaoEvento.BotaoPanicoAtivado;
            GetBestPosition.LocationManager.RequestLocation();
            //EnviaPosicao((int)EnumPosicaoEvento.BotaoPanicoAtivado);
        }

        public void StopService()
        {
            AppDelegate.StopLocation = true;
            isStop = true;

            _cancellationToken.Cancel();
            UIApplication.SharedApplication.EndBackgroundTask(taskID);
        }

        public void EnviaPosicao(Int32 paramNumEvento)
        {
            CLLocation bestLocation = null;

            if (GetBestPosition.LocationManager == null)
            {
                AppDelegate.LocationManager = new GetBestPosition();
            }

            if (CanSendLastPosition(GetBestPosition.LocationManager.Location))
            {
                bestLocation = GetBestPosition.LocationManager.Location;
            }
            else
            {
                GetBestPosition.LocationManager.RequestLocation();
                if (CanSendLastPosition(GetBestPosition.LocationManager.Location))
                    bestLocation = GetBestPosition.LocationManager.Location;
            }

            if (bestLocation == null)
            {
                if (CanSendLastPosition(GetBestPosition.LastLocation))
                    bestLocation = GetBestPosition.LastLocation;
            }

            if (bestLocation != null)
            {
                PositionServiceHelper positionServiceHelper = new PositionServiceHelper();

                positionServiceHelper.MakeMessage(
                    bestLocation
                    , paramNumEvento
                );
            }
        }

        private Boolean CanSendLastPosition(CLLocation paramLastLocation)
        {
            Boolean can = false;

            if (paramLastLocation != null)
            {
                DateTime dataPos = paramLastLocation.Timestamp.ToDateTime();

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

                can = (dataDiff < 21);

                if (can)
                {
                    System.Diagnostics.Debug.WriteLine(
                        String.Format("FA&)#2 Usou a última")
                    );
                }
            }

            return can;
        }

    }
#pragma warning restore CS4014
}
