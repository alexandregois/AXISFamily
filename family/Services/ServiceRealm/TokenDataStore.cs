using System;
using family.Domain.Realm;
using family.Services.ServiceRealm.Base;

namespace family.Services.ServiceRealm
{
	public class TokenDataStore : RealmBase<TokenRealm>
	{
		public TokenDataStore()
			:base()
		{
		}

		public Boolean? ExistIsLocator()
		{
			TokenRealm token = base.Get(1);
			Boolean? ret = null;

			if(token != null)
				ret = token.IsLocator;

			return ret;
		}

		public String GetAccessToken()
		{
			TokenRealm token = base.Get(1);
			String ret = null;

			if(token != null)
				ret = token.Access_Token;

			return ret;
		}

	}
}
