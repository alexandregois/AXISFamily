using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using family.Domain;
using family.Domain.Dto;
using family.Services.Interfaces;
using Microsoft.AppCenter.Crashes;

namespace family.Services
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
    public class MockDataStore : IDataStore
    {
        private App _app { get; set; }

        private List<PosicaoUnidadeRastreada> _lstPosicaoUnidadeRastreada { get; set; }

        private List<TelefoneContatoDto> _lstTelefone { get; set; }

        private List<PosicaoHistorico> _lstPosicaoHistorico { get; set; }
        private List<PontoControle> _lstPontoControle { get; set; }

        private Int32 delay => 2000;

        public MockDataStore()
        {
            #region PosicaoUnidadeRastreada
            _lstPosicaoUnidadeRastreada = new List<PosicaoUnidadeRastreada>();
            Int32 pos = 0;
            for (Int32 i = 0; i < 1; i++)
            {
                _lstPosicaoUnidadeRastreada.Add(new PosicaoUnidadeRastreada()
                {
                    IdPosicao = i + pos,
                    Latitude = -22.889815783402938,
                    Longitude = -43.115254640579224,
                    DataEvento = new DateTime(2017, 05, 05, 17, 05, 29),
                    Endereco = "Avenida Marquês do Paraná, Centro, Niterói, RJ, Brasil, 24030-060",
                    Velocidade = 50,
                    IdRegraPrioritaria = 5,
                    IdUnidadeRastreada = 5,
                    IdTipoUnidadeRastreada = 2,
                    Identificacao = "Camila de Jesus Ferreira",
                    IdRastreadorUnidadeRastreada = 1,
                    IdRastreador = 0
                });

                pos++;
                _lstPosicaoUnidadeRastreada.Add(new PosicaoUnidadeRastreada()
                {
                    IdPosicao = i + pos,
                    Latitude = -22.883074735421907,
                    Longitude = -43.070075511932373,
                    DataEvento = new DateTime(2017, 05, 19, 19, 41, 18),
                    Endereco = "Rodovia Niterói-Manilha, Caramujo, Niterói, RJ, Brasil, 24120191",
                    Velocidade = 99,
                    Ignicao = true,
                    GPSValido = true,
                    IdRegraPrioritaria = 23,
                    IdUnidadeRastreada = 9,
                    IdTipoUnidadeRastreada = 1,
                    Identificacao = "New Eco Sport",
                    IdRastreadorUnidadeRastreada = 2,
                    IdRastreador = 2
                });

                pos++;
                _lstPosicaoUnidadeRastreada.Add(new PosicaoUnidadeRastreada()
                {
                    IdPosicao = i + pos,
                    Latitude = -22.9065977589198,
                    Longitude = -43.0944192409515,
                    DataEvento = new DateTime(2017, 04, 25, 20, 08, 41),
                    Endereco = "Rua Desembargador Aires Itabaiana, Vital Brazil, " +
                        "Niterói, RJ, Brasil, 24241-000",
                    Velocidade = 75,
                    IdRegraPrioritaria = 0,
                    IdUnidadeRastreada = 2,
                    IdTipoUnidadeRastreada = 1,
                    Identificacao = "Uno Way",
                    IdRastreadorUnidadeRastreada = 3,
                    IdRastreador = 3
                });

                pos++;
                _lstPosicaoUnidadeRastreada.Add(new PosicaoUnidadeRastreada()
                {
                    IdPosicao = i + pos,
                    Latitude = -22.9005296045921,
                    Longitude = -43.112293481826782,
                    DataEvento = new DateTime(2017, 05, 29, 18, 43, 53),
                    Endereco = "Rua Fagundes Varela, Ingá, Niterói, RJ, Brasil, 24220-008",
                    Velocidade = 75,
                    Ignicao = false,
                    GPSValido = false,
                    IdRegraPrioritaria = 0,
                    IdUnidadeRastreada = 1000078,
                    IdTipoUnidadeRastreada = 2,
                    Identificacao = "Marco Antonio  José Nunes Malvessi",
                    IdRastreadorUnidadeRastreada = 4,
                    IdRastreador = 4
                });

                pos++;
            }
            #endregion

            #region Posição Histórico
            _lstPosicaoHistorico = new List<PosicaoHistorico>();

            _lstPosicaoHistorico.Add(new PosicaoHistorico()
            {
                IdPosicao = 1,
                Velocidade = 1,
                DataEvento = DateTime.UtcNow,
                IdTipoUnidadeRastreada = 2,
                Latitude = -22.889815783402938,
                Longitude = -43.115254640579224,
                Total = 5
            });

            _lstPosicaoHistorico.Add(new PosicaoHistorico()
            {
                IdPosicao = 2,
                Velocidade = 2,
                DataEvento = new DateTime(2017, 05, 19, 19, 41, 18),
                IdTipoUnidadeRastreada = 2,
                Latitude = -22.883074735421907,
                Longitude = -43.070075511932373,
                Total = 5
            });

            _lstPosicaoHistorico.Add(new PosicaoHistorico()
            {
                IdPosicao = 3,
                Velocidade = 3,
                DataEvento = new DateTime(2017, 04, 25, 20, 08, 41),
                IdTipoUnidadeRastreada = 2,
                Latitude = -22.9065977589198,
                Longitude = -43.0944192409515,
                Total = 5
            });

            _lstPosicaoHistorico.Add(new PosicaoHistorico()
            {
                IdPosicao = 4,
                Velocidade = 4,
                DataEvento = new DateTime(2017, 05, 29, 18, 43, 53),
                IdTipoUnidadeRastreada = 2,
                Latitude = -22.9005296045921,
                Longitude = -43.112293481826782,
                Total = 5
            });

            _lstPosicaoHistorico.Add(new PosicaoHistorico()
            {
                IdPosicao = 5,
                Velocidade = 5,
                DataEvento = new DateTime(2017, 05, 29, 18, 43, 53),
                IdTipoUnidadeRastreada = 2,
                Latitude = -22.9035225,
                Longitude = -43.1064638,
                Total = 5
            });
            #endregion

            #region Ponto de Controle
            _lstPontoControle = new List<PontoControle>();

            _lstPontoControle.Add(new PontoControle()
            {
                IdGeography = 212,
                NomePonto = "Ponto de Controle: Teste ponto leo",
                Tolerancia = 300,
                Latitude = -22.8709558220868,
                Longitude = -43.117561340332
            });

            _lstPontoControle.Add(new PontoControle()
            {
                IdGeography = 215,
                NomePonto = "Ponto de Controle: Teste leo 4",
                Tolerancia = 300,
                Latitude = -22.923931022934,
                Longitude = -43.1216812133789
            });

            _lstPontoControle.Add(new PontoControle()
            {
                IdGeography = 214,
                NomePonto = "Ponto de Controle: teste leo 3",
                Tolerancia = 500,
                Latitude = -22.8623355127308,
                Longitude = -43.1052017211914
            });

            _lstPontoControle.Add(new PontoControle()
            {
                IdGeography = 213,
                NomePonto = "Ponto de Controle: teste leo 2",
                Tolerancia = 300,
                Latitude = -22.871193070573,
                Longitude = -43.1071758270264
            });

            _lstPontoControle.Add(new PontoControle()
            {
                IdGeography = 120,
                NomePonto = "Ponto de Controle: 20170612",
                Tolerancia = 109,
                Latitude = -22.870876739166,
                Longitude = -43.0957818031311
            });

            _lstPontoControle.Add(new PontoControle()
            {
                IdGeography = 112,
                NomePonto = "Ponto de Controle: 201706051142-Teste Ponto de controle",
                Tolerancia = 126,
                Latitude = -22.8900134630412,
                Longitude = -43.1152439117432
            });

            _lstPontoControle.Add(new PontoControle()
            {
                IdGeography = 105,
                NomePonto = "Ponto de Controle: 201706021720",
                Tolerancia = 300,
                Latitude = -22.8504718075893,
                Longitude = -43.0951595306396
            });

            _lstPontoControle.Add(new PontoControle()
            {
                IdGeography = 104,
                NomePonto = "Ponto de Controle: 201706021602",
                Tolerancia = 361,

                Latitude = -22.8833712694987,
                Longitude = -43.1157159805298
            });
            #endregion
        }

        public async Task<ServiceResult<List<PosicaoUnidadeRastreada>>> BuscarUnidadeRastreadaRestLista(
            CancellationToken paramToken
        )
        {
            await Task.Delay(delay);

            ServiceResult<List<PosicaoUnidadeRastreada>> result =
                new ServiceResult<List<PosicaoUnidadeRastreada>>();

            try
            {
                TokenDto tempToken = new TokenDto()
                {
                    Access_Token = "Bl7Chqvyob2lAI8iAeuyiS+85pFwKTt/wl8ytytiS7eGrRp",
                    LstFuncao = new List<Int32>() { 361, 385, 386, 387, 388, 389, 390,
                        391, 392, 393, 394, 395, 396, 397 },
                    TempoTransmissao = 180,
                    Aplicativo = new AplicativoDto()
                    {
                        IdAplicativo = 1,
                        IP = "10.10.3.13",
                        IsLocator = false,
                        Porta = 8002,
                        Identificacao = "1"
                    }
                };
                result.RefreshToken = tempToken;
                result.Data = _lstPosicaoUnidadeRastreada;
            }
            catch (HttpRequestException)
            {
                result.MessageError = "HttpRequestException";
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }

            return result;
        }

        public async Task<ServiceResult<List<TelefoneContatoDto>>> BuscarTelefoneRestLista(
            CancellationToken paramToken
        )
        {
            await Task.Delay(delay);

            ServiceResult<List<TelefoneContatoDto>> result =
                new ServiceResult<List<TelefoneContatoDto>>();

            try
            {
                TokenDto tempToken = new TokenDto()
                {
                    Access_Token = "Bl7Chqvyob2lAI8iAeuyiS+85pFwKTt/wl8ytytiS7eGrRp",
                    LstFuncao = new List<Int32>() { 361, 385, 386, 387, 388, 389, 390,
                        391, 392, 393, 394, 395, 396, 397 },
                    TempoTransmissao = 180,
                    Aplicativo = new AplicativoDto()
                    {
                        IdAplicativo = 1,
                        IP = "10.10.3.13",
                        IsLocator = false,
                        Porta = 8002,
                        Identificacao = "1"
                    }
                };
                result.RefreshToken = tempToken;
                result.Data = _lstTelefone;

            }
            catch (HttpRequestException)
            {
                result.MessageError = "HttpRequestException";
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }

            return result;
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

            ServiceResult<TokenDto> result = new ServiceResult<TokenDto>();

            await Task.Delay(delay);
            try
            {
                result.Data = new TokenDto()
                {
                    Access_Token = "Bl7Chqvyob2lAI8iAeuyiS+85pFwKTt/wl8ytytiS7eGrRp",
                    expires_in = "300",
                    LstFuncao = new List<Int32>() { 361, 385, 386, 387, 388, 389, 390,
                        391, 392, 393, 394, 395, 396, 397 },
                    TempoTransmissao = 20,
                    Aplicativo = new AplicativoDto()
                    {
                        IdAplicativo = 1,
                        IP = "10.10.3.13",
                        IsLocator = true,
                        Porta = 8002,
                        Identificacao = "1"
                    },
                    LastPositions = _lstPosicaoUnidadeRastreada
                };
            }
            catch (HttpRequestException)
            {
                result.MessageError = "HttpRequestException";
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }

            return result;
        }

        public async Task<ServiceResult<List<PosicaoHistorico>>> BuscarHistoricoPosicao(
            Int32 paramIdRastreadorUnidadeRastreada
            , Byte paramOrdemRastreador
            , String paramInitialPeriod
            , String paramFinalPeriod
            , Int32 paramStartRowIndex
            , Int32 paramPageSize
            , Boolean paramIsReverse
            , CancellationToken paramToken
        )
        {
            await Task.Delay(delay);
            ServiceResult<List<PosicaoHistorico>> result = new ServiceResult<List<PosicaoHistorico>>();

            try
            {
                List<PosicaoHistorico> tempData = new List<PosicaoHistorico>();
                for (Int32 i = 0; i < paramPageSize; i += (paramPageSize / 2))
                {
                    PosicaoHistorico temp0 = _lstPosicaoHistorico[0];
                    temp0.Velocidade += i;
                    tempData.Add(temp0);

                    PosicaoHistorico temp1 = _lstPosicaoHistorico[1];
                    temp1.Velocidade += i;
                    tempData.Add(temp1);

                    PosicaoHistorico temp2 = _lstPosicaoHistorico[2];
                    temp2.Velocidade += i;
                    tempData.Add(temp2);

                    PosicaoHistorico temp3 = _lstPosicaoHistorico[3];
                    temp3.Velocidade += i;
                    tempData.Add(temp3);

                    PosicaoHistorico temp4 = _lstPosicaoHistorico[4];
                    temp4.Velocidade += i;
                    tempData.Add(temp4);
                }

                result.Data = tempData;
            }
            catch (HttpRequestException)
            {
                result.MessageError = "HttpRequestException";
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
            await Task.Delay(delay);

            ServiceResult<PosicaoUnidadeRastreada> result =
                new ServiceResult<PosicaoUnidadeRastreada>();

            try
            {
                result.Data = _lstPosicaoUnidadeRastreada[Convert.ToInt32(paramIdPosicao)];
            }
            catch (HttpRequestException)
            {
                result.MessageError = "HttpRequestException";
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }

            return result;
        }

        public async Task<ServiceResult<Boolean?>> InsertPontoControle(
            PontoControle paramPontoControle
            , CancellationToken paramToken
        )
        {
            await Task.Delay(delay);

            ServiceResult<Boolean?> result = new ServiceResult<Boolean?>();

            try
            {
                result.Data = true;
            }
            catch (HttpRequestException)
            {
                result.MessageError = "HttpRequestException";
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }

            return result;
        }

        public async Task<ServiceResult<Boolean?>> UpdatePontoControle(
            PontoControle paramPontoControle
            , CancellationToken paramToken
        )
        {
            await Task.Delay(delay);

            ServiceResult<Boolean?> result = new ServiceResult<Boolean?>();

            try
            {
                result.Data = true;
            }
            catch (HttpRequestException)
            {
                result.MessageError = "HttpRequestException";
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }

            return result;
        }

        public async Task<ServiceResult<Boolean?>> DeletePontoControle(
            int value
            , CancellationToken paramToken
        )
        {
            await Task.Delay(delay);
            ServiceResult<Boolean?> result = new ServiceResult<Boolean?>();
            try
            {
                result.Data = true;
            }
            catch (HttpRequestException)
            {
                result.MessageError = "HttpRequestException";
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }

            return result;
        }


        public async Task<ServiceResult<List<PontoControle>>> ListaPontoControle(
            CancellationToken paramToken
        )
        {
            await Task.Delay(delay);
            ServiceResult<List<PontoControle>> result = new ServiceResult<List<PontoControle>>();

            try
            {
                result.Data = _lstPontoControle;
            }
            catch (HttpRequestException)
            {
                result.MessageError = "HttpRequestException";
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }

            return result;
        }

        public async Task<ServiceResult<ManutencaoBasicaDto>> ListaManutencao(ManutencaoBasicaDto paramManutencao,
            CancellationToken paramToken
        )
        {

            ManutencaoBasicaDto paramManutencaoBasicaDto = new ManutencaoBasicaDto();
            paramManutencaoBasicaDto.TrocaOleo = 55000;
            paramManutencaoBasicaDto.ProximaRevisao = 65000;
            paramManutencaoBasicaDto.RodizioPneu = 65000;
            paramManutencaoBasicaDto.ValidadeSeguro = Convert.ToDateTime("28/04/2018");

            await Task.Delay(delay);
            ServiceResult<ManutencaoBasicaDto> result = new ServiceResult<ManutencaoBasicaDto>();

            try
            {
                result.Data = paramManutencaoBasicaDto;
            }
            catch (HttpRequestException)
            {
                result.MessageError = "HttpRequestException";
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }

            return result;
        }

        public async Task<ServiceResult<ManutencaoBasicaDto>> UpdateManutencao(ManutencaoBasicaDto paramManutencao,
           CancellationToken paramToken
       )
        {

            ManutencaoBasicaDto paramManutencaoBasicaDto = new ManutencaoBasicaDto();
            paramManutencaoBasicaDto.TrocaOleo = 55000;
            paramManutencaoBasicaDto.ProximaRevisao = 65000;
            paramManutencaoBasicaDto.RodizioPneu = 65000;
            paramManutencaoBasicaDto.ValidadeSeguro = Convert.ToDateTime("28/04/2018");

            await Task.Delay(delay);
            ServiceResult<ManutencaoBasicaDto> result = new ServiceResult<ManutencaoBasicaDto>();

            try
            {
                result.Data = paramManutencaoBasicaDto;
            }
            catch (HttpRequestException)
            {
                result.MessageError = "HttpRequestException";
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }

            return result;
        }

        public async Task<ServiceResult<AncoraAtivacaoDto>> AtivarAncora(
            int? idUnidadeRastreada
            , string paramLatitude
            , string paramLongitude
            , int paramTolerancia
            , CancellationToken paramToken
        )
        {
            await Task.Delay(delay);

            ServiceResult<AncoraAtivacaoDto> result = new ServiceResult<AncoraAtivacaoDto>();

            try
            {
                result.Data = new AncoraAtivacaoDto();
            }
            catch (HttpRequestException)
            {
                result.MessageError = "HttpRequestException";
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }

            return result;
        }

        public async Task<ServiceResult<int?>> DesativarAncora(
            int? idUnidadeRastreada
            , CancellationToken paramToken
        )
        {
            await Task.Delay(delay);
            ServiceResult<int?> result = new ServiceResult<int?>();
            try
            {
                result.Data = 0;
            }
            catch (HttpRequestException)
            {
                result.MessageError = "HttpRequestException";
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }

            return result;
        }

        public async Task<ServiceResult<RetornoSolicitacaoRastreamentoDto>> RastrearDispositivo(
            String paramEmail
            , Int32 paramIdAplicativo
            , String paramImei
            , CancellationToken paramToken
        )
        {
            await Task.Delay(delay);
            ServiceResult<RetornoSolicitacaoRastreamentoDto> result = new ServiceResult<RetornoSolicitacaoRastreamentoDto>();
            try
            {
                result.Data.Aplicativo.IsLocator = true;
            }
            catch (HttpRequestException)
            {
                result.MessageError = "HttpRequestException";
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }
            return result;
        }

        public async Task<ServiceResult<PosicaoUnidadeRastreada>> BuscarUltimaPosicao(
            int idRastreadorUnidadeRastreada
            , CancellationToken paramToken
        )
        {
            await Task.Delay(delay);
            ServiceResult<PosicaoUnidadeRastreada> result =
                new ServiceResult<PosicaoUnidadeRastreada>();
            try
            {
                Random number = new Random();
                result.Data =
                          _lstPosicaoUnidadeRastreada[number.Next(0, (_lstPosicaoUnidadeRastreada.Count - 1))];
            }
            catch (HttpRequestException)
            {
                result.MessageError = "HttpRequestException";
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }
            return result;
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
            await Task.Delay(delay);
            ServiceResult<int?> result = new ServiceResult<int?>();

            try
            {
                result.Data = 0;
            }
            catch (HttpRequestException)
            {
                result.MessageError = "HttpRequestException";
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }
            return result;
        }

        public async Task<ServiceResult<bool?>> Logoff(
            Int32 paramIdAplicativo
            , CancellationToken paramToken
        )
        {
            await Task.Delay(delay);
            ServiceResult<bool?> result = new ServiceResult<bool?>();

            try
            {
                result.Data = null;
            }
            catch (HttpRequestException)
            {
                result.MessageError = "HttpRequestException";
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }
            return result;
        }

        public async Task<ServiceResult<bool>> Atualiza_PushKey(
            int paramIdAplicativo
            , string paramPushKey
            , CancellationToken paramToken
        )
        {
            await Task.Delay(delay);
            ServiceResult<bool> result = new ServiceResult<bool>();

            try
            {
                result.Data = true;
            }
            catch (HttpRequestException)
            {
                result.MessageError = "HttpRequestException";
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }

            return result;
        }

        public async Task<ServiceResult<bool>> RetornaCompra_Realizada(
            CancellationToken paramToken
        )
        {
            await Task.Delay(delay);
            ServiceResult<bool> result = new ServiceResult<bool>();

            try
            {
                result.Data = true;
            }
            catch (HttpRequestException)
            {
                result.MessageError = "HttpRequestException";
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }

            return result;
        }

        public async Task<ServiceResult<bool>> Grava_CompraRealizada(
            CancellationToken paramToken
        )
        {
            await Task.Delay(delay);
            ServiceResult<bool> result = new ServiceResult<bool>();

            try
            {
                result.Data = true;
            }
            catch (HttpRequestException)
            {
                result.MessageError = "HttpRequestException";
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }

            return result;
        }

        public async Task<ServiceResult<StatusComandoDto>> StatusBloqueio(
            int paramIdTrackedUnitTracker
            , int paramIdTracker
            , CancellationToken paramToken
        )
        {
            await Task.Delay(delay);
            ServiceResult<StatusComandoDto> result = new ServiceResult<StatusComandoDto>();

            try
            {
                result.Data = new StatusComandoDto()
                {
                    IdStatusComando = 9,
                    IsBloqueado = false
                };
            }
            catch (HttpRequestException)
            {
                result.MessageError = "HttpRequestException";
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }

            return result;
        }

        public async Task<ServiceResult<StatusComandoDto>> ComandoBloqueio(
            int paramIdTracker
            , int paramIdRastreadorUnidadeRastreada
            , int paramOrder
            , String paramLstParametresCommand
            , CancellationToken paramToken
        )
        {
            await Task.Delay(delay);
            ServiceResult<StatusComandoDto> result = new ServiceResult<StatusComandoDto>();

            return result;
        }

        public async Task<ServiceResult<Boolean>> ComandoCancelar(
            int paramIdCommandLog
            , CancellationToken paramToken
        )
        {
            await Task.Delay(delay);
            ServiceResult<Boolean> result = new ServiceResult<Boolean>();

            return result;
        }

        public async Task<ServiceResult<List<Byte>>> CheckKeepAlive(
            CancellationToken paramToken
        )
        {
            ServiceResult<List<Byte>> result = new ServiceResult<List<Byte>>();

            Byte parametroKeep1 = new Byte();
            parametroKeep1 = 3;

            Byte parametroKeep2 = new Byte();
            parametroKeep2 = 15;

            result.Data.Add(parametroKeep1);
            result.Data.Add(parametroKeep2);


            return result;

        }

        public async Task<ServiceResult<String>> GetAplicativoKeepAlive(
           Byte paramIdObjetoKeepAlive,
            CancellationToken paramToken
)
        {
            ServiceResult<String> result = new ServiceResult<String>();

            return result;
        }

        public async Task<ServiceResult<String>> RetornaObjetoKeepAlive(Byte paramIdObjetoKeepAlive,
            CancellationToken paramToken
        )
        {
            ServiceResult<String> result = new ServiceResult<String>();

            return result;
        }

        public async Task<ServiceResult<Boolean>> AtualizarObjetoKeepAlive(Byte paramIdObjetoKeepAlive,
            CancellationToken paramToken
        )
        {
            ServiceResult<Boolean> result = new ServiceResult<Boolean>();
            result.Data = true;

            return result;
        }

        public async Task<ServiceResult<MensagemSistemaDto>> RetornaMensagemSistema(Int16 paramIdMensagemSistema,
            CancellationToken paramToken
        )
        {
            ServiceResult<MensagemSistemaDto> result = new ServiceResult<MensagemSistemaDto>();

            return result;
        }
    }
#pragma warning restore CS1998
#pragma warning restore RECS0022
#pragma warning restore CS4014
}