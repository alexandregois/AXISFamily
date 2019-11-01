using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using family.Domain.Dto;
using Microsoft.AppCenter.Crashes;

namespace family.Model
{
	public class ModelPosicao : ModelBase
	{
		public ModelPosicao()
			: base()
		{
		}

		public async Task<ServiceResult<PosicaoUnidadeRastreada>> 
		BuscarUltimaPosicao(
			int paramIdRastreadorUnidadeRastreada
			, CancellationToken paramToken
		)
		{
			ServiceResult<PosicaoUnidadeRastreada> result 
			= new ServiceResult<PosicaoUnidadeRastreada>();

			try
			{
				result = await DataStore.BuscarUltimaPosicao(
					paramIdRastreadorUnidadeRastreada
					, paramToken
				);
			}
			catch (Exception ex)
			{
                Crashes.TrackError(ex);
			}
			return result;
		}


		public async Task<ServiceResult<List<PosicaoHistorico>>> BuscarHistoricoPosicao(
			Int32 paramIdUnidadeRastreada
			, Byte paramOrdemRastreador
			, String paramInitialPeriod
			, String paramFinalPeriod
			, Int32 paramStartRowIndex
			, Int32 paramPageSize
			, Boolean paramIsReverse
			, CancellationToken paramToken
		)
		{

			ServiceResult<List<PosicaoHistorico>> result
				= new ServiceResult<List<PosicaoHistorico>>();
			result.IsValid = false;

			try
			{
				result = await DataStore.BuscarHistoricoPosicao(
					paramIdUnidadeRastreada
					, paramOrdemRastreador
					, paramInitialPeriod
					, paramFinalPeriod
					, paramStartRowIndex
					, paramPageSize
					, paramIsReverse
					, paramToken
				);
			}
			catch (Exception ex)
            {
                Crashes.TrackError(ex);

			}

			return result;

		}

		public async Task<ServiceResult<PosicaoUnidadeRastreada>> BuscarDetalhePosicao(
			Int64 paramIdPosicao
			, Int32 paramIdUnidadeRastreada
			, Byte paramOrdem
			, CancellationToken paramToken
		)
		{
			ServiceResult<PosicaoUnidadeRastreada> result = new ServiceResult<PosicaoUnidadeRastreada>();
			try
			{
				result =  await DataStore.BuscarDetalhePosicao(
					paramIdPosicao
					, paramIdUnidadeRastreada
					, paramOrdem
					, paramToken
				);
			}
			catch(Exception ex)
			{
                Crashes.TrackError(ex);

            }

			return result;

		}

    }
}
