using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using family.Domain.Dto;
using Microsoft.AppCenter.Crashes;

namespace family.Model
{
	public class ModelKeepAlive : ModelBase
	{
		public ModelKeepAlive()
			: base()
		{
		}
        
        public async Task<ServiceResult<List<Byte>>> CheckKeepAlive(
            CancellationToken paramToken)
        {
            ServiceResult<List<Byte>> result = new ServiceResult<List<Byte>>();
            try
            {
                result = await DataStore.CheckKeepAlive(paramToken);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }

            return result;

        }

        public async Task<ServiceResult<String>> RetornaObjetoKeepAlive(
            Byte paramIdObjetoKeepAlive,
            CancellationToken paramToken)
        {
            ServiceResult<String> result = new ServiceResult<String>();
            try
            {
                result = await DataStore.RetornaObjetoKeepAlive(paramIdObjetoKeepAlive, paramToken);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return result;

        }

        public async Task<ServiceResult<String>> GetAplicativoKeepAlive(
            Byte paramIdObjetoKeepAlive,
            CancellationToken paramToken)
        {
            ServiceResult<String> result = new ServiceResult<String>();
            try
            {
                result = await DataStore.GetAplicativoKeepAlive(paramIdObjetoKeepAlive, paramToken);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }

            return result;

        }
        
        public async Task<ServiceResult<Boolean>> AtualizarObjetoKeepAlive(
            Byte paramIdObjetoKeepAlive,
            CancellationToken paramToken)
        {
            ServiceResult<Boolean> result = new ServiceResult<Boolean>();
            try
            {
                result = await DataStore.AtualizarObjetoKeepAlive(paramIdObjetoKeepAlive, paramToken);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }

            return result;

        }

        public async Task<ServiceResult<MensagemSistemaDto>> RetornaMensagemSistema(Int16 paramIdMensagemSistema,
            CancellationToken paramToken
        )
        {
            ServiceResult<MensagemSistemaDto> result = new ServiceResult<MensagemSistemaDto>();
            try
            {
                result = await DataStore.RetornaMensagemSistema(paramIdMensagemSistema, paramToken);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }

            return result;
           
        }

    }
}
