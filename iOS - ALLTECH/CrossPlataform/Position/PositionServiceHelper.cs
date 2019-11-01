using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CoreLocation;
using family.Domain.Enum;
using family.Domain.Protocolo;
using family.Domain.Realm;
using family.Services.ServiceRealm;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;

namespace family.iOS.CrossPlataform.Position
{
    public class PositionServiceHelper
    {
        public App _app => (Xamarin.Forms.Application.Current as App);

        public async Task MakeMessage(
             CLLocation paramLocation
             , Int32 paramEvento
         )
        {
            try
            {

                _app.GetSSIDName();

                TokenDataStore dataStore = new TokenDataStore();
                TokenRealm token = dataStore.Get(1);

                Configuracao Configuracao = new Configuracao();
                SubmitProtocoloDataStore store = new SubmitProtocoloDataStore();
                CrossPlataformUtil _util = new CrossPlataformUtil();

                String sequencial;
                try
                {
                    sequencial = _util.GetSequencialPosicao();
                }
                catch (Exception ex)
                {
                    sequencial = null;

                    Crashes.TrackError(ex);
                }

                Int32 novoSequencial;
                if (String.IsNullOrEmpty(sequencial))
                {
                    novoSequencial = 1;
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

                Boolean gpsValido = true;

                AxisOnboardHeader headerObject = new AxisOnboardHeader();
                headerObject.IdentificacaoAVL = token.Identificacao;
                headerObject.Sequencial = novoSequencial;

                if (paramEvento == (int)EnumPosicaoEvento.BotaoPanicoAtivado)
                {
                    headerObject.Sequencial = -1;
                }

                Byte[] headerBuf = headerObject.TransformToArrayByte();

                AxisOnboardPosition posicao = new AxisOnboardPosition();
                posicao.Lat = paramLocation.Coordinate.Latitude;
                posicao.Long = paramLocation.Coordinate.Longitude;

                DateTime dtPosicao = _util.NSDateToDateTime(paramLocation.Timestamp);

                posicao.DtEvt = (long)(dtPosicao.ToUniversalTime()
                                       .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                                       .TotalSeconds);

                posicao.IdEvt = paramEvento;

                posicao.In = 0;

                Dictionary<Byte, Double> ListTelemetria = new Dictionary<Byte, double>();

                Double velocidade = Math.Round(paramLocation.Speed * 3.6f);
                if (velocidade < 0)
                    velocidade = 0;

                ListTelemetria.Add(1, velocidade);

                ListTelemetria.Add(2, paramLocation.Course);

                ListTelemetria.Add(3, paramLocation.HorizontalAccuracy);

                Int32? bateria = _util.RetornaNivelBateria();
                if (bateria.HasValue)
                    ListTelemetria.Add(4, bateria.Value);

                ListTelemetria.Add(5, paramLocation.Altitude);


                //Get Status Bateria
                if (_app.StatusBateria != null)
                {
                    if ((Int32)Enum.Parse(typeof(EnumStatusBateria), _app.StatusBateria) == 1 || (Int32)Enum.Parse(typeof(EnumStatusBateria), _app.StatusBateria) == 4)
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
                protocolo.id = headerObject.Sequencial;
                protocolo.IdentificacaoAVL = token.Identificacao;
                protocolo.message = message;

                store.CreateUpadate(protocolo);

                System.Diagnostics.Debug.WriteLine(String.Format(
                    "FA&)#2 MakeMessage() IdPosicao: {1} || Evento: {0} / Data: {2}"
                    , paramEvento
                    , headerObject.Sequencial
                    , DateTime.UtcNow));

                Boolean isPanico = false;
                if (paramEvento == (int)EnumPosicaoEvento.PosicaoTemporizada)
                {
                    _util.SaveSequencialPosicao((novoSequencial).ToString());
                }

                AppDelegate.PosicaoSenderService.SendPosition();

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

    }
}
