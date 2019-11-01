using System;
using Realms;
using System.Linq;
using System.Collections.Generic;
using family.Model.ModelRealm;

namespace family.Services.ServiceRealm.Base
{
	public abstract class RealmBase<TEntity> where TEntity : RealmObject
	{
		public Realm RealmInstance
		{
			get
			{
				return Realm.GetInstance(RealmConfig.BuildDefaultConfig());
			}
		}

		public void Clean()
		{
			Realm _realm = RealmInstance;
			using (Transaction transaction = _realm.BeginWrite())
			{
				_realm.RemoveAll<TEntity>();
				transaction.Commit();
			}
		}

		public virtual IReadOnlyList<TEntity> List()
		{
			Realm _realm = RealmInstance;
			return _realm.All<TEntity>().ToList();
		}

		public TEntity Get(Int64 paramId)
		{
			Realm _realm = RealmInstance;
			return _realm.Find<TEntity>(paramId);
		}

		public void Remove(TEntity paramObject)
		{
			Realm _realm = RealmInstance;
			using (Transaction transaction = _realm.BeginWrite())
			{
				_realm.Remove(paramObject);
				transaction.Commit();
			}
		}

		public void CreateUpadate(TEntity paramObject)
		{
			Realm _realm = RealmInstance;
			using (Transaction transaction = _realm.BeginWrite())
			{
				_realm.Add(paramObject, true);
				transaction.Commit();
			}
		}
	}
}
