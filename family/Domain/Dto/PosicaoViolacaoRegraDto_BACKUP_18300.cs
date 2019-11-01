using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Realms;

namespace family.Domain.Dto
{
<<<<<<< HEAD
    public class PosicaoViolacaoRegraDto: RealmObject
=======
    public class PosicaoViolacaoRegraDto : RealmObject
>>>>>>> 2018-03
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
