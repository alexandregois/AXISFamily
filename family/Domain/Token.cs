using System;
using family.Domain.Realm;
namespace family.Domain
{
	public class Token
	{
		public String Access_Token { get; set; }
		public String expires_in { get; set; }
		public Boolean IsUsuarioAdminPadrao { get; set; }
		public String LstFuncao { get; set; }
        public String UrlLogo { get; set; }

        #region Aplicativo
        public Int32 IdAplicativo { get; set; }
		public Int32 Porta { get; set; }
		public String IP { get; set; }
		public Boolean IsLocator { get; set; }
		public Int32 TempoTransmissao { get; set; } = 180;
		public Int64 Identificacao { get; set; }
		#endregion

		public void TransformFromRealm(TokenRealm paramToken)
		{
			Access_Token = paramToken.Access_Token;
			expires_in = paramToken.expires_in;
			IsUsuarioAdminPadrao = paramToken.IsUsuarioAdminPadrao;
			LstFuncao = paramToken.LstFuncao;
          
			IdAplicativo = paramToken.IdAplicativo;
			Porta = paramToken.Porta;
			IP = paramToken.IP;
			IsLocator = paramToken.IsLocator;
			TempoTransmissao = paramToken.TempoTransmissao;
			Identificacao = paramToken.Identificacao;
            UrlLogo = paramToken.UrlLogo;
		}
	}
}
