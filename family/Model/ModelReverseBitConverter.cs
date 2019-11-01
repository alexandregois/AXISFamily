using System;
namespace family.Model
{
	public class ModelReverseBitConverter
	{
		public Int16 ToInt16(Byte[] Value, Int32 StartIndex)
		{
			return (Int16)ToUInt16(Value, StartIndex);
		}

		public UInt16 ToUInt16(Byte[] Value, Int32 StartIndex)
		{
			UInt32 Result = BitConverter.ToUInt16(Value, StartIndex);

			return (UInt16)((0x00FF) & (Result >> 8) | (0xFF00) & (Result << 8));
		}

		public Int32 ToInt32(Byte[] Value, Int32 StartIndex)
		{
			return (Int32)ToUInt32(Value, StartIndex);
		}

		public Int64 ToInt64(Byte[] Value, Int32 StartIndex)
		{
			return (Int64)ToUInt64(Value, StartIndex);
		}

		public UInt32 ToUInt32(Byte[] Value, Int32 StartIndex)
		{
			UInt32 Result = BitConverter.ToUInt32(Value, StartIndex);

			return ((0x000000FF) & (Result >> 24) |
			        (0x0000FF00) & (Result >> 8) |
			        (0x00FF0000) & (Result << 8) |
			        (0xFF000000) & (Result << 24));
		}

		//public Double ToDouble(Byte[] Value, Int32 StartIndex, Int32 Length)
		//{
		//    Byte[] aux = new Byte[8];

		//    for (int i = 0; i < Length; i++)
		//    {
		//         aux[7-i] = Value[StartIndex+i];
		//    }
		//    return BitConverter.ToDouble(aux, 0);
		//}

		public UInt64 ToUInt64(Byte[] Value, Int32 StartIndex)
		{
			UInt64 Result = BitConverter.ToUInt64(Value, StartIndex);

			return (Result & 0x00000000000000FF) << 56 |
				(Result & 0x000000000000FF00) << 40 |
				(Result & 0x0000000000FF0000) << 24 |
				(Result & 0x00000000FF000000) << 8 |
				(Result & 0x000000FF00000000) >> 8 |
				(Result & 0x0000FF0000000000) >> 24 |
				(Result & 0x00FF000000000000) >> 40 |
				(Result & 0xFF00000000000000) >> 56;
		}

		public Byte[] GetBytes(Double Word)
		{
			Byte[] Result = new Byte[2];

			Result = BitConverter.GetBytes(Word);

			Array.Reverse(Result);

			return Result;
		}

		public Byte[] GetBytes(Int16 Word)
		{
			Byte[] Result = new Byte[2];

			Result = BitConverter.GetBytes(Word);

			Array.Reverse(Result);

			return Result;
		}

		public Byte[] GetBytes(UInt16 Word)
		{
			Byte[] Result = new Byte[2];

			Result = BitConverter.GetBytes(Word);

			Array.Reverse(Result);

			return Result;
		}

		public Byte[] GetBytes(UInt32 Word)
		{
			Byte[] Result = new Byte[4];

			Result = BitConverter.GetBytes(Word);

			Array.Reverse(Result);

			return Result;
		}

		public Byte[] GetBytes(Int32 Word)
		{
			Byte[] Result = new Byte[4];

			Result = BitConverter.GetBytes(Word);

			Array.Reverse(Result);

			return Result;
		}

		public Byte[] GetBytes(Int64 Word)
		{
			Byte[] Result = new Byte[8];

			Result = BitConverter.GetBytes(Word);

			Array.Reverse(Result);

			return Result;
		}
	}
}
