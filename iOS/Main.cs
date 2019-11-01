using UIKit;

namespace family.iOS
{
	public class Application
	{
		// This is the main entry point of the application.
		static void Main(string[] args)
		{
            try
            {
                // if you want to use a different Application Delegate class from "AppDelegate"
                // you can specify it here.
                UIApplication.Main(args, null, "AppDelegate");
            }
            catch (System.Exception ex)
            {
                UIApplication.Main(args, null, "AppDelegate");
            }
			
		}
	}
}
