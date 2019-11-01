using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;

namespace family.Droid.CustomClass
{
	public class PermissionsHandler
	{
		public static int permissionIndex = -1;
		public static Dictionary<Int32, String> manifestPermissions = new Dictionary<Int32, String>();
		public static Context thisContext;
		public static Activity thisActivity;


		public static void CheckPermissions(
			Context parentContext
			, Activity parentActivity
		)
		{
			thisContext = parentContext;
			thisActivity = parentActivity;
			permissionIndex = -1;
			SetPermissionMap();
		}

		public static void SetPermissionMap()
		{
			if (Build.VERSION.SdkInt >= BuildVersionCodes.LollipopMr1)
			{
				manifestPermissions.Add(0, Android.Manifest.Permission.Internet);
				manifestPermissions.Add(1, Android.Manifest.Permission.AccessFineLocation);
				manifestPermissions.Add(2, Android.Manifest.Permission.AccessCoarseLocation);
				manifestPermissions.Add(3, Android.Manifest.Permission.AccessNetworkState);
				manifestPermissions.Add(4, Android.Manifest.Permission.WriteExternalStorage);
				manifestPermissions.Add(5, Android.Manifest.Permission.ReadExternalStorage);
				manifestPermissions.Add(6, Android.Manifest.Permission.AccessWifiState);
				manifestPermissions.Add(7, Android.Manifest.Permission.AccessLocationExtraCommands);
				manifestPermissions.Add(8, Android.Manifest.Permission.WakeLock);
				manifestPermissions.Add(9, Android.Manifest.Permission.ReadPhoneState);
				manifestPermissions.Add(10, Android.Manifest.Permission.ReceiveBootCompleted);
				manifestPermissions.Add(11, Android.Manifest.Permission.LocationHardware);
				manifestPermissions.Add(12, Android.Manifest.Permission.RequestIgnoreBatteryOptimizations);

				GetNextPermission();
			}
		}

		public static void GetNextPermission()
		{
			if (permissionIndex < manifestPermissions.Count)
			{
				permissionIndex++;
				if (manifestPermissions[permissionIndex] != null)
				{
					System.Diagnostics.Debug.WriteLine("PermissionHandler",
					                                   "getting permission: " + permissionIndex);
					GetPermissions(permissionIndex);
				}
			}
			else
				System.Diagnostics.Debug.WriteLine("PermissionsHandler", "All permissions granted");
		}

		private static void GetPermissions(int listIndex)
		{

			if (ContextCompat.CheckSelfPermission(thisContext, manifestPermissions[listIndex])
			    != Permission.Granted)
			{
				//            AlertDialog.Builder ad = new AlertDialog.Builder(thisContext);
				//            ad.setMessage(thisContext.getResources().getIdentifier(stringValue,"string",thisContext.getPackageName())).setCancelable(false).setPositiveButton("Ok!",
				//                    new DialogInterface.OnClickListener() {
				//                        @Override
				//                        public void onClick(DialogInterface dialogInterface, int i) {
				//                            ActivityCompat.requestPermissions(thisActivity,new String[]{manifestPermissions.get(listIndex)},listIndex);
				//                            dialogInterface.cancel();
				//                        }
				//                    });
				//            AlertDialog alert = ad.create();
				//            alert.show();
				ActivityCompat.RequestPermissions(thisActivity
				                                  , new String[] { manifestPermissions[listIndex] }
				                                  , listIndex);
				System.Diagnostics.Debug.WriteLine("PermissionsHandler"
				                                   , "permissions " + manifestPermissions[listIndex] + " not granted");
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("PermissionsHandler"
				                                   , "permissions " + manifestPermissions[listIndex] + " granted");
				GetNextPermission();
			}
		}

		public void OnRequestPermissionsResult(int requestCode
		                                       , String[] permissions, int[] grantResults)
		{
			if (grantResults.Length > 0 && grantResults[0] != (int)Permission.Granted)
			{
				permissionIndex--;
			}
			GetNextPermission();
		}

		public static void RecheckPermissions(Context thisContext, Activity thisActivity)
		{
			if (Build.VERSION.SdkInt >= BuildVersionCodes.LollipopMr1)
				CheckPermissions(thisContext, thisActivity);
			else
				System.Diagnostics.Debug.WriteLine("PermissionsHandler", "All permissions granted");
		}

	}
}
