using System;
using System.Collections.Generic;
using System.Linq;
using family.Domain.Dto;
using family.Domain.Enum;
using Realms;

namespace family.Domain.Realm
{
    public class PosicaoUnidadeRastreadaRealm : RealmObject
    {
        #region Posicao
        [PrimaryKey]
        public Int32 IdRastreador { get; set; }
        public Int64 IdPosicao { get; set; }
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }
        public DateTimeOffset DataEvento { get; set; }
        public String Endereco { get; set; }
        public Double? Velocidade { get; set; }
        public Boolean? Ignicao { get; set; }
        public Boolean? GPSValido { get; set; }
        public Int32? IdRegraPrioritaria { get; set; }
        public Double? BateriaPrincipal { get; set; }
        public Double? BateriaBackup { get; set; }
        public DateTimeOffset? DataAtualizacao { get; set; }
        public String Evento { get; set; }
        public Double? SinalGPRS { get; set; }
        public Double? Odometro { get; set; }
        public Byte OrdemRastreador { get; set; }

        public Int16 IdUnidadeMedidaBateriaPrincipal { get; set; }

        #endregion

        #region UnidadeRastreada
        public Int32 IdUnidadeRastreada { get; set; }
        public Int32 IdRastreadorUnidadeRastreada { get; set; }
        public Byte IdTipoUnidadeRastreada { get; set; }
        public String Identificacao { get; set; }
        #endregion

        #region Ancora
        public Double? Ancora_Latitude { get; set; }
        public Double? Ancora_Longitude { get; set; }
        public Int32? Ancora_Tolerancia { get; set; }
        #endregion

        public IList<PosicaoViolacaoRegraDto> ListaViolacaoRegra { get; }


        #region Elementos Calculados
        [Ignored]
        public String IdentificacaoFinal
        {
            get
            {
                return String.Format(
                    "{0} ({1})"
                    , Identificacao
                    , Convert.ToInt32(OrdemRastreador) + 1
                //, OrdemRastreador
                );
            }
        }

        [Ignored]
        public String PathImage
        {
            get
            {
                string temp = string.Empty;

                if (IconePadrao != null)
                    temp = IconePadrao;
                else
                {
                    temp = "ic_car2_cinza.png";

                    switch (this.IdTipoUnidadeRastreada)
                    {
                        case (Byte)EnumTipoUnidadeRastreada.Pessoa:
                            temp = "ic_celular_cinza.png";
                            break;
                    }

                }


                return temp;

            }
        }

        //Largura máxima 31
        [Ignored]
        public Double PathImageWidth
        {
            get
            {
                Double temp = 31;
                switch (this.IdTipoUnidadeRastreada)
                {
                    case 2:
                        temp = 17;
                        break;
                }
                return temp;
            }
        }

        //Altura máxima 33
        [Ignored]
        public Double PathImageHeight
        {
            get
            {
                Double temp = 22;
                switch (this.IdTipoUnidadeRastreada)
                {
                    case 2:
                        temp = 33;
                        break;
                }
                return temp;
            }
        }

        [Ignored]
        public String Icone
        {
            get
            {
                string temp = "pin_ultima_posicao_carro.png";
                switch (this.IdTipoUnidadeRastreada)
                {
                    case 2:
                        temp = "pin_ultima_posicao_cel.png";
                        break;
                }
                return temp;
            }
        }

        [Ignored]
        public String StringVelocidade
        {
            get
            {
                string temp = "";
                if (this.Velocidade.HasValue)
                    temp = this.Velocidade.Value.ToString();
                return temp + " Km/h";
            }
        }

        [Ignored]
        public String StringDataEvento_Posicao
        {
            get
            {
                string temp = this.DataEvento.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss");
                return temp;
            }
        }

        [Ignored]
        public String StringDataAtualizacao
        {
            get
            {
                string temp = "";
                if (this.DataAtualizacao.HasValue)
                    temp = this.DataAtualizacao.Value.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss");
                return temp;
            }
        }

        public string IconUrl { get; set; }
        public string IconePadrao { get; set; }

        #endregion



        public void Transform(PosicaoUnidadeRastreada paramPosi)
        {
            IdRastreador = paramPosi.IdRastreador;
            IdPosicao = paramPosi.IdPosicao;
            Latitude = paramPosi.Latitude.Value;
            Longitude = paramPosi.Longitude.Value;
            DataEvento = paramPosi.DataEvento.Value;
            Endereco = paramPosi.Endereco;
            Velocidade = paramPosi.Velocidade;
            Ignicao = paramPosi.Ignicao;
            GPSValido = paramPosi.GPSValido;
            IdRegraPrioritaria = paramPosi.IdRegraPrioritaria;
            BateriaPrincipal = paramPosi.BateriaPrincipal;
            BateriaBackup = paramPosi.BateriaBackup;
            DataAtualizacao = paramPosi.DataAtualizacao;
            Evento = paramPosi.Evento;
            SinalGPRS = paramPosi.SinalGPRS;
            Odometro = paramPosi.Odometro;
            OrdemRastreador = paramPosi.OrdemRastreador;

            IdUnidadeMedidaBateriaPrincipal = paramPosi.IdUnidadeMedidaBateriaPrincipal;

            IdUnidadeRastreada = paramPosi.IdUnidadeRastreada.Value;
            IdRastreadorUnidadeRastreada = paramPosi.IdRastreadorUnidadeRastreada.Value;
            IdTipoUnidadeRastreada = paramPosi.IdTipoUnidadeRastreada.Value;
            Identificacao = paramPosi.Identificacao;

            Ancora_Latitude = paramPosi.Ancora_Latitude;
            Ancora_Longitude = paramPosi.Ancora_Longitude;
            Ancora_Tolerancia = paramPosi.Ancora_Tolerancia;

            //ListaViolacaoRegra = paramPosi.ListaViolacaoRegra;

            IconUrl = paramPosi.IconUrl;
            IconePadrao = paramPosi.IconePadrao;


        }

    }
}
