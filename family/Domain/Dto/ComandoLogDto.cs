using System;

namespace family.Domain.Dto
{
	public class ComandoLogDto
    {
        public Int32 IdComandoLog { get; set; }
        public Int32 IdRastreadorUnidadeRastreada { get; set; }
        public Byte Ordem { get; set; }
        public Int32 IdClienteEnvio { get; set; }
        public Byte IdStatusComando { get; set; }
        public Byte IdTipoComando { get; set; }
        public Int32 IdEnvioComandoMultiplo { get; set; }
        public Int32 IdUsuario { get; set; }
        public Int32 IdGateway { get; set; }
        public DateTime DataFila { get; set; }
        public DateTime? DataEnvio { get; set; }
        public DateTime? DataFinalizacao { get; set; }
        public Int32? IdMensagemLog { get; set; }
        public String StringComandoEnviado { get; set; }

        //public ComandoConfiguracaoRastreadorSaidaDto ComandoConfiguracaoRastreadorSaida { get; set; }

    }
}
