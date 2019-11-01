using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using family.iOS.Services;
using family.Services.ServiceRealm;
using family.Domain.Realm;
using family.Domain.Protocolo;
using family.Model;

namespace family.iOS.CrossPlataform.Position
{
	public class PositionSender
	{

		public static Socket _socket { get; set; }
		public static EndPoint _remoteEndPoint { get; set; }
		public static Int32 tentativas { get; set; }
		public static UdpClient udpClient { get; set; }
		public static PosicaoSenderService _servico { get; set; }

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
				Task.Run(() =>
				{
					RunInBackground();
				});
			}
			catch(Exception)
			{

			}
		}

		public static void RunInBackground()
		{
			Boolean continua = false;
			try
			{
				TokenDataStore dataStore = new TokenDataStore();
				TokenRealm token = dataStore.Get(1);

				Int32 timeout = (Int32)TimeSpan.FromSeconds(15).TotalMilliseconds;
				udpClient = new UdpClient()
				{
					EnableBroadcast = true
				};

				IPAddress.TryParse(token.IP, out IPAddress ip);

				udpClient.Connect(ip, token.Porta);

				SubmitProtocoloDataStore store = new SubmitProtocoloDataStore();

				SubmitProtocoloRealm send = store.GetFirst();

				if(send != null)
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

					ModelReverseBitConverter reverse = new ModelReverseBitConverter();

					Int32 inicial = 0;
					udpResult.Sequencial = reverse.ToInt32(receiveBytes, inicial);
					inicial += 4;
					udpResult.IdentificacaoAVL = reverse.ToInt64(receiveBytes, inicial);

					if(
						udpResult.IdentificacaoAVL == send.IdentificacaoAVL
						&& send.id == udpResult.Sequencial
					)
					{
						SubmitProtocoloRealm temp = store.Get(udpResult.Sequencial);
						store.Remove(temp);

						tentativas = 0;
						continua = true;
					}
					else
					{
						tentativas++;
					}

					udpClient.Close();
				}
				else
				{
					CrossPlataformUtil _util = new CrossPlataformUtil();
					_util.SaveSequencialPosicao("0");
					_servico.Stop();
				}
			}
			catch(SocketException ex)
			{

				System.Diagnostics.Debug.WriteLine(
					String.Format(
						"FA&)#2 StarSender_Finish() " +
						"InnerException: {0} || Message: {1}"
						, ex.InnerException
						, ex.Message
					)
				);

				if(udpClient != null)
					udpClient.Close();

				tentativas++;
				if(tentativas < 3)
				{
					continua = true;
				}
			}
			catch(Exception ex)
			{
				if(udpClient != null)
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
			}

			if(continua)
			{
				PositionSender.StarSender();
			}
			else
			{
				_servico.Stop();
			}

		}
	}
}
