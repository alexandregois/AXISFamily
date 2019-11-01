using System;
namespace family.Domain.Dto
{
    public class AplicativoDto
    {
        public Int32 IdAplicativo
        {
            get;
            set;
        }

        public Int32 Porta
        {
            get;
            set;
        }

        public String IP
        {
            get;
            set;
        }

        public Boolean IsLocator
        {
            get;
            set;
        }
        public string Identificacao { get; set; }

        public Int32 IntervaloComunicacaoMovimento { get; set; }

        public Boolean IsDeleted { get; set; }

        public Boolean HasSolicitacaoRastreamento { get; set; }

        public Boolean IsClienteAtivo { get; set; }
        public Boolean IsClienteBloqueado { get; set; }
        public Boolean IsPessoaAtivo { get; set; }
        public String Logo { get; set; }
    }
}
