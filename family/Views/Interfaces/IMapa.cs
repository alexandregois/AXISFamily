using family.CustomElements;
using family.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace family.Views.Interfaces
{
	public interface IMapa : ILoader
	{
		ViewModelMapaGoogle GetModelMapaGoogle();
		void AddPartialPage(ContentView paramContentView);
		void ChangeColor(Color paramColorStatusBar, Color paramColorLoad);
		void SizeBox();
		void ShowAlert(View paramView);
		CustomDialogAlert DialogAlert { get; set; }

        Map mapaPosicao { get; set; }
    }
}
