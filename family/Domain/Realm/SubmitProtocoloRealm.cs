using System;
using Realms;

namespace family.Domain.Realm
{
	public class SubmitProtocoloRealm : RealmObject
	{
		[PrimaryKey]
		public Int32 id { get; set; }
		public Int64 IdentificacaoAVL { get; set; }
		public Byte[] message { get; set; }
	}
}
