using System;
using family.Domain.Dto;
namespace family.Views.Interfaces
{
	public interface IPartialViewHistorico : IPartialView
	{
		void BeginRefresh();
		void EndRefresh();
		void DataFiltroFocus();
		void ScrollTop(PosicaoHistorico paramPosicao);
		void SelectedItem(PosicaoHistorico paramPosicao);
	}
}
