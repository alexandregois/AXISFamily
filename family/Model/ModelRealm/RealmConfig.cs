using Realms;

namespace family.Model.ModelRealm
{
	public class RealmConfig
	{
		public static RealmConfiguration BuildDefaultConfig()
		{
			RealmConfiguration _realmConfig = new RealmConfiguration();
			_realmConfig.ShouldDeleteIfMigrationNeeded = true;
			_realmConfig.SchemaVersion = 0;
			return _realmConfig;
		}
	}
}
