using System;
using CoreLocation;

namespace family.iOS.Services
{
	public class PositionController: CLLocationManagerDelegate
	{
		CLLocationManager locationManager;

		public PositionController()
		{
			locationManager = new CLLocationManager();
			locationManager.Delegate = this;

			AppDelegate.LocationManager = new GetBestPosition();
			locationManager.StartMonitoringSignificantLocationChanges();
		}
	}
}
