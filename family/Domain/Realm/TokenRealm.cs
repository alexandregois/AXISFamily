using System;
using Realms;

namespace family.Domain.Realm
{
	public class TokenRealm: RealmObject
	{
		[PrimaryKey]
		public int Id { get; set; }
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
		public Int32 TempoTransmissao { get; set; }

        public Int64 Identificacao { get; set; }
		#endregion
	}
}
