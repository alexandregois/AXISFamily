using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using family.Domain.Dto;
using Microsoft.AppCenter.Crashes;

namespace family.Model
{
	public class ModelTelefone : ModelBase
	{
		public ModelTelefone()
			: base()
		{
		}

		public async Task<ServiceResult<List<TelefoneContatoDto>>> GetTelefone(
			CancellationToken paramToken
		)
		{
			ServiceResult<List<TelefoneContatoDto>> result 
				= new ServiceResult<List<TelefoneContatoDto>>();
			result.IsValid = false;

			try
			{
				result = await DataStore.BuscarTelefoneRestLista(paramToken);
			}
			catch (Exception ex)
			{
				Crashes.TrackError(ex);
			}

			return result;
		}
	}
}
