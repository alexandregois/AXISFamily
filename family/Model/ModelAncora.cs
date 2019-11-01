using System;
using System.Threading;
using System.Threading.Tasks;
using family.Domain.Dto;
using Microsoft.AppCenter.Crashes;

namespace family.Model
{
	#pragma warning disable CS4014
	#pragma warning disable RECS0022
	#pragma warning disable CS1998
	public class ModelAncora : ModelBase
	{
		public ModelAncora()
			: base()
		{
		}

		public async Task<ServiceResult<AncoraAtivacaoDto>> AtivarAncora(
			Double paramLatitude
			, Double paramLongitude
			, Int32 paramTolerancia
			, Int32 paramIdUnidadeRastreada
			, CancellationToken paramToken
		)
		{
			ServiceResult<AncoraAtivacaoDto> result = new ServiceResult<AncoraAtivacaoDto>();
			result.IsValid = false;
			try
			{
				result =
					await DataStore.AtivarAncora(
						paramIdUnidadeRastreada
						, paramLatitude.ToString().Replace(",", ".")
						, paramLongitude.ToString().Replace(",", ".")
						, paramTolerancia
						, paramToken
					);
			}
			catch (Exception ex)
			{
				result.IsValid = false;
				Crashes.TrackError(ex);
			}

			return result;
		}

		public async Task<ServiceResult<Int32?>> DesativarAncora(
			Int32 paramIdUnidadeRastreada
			, CancellationToken paramToken
		)
		{
			ServiceResult<Int32?> result = new ServiceResult<int?>();
			try
			{
				result = await DataStore.DesativarAncora(
					paramIdUnidadeRastreada
					, paramToken
				);
			}
			catch (Exception)
			{
				result.IsValid = false;
				//Crashes.TrackError(ex);
			}

			return result;
		}

	}
	#pragma warning restore CS1998
	#pragma warning restore RECS0022
	#pragma warning restore CS4014
}