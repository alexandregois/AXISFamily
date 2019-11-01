using System;
using System.Net;
using System.Net.Sockets;
using Android.OS;
using family.Domain.Protocolo;
using family.Domain.Realm;
using family.Droid.CrossPlataform;
using family.Droid.Services;
using family.Model;
using family.Services.ServiceRealm;
using Microsoft.AppCenter.Crashes;

namespace family.Droid.CustomClass.Position
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
    public class PositionSender
    {
        public static Int32 tentativas;

        public static PosicaoSenderService _servico;

        public static Socket _socket;
        public static EndPoint _remoteEndPoint;
        public static UdpClient udpClient;

        public PositionSender(
            PosicaoSenderService Servico
        )
        {
            tentativas = 0;
            _servico = Servico;
            StarSender();
        }

        public static void StarSender()
        {
            try
            {
                new SenderTask().Execute("");
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private class SenderTask
            : AsyncTask<String, Int32, Boolean>
        {
            protected override Boolean RunInBackground(
                params string[] @params
            )
            {
                Boolean continua = false;
                try
                {
                    TokenDataStore tokenDataStore = new TokenDataStore();
                    TokenRealm token = tokenDataStore.Get(1);
                    Int32 timeout = (Int32)TimeSpan.FromSeconds(15).TotalMilliseconds;
                    udpClient = new UdpClient()
                    {
                        EnableBroadcast = true
                    };

                    IPAddress.TryParse(token.IP, out IPAddress ip);

                    udpClient.Connect(ip, token.Porta);

                    SubmitProtocoloDataStore store = new SubmitProtocoloDataStore();

                    SubmitProtocoloRealm send = store.GetFirst();

                    if (send != null)
                    {

                        udpClient.Send(
                            send.message
                            , send.message.Length
                        );

                        udpClient.Client.SendTimeout = timeout;
                        udpClient.Client.ReceiveTimeout = timeout;

                        IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                        Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);

                        AxisOnboardData udpResult = new AxisOnboardData();

                        //Byte[] retornoToReverse = new byte[24];
                        //Array.Copy(receiveBytes, retornoToReverse, 24);
                        ModelReverseBitConverter mReverse = new ModelReverseBitConverter();
                        Int32 inicial = 0;
                        udpResult.Sequencial = mReverse.ToInt32(receiveBytes, inicial);
                        inicial += 4;
                        udpResult.IdentificacaoAVL = mReverse.ToInt64(receiveBytes, inicial);

                        if (
                            udpResult.IdentificacaoAVL == send.IdentificacaoAVL
                            && send.id == udpResult.Sequencial
                        )
                        {
                            SubmitProtocoloRealm temp = store.Get(udpResult.Sequencial);
                            store.Remove(temp);

                            tentativas = 0;

                            System.Diagnostics
                                  .Debug.WriteLine(
                                      "FA&)#2 Receive() Sucesso " +
                                      "Senquencial: {0}"
                                      , udpResult.Sequencial
                                     );

                        }
                        else
                        {
                            tentativas++;

                            System.Diagnostics
                                  .Debug.WriteLine(
                                      "FA&)#2 Receive() Erro " +
                                      "Senquencial Recebido: {0}"
                                      , udpResult.Sequencial
                                     );
                        }



                        System.Diagnostics
                                  .Debug.WriteLine(
                                      "FA&)#2 Receive() Data: {0}"
                                      , DateTime.UtcNow
                                     );

                        udpClient.Close();

                        continua = true;
                    }
                    else
                    {
                        CrossPlataformUtil _util = new CrossPlataformUtil();
                        _util.SaveSequencialPosicao("0");
                        _servico.Stop();
                    }
                }
                catch (SocketException ex)
                {
                    try
                    {

                        TokenDataStore tokenDataStore = new TokenDataStore();
                        TokenRealm token = tokenDataStore.Get(1);

                        System.Diagnostics.Debug.WriteLine(
                            String.Format(
                                "FA&)#2 Token " +
                                "IP: {0} || Porta: {1}"
                                , token.IP
                                , token.Porta
                            )
                        );

                        System.Diagnostics.Debug.WriteLine(
                            String.Format(
                                "FA&)#2 StarSender_Finish() " +
                                "InnerException: {0} || Message: {1}"
                                , ex.InnerException
                                , ex.Message
                            )
                        );

                        if (udpClient != null)
                            udpClient.Close();

                        tentativas++;
                        if (tentativas < 3)
                        {
                            continua = true;
                        }


                        Crashes.TrackError(ex);


                    }
                    catch (Exception e)
                    {
                        Crashes.TrackError(e);
                    }


                }
                catch (Exception ex)
                {
                    if (udpClient != null)
                        udpClient.Close();
                    continua = false;
                    System.Diagnostics.Debug.WriteLine(
                        String.Format(
                            "FA&)#2 StarSender_Finish() " +
                            "InnerException: {0} || Message: {1}"
                            , ex.InnerException
                            , ex.Message
                        )
                    );

                    Crashes.TrackError(ex);
                }

                if (continua)
                {
                    PositionSender.StarSender();
                }
                else
                {
                    _servico.Stop();
                }

                return continua;
            }
        }

    }
#pragma warning restore CS1998
#pragma warning restore RECS0022
#pragma warning restore CS4014
}
