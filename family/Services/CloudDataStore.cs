using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using family.Domain;
using family.Domain.Dto;
using family.Resx;
using family.Services.Interfaces;
using family.Services.ServiceRealm;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace family.Services
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
    public class CloudDataStore : IDataStore
    {
        private App _app => (Application.Current as App);
        private HttpClient _client { get; set; }

        private String _mediaType { get; set; } = "application/x-www-form-urlencoded";

        public CloudDataStore()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(_app.Configuracao.URLStringWS)
            };
        }

        private void ConfiguraChamadaPadrao()
        {
            TokenDataStore token = new TokenDataStore();
            String accessToken = token.GetAccessToken();
            if (!String.IsNullOrWhiteSpace(accessToken))
                _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Basic"
                        , accessToken
                    );
            _client.DefaultRequestHeaders.IfModifiedSince = DateTime.Now;
        }

        public async Task<ServiceResult<TokenDto>> LoginRest(
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

            List<KeyValuePair<string, string>> requisaoParametros =
                new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(
                    "username"
                    , paramUsuario
                ),

                new KeyValuePair<string, string>(
                    "password"
                    , paramSenha
                ),

                new KeyValuePair<string, string>(
                    "hash"
                    , paramHash
                ),

                new KeyValuePair<string, string>(
                    "identificacao"
                    , paramIdentificacao
                ),

                new KeyValuePair<string, string>(
                    "idsistemaoperacional"
                    , paramIdSistemaOperacional
                ),

                new KeyValuePair<string, string>(
                    "idaplicacao"
                    , "7"
                ),

                new KeyValuePair<string, string>(
                    "pushkey"
                    , paramPushkey
                )


            };
            FormUrlEncodedContent content = new FormUrlEncodedContent(requisaoParametros);

            content.Headers.ContentType = new MediaTypeHeaderValue(_mediaType)
            {
                CharSet = "UTF-8"
            };

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));

            String urlRequisicao = "login/AuthenticateUser";

            return await MakeRequisicion<TokenDto>(
                HttpMethod.Post
                , urlRequisicao
                , content
                , paramToken
            );
        }


        public async Task<ServiceResult<List<PosicaoUnidadeRastreada>>>
        BuscarUnidadeRastreadaRestLista(CancellationToken paramToken)
        {
            String urlRequisicao = "TracedUnit";

            return await MakeRequisicion<List<PosicaoUnidadeRastreada>>(
                HttpMethod.Get
                , urlRequisicao
                , null
                , paramToken
            );
        }

        public async Task<ServiceResult<List<TelefoneContatoDto>>>
        BuscarTelefoneRestLista(CancellationToken paramToken)
        {
            String urlRequisicao = "ContactPhone/?paramIdContactPhoneFunction=1";

            return await MakeRequisicion<List<TelefoneContatoDto>>(
                HttpMethod.Get
                , urlRequisicao
                , null
                , paramToken
            );
        }

        #region Posição
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

            String urlRequisicao = String.Format(
                "PositionHistory/{0}?paramTipo={1}" +
                "&paramInitialPeriod={2}&paramFinalPeriod={3}" +
                "&paramStartRowIndex={4}&paramPageSize={5}" +
                "&paramIsReverse={6}&paramOrdem={7}"
                , paramIdUnidadeRastreada
                , 1
                , paramInitialPeriod
                , paramFinalPeriod
                , paramStartRowIndex
                , paramPageSize
                , paramIsReverse
                , paramOrdemRastreador
            );

            return await MakeRequisicion<List<PosicaoHistorico>>(
                HttpMethod.Get
                , urlRequisicao
                , null
                , paramToken
            );

        }

        public async Task<ServiceResult<PosicaoUnidadeRastreada>> BuscarUltimaPosicao(
            int paramIdRastreadorUnidadeRastreada
            , CancellationToken paramToken
        )
        {
            String urlRequisicao = "LastPosition/" + paramIdRastreadorUnidadeRastreada.ToString();

            return await MakeRequisicion<PosicaoUnidadeRastreada>(
                HttpMethod.Get
                , urlRequisicao
                , null
                , paramToken
            );
        }

        public async Task<ServiceResult<PosicaoUnidadeRastreada>> BuscarDetalhePosicao(
            Int64 paramIdPosicao
            , Int32 paramIdUnidadeRastreada
            , Byte paramOrdem
            , CancellationToken paramToken
        )
        {
            String urlRequisicao = "Position/" + paramIdPosicao +
                "?paramIsCompleta=false" +
                "&paramUnidadeRastreada=" + paramIdUnidadeRastreada +
                "&paramOrdem=" + paramOrdem;

            return await MakeRequisicion<PosicaoUnidadeRastreada>(
                HttpMethod.Get
                , urlRequisicao
                , null
                , paramToken
            );
        }
        #endregion

        #region Ancora
        public async Task<ServiceResult<AncoraAtivacaoDto>> AtivarAncora(
            int? idUnidadeRastreada
            , string paramLatitude
            , string paramLongitude
            , int paramTolerancia
            , CancellationToken paramToken
        )
        {

            List<KeyValuePair<string, string>> requisaoParametros =
                new List<KeyValuePair<string, string>>();

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramIdTracedUnit"
                    , idUnidadeRastreada.ToString()
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramLatitude"
                    , paramLatitude.ToString()
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramLongitude"
                    , paramLongitude.ToString()
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramTolerance"
                    , paramTolerancia.ToString()
                )
            );

            FormUrlEncodedContent content = new FormUrlEncodedContent(requisaoParametros);

            content.Headers.ContentType = new MediaTypeHeaderValue(_mediaType);

            content.Headers.ContentType.CharSet = "UTF-8";

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));

            String urlRequisicao = "Anchor";

            return await MakeRequisicion<AncoraAtivacaoDto>(
                HttpMethod.Post
                , urlRequisicao
                , content
                , paramToken
            );
        }

        public async Task<ServiceResult<int?>> DesativarAncora(
            int? idUnidadeRastreada
            , CancellationToken paramToken
        )
        {

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));

            String urlRequisicao = "Anchor/" + idUnidadeRastreada.ToString();

            return await MakeRequisicion<Int32?>(
                HttpMethod.Delete
                , urlRequisicao
                , null
                , paramToken
            );
        }
        #endregion

        #region Manutencao

        public async Task<ServiceResult<ManutencaoBasicaDto>> ListaManutencao(
            ManutencaoBasicaDto paramManutencao
            , CancellationToken paramToken
        )
        {
            String urlRequisicao = "Manutencao" +
                "?paramIdUnidadeRastreada=" + paramManutencao.IdUnidadeRastreada +
                "&paramProximaRevisao=" + paramManutencao.ProximaRevisao +
                "&paramTrocaOleo=" + paramManutencao.TrocaOleo +
                "&RodizioPneu=" + paramManutencao.RodizioPneu +
                "&ValidadeSeguro=" + paramManutencao.ValidadeSeguro;


            return await MakeRequisicion<ManutencaoBasicaDto>(
                HttpMethod.Get
                , urlRequisicao
                , null
                , paramToken
            );
        }

        public async Task<ServiceResult<ManutencaoBasicaDto>> UpdateManutencao(
            ManutencaoBasicaDto paramManutencao
            , CancellationToken paramToken
        )
        {
            String urlRequisicao = "Manutencao" +
                "?paramIdUnidadeRastreada=" + paramManutencao.IdUnidadeRastreada +
                "&paramProximaRevisao=" + paramManutencao.ProximaRevisao +
                "&paramTrocaOleo=" + paramManutencao.TrocaOleo +
                "&paramRodizioPneu=" + paramManutencao.RodizioPneu +
                "&paramValidadeSeguro=" + paramManutencao.StringValidadeSeguro;


            return await MakeRequisicion<ManutencaoBasicaDto>(
                HttpMethod.Post
                , urlRequisicao
                , null
                , paramToken
            );
        }

        #endregion

        #region Ponto de Controle
        public async Task<ServiceResult<List<PontoControle>>> ListaPontoControle(
            CancellationToken paramToken
        )
        {
            String urlRequisicao = "ControlPoint";

            return await MakeRequisicion<List<PontoControle>>(
                HttpMethod.Get
                , urlRequisicao
                , null
                , paramToken
            );

        }

        public async Task<ServiceResult<Boolean?>> InsertPontoControle(
            PontoControle paramPontoControle
            , CancellationToken paramToken
        )
        {

            List<KeyValuePair<string, string>> requisaoParametros =
                new List<KeyValuePair<string, string>>();

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramNome"
                    , paramPontoControle.NomePonto
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramEndereco"
                    , paramPontoControle.Endereco
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramLatitude"
                    , paramPontoControle.Latitude.ToString()
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramLongitude"
                    , paramPontoControle.Longitude.ToString()
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramTolerance"
                    , paramPontoControle.Tolerancia.ToString()
                )
            );


            if (paramPontoControle.HoraInicial.ToString() != "00:00:00" && paramPontoControle.HoraFinal.ToString() != "00:00:00")
            {

                requisaoParametros.Add(
              new KeyValuePair<string, string>(
                  "paramDiasSemana"
                  , paramPontoControle.LstDiasSemana.ToString()
              )
            );

                requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramHoraIncio"
                    , paramPontoControle.HoraInicial.ToString(@"hh\:mm")
                )
            );

                requisaoParametros.Add(
                    new KeyValuePair<string, string>(
                        "paramHoraFim"
                        , paramPontoControle.HoraFinal.ToString(@"hh\:mm")
                    )
                );

            }

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramIsNotificaPontoHorario"
                    , Convert.ToString(paramPontoControle.IsNotificaPontoHorario)
                )
            );


            FormUrlEncodedContent content = new FormUrlEncodedContent(requisaoParametros);

            content.Headers.ContentType = new MediaTypeHeaderValue(_mediaType);

            content.Headers.ContentType.CharSet = "UTF-8";

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));

            String urlRequisicao = "ControlPoint";

            return await MakeRequisicion<Boolean?>(
                HttpMethod.Post
                , urlRequisicao
                , content
                , paramToken
            );
        }

        public async Task<ServiceResult<Boolean?>> UpdatePontoControle(
            PontoControle paramPontoControle
            , CancellationToken paramToken
        )
        {

            List<KeyValuePair<string, string>> requisaoParametros =
                new List<KeyValuePair<string, string>>();

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramNome"
                    , paramPontoControle.NomePonto
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramEndereco"
                    , paramPontoControle.Endereco
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramLatitude"
                    , paramPontoControle.Latitude.ToString()
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramLongitude"
                    , paramPontoControle.Longitude.ToString()
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramTolerance"
                    , paramPontoControle.Tolerancia.ToString()
                )
            );

            if (paramPontoControle.HoraInicial.ToString() != "00:00:00" && paramPontoControle.HoraFinal.ToString() != "00:00:00")
            {

                requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramDiasSemana"
                    , paramPontoControle.LstDiasSemana
                )
            );

                requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramHoraIncio"
                    , paramPontoControle.HoraInicial.ToString(@"hh\:mm")
                )
            );

                requisaoParametros.Add(
                    new KeyValuePair<string, string>(
                        "paramHoraFim"
                        , paramPontoControle.HoraFinal.ToString(@"hh\:mm")
                    )
                );

            }

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramIsNotificaPontoHorario"
                    , Convert.ToString(paramPontoControle.IsNotificaPontoHorario)
                )
            );


            FormUrlEncodedContent content = new FormUrlEncodedContent(requisaoParametros);

            content.Headers.ContentType = new MediaTypeHeaderValue(_mediaType);

            content.Headers.ContentType.CharSet = "UTF-8";

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));

            String urlRequisicao = "ControlPoint/" + paramPontoControle.IdGeography.ToString();

            return await MakeRequisicion<Boolean?>(
                HttpMethod.Put
                , urlRequisicao
                , content
                , paramToken
            );
        }

        public async Task<ServiceResult<Boolean?>> DeletePontoControle(
            int paramIdGeography
            , CancellationToken paramToken
        )
        {

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));

            String urlRequisicao = "ControlPoint/" + paramIdGeography.ToString();

            return await MakeRequisicion<Boolean?>(
                HttpMethod.Delete
                , urlRequisicao
                , null
                , paramToken
            );
        }
        #endregion

        public async Task<ServiceResult<bool>> RetornaCompra_Realizada(
            CancellationToken paramToken
        )
        {

            String urlRequisicao = "Payment/?paramIdAplicacao=7";

            return await MakeRequisicion<Boolean>(
                HttpMethod.Get
                , urlRequisicao
                , null
                , paramToken
            );
        }

        public async Task<ServiceResult<bool>> Grava_CompraRealizada(
            CancellationToken paramToken
        )
        {
            List<KeyValuePair<string, string>> requisaoParametros =
                new List<KeyValuePair<string, string>>();

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramIdAplicacao"
                    , "7"
                )
            );

            FormUrlEncodedContent content = new FormUrlEncodedContent(requisaoParametros);

            content.Headers.ContentType = new MediaTypeHeaderValue(_mediaType);

            content.Headers.ContentType.CharSet = "UTF-8";

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));

            String urlRequisicao = "Payment";

            return await MakeRequisicion<Boolean>(
                HttpMethod.Post
                , urlRequisicao
                , content
                , paramToken
            );
        }


        public async Task<ServiceResult<RetornoSolicitacaoRastreamentoDto>> RastrearDispositivo(
            String paramEmail
            , Int32 paramIdAplicativo
            , String paramImei
            , CancellationToken paramToken
        )
        {

            List<KeyValuePair<string, string>> requisaoParametros = new List<KeyValuePair<string, string>>();

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramEmail"
                    , paramEmail
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramAplicativo"
                    , paramIdAplicativo.ToString()
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramImei"
                    , paramImei
                )
            );

            FormUrlEncodedContent content = new FormUrlEncodedContent(requisaoParametros);

            content.Headers.ContentType = new MediaTypeHeaderValue(_mediaType);

            content.Headers.ContentType.CharSet = "UTF-8";

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));

            String urlRequisicao = "TracedUnit";

            return await MakeRequisicion<RetornoSolicitacaoRastreamentoDto>(
                HttpMethod.Post
                , urlRequisicao
                , content
                , paramToken
            );
        }

        public async Task<ServiceResult<int?>> CadastrarUsuario(
            string paramNome
            , string paramCep
            , string paramTelefone
            , string paramEmail
            , string paramSenha
            , bool paramIsAssocia
            , string paramHash
            , LatLong paramLatLng
            , CancellationToken paramToken
        )
        {
            List<KeyValuePair<string, string>> requisaoParametros =
                new List<KeyValuePair<string, string>>();

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramNome"
                    , paramNome
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramCep"
                    , paramCep
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramTelefone"
                    , paramTelefone
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramEmail"
                    , paramEmail
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramSenha"
                    , paramSenha
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramGmt"
                    , TimeZoneInfo.Local.BaseUtcOffset.ToString()
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramCodigoEmpresa"
                    , paramHash
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramLatitude"
                    , paramLatLng.Latitude.ToString()
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramLongitude"
                    , paramLatLng.Longitude.ToString()
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramTipoCadastro", ""));

            FormUrlEncodedContent content = new FormUrlEncodedContent(requisaoParametros);

            content.Headers.ContentType = new MediaTypeHeaderValue(_mediaType);

            content.Headers.ContentType.CharSet = "UTF-8";

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));

            String urlRequisicao = "HotSite/CadastroUsuario";

            return await MakeRequisicion<Int32?>(
                HttpMethod.Post
                , urlRequisicao
                , content
                , paramToken
            );
        }

        public async Task<ServiceResult<bool?>> Logoff(
            Int32 paramIdAplicativo
            , CancellationToken paramToken
        )
        {
            List<KeyValuePair<string, string>> requisaoParametros =
                new List<KeyValuePair<string, string>>();

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramAplicativo"
                    , paramIdAplicativo.ToString()
                )
            );

            FormUrlEncodedContent content = new FormUrlEncodedContent(requisaoParametros);

            content.Headers.ContentType = new MediaTypeHeaderValue(_mediaType);

            content.Headers.ContentType.CharSet = "UTF-8";

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));

            String urlRequisicao = "Logoff";

            return await MakeRequisicion<Boolean?>(
                HttpMethod.Post
                , urlRequisicao
                , content
                , paramToken
            );
        }


        public async Task<ServiceResult<bool>> Atualiza_PushKey(
            int paramIdAplicativo
            , String paramPushKey
            , CancellationToken paramToken
        )
        {
            List<KeyValuePair<string, string>> requisaoParametros =
                new List<KeyValuePair<string, string>>();

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "idaplicativo"
                    , paramIdAplicativo.ToString()
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "pushkey"
                    , paramPushKey
                )
            );

            FormUrlEncodedContent content = new FormUrlEncodedContent(requisaoParametros);

            content.Headers.ContentType = new MediaTypeHeaderValue(_mediaType);

            content.Headers.ContentType.CharSet = "UTF-8";

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));

            String urlRequisicao = "login/UpdatePushKey";

            return await MakeRequisicion<Boolean>(
                HttpMethod.Post
                , urlRequisicao
                , content
                , paramToken
            );
        }

        public async Task<ServiceResult<StatusComandoDto>> StatusBloqueio(
            int paramIdTrackedUnitTracker
            , int paramIdTracker
            , CancellationToken paramToken
        )
        {
            String urlRequisicao =
                "Command/" + paramIdTrackedUnitTracker +
                "?paramIdTracker=" + paramIdTracker;

            return await MakeRequisicion<StatusComandoDto>(
                HttpMethod.Get
                , urlRequisicao
                , null
                , paramToken
            );
        }

        public async Task<ServiceResult<MensagemSistemaDto>> RetornaMensagemSistema(Int16 paramIdMensagemSistema,
            CancellationToken paramToken
        )
        {
            String urlRequisicao = "SystemMessage/?paramIdMensagemSistema=" + paramIdMensagemSistema.ToString();

            return await MakeRequisicion<MensagemSistemaDto>(
                HttpMethod.Get
                , urlRequisicao
                , null
                , paramToken
            );
        }

        public async Task<ServiceResult<List<Byte>>> CheckKeepAlive(
            CancellationToken paramToken
        )
        {
            String urlRequisicao = "Synchronism/Check";

            return await MakeRequisicion<List<Byte>>(
                HttpMethod.Get
                , urlRequisicao
                , null
                , paramToken
            );
        }

        public async Task<ServiceResult<String>> RetornaObjetoKeepAlive(Byte paramIdObjetoKeepAlive,
            CancellationToken paramToken
        )
        {
            String urlRequisicao = "Synchronism/?paramIdTipoObjeto=" + paramIdObjetoKeepAlive.ToString();

            return await MakeRequisicion<String>(
                HttpMethod.Get
                , urlRequisicao
                , null
                , paramToken
            );
        }

        public async Task<ServiceResult<Boolean>> AtualizarObjetoKeepAlive(Byte paramIdObjetoKeepAlive,
            CancellationToken paramToken
        )
        {

            List<KeyValuePair<string, string>> requisaoParametros =
                new List<KeyValuePair<string, string>>();

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramIdTipoObjeto"
                    , paramIdObjetoKeepAlive.ToString()
                )
            );

            FormUrlEncodedContent content = new FormUrlEncodedContent(requisaoParametros);

            content.Headers.ContentType = new MediaTypeHeaderValue(_mediaType);

            content.Headers.ContentType.CharSet = "UTF-8";

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));

            String urlRequisicao = "Synchronism";


            return await MakeRequisicion<Boolean>(
                HttpMethod.Post
                , urlRequisicao
                , content
                , paramToken
            );
        }

        public async Task<ServiceResult<String>> GetAplicativoKeepAlive(
           Byte paramIdObjetoKeepAlive,
            CancellationToken paramToken
       )
        {

            String urlRequisicao = "Synchronism/?paramIdTipoObjeto=" + paramIdObjetoKeepAlive.ToString();

            return await MakeRequisicion<String>(
                HttpMethod.Get
                , urlRequisicao
                , null
                , paramToken
            );

        }

        #region Bloqueio

        public async Task<ServiceResult<StatusComandoDto>> ComandoBloqueio(
            int paramIdTracker
            , int paramOrder
            , Int32 paramIdRastreadorUnidadeRastreada
            , String paramLstParametresCommand
            , CancellationToken paramToken
        )
        {
            List<KeyValuePair<string, string>> requisaoParametros =
                new List<KeyValuePair<string, string>>();

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramIdTracker"
                    , paramIdTracker.ToString()
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramOrder"
                    , paramOrder.ToString()
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramIdCommand"
                    , "-1"
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramIdRastreadorUnidadeRastreada"
                    , paramIdRastreadorUnidadeRastreada.ToString()
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramLstParametresCommand"
                    , paramLstParametresCommand
                )
            );

            FormUrlEncodedContent content = new FormUrlEncodedContent(requisaoParametros);

            content.Headers.ContentType = new MediaTypeHeaderValue(_mediaType);

            content.Headers.ContentType.CharSet = "UTF-8";

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));

            String urlRequisicao = "Command";

            return await MakeRequisicion<StatusComandoDto>(
                HttpMethod.Post
                , urlRequisicao
                , content
                , paramToken
            );
        }

        public async Task<ServiceResult<Boolean>> ComandoCancelar(
            int paramIdCommandLog
    , CancellationToken paramToken
)
        {

            _client.DefaultRequestHeaders
                  .Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));

            String urlRequisicao;

            urlRequisicao = "Command?paramIdCommandLog=" + paramIdCommandLog.ToString();

            return await MakeRequisicion<Boolean>(
                HttpMethod.Put
                , urlRequisicao
                , null
                , paramToken
            );

        }

        #endregion

        private async Task<ServiceResult<TEntity>> MakeRequisicion<TEntity>(
            HttpMethod paramMethod
            , String paramEndereco
            , HttpContent paramContent
            , CancellationToken paramToken
        )
        {
            DateTime dataInicio = DateTime.UtcNow;
            TimeSpan WaitTime = TimeSpan.FromSeconds(1);

            ServiceResult<TEntity> result = new ServiceResult<TEntity>();
            RequestResult<TEntity> requestResult = new RequestResult<TEntity>();

            try
            {

                ConfiguraChamadaPadrao();

                String enderecoFinal = String.Format("/api/{0}", paramEndereco);

                HttpRequestMessage request = new HttpRequestMessage(paramMethod, enderecoFinal);

                if (paramContent != null)
                    request.Content = paramContent;

                HttpResponseMessage response = await _client.SendAsync(
                    request
                    , paramToken
                );

                String resultJson = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {

                    requestResult = JsonConvert.DeserializeObject<RequestResult<TEntity>>(resultJson);
                    if (requestResult.Result != null)
                    {
                        result = requestResult.Result;
                    }
                    else
                    {
                        result = JsonConvert.DeserializeObject<ServiceResult<TEntity>>(resultJson);
                    }

                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result.MessageError = AppResources.LoginSenhaInvalido;
                }
                else
                {
                    result.MessageError = response.StatusCode.ToString();
                }
            }
            catch (HttpRequestException ex)
            {
                if (ex.Message == "Error while copying content to a stream.")
                    result.MessageError = AppResources.LoginSenhaInvalido;
                else
                    result.MessageError = "HttpRequestException";

                Crashes.TrackError(ex);
            }
            catch (TaskCanceledException ex)
            {

                Crashes.TrackError(ex);
            }
            catch (Exception ex)
            {
                //result.MessageError = "StoreException";
                result.MessageError = AppResources.LoginSenhaInvalido;


                Crashes.TrackError(ex);
            }

            int totalDemorado = (int)DateTime.UtcNow
                                             .Subtract(dataInicio)
                                             .TotalMilliseconds;
            Int32 tempo = (Int32)(WaitTime.TotalMilliseconds - totalDemorado);

            if (tempo > 0)
            {
                await Task.Delay(tempo);
            }


            return result;
        }


    }
#pragma warning restore CS1998
#pragma warning restore RECS0022
#pragma warning restore CS4014
}