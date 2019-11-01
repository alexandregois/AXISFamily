using family.Domain;

namespace family.Views.Interfaces
{
	public interface IPartialViewPontoControle : IPartialView
	{
		void BeginRefresh();
		void EndRefresh();
		void ScrollTop(PontoControle paramPosicao);
		void SelectedItem(PontoControle paramPosicao);
	}
}
