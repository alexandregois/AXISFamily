using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using family.Domain.Dto;
using family.Domain.Enum;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;

namespace family.Model
{
	#pragma warning disable CS4014
	#pragma warning disable RECS0022
	#pragma warning disable CS1998
	public class ModelComando : ModelBase
	{
		public ModelComando()
			: base()
		{
		}

		public async Task<ServiceResult<StatusComandoDto>> GetStatusBloqueio(
			Int32 paramIdTrackedUnitTracker
			, Int32 paramIdTracker
			, CancellationToken paramToken
		)
		{
			
			ServiceResult<StatusComandoDto> result = new ServiceResult<StatusComandoDto>();
			try
			{
				result =
					await DataStore.StatusBloqueio(
						paramIdTrackedUnitTracker
						, paramIdTracker
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

		public async Task<ServiceResult<StatusComandoDto>> ComandoBloqueio(
			Int32 paramIdTracker
			, Int32 paramOrder
			, Int32 paramIdRastreadorUnidadeRastreada
			, String paramSenha
			, Boolean paramLock
			, CancellationToken paramToken
		)
		{
			ServiceResult<StatusComandoDto> result = new ServiceResult<StatusComandoDto>();
			try
			{

				List<ParametroDto> lst = new List<ParametroDto>();
				lst.Add(new ParametroDto(){
					IdTipoParametro = (Int32)EnumTipoParametro.Bloqueio,
					Valor = paramLock.ToString()
				});
				lst.Add(new ParametroDto(){
					IdTipoParametro = (Int32)EnumTipoParametro.Senha,
					Valor = paramSenha
				});
				
				result = await DataStore.ComandoBloqueio(
					paramIdTracker
					, paramOrder
					, paramIdRastreadorUnidadeRastreada
					, JsonConvert.SerializeObject(lst)
					, paramToken
				);
			}
			catch (Exception ex)
			{
				Crashes.TrackError(ex);
			}

			return result;
		}

        public async Task<ServiceResult<Boolean>> ComandoCancelar(Int32 paramIdCommandLog           
            , CancellationToken paramToken
        )
        {
            ServiceResult<Boolean> result = new ServiceResult<Boolean>();
            try
            {                

                result = await DataStore.ComandoCancelar(
                    paramIdCommandLog
                    , paramToken
                );
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return result;
        }

    }
#pragma warning restore CS1998
#pragma warning restore RECS0022
#pragma warning restore CS4014
}