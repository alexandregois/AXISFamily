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
	public class ModelManutencao : ModelBase
	{
		public ModelManutencao()
			: base()
		{
		}

		public async Task<ServiceResult<ManutencaoBasicaDto>> ListaManutencao(ManutencaoBasicaDto paramManutencao,
            CancellationToken paramToken
		)
		{
			ServiceResult<ManutencaoBasicaDto> result = new ServiceResult<ManutencaoBasicaDto>();
			try
			{
                result = await DataStore.ListaManutencao(paramManutencao, paramToken);
			}
			catch (Exception ex)
            {
				result.IsValid = false;
				Crashes.TrackError(ex);
			}

			return result;
		}

        public async Task<ServiceResult<ManutencaoBasicaDto>> UpdateManutencao(ManutencaoBasicaDto paramManutencao,
            CancellationToken paramToken
        )
        {
            ServiceResult<ManutencaoBasicaDto> result = new ServiceResult<ManutencaoBasicaDto>();
            try
            {
                result = await DataStore.UpdateManutencao(paramManutencao, paramToken);
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