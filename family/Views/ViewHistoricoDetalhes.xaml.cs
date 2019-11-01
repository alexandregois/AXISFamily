using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace family.Views
{
	public partial class ViewHistoricoDetalhes : ContentPage
	{
		public ViewHistoricoDetalhes ()
		{
			InitializeComponent ();
		}

        private void btnCancelar_Clicked(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage.Navigation.PopAsync();
            });
        }
    }
}