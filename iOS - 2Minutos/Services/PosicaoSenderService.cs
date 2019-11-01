using System;
using family.Domain.Realm;
using family.iOS.CrossPlataform.Position;
using family.Services.ServiceRealm;

namespace family.iOS.Services
{
	public class PosicaoSenderService
	{

		private static PositionSender _positionSender { get; set; }
		private static DateTime? _dataInicio { get; set; }

		public void SendPosition()
		{
			if(_positionSender != null)
			{
				if(PositionSender.udpClient != null)
				{
					if(_dataInicio.HasValue)
					{
						Int64 tempo = (long)DateTime.UtcNow.ToUniversalTime()
						                            .Subtract(_dataInicio.Value)
						                            .TotalSeconds;

						TokenDataStore dataStore = new TokenDataStore();
						TokenRealm token = dataStore.Get(1);

						if(
							tempo >= token.TempoTransmissao
						)
						{
							PositionSender.udpClient.Close();
							PositionSender.StarSender();
						}
					}
				}
			}
			else
			{
				_positionSender = new PositionSender(this);
				_dataInicio = DateTime.UtcNow;
			}
		}

		public void Stop()
		{
			_positionSender = null;
			_dataInicio = null;
		}
	}
}
