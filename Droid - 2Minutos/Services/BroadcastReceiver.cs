using Android.App;
using Android.Content;
using Android.Widget;
using family.Droid.CrossPlataform;
using Microsoft.AppCenter.Crashes;
using System;
using System.Threading.Tasks;

namespace family.Droid.Services
{
    [BroadcastReceiver(
        Enabled = true
        , Exported = false
        , DirectBootAware = true
        , Name = "br.com.doisminutos.BootReceiver"
    )]
    [IntentFilter(new[] { Intent.ActionBootCompleted, Intent.ActionLockedBootCompleted,
        "android.intent.action.QUICKBOOT_POWERON",
        "android.intent.action.BOOT_COMPLETED",
        "android.intent.action.LOCKED_BOOT_COMPLETED",
        "android.intent.action.MY_PACKAGE_REPLACED"
    }, Priority = (int)IntentFilterPriority.HighPriority)]
    public class BootReceiver : BroadcastReceiver
    {
        public BootReceiver()
        {            
            CrossPlataformUtil util = new CrossPlataformUtil();
            util.TrackKeepAlive(true);
            util.TrackService(true);

            Intent i = new Intent(Android.App.Application.Context, typeof(MainActivity)); //Abre o App no reboot do celular
            i.AddFlags(ActivityFlags.NewTask);
            Android.App.Application.Context.StartActivity(i);

        }

        public override void OnReceive(Context context, Intent intent)
        {

            try
            {
                CrossPlataformUtil util = new CrossPlataformUtil();
                util.TrackKeepAlive(true);
                util.TrackService(true);

                //Intent ii = new Intent(context, typeof(PosicaoService));
                //ii.AddFlags(ActivityFlags.NewTask);
                //context.StartActivity(ii);
                
                //Intent i = new Intent(context, typeof(MainActivity)); //Abre o App no reboot do celular
                //i.AddFlags(ActivityFlags.NewTask);
                //context.StartActivity(i);


            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

        }
    }
}
