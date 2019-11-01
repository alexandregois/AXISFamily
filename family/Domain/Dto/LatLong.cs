using System;

namespace family.Domain.Dto
{
	public class LatLong
	{
		public Double Latitude { get; set; }
		public Double Longitude { get; set; }

		public LatLong(Double paramLatitude, Double paramLongitude)
		{
			Latitude = paramLatitude;
			Longitude = paramLongitude;
		}

	}
}
