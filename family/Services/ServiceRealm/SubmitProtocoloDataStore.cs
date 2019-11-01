using System.Linq;
using family.Domain.Realm;
using family.Services.ServiceRealm.Base;

namespace family.Services.ServiceRealm
{
	public class SubmitProtocoloDataStore
		: RealmBase<SubmitProtocoloRealm>
	{
		public SubmitProtocoloDataStore()
			:base()
		{
		}

		public SubmitProtocoloRealm GetFirst()
		{
			return base.List().OrderBy(x => x.id).FirstOrDefault();
		}
	}
}
