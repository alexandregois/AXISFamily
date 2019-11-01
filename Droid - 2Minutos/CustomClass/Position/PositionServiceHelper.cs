using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Preferences;
using family.Domain.Enum;
using family.Domain.Protocolo;
using family.Domain.Realm;
using family.Droid.CrossPlataform;
using family.Droid.Services;
using family.Services.ServiceRealm;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Plugin.Battery;
using Plugin.LocalNotifications;
using Xamarin.Forms;

namespace family.Droid.CustomClass.Position
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
    public class PositionServiceHelper
    {
        public App _app => (Application.Current as App);

        public void VerificaGPS(LocationManager paramLocationManager)
        {
            if (paramLocationManager.IsProviderEnabled(LocationManager.GpsProvider))
            {
                //Do what you need if enabled...
            }
            else
            {
                //Do what you need if not enabled...
                //Toast.MakeText(this, "Network Not Enable", ToastLength.Long).Show();
            }

            //Android.Net.ConnectivityManager connectivityManager = (Android.Net.ConnectivityManager)GetSystemService(ConnectivityService);
            //Android.Net.NetworkInfo activeConnection = connectivityManager.ActiveNetworkInfo;
            //bool isOnline = (activeConnection != null) && activeConnection.IsConnected;
            //if (isOnline == false)
            //{
            //    Toast.MakeText(this, "Network Not Enable", ToastLength.Long).Show();
            //}
            //else
            //{
            //    LocationManager mlocManager = (LocationManager)GetSystemService(LocationService); ;
            //    bool enabled = mlocManager.IsProviderEnabled(LocationManager.GpsProvider);
            //    if (enabled == false)
            //    {
            //        Toast.MakeText(this, "GPS Not Enable", ToastLength.Long).Show();
            //    }
            //}

        }


        public async Task MakeMessage(
            Location paramLocation
            , Int32 paramEvento
            , LocationManager paramLocationManager
            , Context paramContexto
        )
        {
            try
            {


                _app.GetSSIDName();


                Configuracao Configuracao = new Configuracao();
                TokenDataStore tokenDataSource = new TokenDataStore();
                SubmitProtocoloDataStore protocoloDataStore = new SubmitProtocoloDataStore();

                TokenRealm _token = tokenDataSource.Get(1);

                CrossPlataformUtil _util = new CrossPlataformUtil();

                String sequencial = null;
                try
                {
                    ISharedPreferences preferences =
                        PreferenceManager.GetDefaultSharedPreferences(paramContexto);
                    sequencial = preferences.GetString("posicao_ordem", null);
                }
                catch (Exception ex)
                {
                    sequencial = null;

                    Crashes.TrackError(ex);

                }

                Int32 novoSequencial;
                if (String.IsNullOrEmpty(sequencial))
                {
                    IReadOnlyList<SubmitProtocoloRealm> lst = protocoloDataStore.List();

                    if (lst != null)
                    {
                        SubmitProtocoloRealm sub = lst.OrderBy(x => x.id).LastOrDefault();
                        if (sub != null)
                        {
                            novoSequencial = sub.id + 1;
                        }
                        else
                        {
                            novoSequencial = 1;
                        }
                    }
                    else
                    {
                        novoSequencial = 1;
                    }
                }
                else
                {
                    if (!Int32.TryParse(sequencial, out novoSequencial))
                    {
                        novoSequencial = 1;
                    }
                    else
                    {
                        novoSequencial++;
                    }
                }


                VerificaGPS(paramLocationManager);


                Boolean gpsValido = false;
                try
                {
                    gpsValido = (paramLocation.Provider == LocationManager.GpsProvider || paramLocation.Provider == LocationManager.NetworkProvider);

                }
                catch (Exception ex)
                {
                    Android.App.Application.Context.StartActivity(new Android.Content.Intent(Android.Provider.Settings.ActionLocationSourceSettings));
                    Crashes.TrackError(ex);
                }

                AxisOnboardHeader headerObject = new AxisOnboardHeader();
                headerObject.Sequencial = novoSequencial;

                if (paramEvento == (int)EnumPosicaoEvento.BotaoPanicoAtivado)
                {
                    headerObject.Sequencial = -1;
                }

                headerObject.IdentificacaoAVL = _token.Identificacao;

                Byte[] headerBuf = headerObject.TransformToArrayByte();

                AxisOnboardPosition posicao = new AxisOnboardPosition();
                posicao.Lat = paramLocation.Latitude;
                posicao.Long = paramLocation.Longitude;
                posicao.DtEvt = (long)_util.Epoch2DateTime(paramLocation.Time)
                    .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                    .TotalSeconds;

                posicao.IdEvt = paramEvento;

                posicao.In = 0;


                Dictionary<Byte, Double> ListTelemetria = new Dictionary<Byte, double>();
                Double velo = 0;


                if (paramLocation.HasSpeed)
                    velo = (paramLocation.Speed * 3.6f);


                if (velo < 0)
                    velo = 0;

                ListTelemetria.Add(1, Math.Round(velo));

                if (paramLocation.HasBearing)
                    ListTelemetria.Add(2, paramLocation.Bearing);

                if (paramLocation.HasAccuracy)
                    ListTelemetria.Add(3, paramLocation.Accuracy);

                Int32? bateria = _util.RetornaNivelBateria();
                if (bateria.HasValue)
                    ListTelemetria.Add(4, bateria.Value);

                if (paramLocation.HasAltitude)
                    ListTelemetria.Add(5, paramLocation.Altitude);

                //Get Status Bateria
                if (!String.IsNullOrWhiteSpace(_app.StatusBateria))
                {
                    if ((Int32)Enum.Parse(typeof(EnumStatusBateria), _app.StatusBateria) == 1 || (Int32)Enum.Parse(typeof(EnumStatusBateria),
                        _app.StatusBateria) == 4)
                        posicao.In = 1;
                }


                if (gpsValido)
                    posicao.In |= 2;



                Dictionary<Byte, String> ListTelemetriaTxt = new Dictionary<Byte, String>();

                //Get Nome SSID(Wi-fi)
                if (_app.NamebSSID == null)
                    ListTelemetriaTxt.Add(5, null);
                else
                    ListTelemetriaTxt.Add(5, _app.NamebSSID);



                //TELEMETRIA TXT
                posicao.ListTelTxt = ListTelemetriaTxt;


                posicao.ListTel = ListTelemetria;

                String posicaoStr = JsonConvert.SerializeObject(posicao);

                Byte[] posicaoBuf = Encoding.ASCII.GetBytes(posicaoStr);

                Byte[] message = new byte[headerBuf.Length + posicaoBuf.Length];
                headerBuf.CopyTo(message, 0);
                posicaoBuf.CopyTo(message, headerBuf.Length);

                SubmitProtocoloRealm protocolo = new SubmitProtocoloRealm();
                protocolo.message = message;
                protocolo.IdentificacaoAVL = headerObject.IdentificacaoAVL;
                protocolo.id = headerObject.Sequencial;

                protocoloDataStore.CreateUpadate(protocolo);

                System.Diagnostics.Debug.WriteLine(String.Format(
                    "FA&)#2 MakeMessage() IdPosicao: {1} || Evento: {0} / Data: {2}"
                    , paramEvento
                    , headerObject.Sequencial
                    , DateTime.UtcNow));

                Intent startServiceIntent = new Intent(
                    paramContexto
                    , typeof(PosicaoSenderService)
                );

                if (paramEvento == (int)EnumPosicaoEvento.PosicaoTemporizada)
                {
                    ISharedPreferences preferences =
                        PreferenceManager.GetDefaultSharedPreferences(paramContexto);
                    ISharedPreferencesEditor editor = preferences.Edit();

                    // Save to SharedPreferences
                    editor.PutString("posicao_ordem", novoSequencial.ToString());
                    editor.Apply();
                }

                paramContexto.StartService(startServiceIntent);
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
