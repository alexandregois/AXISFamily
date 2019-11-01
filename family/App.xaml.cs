using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using family.CrossPlataform;
using family.Domain;
using family.Domain.Dto;
using family.Domain.Realm;
using family.Resx;
using family.Services;
using family.Services.ServiceRealm;
using family.ViewModels;
using family.ViewModels.InterfaceServices;
using family.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;
using Plugin.Battery;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace family
{
    public partial class App : Application
    {

        public PosicaoUnidadeRastreadaRealm ActualPosition { get; set; }

        private INavigationService _navigationService;
        protected IMessageService _messageService;

        public String strImei { get; set; }

        private ICrossPlataformUtil _util;
        public ICrossPlataformUtil Util
        {
            get
            {
                if (_util == null)
                    _util = DependencyService.Get<ICrossPlataformUtil>();

                return _util;
            }
            set
            {
                _util = value;
            }
        }

        private Double _screenWidth;
        public Double ScreenWidth
        {
            get
            {
                if (Math.Abs(_screenWidth) < Double.Epsilon)
                    _screenWidth = Util.GetScreenWidth();

                return _screenWidth;
            }
            set
            {
                _screenWidth = value;
            }
        }

        private Double _screenHeight;
        public Double ScreenHeight
        {
            get
            {
                if (Math.Abs(_screenHeight) < Double.Epsilon)
                    _screenHeight = Util.GetScreenHeight();

                return _screenHeight;
            }
            set
            {
                _screenHeight = value;
            }
        }

        private Double _reduceTop;
        public Double ReduceTop
        {
            get
            {
                if (Math.Abs(_reduceTop) < Double.Epsilon)
                    _reduceTop = Util.GetHeightStatusBar();

                return _reduceTop;
            }
            set
            {
                _reduceTop = value;
            }
        }

        private Configuracao _configuracao;
        public Configuracao Configuracao
        {
            get
            {
                if (_configuracao == null)
                    _configuracao = new Configuracao();

                return _configuracao;
            }
            set
            {
                _configuracao = value;
            }
        }

        private Token _token;
        public Token Token
        {
            get
            {
                if (_token == null)
                    _token = GetToken();
                return _token;
            }
            set
            {
                _token = value;
            }
        }

        public Int16 _countClick { get; set; }

        public Boolean _isBloqueadoGlobal { get; set; }

        public Boolean isPersonalizado { get; set; }

        public String nameProject { get; set; }

        public Object ObjetoTransferencia { get; set; }

        public Pin selectPinUltimaPosicao { get; set; }

        #region Template
        public Thickness DefaultTemplateMargin
        {
            get
            {
                Double reduce = 0;
                if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
                {
                    reduce = ReduceTop;
                }

                return new Thickness(
                    0
                    , reduce
                    , 0
                    , 0
                );
            }
        }

        public double DefaultTemplateHeightNavegationBar
        {
            get
            {
                return 53;
            }
        }

        public double DefaultTemplateNavegationLine
        {
            get
            {
                return 2;
            }
        }

        public double DefaultTemplateHeightContent
        {
            get
            {
                return ScreenHeight
                    - (
                        DefaultTemplateHeightNavegationBar
                        + DefaultTemplateNavegationLine
                        + DefaultTemplateMargin.Top
                    );
            }
        }

        public static StackLayout PanelBarra { get; set; }

        #endregion

        public String _namebSSID;
        public String NamebSSID
        {
            get
            {
                //if (_namebSSID == null)
                _namebSSID = GetSSIDName();

                return _namebSSID;
            }
            set
            {
                _namebSSID = value;
            }
        }

        //public Boolean SolicitacaoRastreamentoEmAndamento { get; set; }

        public String _statusBateria;
        public String StatusBateria
        {
            get
            {
                //if (_statusBateria == null)
                _statusBateria = CrossBattery.Current.Status.ToString();

                //CrossLocalNotifications.Current.Show(AppResources.Alert, _statusBateria);

                return _statusBateria;
            }
            set
            {
                _statusBateria = value;
            }
        }

        public String _statusBateriaResource;
        public String StatusBateriaResource
        {
            get
            {
                //if (_statusBateriaResource == null)
                _statusBateriaResource = AppResources.ResourceManager.GetString(CrossBattery.Current.Status.ToString());

                return _statusBateriaResource;
            }
            set
            {
                _statusBateriaResource = value;
            }
        }

        public List<TelefoneContatoDto> ListaTelefone { get; set; }

        public App(Boolean paramIsPersonalizado, String paramNameProject)
        {

            try
            {

                InitializeComponent();


                ListaTelefone = new List<TelefoneContatoDto>();


                LocalizeApp();

                GetDependency();

                PanelBarra = new StackLayout()
                {
                    Margin = new Thickness(0),
                    HeightRequest = DefaultTemplateNavegationLine,
                    Spacing = 0
                };

                PanelBarra.SetBinding(
                    StackLayout.BackgroundColorProperty
                    , new TemplateBinding("Parent.BindingContext.PageColor")
                );


                isPersonalizado = paramIsPersonalizado;
                nameProject = paramNameProject;


                GetSSIDName();


                if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
                {
                    Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(_configuracao.TempoKeepAlive), () =>
                    {

                        if (_configuracao.IsLogado == true)
                        {
                            KeepAlive();
                        }

                        return true;
                    });
                }

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

        }

        public String GetSSIDName()
        {

            String returnSSIDName = String.Empty;
            var networkConnection = DependencyService.Get<INetworkConnection>();
            networkConnection.CheckNetworkConnection();

            returnSSIDName = networkConnection.SSIDName;

            if (returnSSIDName != null)
            {
                if (returnSSIDName.IndexOf("\"") > -1)
                {
                    returnSSIDName = returnSSIDName.Substring(1, returnSSIDName.Length - 2);
                }

                _namebSSID = returnSSIDName;

            }

            return returnSSIDName;

        }

        protected override void OnStart()
        {

            try
            {

                String mobileKey = String.Empty;

                if (nameProject == "family")
                {
                    mobileKey = "ios=6fe8e8b4-8ba4-44da-ad77-0ec70508b062;" +
                        "android=ddea3d2d-538f-4994-8793-e2b6417e393c";
                }

                if (nameProject == "khronos")
                {
                    mobileKey = "ios=86dc3125-3232-4046-9d65-698d4d2379e1;" +
                        "android=7a99f5fc-c828-4a02-9830-9b2f45e8cb44";
                }

                if (nameProject == "maxima")
                {
                    mobileKey = "ios=8c81f881-5c5b-43e3-b688-67d51d5f3dad;" +
                        "android=34e6d979-8356-49f5-9bf7-dbee5c8cafc4";
                }

                if (nameProject == "spywave")
                {
                    mobileKey = "ios=93bce76e-96f7-40ef-b081-6e97888dc1e3;" +
                        "android=93bce76e-96f7-40ef-b081-6e97888dc1e3";
                }


                //String mobileKey = "ios=5a8428f7-3975-48fc-b79a-96ba29e068d2;" +
                //"android=5560adc3-2d6d-447a-90af-fa45305c5272";

                //String mobileKey = "ios=803545d9-3f27-428f-98c7-65c80d2ef0a2;" +
                //"android=ddea3d2d-538f-4994-8793-e2b6417e393c;";


                AppCenter.Start(
                    mobileKey
                    , typeof(Analytics)
                    , typeof(Crashes)
                    , typeof(Push)
                );


                ViewModelConfiguracao viewModelConfiguracao = new ViewModelConfiguracao();

                viewModelConfiguracao.Lista_TelefoneAction();


                TokenDataStore token = new TokenDataStore();
                Boolean? existIsLocator = token.ExistIsLocator();

                _navigationService =
                    DependencyService.Get<INavigationService>();
                _messageService =
                        DependencyService.Get<IMessageService>();
                

                if (existIsLocator.HasValue)
                {
                    Util.TrackService(existIsLocator.Value);
                    MainPage = new NavigationPage(new ViewListaUnidadeRastreada());
                    return;
                }
                else
                {
                    if (isPersonalizado)
                        MainPage = new NavigationPage(new SplashPersonalizado());
                    else
                        MainPage = new NavigationPage(new ViewLogin(isPersonalizado, nameProject));
                }


            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

        }

        public void KeepAlive()
        {
            ViewModelConfiguracao viewModelConfiguracao = new ViewModelConfiguracao();
            viewModelConfiguracao.CheckKeepAlive();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private void GetDependency()
        {
            if (Configuracao.UseMockDataStore)
            {
                DependencyService.Register<MockDataStore>();
            }
            else
            {
                DependencyService.Register<CloudDataStore>();
            }


            DependencyService
                .Register<IMessageService>();

            DependencyService
                .Register<INavigationService>();

            DependencyService.Register<ICrossPlataformUtil>();
        }

        private void LocalizeApp()
        {
            //System.Diagnostics.Debug.WriteLine("====== resource debug info =========");
            //var assembly = typeof(App).GetTypeInfo().Assembly;
            //foreach (var res in assembly.GetManifestResourceNames()) 
            //	System.Diagnostics.Debug.WriteLine("found resource: " + res);
            //System.Diagnostics.Debug.WriteLine("====================================");

            // determine the correct, supported .NET culture
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resx.AppResources.Culture = ci; // set the RESX for resource localization
            DependencyService.Get<ILocalize>().SetLocale(ci); // set the Thread for locale-aware methods
        }

        public static Token GetToken()
        {
            Token token = null;
            TokenDataStore dataSource = new TokenDataStore();
            TokenRealm tokenTemp = dataSource.Get(1);

            if (tokenTemp != null)
            {
                token = new Token();
                token.TransformFromRealm(tokenTemp);
            }

            return token;
        }

        public void ExibeToast(String Messagem)
        {
            //ShowToast(new NotificationOptions()
            //{
            //    Title = AppResources.Alert, //"The Title Line",
            //    Description = Messagem,
            //    IsClickable = true,
            //    WindowsOptions = new WindowsOptions() { LogoUri = "ic_launcher.png" },
            //    ClearFromHistory = false,
            //    AllowTapInNotificationCenter = false,
            //    AndroidOptions = new AndroidOptions()
            //    {
            //        HexColor = "#F99D1C",
            //        ForceOpenAppOnNotificationTap = true
            //    }
            //});
        }

        public void ShowToast(String toastString)
        {
            DependencyService.Get<IMessage>().ShowToast(toastString);
        }

        public async Task<Boolean> ShowMessageGPS()
        {
            //CrossLocalNotifications.Current.Show("GPS", AppResources.Gps);

            ServiceResult<Boolean> result = new ServiceResult<Boolean>();
            result.Data = false;

            String answer;
            answer = await _messageService.ShowMessageAsync(
                AppResources.GpsSettings,
                AppResources.Cancel,
                null,
                new string[]
                {
                    AppResources.GpsOpenSettings
                });

            if (answer != AppResources.Cancel)
            {
                result.Data = true;
            }

            return result.Data;

        }

        public String MessageGps(String paramMessage)
        {
            String retorno = null;

            if (paramMessage == "Gps")
                retorno = AppResources.Gps;

            if (paramMessage == "GpsOpenSettings")
                retorno = AppResources.GpsOpenSettings;

            if (paramMessage == "GpsSettings")
                retorno = AppResources.GpsSettings;

            if (paramMessage == "Alert")
                retorno = AppResources.Alert;

            if (paramMessage == "No")
                retorno = AppResources.No;


            return retorno;
        }

    }
}
