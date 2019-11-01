using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Realms;

namespace family.Domain.Dto
{
    public class PosicaoViolacaoRegraDto : RealmObject
    {
        //Regra
        public Int32 IdRegra { get; set; }
        public Int32 IdCliente { get; set; }
        public Byte Prioridade { get; set; }
        public String CodigoIntegracao { get; set; }
        public String NomeRegra { get; set; }
        public String Cor { get; set; }
    }
}
