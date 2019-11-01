using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace family.Domain.Dto
{
    public class ManutencaoBasicaDto
    {
        public Int32 IdUnidadeRastreada { get; set; }
        public Int32? ProximaRevisao { get; set; }
        public Int32? TrocaOleo { get; set; }
        public Int32? RodizioPneu { get; set; }
        public DateTime? ValidadeSeguro { get; set; }
        public String StringValidadeSeguro { get; set; }
    }
}
