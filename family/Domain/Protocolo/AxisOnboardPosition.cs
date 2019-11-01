using System;
using System.Collections.Generic;

namespace family.Domain.Protocolo
{
	public class AxisOnboardPosition
	{
		//public Int32? IdPes { get; set; }
		public Double Lat { get; set; }
		public Double Long { get; set; }
		//public Int64 DtGPS { get; set; }
		public Int64 DtEvt { get; set; }
		public Int32 IdEvt { get; set; }
		public Int64 In { get; set; }
		public Int32 Out { get; set; }
		public Dictionary<Byte, Double> ListTel { get; set; }

        //TELEMETRIA TXT
        public Dictionary<Byte, String> ListTelTxt { get; set; }

        //public List<Int32> ListVR { get; set; }
        //public List<LBSInfo> ListLBS { get; set; }
    }
}
