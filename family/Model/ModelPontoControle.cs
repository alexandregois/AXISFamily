using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using family.Domain;
using family.Domain.Dto;
using Microsoft.AppCenter.Crashes;

namespace family.Model
{
	#pragma warning disable CS4014
	#pragma warning disable RECS0022
	#pragma warning disable CS1998
	public class ModelPontoControle : ModelBase
	{
		public ModelPontoControle()
			: base()
		{
		}

		public async Task<ServiceResult<List<PontoControle>>> BuscarPontoControle(
			CancellationToken paramToken
		)
		{
			ServiceResult<List<PontoControle>> result = new ServiceResult<List<PontoControle>>();
			try
			{ 
				result = await DataStore.ListaPontoControle(
					paramToken
				);

			}
			catch (Exception ex)
            {
				result.IsValid = false;
				Crashes.TrackError(ex);
			}

			return result;
		}

		public async Task<ServiceResult<Boolean?>> InsertPontoControle(
			PontoControle PontosControle
			, CancellationToken paramToken
		)
		{
			ServiceResult<Boolean?> result = new ServiceResult<bool?>();
			try
			{
				result = await DataStore.InsertPontoControle(
					PontosControle
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

		public async Task<ServiceResult<Boolean?>> UpdatePontoControle(
			PontoControle PontosControle
			, CancellationToken paramToken
		)
		{
			ServiceResult<Boolean?> result = new ServiceResult<bool?>();
			try
			{
				result = await DataStore.UpdatePontoControle(
					PontosControle
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

		public async Task<ServiceResult<Boolean?>> DeletePontoControle(
			int paramIdGeography
			, CancellationToken paramToken
		)
		{
			ServiceResult<Boolean?> result = new ServiceResult<bool?>();
			try
			{
				result = await DataStore.DeletePontoControle(
					paramIdGeography
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


	}
	#pragma warning restore CS1998
	#pragma warning restore RECS0022
	#pragma warning restore CS4014
}