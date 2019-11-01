using System;
namespace family.Domain.Protocolo
{
	public class LBSInfo
	{
		public Int32 MCC { get; set; }
		public Int32 MNC { get; set; }
		public Int32 LAC { get; set; }
		public Int32 CellId { get; set; }
		public Byte? RSSI { get; set; }
	}
}
