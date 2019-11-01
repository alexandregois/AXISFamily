using System;
using Xamarin.Forms;

namespace family.Domain.Dto
{
	public class PosicaoHistorico
	{
		public long IdPosicao { get; set; }
		public Double? Velocidade { get; set; }
		public Double? Latitude { get; set; }
		public Double? Longitude { get; set; }
		public Byte? IdTipoUnidadeRastreada { get; set; }
		public DateTime? DataEvento { get; set; }
		public Int32 Total { get; set; }
		public String CorRegraPrioritaria { get; set; }
		public String NomeRegraViolada { get; set; }
		public Boolean? Ignicao { get; set; }
		public Int32? IdUnidadeRastreada { get; set; }
		public String Identificacao { get; set; }
		public Byte OrdemRastreador { get; set; }
		public string ResponsavelUnidadeRastreada { get; set; }
		public string IconUrl { get; set; }
		public Byte Status { get; set; }
		public Color? CorLinhaHistorico { get; set; }



		#region Elementos Calculados
		public String Icone
		{
			get
			{
				string temp = "pinCarroRoxo.png";
				if (this.IdTipoUnidadeRastreada.HasValue)
				{
					switch (this.IdTipoUnidadeRastreada)
					{
						case 2:
							temp = "pinCel_Roxo.png";
							break;
					}
				}
				return temp;
			}
		}

		public String StringVelocidade
		{
			get
			{
				string temp = "";
				if (this.Velocidade.HasValue)
					temp = this.Velocidade.Value.ToString();
				return temp + " Km/h";
			}
		}

		public String StringDataEvento
		{
			get
			{
				string temp = "";
				if (this.DataEvento.HasValue)
				{
					temp = this.DataEvento.Value.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss");
				}
				return temp;
			}
		}

		#endregion

	}
}
