using System;
using family.CrossPlataform;
using Xamarin.Forms;

namespace family
{
    public class Configuracao
    {
        public Boolean PodeCadastrar { get; }
        public String CodigoEmpresa { get; }
        public String URLStringWS { get; }

        private String _imei;
        public String Imei
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_imei))
                    _imei = DependencyService.Get<ICrossPlataformUtil>().GetIdentifier();
                return _imei;
            }
        }

        public String NomeDataBase { get; set; }

        public TimeSpan LoopTimeSpan { get; set; }

        public Boolean UseMockDataStore = false; //PARA MOCAR = true

        public Int32 TempoTransmissaoPadrao { get; set; }

        public Int32 TempoKeepAlive { get; set; }

        public Int32 IdCultura { get; set; }

        #region FireBase
        public String FireBase_AuthorizedEntity = "family-45d37"; // Project id from Google Developer Console
        public String FireBase_Scope = "FCM";
        #endregion

        private Boolean _isLogado;
        public Boolean IsLogado
        {
            get
            {
                _isLogado = false;

                if (Application.Current.Properties.ContainsKey("IsLogado"))
                {
                    if (Application.Current.Properties["IsLogado"] != null)
                        _isLogado = (Boolean)Application.Current.Properties["IsLogado"];

                }

                return _isLogado;
            }
            set
            {
                _isLogado = value;
            }
        }

        public String PosicaoStartAction = "ACTION_START_SERVICE";
        public String KeepAliveStartAction = "ACTION_START_SERVICE";
        public String TimerRestart = "TIMER_RESTART";
        public String MainActivityAction = "";
        public Int32 PosicaoForegroundId = 10000;
        public Int32 RequestCodeAlarm = 10001;

        public Configuracao()
        {
            PodeCadastrar = true;

#if DEBUG

            //UseMockDataStore = true;

            //URLStringWS = "http://10.10.3.111:43625"; // Homologação     
            //URLStringWS = "http://10.10.3.174:43625"; // Homologação            
            //URLStringWS = "http://service.systemsatx.com.br:94"; //IP Externo


            URLStringWS = "https://service.systemsatx.com.br"; //IP Externo


            //TempoKeepAlive = 120; //2 minutos
            TempoKeepAlive = 300; //5 minutos 

#else

            UseMockDataStore = false;
            URLStringWS = "http://service.systemsatx.com.br"; //IP Externo

            //URLStringWS = "http://200.152.54.164:81"; //Ip Homologação Externo
            //URLStringWS = "http://10.10.6.166:2826"; // Desenv

            //URLStringWS = "http://10.1.3.124:81"; //Ip Homologação


            TempoKeepAlive = 300; //5 minutos           

#endif


            //CodigoEmpresa = "DSV";
            NomeDataBase = "TodoSQLite.db3";

            LoopTimeSpan = TimeSpan.FromMinutes(1);

            //TempoTransmissaoPadrao = 180; //Padrão
            TempoTransmissaoPadrao = 120; //1 minuto            


        }

    }
}
