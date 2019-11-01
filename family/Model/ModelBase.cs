using family.Services.Interfaces;
using Xamarin.Forms;

namespace family.Model
{
	public class ModelBase
	{
		public IDataStore DataStore => DependencyService.Get<IDataStore>();
		public App _app => (Application.Current as App);

		public ModelBase()
		{

		}

	}
}