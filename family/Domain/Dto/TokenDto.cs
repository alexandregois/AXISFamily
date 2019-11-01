using System;
using System.Collections.Generic;

namespace family.Domain.Dto
{
	public class TokenDto
	{
		public String Access_Token { get; set; }
		public String expires_in { get; set; }
		public List<PosicaoUnidadeRastreada> LastPositions { get; set; }
		public AplicativoDto Aplicativo { get; set; }
		public List<Int32> LstFuncao { get; set; }
		public Boolean IsUsuarioAdminPadrao { get; set; }
		public Int32? TempoTransmissao { get; set; }
        public String UrlLogo { get; set; }

    }
}
