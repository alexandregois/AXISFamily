using System;
using Android.App;
using Android.Content;

namespace family.Droid.Services
{
	[Service(
		Enabled = true
		, Exported = true
		, IsolatedProcess = true
		, Name = "br.com.systemsat.ConnectivityChangeService"
	)]
	public class ConnectivityChangeService : BroadcastReceiver
	{
		public ConnectivityChangeService()
		{
		}

		public override void OnReceive(Context context, Intent intent)
		{
			//Intent startPosicaoSenderServiceIntent = new Intent(
			//	context
			//	, typeof(PosicaoSenderService)
			//);
			//context.StartService(startPosicaoSenderServiceIntent);

		}
	}
}
