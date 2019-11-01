using System;
using Xamarin.Forms;

namespace family.Views.Services
{
	public class PermissaoService
	{

		protected App _app => (Application.Current as App);

		public Boolean PodeEnviarPanico()
		{
			Boolean ret = false;
			if(_app.Token != null){
				if(_app.Token.LstFuncao != null){
					ret = _app.Token.LstFuncao.Contains("|397|");
				}
			}
			return ret;
		}

		public Boolean PodeTornarAppUnidadeRastreada()
		{
			Boolean ret = false;
			if(_app.Token != null){
				if(_app.Token.LstFuncao != null){
					ret = _app.Token.LstFuncao.Contains("|361|");
				}
			}
			return ret;
		}

		public Boolean PodeConvidar()
		{
			return false;
		}

		public Boolean PodeVisualizarHistorico()
		{
			Boolean ret = false;
			if(_app.Token != null){
				if(_app.Token.LstFuncao != null){
					ret = _app.Token.LstFuncao.Contains("|394|");
				}
			}
			return ret;
		}

		public Boolean PodeManterAncora()
		{
			Boolean ret = false;
			if(_app.Token != null){
				if(_app.Token.LstFuncao != null){
					ret = _app.Token.LstFuncao.Contains("|392|");
				}
			}
			return ret;
		}

		public Boolean PodeManterPontoControle()
		{
			Boolean ret = false;
			if(_app.Token != null){
				if(_app.Token.LstFuncao != null){
					ret = _app.Token.LstFuncao.Contains("|387|");
				}
			}
			return ret;
		}

		public Boolean PodeManterBloqueio()
		{
			Boolean ret = false;
			if(_app.Token != null){
				if(_app.Token.LstFuncao != null){
					ret = _app.Token.LstFuncao.Contains("|395|");
				}
			}
			return ret;
		}

        public Boolean PodeManterManutencao()
        {
            Boolean ret = false;
            if (_app.Token != null)
            {
                if (_app.Token.LstFuncao != null)
                {
                    ret = _app.Token.LstFuncao.Contains("|388|"); //CORRETO
                    //ret = _app.Token.LstFuncao.Contains("|395|"); //TESTE
                }
            }
            return ret;
        }
    }
}
