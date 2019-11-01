using family.CustomElements;
using Xamarin.Forms;

namespace family.Views.Interfaces
{
	public interface IListaUnidadeRastreada : ILoader
	{
		void BeginRefresh();
		void EndRefresh();
		void MakeFrameDefault(
			ref Frame paramFrame
			, ref StackLayout paramContent
		);

		void ShowAlert(View paramView);
		void BuildTemplatePage();

		CustomDialogAlert DialogAlert { get; set; }


    }
}
