using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace family.Domain.Dto
{
    public class RetornoSolicitacaoRastreamentoDto
    {
        public Boolean IsAutorizadoAutomaticamente { get; set; }
        public Boolean IsGestorConta { get; set; }
        public AplicativoDto Aplicativo { get; set; }
        public String Mensagem { get; set; }
    }

}
