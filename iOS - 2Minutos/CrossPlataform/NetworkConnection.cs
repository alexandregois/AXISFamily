using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CoreFoundation;
using family.CrossPlataform;
using family.iOS.CrossPlataform;
using Foundation;
using Plugin.Connectivity;
using SystemConfiguration;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(NetworkConnection))]
namespace family.iOS.CrossPlataform
{
	public class NetworkConnection : INetworkConnection
	{
		public bool IsConnected { get; set; }
		public String SSIDName { get; set; }

		public void CheckNetworkConnection()
		{
			InternetConnectionStatus();
		}

		private void UpdateNetworkStatus()
		{
			if (InternetConnectionStatus())
			{
				IsConnected = true;
			}
			else if (LocalWifiConnectionStatus())
			{
				IsConnected = true;
			}
			else
			{
				IsConnected = false;
			}
		}

		private event EventHandler ReachabilityChanged;
		private void OnChange(NetworkReachabilityFlags flags)
		{
			var h = ReachabilityChanged;
			if (h != null)
				h(null, EventArgs.Empty);
		}

		private NetworkReachability defaultRouteReachability;
		private bool IsNetworkAvailable(out NetworkReachabilityFlags flags)
		{
			if (defaultRouteReachability == null)
			{
				defaultRouteReachability = new NetworkReachability(new IPAddress(0));
				//defaultRouteReachability.SetCallback(OnChange);
				defaultRouteReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
			}
			if (!defaultRouteReachability.TryGetFlags(out flags))
				return false;
			return IsReachableWithoutRequiringConnection(flags);
		}

		private NetworkReachability adHocWiFiNetworkReachability;
		private bool IsAdHocWiFiNetworkAvailable(out NetworkReachabilityFlags flags)
		{
			if (adHocWiFiNetworkReachability == null)
			{
				adHocWiFiNetworkReachability = new NetworkReachability(new IPAddress(new byte[] { 169, 254, 0, 0 }));
				//adHocWiFiNetworkReachability.SetCallback(OnChange);
				adHocWiFiNetworkReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
			}

			if (!adHocWiFiNetworkReachability.TryGetFlags(out flags))
				return false;

			return IsReachableWithoutRequiringConnection(flags);
		}

		public static bool IsReachableWithoutRequiringConnection(NetworkReachabilityFlags flags)
		{
			// Is it reachable with the current network configuration?
			bool isReachable = (flags & NetworkReachabilityFlags.Reachable) != 0;

			// Do we need a connection to reach it?
			bool noConnectionRequired = (flags & NetworkReachabilityFlags.ConnectionRequired) == 0;

			// Since the network stack will automatically try to get the WAN up,
			// probe that
			if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
				noConnectionRequired = true;

			return isReachable && noConnectionRequired;
		}

		private bool InternetConnectionStatus()
		{
			NetworkReachabilityFlags flags;
			bool defaultNetworkAvailable = IsNetworkAvailable(out flags);
			if (defaultNetworkAvailable && ((flags & NetworkReachabilityFlags.IsDirect) != 0))
			{
				return false;
			}
			else if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
			{
				return true;
			}
			else if (flags == 0)
			{
				return false;
			}

			String ssid = String.Empty;


			string[] supportedInterfaces;
			StatusCode status;
			if ((status = CaptiveNetwork.TryGetSupportedInterfaces(out supportedInterfaces)) != StatusCode.OK)
			{

			}
			else
			{
				foreach (var item in supportedInterfaces)
				{
					NSDictionary info;
					status = CaptiveNetwork.TryCopyCurrentNetworkInfo(item, out info);
					if (status != StatusCode.OK)
					{
						continue;
					}

					ssid = info[CaptiveNetwork.NetworkInfoKeySSID].ToString();
				}
			}



			SSIDName = ssid;


			var result = MakeWebRequestWifiOnly();

			return true;
		}

		public string MakeWebRequestWifiOnly()
		{
			var wifi = Plugin.Connectivity.Abstractions.ConnectionType.WiFi;
			var connectionTypes = CrossConnectivity.Current.ConnectionTypes;
			if (!connectionTypes.Contains(wifi))
			{
				//You do not have wifi
				return null;
			}

			//Make web request here
			return null;
		}

		private bool LocalWifiConnectionStatus()
		{
			NetworkReachabilityFlags flags;

			String ssid = String.Empty;
			if (IsAdHocWiFiNetworkAvailable(out flags))
			{

				string[] supportedInterfaces;
				StatusCode status;
				if ((status = CaptiveNetwork.TryGetSupportedInterfaces(out supportedInterfaces)) != StatusCode.OK)
				{

				}
				else
				{
					foreach (var item in supportedInterfaces)
					{
						NSDictionary info;
						status = CaptiveNetwork.TryCopyCurrentNetworkInfo(item, out info);
						if (status != StatusCode.OK)
						{
							continue;
						}

						ssid = info[CaptiveNetwork.NetworkInfoKeySSID].ToString();
					}
				}

				SSIDName = ssid;


				var result = MakeWebRequestWifiOnly();

				if ((flags & NetworkReachabilityFlags.IsDirect) != 0)
					return true;
			}
			return false;
		}
	}
}