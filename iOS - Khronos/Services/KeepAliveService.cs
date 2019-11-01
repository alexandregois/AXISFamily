using System;
using family.ViewModels;

namespace family.iOS.Services
{
	public class KeepAliveService
	{
		private Configuracao _configuracao;
		public Configuracao PropConf
		{
			get
			{
				if (_configuracao == null)
					_configuracao = new Configuracao();
				return _configuracao;
			}
		}

		public KeepAliveService()
		{

			Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(_configuracao.TempoKeepAlive), () =>
			{

                if (PropConf.IsLogado == true)
                {
                    ViewModelConfiguracao viewModelConfiguracao = new ViewModelConfiguracao();
                    viewModelConfiguracao.CheckKeepAlive();
                }

                return true;
			});

		}

		public void KeepAlive()
		{
			ViewModelConfiguracao viewModelConfiguracao = new ViewModelConfiguracao();
			viewModelConfiguracao.CheckKeepAlive();
		}
	}
}
