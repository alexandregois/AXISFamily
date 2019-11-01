using family.Domain.Enum;
using System;

namespace family.ViewModels.InterfaceServices
{
	public interface INavigationService
	{
		void Voltar();
		void NavigateToCadastro();
		void NavigateToListaUnidadeRastreadaPage();
		void NavigateToLogin();
		void NavigateToConfiguracao();
		void NavigateToMapa(EnumPage paramPartialPage, int paramIdRastreador);
        void NavigateToStreetView(Double paramLatitude, Double paramLongitude, Boolean paramExibe);
        void NavigateToTelefone();
    }
}
