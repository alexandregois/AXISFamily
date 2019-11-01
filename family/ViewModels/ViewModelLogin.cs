using System;
using System.Threading.Tasks;
using System.Windows.Input;
using family.Domain;
using family.Domain.Dto;
using family.Domain.Enum;
using family.Domain.Realm;
using family.Model;
using family.Resx;
using family.Services.ServiceRealm;
using family.ViewModels.Base;
using family.Views.Interfaces;
using Xamarin.Forms;
using System.Threading;

using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System.Net;
using Microsoft.AppCenter.Push;

namespace family.ViewModels
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
    public class ViewModelLogin : ViewModelBase
    {
        protected App _app => (Application.Current as App);

        public ILoader _view { get; set; }

        public ICommand LoginCommand { get; set; }

        private string _email;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                this.Notify("Email");
            }
        }

        private string password;
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                this.Notify("Password");
            }
        }

        private Double _powerByHeight;
        public Double PowerByHeight
        {
            get
            {
                return _powerByHeight;
            }
            set
            {
                _powerByHeight = value;
                this.Notify("PowerByHeight");
            }
        }

        private Double _panelLogoHeight;
        public Double PanelLogoHeight
        {
            get
            {
                return _panelLogoHeight;
            }
            set
            {
                _panelLogoHeight = value;
                this.Notify("PanelLogoHeight");
            }
        }

        private Double _loginHeight;
        public Double LoginHeight
        {
            get
            {
                return _loginHeight;
            }
            set
            {
                _loginHeight = value;
                this.Notify("LoginHeight");
            }
        }

        private Double _panelHeight;
        public Double PanelHeight
        {
            get
            {
                return _panelHeight;
            }
            set
            {
                _panelHeight = value;
                this.Notify("PanelHeight");
            }
        }

        private ImageSource _logoSource;
        public ImageSource LogoSource
        {
            get
            {
                return _logoSource;
            }
            set
            {
                _logoSource = value;
                this.Notify("LogoSource");
            }
        }

        private Double _logoWidth;
        public Double LogoWidth
        {
            get
            {
                return _logoWidth;
            }
            set
            {
                _logoWidth = value;
                this.Notify("LogoWidth");
            }
        }

        private Double _logoHeight;
        public Double LogoHeight
        {
            get
            {
                return _logoHeight;
            }
            set
            {
                _logoHeight = value;
                this.Notify("LogoHeight");
            }
        }

        private Color _barColor;
        public Color BarColor
        {
            get
            {
                return _barColor;
            }
            set
            {
                _barColor = value;
                this.Notify("BarColor");
            }
        }

        private Color _corFundoBotao;
        public Color CorFundoBotao
        {
            get
            {
                return _corFundoBotao;
            }
            set
            {
                _corFundoBotao = value;
                this.Notify("CorFundoBotao");
            }
        }

        private Color _corFundoLogin;
        public Color CorFundoLogin
        {
            get
            {
                return _corFundoLogin;
            }
            set
            {
                _corFundoLogin = value;
                this.Notify("CorFundoLogin");
            }
        }

        private ImageSource _powerBySource;
        public ImageSource PowerBySource
        {
            get
            {
                return _powerBySource;
            }
            set
            {
                _powerBySource = value;
                this.Notify("PowerBySource");
            }
        }

        private Double _powerByImageWidth;
        public Double PowerByImageWidth
        {
            get
            {
                return _powerByImageWidth;
            }
            set
            {
                _powerByImageWidth = value;
                this.Notify("PowerByImageWidth");
            }
        }

        private String _lblUseEmailPassText;
        public String LblUseEmailPassText
        {
            get
            {
                return _lblUseEmailPassText;
            }
            set
            {
                _lblUseEmailPassText = value;
                this.Notify("LblUseEmailPassText");
            }
        }

        private Double _lblUseEmailPassHeight;
        public Double LblUseEmailPassHeight
        {
            get
            {
                return _lblUseEmailPassHeight;
            }
            set
            {
                _lblUseEmailPassHeight = value;
                this.Notify("LblUseEmailPassHeight");
            }
        }

        private Double _entryWidth;
        public Double EntryWidth
        {
            get
            {
                return _entryWidth;
            }
            set
            {
                _entryWidth = value;
                this.Notify("EntryWidth");
            }
        }

        private Double _entryHeight;
        public Double EntryHeight
        {
            get
            {
                return _entryHeight;
            }
            set
            {
                _entryHeight = value;
                this.Notify("EntryHeight");
            }
        }

        private Thickness _txtEmailMargin;
        public Thickness TxtEmailMargin
        {
            get
            {
                return _txtEmailMargin;
            }
            set
            {
                _txtEmailMargin = value;
                this.Notify("TxtEmailMargin");
            }
        }

        private Double _txtSenhaWidth;
        public Double TxtSenhaWidth
        {
            get
            {
                return _txtSenhaWidth;
            }
            set
            {
                _txtSenhaWidth = value;
                this.Notify("TxtSenhaWidth");
            }
        }


        private ImageSource _buttonLoginSource;
        public ImageSource ButtonLoginSource
        {
            get
            {
                return _buttonLoginSource;
            }
            set
            {
                _buttonLoginSource = value;
                this.Notify("ButtonLoginSource");
            }
        }

        private Double _buttonLoginWidth;
        public Double ButtonLoginWidth
        {
            get
            {
                return _buttonLoginWidth;
            }
            set
            {
                _buttonLoginWidth = value;
                this.Notify("ButtonLoginWidth");
            }
        }

        private Double _lblCadastroHeight;
        public Double LblCadastroHeight
        {
            get
            {
                return _lblCadastroHeight;
            }
            set
            {
                _lblCadastroHeight = value;
                this.Notify("LblCadastroHeight");
            }
        }

        private String _lblCadastroText;
        public String LblCadastroText
        {
            get
            {
                return _lblCadastroText;
            }
            set
            {
                _lblCadastroText = value;
                this.Notify("LblCadastroText");
            }
        }

        private Command _lblCadastroCommand;
        public Command LblCadastroCommand
        {
            get
            {
                return _lblCadastroCommand;
            }
            set
            {
                _lblCadastroCommand = value;
                this.Notify("LblCadastroCommand");
            }
        }



        private Boolean _loginIsEnabled;
        public Boolean LoginIsEnabled
        {
            get
            {
                return _loginIsEnabled;
            }
            set
            {
                _loginIsEnabled = value;
                this.Notify("LoginIsEnabled");
            }
        }

        public Boolean isPersonalizado { get; set; }
        public String nameProject { get; set; }

        public ViewModelLogin(Boolean paramIsPersonalizado, String paramNameProject)
        {
            this.LoginCommand = new Command(this.Login);

            PowerByHeight = 35;
            LoginHeight = 205;
            ShadowHeight = DefaultHeight;
            PanelHeight = DefaultHeight - _app.DefaultTemplateMargin.Top;

            double tempHeight = PanelHeight - (PowerByHeight + LoginHeight);


            Device.BeginInvokeOnMainThread(() =>
            {

                if (!paramIsPersonalizado)
                {
                    if (Application.Current.Properties.ContainsKey("UrlLogo"))
                    {
                        String strUrl = (string)Application.Current.Properties["UrlLogo"];

                        if (strUrl != null && strUrl.IndexOf("http") > -1)
                            LogoSource = ImageSource.FromUri(new Uri(strUrl));
                        else
                            LogoSource = ImageSource.FromFile("family_splash.png");
                    }
                    else
                        LogoSource = ImageSource.FromFile("family_splash.png");
                }
                else
                {
                    LogoSource = ImageSource.FromFile("splash.png");
                }





                //if (_app.nameProject == "khronos")
                //LogoSource = ImageSource.FromFile("family_splash.png");



                LogoWidth = 200;
                LogoHeight = 200;


                if (tempHeight < LogoHeight)
                    tempHeight = LogoHeight;

                PanelLogoHeight = tempHeight;

                BarColor = Color.FromHex("#FF2F2645");
                CorFundoLogin = Color.FromHex("#FF211C32");
                CorFundoBotao = Color.FromHex("#FF974392");

                PowerBySource = ImageSource.FromFile("systemsatPowered.png");

                if (paramIsPersonalizado)
                    PowerBySource = ImageSource.FromFile("systemsatPoweredOff.png");


                if (_app.nameProject == "atm")
                {
                    CorFundoLogin = Color.FromHex("#440103");
                    BarColor = Color.FromHex("#440103");
                    CorFundoBotao = Color.FromHex("#434341");
                }


                PowerByImageWidth = 80;

                LblUseEmailPassText = AppResources.UseEmailPass;
                LblUseEmailPassHeight = 50;

                if (DefaultWidth < 320)
                {
                    EntryWidth = DefaultWidth - 20;
                }
                else
                {
                    EntryWidth = 300;
                }

                EntryHeight = 53;

                TxtEmailMargin = new Thickness(0, 0, 0, 2);

                ButtonLoginWidth = 53;

                TxtSenhaWidth = EntryWidth - ButtonLoginWidth;
                ButtonLoginSource = ImageSource.FromFile("ic_next.png");

                LblCadastroHeight = 49;
                LblCadastroText = AppResources.Registered;
                LblCadastroCommand = new Command(this.CadastroCommand);



            });

        }

        public override void OnAppearing()
        {
            if (_app.ObjetoTransferencia != null)
            {
                try
                {
                    Usuario usu = (Usuario)_app.ObjetoTransferencia;

                    Password = usu.Senha;
                    Email = usu.Email;
                }
                catch (Exception) { }

                _app.ObjetoTransferencia = null;
            }
        }

        public override void OnDisappearing()
        {
        }

        public override void OnLayoutChanged()
        {
        }

        private async void CadastroCommand()
        {
            this._navigationService.NavigateToCadastro();
        }

        private async void Login()
        {
            try
            {
                LoginIsEnabled = false;
                _view.ExibirLoad();

                String paramHash = _app.Configuracao.CodigoEmpresa;
                String paramIdentificacao = _app.Configuracao.Imei;
                String paramIdSistemaOperacional;
                String msgError = "";

                if (String.IsNullOrEmpty(paramIdentificacao))
                {
                    _messageService.ShowAlertAsync(
                        AppResources.AppWithChip
                        , AppResources.Alert
                    );
                    return;
                }

                if (String.IsNullOrWhiteSpace(Email))
                {
                    msgError = AppResources.EmailPlease;
                }

                if (String.IsNullOrWhiteSpace(Password))
                {
                    if (!String.IsNullOrWhiteSpace(msgError))
                        msgError += Environment.NewLine;
                    msgError += AppResources.PassworPlease;
                }

                if (!String.IsNullOrWhiteSpace(msgError))
                {
                    _messageService.ShowAlertAsync(
                        msgError
                        , AppResources.Alert
                    );
                    LoginIsEnabled = true;
                    _view.EscondeLoad();
                }
                else
                {

                    try
                    {

                        _view.ExibirLoad();

                        //await Push.SetEnabledAsync(true);

                        LoginRequisicao();

                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics
                          .Debug.WriteLine(
                              "FA&)#2 ImageSetaLogin_Tap() " +
                              "IniciaLocation PosiçãoService: InnerException: "
                              , ex.InnerException
                             );
                        System.Diagnostics
                          .Debug.WriteLine(
                              "FA&)#2 ImageSetaLogin_Tap() " +
                              "IniciaLocation PosiçãoService: Message: "
                              , ex.Message
                             );
                    }
                }


            }
            catch (Exception)
            {
                _view.EscondeLoad();
            }
        }

        private async void LoginRequisicao()
        {

            SetPreferences(Email, Password);

            ModelPushNotification classPushNotification = new ModelPushNotification();
            var resultPush = await classPushNotification.GetRegistraPushKey();

            Task.Run(async () =>
            {
                _view.ExibirLoad();
                ServiceResult<TokenDto> result = new ServiceResult<TokenDto>();
                try
                {
                    String paramHash = _app.Configuracao.CodigoEmpresa;
                    String paramIdentificacao = _app.Configuracao.Imei;
                    String paramIdSistemaOperacional;


                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        paramIdSistemaOperacional =
                            ((byte)EnumSistemaOperacional.iOS).ToString();
                    }
                    else
                    {
                        paramIdSistemaOperacional =
                            ((byte)EnumSistemaOperacional.Android).ToString();
                    }


                    CancellationTokenSource tokenSource = new CancellationTokenSource();

                    ModelUsuario bll = new ModelUsuario();
                    result = await bll.Logar(
                        Email
                        , Password
                        , null
                        , paramHash
                        //, null
                        , resultPush.Data
                        , paramIdentificacao
                        , paramIdSistemaOperacional
                        , tokenSource.Token
                    );

                    if (result.MessageError == AppResources.LoginSenhaInvalido)
                    {
                        result.MessageError = AppResources.LoginSenhaInvalido;
                    }
                    else
                    {

                        if (result.Data.Aplicativo.IsClienteBloqueado == true)
                        {
                            result.MessageError = AppResources.ClienteBloqueado;
                        }


                        if (result.Data.Aplicativo.IsClienteAtivo == false)
                        {
                            result.MessageError = AppResources.ClienteDesativado;
                        }

                    }

                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);

                }
                finally
                {
                    LoginFinish(result);
                }

            });
        }

        private void SetPreferences(String User, String Pass)
        {
            Application.Current.Properties["User"] = User;
            Application.Current.Properties["Pass"] = Pass;
        }

        private void LoginFinish(
            ServiceResult<TokenDto> paramResult
        )
        {
            try
            {


                if (String.IsNullOrWhiteSpace(paramResult.MessageError))
                {

                    Application.Current.Properties["IsLogado"] = true;

                    TokenRealm _tokenRealm = new TokenRealm()
                    {
                        Id = 1,
                        Access_Token = paramResult.Data.Access_Token,
                        expires_in = paramResult.Data.expires_in,
                        IsUsuarioAdminPadrao = paramResult.Data.IsUsuarioAdminPadrao,
                        UrlLogo = paramResult.Data.UrlLogo
                    };


                    Application.Current.Properties["UrlLogo"] = _tokenRealm.UrlLogo;


                    if (paramResult.Data.LstFuncao != null)
                    {
                        _tokenRealm.LstFuncao = "|" + String.Join("|", paramResult.Data.LstFuncao.ToArray()) + "|";
                    }

                    if (
                        paramResult.Data.TempoTransmissao.HasValue
                        && paramResult.Data.TempoTransmissao.Value > 0
                    )
                    {
                        if (paramResult.Data.TempoTransmissao.Value < _app.Configuracao.TempoTransmissaoPadrao)
                            _tokenRealm.TempoTransmissao = _app.Configuracao.TempoTransmissaoPadrao;
                        else
                            _tokenRealm.TempoTransmissao = paramResult.Data.TempoTransmissao.Value;

                    }
                    else
                    {
                        _tokenRealm.TempoTransmissao = _app.Configuracao.TempoTransmissaoPadrao;
                    }



                    if (paramResult.Data.Aplicativo != null)
                    {
                        _tokenRealm.IP = paramResult.Data.Aplicativo.IP;
                        _tokenRealm.IsLocator = paramResult.Data.Aplicativo.IsLocator;
                        _tokenRealm.IdAplicativo = paramResult.Data.Aplicativo.IdAplicativo;
                        _tokenRealm.Porta = paramResult.Data.Aplicativo.Porta;
                        _tokenRealm.Identificacao = Convert.ToInt64(paramResult.Data.Aplicativo.Identificacao);
                    }



#if DEBUG
                        // TESTE MYCHELLE
                        //_tokenRealm.IP = "200.152.54.164";
#endif


                    TokenDataStore dataSource = new TokenDataStore();

                    dataSource.CreateUpadate(_tokenRealm);

                    Token _token = new Token();
                    _token.TransformFromRealm(_tokenRealm);
                    _app.Token = _token;

                    PosicaoDataStore posicaoStore = new PosicaoDataStore();
                    posicaoStore.Add(paramResult.Data.LastPositions);

                    CancellationTokenSource tokenSource = new CancellationTokenSource();
                    ModelPushNotification classPushNotification = new ModelPushNotification();
                    classPushNotification.RegistraPushKey(tokenSource.Token);

                    _app.Util.SaveEmail(Email);

                    _navigationService.NavigateToListaUnidadeRastreadaPage();

                }
                else
                {

                    ShowErrorAlert(paramResult.MessageError);
                }

            }
            catch (Exception ex)
            {
                _messageService.ShowAlertAsync(
                    AppResources.ErroSuporte
                    , AppResources.Error
                );

                Crashes.TrackError(ex);
            }
            finally
            {
                LoginIsEnabled = true;
                _view.EscondeLoad();
            }
        }

    }
#pragma warning restore CS1998
#pragma warning restore RECS0022
#pragma warning restore CS4014
}