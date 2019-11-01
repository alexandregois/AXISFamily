using System;
using family.Domain.Enum;
using Xamarin.Forms;

namespace family.Domain.Dto
{
	public class MenuMapa
	{
		public EnumPage Pagina { get; set; }
		public String Identificacao { get; set; }
		public String Icone { get; set; }
		public int IconeLargura { get; set; }
		public int IconeAltura { get; set; }
		public Button BoxMenu { get; set; }

		#region color
		public Color Color { get; set; }
		public Color ColorBarra { get; set; }
		public Color ColorLoad { get; set; }
		public Color ColorStatusBar { get; set; }
 		#endregion
	}
}
