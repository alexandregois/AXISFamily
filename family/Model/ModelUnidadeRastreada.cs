using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using family.Domain.Dto;
using Microsoft.AppCenter.Crashes;

namespace family.Model
{
	public class ModelUnidadeRastreada : ModelBase
	{
		public ModelUnidadeRastreada()
			: base()
		{
		}

		public async Task<ServiceResult<List<PosicaoUnidadeRastreada>>> GetUnidadeRastreada(
			CancellationToken paramToken
		)
		{
			ServiceResult<List<PosicaoUnidadeRastreada>> result 
				= new ServiceResult<List<PosicaoUnidadeRastreada>>();
			result.IsValid = false;

			try
			{
				result = await DataStore.BuscarUnidadeRastreadaRestLista(paramToken);
			}
			catch (Exception ex)
			{
				Crashes.TrackError(ex);
			}

			return result;
		}
	}
}
