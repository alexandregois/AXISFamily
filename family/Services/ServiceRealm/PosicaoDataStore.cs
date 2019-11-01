using System;
using System.Collections.Generic;
using family.Domain.Dto;
using family.Domain.Realm;
using family.Services.ServiceRealm.Base;
using System.Linq;
using Realms;

namespace family.Services.ServiceRealm
{
	public class PosicaoDataStore : RealmBase<PosicaoUnidadeRastreadaRealm>
	{
		public PosicaoDataStore()
			:base()
		{
		}

		public List<PosicaoUnidadeRastreadaRealm> Add(
			List<PosicaoUnidadeRastreada> paramLst
		)
		{
			List<PosicaoUnidadeRastreadaRealm> tempList = new List<PosicaoUnidadeRastreadaRealm>();
			try
			{
				if(paramLst != null)
				{
					List<PosicaoUnidadeRastreadaRealm> lst = new List<PosicaoUnidadeRastreadaRealm>();
					foreach(PosicaoUnidadeRastreada posi in paramLst)
					{
						PosicaoUnidadeRastreadaRealm posicaoRealm = new PosicaoUnidadeRastreadaRealm();
						posicaoRealm.Transform(posi);
						base.CreateUpadate(posicaoRealm);
						lst.Add(posicaoRealm);
					}
					tempList = lst.OrderByDescending(x => x.DataEvento).ToList();
				}
				else
				{
					base.Clean();
				}
			} catch { }
			return tempList;
		}

		public override IReadOnlyList<PosicaoUnidadeRastreadaRealm> List()
		{
			List<PosicaoUnidadeRastreadaRealm> lst 
			= base.List().OrderByDescending(x => x.DataEvento).ToList();
			return lst;
		}

		public void UpdateEndereco(
			Int64 paramId
			, String paramEndereco
		)
		{
			Realm _realm = RealmInstance;
			using (Transaction transaction = _realm.BeginWrite())
			{
				PosicaoUnidadeRastreadaRealm entity = 
					_realm.Find<PosicaoUnidadeRastreadaRealm>(paramId);

				entity.Endereco = paramEndereco;
				transaction.Commit();
			}
		}

		public void UpdateAncora(
			Double? paramLatitude
			, Double? paramLongitude
			, Int32? paramRaio
			, Int32 paramIdRastreador
		)
		{
			Realm _realm = RealmInstance;
			using (Transaction transaction = _realm.BeginWrite())
			{
				PosicaoUnidadeRastreadaRealm entity = 
					_realm.Find<PosicaoUnidadeRastreadaRealm>(paramIdRastreador);

				entity.Ancora_Latitude = paramLatitude;
				entity.Ancora_Longitude = paramLongitude;
				entity.Ancora_Tolerancia = paramRaio;
				transaction.Commit();
			}
		}
	}
}
