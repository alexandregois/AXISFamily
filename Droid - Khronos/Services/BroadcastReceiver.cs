using Android.App;
using Android.Content;
using family.Droid.CrossPlataform;

namespace family.Droid.Services
{
	[Service(
		Enabled = true
		, Exported = true
		, IsolatedProcess = true
		, Name = "br.com.khronosnet.BootReceiver"
    )]
	public class BootReceiver : BroadcastReceiver
	{
		public BootReceiver()
		{
		}

		public override void OnReceive(Context context, Intent intent)
		{
            CrossPlataformUtil util = new CrossPlataformUtil();

            util.OpenActivity(true);

            //Intent ii = new Intent(context, typeof(PosicaoService));
            //ii.AddFlags(ActivityFlags.NewTask);
            //context.StartActivity(ii);


            //Intent i = new Intent(context, typeof(MainActivity)); //Abre o App no reboot do celular
            //i.AddFlags(ActivityFlags.NewTask);
            //context.StartActivity(i);

            util.TrackService(true);
            util.TrackKeepAlive(true);
        }
	}
}
