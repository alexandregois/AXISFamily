using System;
using System.Threading;
using System.Threading.Tasks;
using family.Domain.Dto;
using family.Views.Interfaces;
using Microsoft.AppCenter.Crashes;

namespace family.Model
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
    public class ModelUsuario : ModelBase
    {
        public IListaUnidadeRastreada _view { get; set; }

        public ModelUsuario()
            : base()
        {
        }

        public async Task<ServiceResult<Int32?>> Cadastrar(
            String paramNome
            , String paramCep
            , String paramTelefone
            , String paramEmail
            , String paramSenha
            , LatLong paramLatLng
            , Boolean paramIsAssocia
            , CancellationToken paramToken
        )
        {
            ServiceResult<Int32?> result = new ServiceResult<int?>();
            result.IsValid = false;
            try
            {
                if (_app.Util.IsInternetOnline())
                {
                    result =
                        await DataStore.CadastrarUsuario(
                            paramNome
                            , paramCep
                            , paramTelefone
                            , paramEmail
                            , paramSenha
                            , paramIsAssocia
                            , ""
                            , paramLatLng
                            , paramToken
                        );
                }
                else
                {
                    result.MessageError = "HttpRequestException";
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return result;

        }

        public async Task<ServiceResult<TokenDto>> Logar(
            string paramUsuario
            , string paramSenha
            , string paramIdApp
            , string paramHash
            , string paramPushkey
            , string paramIdentificacao
            , string paramIdSistemaOperacional
            , CancellationToken paramToken
        )
        {
            ServiceResult<TokenDto> result = new ServiceResult<TokenDto>();
            try
            {

                result = await DataStore.LoginRest(
                    paramUsuario
                    , paramSenha
                    , paramIdApp
                    , paramHash
                    , paramPushkey
                    , paramIdentificacao
                    , paramIdSistemaOperacional
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

        public async Task<ServiceResult<bool>> RetornaCompra_Realizada(
            CancellationToken paramToken
        )
        {
            ServiceResult<Boolean> result = new ServiceResult<Boolean>();
            try
            {
                result = await DataStore.RetornaCompra_Realizada(
                    paramToken
                );
            }
            catch (Exception) {
                result = false;
            }

            return result;
        }

        public async Task<ServiceResult<bool>> Grava_CompraRealizada(
            CancellationToken paramToken
        )
        {
            ServiceResult<Boolean> result = new ServiceResult<Boolean>();

            try
            {
                result = await DataStore.Grava_CompraRealizada(
                    paramToken
                );
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        public async Task Atualiza_PushKey(
            String paramPushKey
            , CancellationToken paramToken
        )
        {
            try
            {

                ServiceResult<Boolean> result = await DataStore.Atualiza_PushKey(
                    _app.Token.IdAplicativo
                    , paramPushKey
                    , paramToken
                );
            }
            catch (Exception) { }
        }

        public async Task Deslogar(CancellationToken paramToken)
        {
            try
            {
                if (_app.Util.IsInternetOnline())
                {
                    DataStore.Logoff(
                        _app.Token.IdAplicativo
                        , paramToken
                    );
                }
            }
            catch (Exception ex)
            { }
        }

        public async Task<ServiceResult<RetornoSolicitacaoRastreamentoDto>> RastrearDispositivo(
            String paramEmail
            , Int32 paramIdAplicativo
            , String paramImei
            , CancellationToken paramToken
        )
        {
            ServiceResult<RetornoSolicitacaoRastreamentoDto> result = new ServiceResult<RetornoSolicitacaoRastreamentoDto>();
            try
            {
                result = await DataStore.RastrearDispositivo(
                    paramEmail
                    , paramIdAplicativo
                    , paramImei
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

        public async Task EnviaPanico()
        {
            try
            {
                _app.Util.EnviaPanico();

            }
            catch (Exception) { }
        }
    }
#pragma warning restore CS1998
#pragma warning restore RECS0022
#pragma warning restore CS4014
}