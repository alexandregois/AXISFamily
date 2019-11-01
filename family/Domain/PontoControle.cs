using System;
using System.Collections.Generic;

namespace family.Domain
{
	public class PontoControle
	{
		public Int32 IdGeography { get; set; }
		public String Descricao { get; set; }
		public String NomePonto { get; set; }
        public String Endereco { get; set; }
        public Int32 Tolerancia { get; set; }
		public Double Latitude { get; set; }
		public Double Longitude { get; set; }
        public Boolean IsNotificaPontoHorario { get; set; }
        public String LstDiasSemana { get; set; }
        public TimeSpan HoraInicial { get; set; }
        public TimeSpan HoraFinal { get; set; }


        public String StringTolerancia
		{
			get
			{
				return Tolerancia.ToString() + " m";
			}
		}

	}
}
