using System;
using System.Collections.Generic;
using family.Domain.Enum;

namespace family.Domain.Dto
{
	public class PosicaoUnidadeRastreada
	{
		#region Posicao
		public Int32 IdRastreador { get; set; }
		public Int64 IdPosicao { get; set; }
		public Double? Latitude { get; set; }
		public Double? Longitude { get; set; }
		public DateTime? DataEvento { get; set; }
		public String Endereco { get; set; }
		public Double? Velocidade { get; set; }
		public Boolean? Ignicao { get; set; }
		public Boolean? GPSValido { get; set; }
		public Int32? IdRegraPrioritaria { get; set; }
		public Double? BateriaPrincipal { get; set; } //Novo
		public Double? BateriaBackup { get; set; } //Novo
		public DateTime? DataAtualizacao { get; set; }
		public String Evento { get; set; }
		public Double? SinalGPRS { get; set; }
		public Double? Odometro { get; set; }
		public Byte OrdemRastreador { get; set; }
        #endregion

        #region UnidadeRastreada
        public Int32? IdUnidadeRastreada { get; set; }
		public Int32? IdRastreadorUnidadeRastreada { get; set; }
		public Byte? IdTipoUnidadeRastreada { get; set; }
		public String Identificacao { get; set; }
		#endregion

		#region Ancora
		//public Int32? Ancora_IdGeography { get; set; }
		public Double? Ancora_Latitude { get; set; }
		public Double? Ancora_Longitude { get; set; }
		public Int32? Ancora_Tolerancia { get; set; }
		#endregion

		#region Elementos Calculados
		public String PathImage
		{
			get
			{
				//string temp = "ic_car2_cinza.png";

                string temp = IconePadrao;

                //if (this.IdTipoUnidadeRastreada.HasValue)
                //{
                //	switch (this.IdTipoUnidadeRastreada)
                //	{
                //		case (Byte)EnumTipoUnidadeRastreada.Pessoa:
                //			temp = "ic_celular_cinza.png";
                //			break;
                //	}
                //}
                return temp;
			}
		}

		//Largura máxima 31
		public Double PathImageWidth
		{
			get
			{
				Double temp = 31;
				if (this.IdTipoUnidadeRastreada.HasValue)
				{
					switch (this.IdTipoUnidadeRastreada)
					{
						case 2:
							temp = 17;
							break;
					}
				}
				return temp;
			}
		}

		//Altura máxima 33
		public Double PathImageHeight
		{
			get
			{
				Double temp = 22;
				if (this.IdTipoUnidadeRastreada.HasValue)
				{
					switch (this.IdTipoUnidadeRastreada)
					{
						case 2:
							temp = 33;
							break;
					}
				}
				return temp;
			}
		}

		public String Icone
		{
			get
			{
				string temp = "pin_ultima_posicao_carro.png";
				if (this.IdTipoUnidadeRastreada.HasValue)
				{
					switch (this.IdTipoUnidadeRastreada)
					{
						case 2:
							temp = "pin_ultima_posicao_cel.png";
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
					temp = this.DataEvento.Value.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss");
				return temp;
			}
		}

		public String StringDataAtualizacao
		{
			get
			{
				string temp = "";
				if (this.DataAtualizacao.HasValue)
					temp = this.DataAtualizacao.Value.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss");
				return temp;
			}
		}
        #endregion

        public List<PosicaoViolacaoRegraDto> ListaViolacaoRegra { get; set; }

        public string IconUrl { get; set; }
        public string IconePadrao { get; set; }

        public Int16 IdUnidadeMedidaBateriaPrincipal { get; set; }
    }
}
