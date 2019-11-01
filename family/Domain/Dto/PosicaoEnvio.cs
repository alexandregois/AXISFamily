using System;

namespace family.Domain.Dto
{
	public class PosicaoEnvio
	{
		public Int32 IdUnidadeRastreada { get; set; }
		public Double? Latitude { get; set; }
		public Double? Longitude { get; set; }
		public DateTime? DataEvento { get; set; }
		public Double? Velocidade { get; set; }
		public Int32? BateriaPrincipal { get; set; }
		public Double? Altitude { get; set; }

		public string ProtocoloEnvio(){
			//String strUDPMessagePosicaoAtual = "gsfamilyv1;" + "79;" +
			//   Posicao.Imei + ";" +
			//   Posicao.Evento + ";" +
			//   srtLatitude + ";" +
			//   srtLongitude + ";" +
			//   DateTime.UtcNow.ToString(FormatoData) + ";" +
			//   DateTime.UtcNow.ToString(FormatoData) + ";" +
			//   Posicao.Velocidade + ";" +
			//   ";" +
			//   Posicao.Altitude + ";" +
			//   ";" +
			//   Posicao.Bateria + ";" +
			//   ";" +
			//   ";" +
			//   ";" +
			//   ";" +
			//   ";" +
			//   ";";

			String ret = "";


			return ret;
		}

	}
}
