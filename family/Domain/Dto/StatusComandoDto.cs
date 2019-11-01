using System;

namespace family.Domain.Dto
{
	public class StatusComandoDto
    {
        public Int32? IdComandoLog { get; set; }
        public DateTime? DataFila { get; set; }
        public Byte? IdStatusComando { get; set; }
        public Int32? IdRastreador { get; set; }
        public Boolean IsBloqueado { get; set; }

		public Boolean PodeMandarComando
		{
			get
			{
				Boolean desc = true;
				if(this.IdStatusComando.HasValue)
				{
					switch (this.IdStatusComando.Value)
					{
						case 1:
						case 2:
						case 3: 
						case 4: 
							desc = false;
							break;
						default:
							desc = true;
							break;
					}
				}
				return desc;
			}
		}

        public String Descricao
        {
            get
            {
				//string desc = "Enviar";
                string desc = "";
                if (this.IdStatusComando.HasValue)
				{
	                switch (this.IdStatusComando.Value)
	                {
	                    case 7:
							desc = "Enviado";
							break;
	                    case 2:
						case 4: 
							desc = "Enviando";
							break;
	                    case 1:
						case 3: 
							desc = "Aguardando"; //Poderia ser ativado o cancelar comando
							break;
	                    case 5:
	                    case 6: 
							desc = "Cancelado";
							break;
	                    case 8:
						case 9: 
							desc = "Não enviado";
							break;
	                }
				}
				return desc;
            }
        }
    }
}
