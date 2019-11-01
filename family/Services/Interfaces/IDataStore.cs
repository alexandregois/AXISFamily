using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using family.Domain;
using family.Domain.Dto;

namespace family.Services.Interfaces
{
    public interface IDataStore
    {
        Task<ServiceResult<TokenDto>> LoginRest(
            string paramUsuario
            , string paramSenha
            , string paramIdApp
            , string paramHash
            , string paramPushkey
            , string paramIdentificacao
            , string paramIdSistemaOperacional
            , CancellationToken paramToken
        );

        Task<ServiceResult<Int32?>> CadastrarUsuario(
            string paramNome
            , string paramCep
            , string paramTelefone
            , string paramEmail
            , string paramSenha
            , bool paramIsAssocia
            , string paramHash
            , LatLong paramLatLng
            , CancellationToken paramToken
        );

        Task<ServiceResult<List<PosicaoUnidadeRastreada>>> BuscarUnidadeRastreadaRestLista(
            CancellationToken paramToken
        );

        Task<ServiceResult<List<TelefoneContatoDto>>> BuscarTelefoneRestLista(
           CancellationToken paramToken
       );

        #region Ponto de Controle
        Task<ServiceResult<List<PontoControle>>> ListaPontoControle(
            CancellationToken paramToken
        );

        Task<ServiceResult<Boolean?>> InsertPontoControle(
            PontoControle paramPontoControle
            , CancellationToken paramToken
        );

        Task<ServiceResult<Boolean?>> UpdatePontoControle(
            PontoControle paramPontoControle
            , CancellationToken paramToken
        );

        Task<ServiceResult<Boolean?>> DeletePontoControle(
            int paramIdGeography
            , CancellationToken paramToken
        );
        #endregion

        #region Posição
        Task<ServiceResult<List<PosicaoHistorico>>> BuscarHistoricoPosicao(
            Int32 paramIdUnidadeRastreada
            , Byte paramOrdemRastreador
            , String paramInitialPeriod
            , String paramFinalPeriod
            , Int32 paramStartRowIndex
            , Int32 paramPageSize
            , Boolean paramIsReverse
            , CancellationToken paramToken
        );

        Task<ServiceResult<PosicaoUnidadeRastreada>> BuscarUltimaPosicao(
            int idRastreadorUnidadeRastreada
            , CancellationToken paramToken
        );

        Task<ServiceResult<PosicaoUnidadeRastreada>> BuscarDetalhePosicao(
            Int64 paramIdPosicao
            , Int32 paramIdUnidadeRastreada
            , Byte paramOrdem
            , CancellationToken paramToken
        );
        #endregion

        #region Âncora
        Task<ServiceResult<AncoraAtivacaoDto>> AtivarAncora(
            int? idUnidadeRastreada
            , string paramLatitude
            , string paramLongitude
            , int paramTolerancia
            , CancellationToken paramToken
        );
        Task<ServiceResult<bool>> ComandoCancelar(int paramIdCommandLog, CancellationToken paramToken);

        Task<ServiceResult<Int32?>> DesativarAncora(
            int? idUnidadeRastreada
            , CancellationToken paramToken
        );
        #endregion

        Task<ServiceResult<RetornoSolicitacaoRastreamentoDto>> RastrearDispositivo(
            String paramEmail
            , Int32 paramIdAplicativo
            , String paramImei
            , CancellationToken paramToken
        );

        Task<ServiceResult<String>> GetAplicativoKeepAlive(
           Byte paramIdObjetoKeepAlive,
            CancellationToken paramToken
        );

        Task<ServiceResult<List<Byte>>> CheckKeepAlive(
            CancellationToken paramToken
        );

        Task<ServiceResult<String>> RetornaObjetoKeepAlive(Byte paramIdObjetoKeepAlive,
            CancellationToken paramToken
        );

        Task<ServiceResult<Boolean>> AtualizarObjetoKeepAlive(Byte paramIdObjetoKeepAlive,
            CancellationToken paramToken
        );

        Task<ServiceResult<Boolean?>> Logoff(
            Int32 paramIdAplicativo
            , CancellationToken paramToken
        );

        Task<ServiceResult<bool>> Atualiza_PushKey(
            int paramIdAplicativo
            , String paramPushKey
            , CancellationToken paramToken
        );

        Task<ServiceResult<bool>> RetornaCompra_Realizada(
            CancellationToken paramToken
        );

        Task<ServiceResult<bool>> Grava_CompraRealizada(
            CancellationToken paramToken
        );

        #region Bloqueio
        Task<ServiceResult<StatusComandoDto>> StatusBloqueio(
            int paramIdTrackedUnitTracker
            , int paramIdTracker
            , CancellationToken paramToken
        );

        Task<ServiceResult<StatusComandoDto>> ComandoBloqueio(
            int paramIdTracker
            , int paramOrder
            , Int32 paramIdRastreadorUnidadeRastreada
            , String paramLstParametresCommand
            , CancellationToken paramToken
        );
        #endregion


        Task<ServiceResult<ManutencaoBasicaDto>> ListaManutencao(
            ManutencaoBasicaDto paramManutencao
            , CancellationToken paramToken
        );

        Task<ServiceResult<ManutencaoBasicaDto>> UpdateManutencao(
            ManutencaoBasicaDto paramManutencao
            , CancellationToken paramToken
        );

        Task<ServiceResult<MensagemSistemaDto>> RetornaMensagemSistema(Int16 paramIdMensagemSistema,
            CancellationToken paramToken
        );
        
    }
}
