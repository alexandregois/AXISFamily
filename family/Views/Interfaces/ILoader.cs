
using System;
namespace family.Views.Interfaces
{
	public interface ILoader
	{
		void EscondeLoad();
		void ExibirLoad();

        void OpenStreetview(Boolean paramExibe);

        void CloseStreetview(Boolean paramOnAppear);

	}
}
