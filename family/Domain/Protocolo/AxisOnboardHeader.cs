using System;
using family.Model;
namespace family.Domain.Protocolo
{
	public class AxisOnboardHeader
	{
		public Byte Versao { get; set; }
		public Int32 Modelo { get; set; }
		public Int16 Tipo { get; set; }
		public Int32 Sequencial { get; set; }

		public Int64 IdentificacaoAVL { get; set; }

		public AxisOnboardHeader(){
			Versao = 1;
			Modelo = 782;
			Tipo = 100;
		}

		public Byte[] TransformToArrayByte(){
			ModelReverseBitConverter reverse = new ModelReverseBitConverter();
			Byte[] sequencialBuf = reverse.GetBytes(Sequencial);
			Byte[] tipoBuf = reverse.GetBytes(Tipo);
			Byte[] ModeloBuf = reverse.GetBytes(Modelo);
			Byte[] IdentificacaoAVLBuf = reverse.GetBytes(IdentificacaoAVL);

			Byte[] headerBuf = new Byte[1 + ModeloBuf.Length + IdentificacaoAVLBuf.Length + 
			                            sequencialBuf.Length + tipoBuf.Length];
			headerBuf[0] = Versao;

			ModeloBuf.CopyTo(headerBuf, 1);
			tipoBuf.CopyTo(headerBuf, 1 + ModeloBuf.Length);
			sequencialBuf.CopyTo(headerBuf, 1 + ModeloBuf.Length + tipoBuf.Length);
			IdentificacaoAVLBuf.CopyTo(headerBuf, 1 + ModeloBuf.Length + tipoBuf.Length
			                           + sequencialBuf.Length);
			return headerBuf;
		}
	}
}
