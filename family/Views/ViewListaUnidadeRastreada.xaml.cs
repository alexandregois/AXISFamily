using System;
using family.CustomElements;
using family.ViewModels;
using family.Views.Interfaces;
using family.Views.Template;
using Xamarin.Forms;
using family.Views.Services;
using family.Domain.Dto;
using family.Domain.Enum;
using family.Domain.Realm;
using family.ViewModels.InterfaceServices;
using family.CrossPlataform;
using Microsoft.AppCenter.Crashes;
using family.Services.ServiceRealm;

namespace family.Views
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
    public partial class ViewListaUnidadeRastreada : ContentPage, IListaUnidadeRastreada
    {
        private readonly IMessageService _messageService;

        private App _app => (Application.Current as App);

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

        private ViewModelListaUnidadeRastreada _viewModel { get; set; }

        private ViewModelConfiguracao viewModelConfiguracao { get; set; }

        private Boolean _boolStartPeriodic { get; set; }

        //private readonly INetworkProvider _networkProvider;

        private CustomDialogAlert _dialogAlert = null;
        public CustomDialogAlert DialogAlert
        {
            get
            {
                if (_dialogAlert == null)
                {
                    _dialogAlert = new CustomDialogAlert(Panel, Color.FromHex("#80000000"));
                    _dialogAlert.ShadowBox_TapDelegate += (sender, args) =>
                    {
                        //AtivaBotoes();
                    };
                }

                return _dialogAlert;
            }
            set
            {
                _dialogAlert = value;
            }
        }

        private ActivityIndicator _loader = null;
        private ActivityIndicator Loader
        {
            get
            {
                if (_loader == null)
                {
                    _loader = DialogAlert.RequireActivityIndicator();
                    _loader.Color = Color.FromHex("#7ff3ff");
                }
                return _loader;
            }
            set
            {
                _loader = value;
            }
        }

        public ViewListaUnidadeRastreada()
        {

            InitializeComponent();

            try
            {

                PanelGeral.ControlTemplate =
                    new ControlTemplate(typeof(DefaultTemplate));

                _viewModel = new ViewModelListaUnidadeRastreada();
                _viewModel._view = this as IListaUnidadeRastreada;
                this.BindingContext = _viewModel;

                PanelLista.ItemTemplate = new DataTemplate(() =>
                {
                    return new PanelLista_ViewCell(this.BindingContext);
                });


                viewModelConfiguracao = new ViewModelConfiguracao();


                this._messageService =
                        DependencyService.Get<IMessageService>();


                viewModelConfiguracao.CheckKeepAlive();


                BuildTemplatePage();

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }


        }

        protected override void OnAppearing()
        {
            try
            {

                //ScrollViewUnidades.ScrollToAsync(0, 150, true);

                Color statusColor = Color.FromHex("#0090bc");

                _app.Util.changeColorStatusBar(statusColor, this);

                _viewModel.OnAppearing();

                BuildTemplatePage();

                ServiceStart();

                viewModelConfiguracao.Lista_TelefoneAction();

                //EscondeLoad();

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

        }

        protected override void OnDisappearing()
        {
            _viewModel.OnDisappearing();
        }

        public void BuildTemplatePage()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Double paramAlturaBotoes = _viewModel.DefaultButtonHeight;
                Double qntLinha = ExibirMenuFooter();

                PanelMenu.HeightRequest = (paramAlturaBotoes * qntLinha);

                PanelLista.HeightRequest =
                    _app.DefaultTemplateHeightContent
                    - PanelMenu.HeightRequest;

                EscondeLoad();
            });
        }

        private async void ListUnidades_ItemTapped(
            object sender
            , ItemTappedEventArgs e
        )
        {

            PosicaoUnidadeRastreadaRealm dataItem = e.Item as PosicaoUnidadeRastreadaRealm;

            _viewModel.NavegateMapa(
                EnumPage.UltimaPosicao
                , dataItem.IdRastreador
            );

        }

        private Double ExibirMenuFooter()
        {

            //viewModelConfiguracao.Lista_TelefoneAction();

            Double qntButtons = 1;
            Double qntLinhas = 1;

            try
            {

                PermissaoService permissao = new PermissaoService();

                if (_app.Token.IsLocator)
                {
                    if (permissao.PodeEnviarPanico())
                    {
                        _viewModel.BtnPanicoIsVisible = true;
                        _viewModel.BtnAddDispositivoIsVisible = false;
                        qntLinhas++;

                        //Util.TrackService(true);
                    }


                }
                else if (permissao.PodeTornarAppUnidadeRastreada())
                {

                    Boolean solicitacaoRastreamentoEmAndamento = false;

                    if (Application.Current.Properties.ContainsKey("SolicitacaoRastreamentoEmAndamento"))
                    {
                        if (Application.Current.Properties["SolicitacaoRastreamentoEmAndamento"] != null)
                            solicitacaoRastreamentoEmAndamento = (Boolean)Application.Current.Properties["SolicitacaoRastreamentoEmAndamento"];
                    }

                    if (solicitacaoRastreamentoEmAndamento == false)
                    {
                        _viewModel.BtnPanicoIsVisible = false;
                        _viewModel.BtnAddDispositivoIsVisible = true;

                        qntButtons++;
                    }
                    else
                    {
                        _viewModel.BtnPanicoIsVisible = false;
                        _viewModel.BtnAddDispositivoIsVisible = false;

                        //_viewModel.BtnsWidthRequestFooter = (_viewModel.DefaultWidth / 2);
                    }


                }

                if (_app.ListaTelefone != null && _app.ListaTelefone.Count > 0)
                {
                    qntButtons++;
                }


                _viewModel.BtnsWidthRequest = (_viewModel.DefaultWidth / qntButtons);

                if (_app.ListaTelefone != null)
                {
                    if (_app.ListaTelefone.Count > 0)
                    {
                        if (permissao.PodeTornarAppUnidadeRastreada())
                        {
                            if (!_app.Token.IsLocator)
                            {

                                if (Application.Current.Properties.ContainsKey("SolicitacaoRastreamentoEmAndamento"))
                                {
                                    if ((Boolean)Application.Current.Properties["SolicitacaoRastreamentoEmAndamento"] == false)
                                        _viewModel.BtnsWidthRequestFooter = (_viewModel.DefaultWidth / 3);
                                    else
                                        _viewModel.BtnsWidthRequestFooter = (_viewModel.DefaultWidth / 2);
                                }
                                else
                                    _viewModel.BtnsWidthRequestFooter = (_viewModel.DefaultWidth / 3);
                            }
                            else
                            {
                                _viewModel.BtnsWidthRequestFooter = (_viewModel.DefaultWidth / 2);
                            }

                        }
                        else
                            _viewModel.BtnsWidthRequestFooter = (_viewModel.DefaultWidth / 2);



                        _viewModel.BtnTelefoneIsVisible = true;
                    }
                    else
                    {
                        if (permissao.PodeTornarAppUnidadeRastreada())
                        {
                            if (!_app.Token.IsLocator)
                            {
                                if (_viewModel.BtnAddDispositivoIsVisible == false)
                                    _viewModel.BtnsWidthRequestFooter = (_viewModel.DefaultWidth / 1);
                                else
                                    _viewModel.BtnsWidthRequestFooter = (_viewModel.DefaultWidth / 2);
                            }
                            else
                                _viewModel.BtnsWidthRequestFooter = (_viewModel.DefaultWidth / 1);

                        }
                        else
                            _viewModel.BtnsWidthRequestFooter = (_viewModel.DefaultWidth / 1);



                        _viewModel.BtnTelefoneIsVisible = false;

                    }
                }
                else
                {

                    if (permissao.PodeTornarAppUnidadeRastreada())
                    {
                        if (!_app.Token.IsLocator)
                            _viewModel.BtnsWidthRequestFooter = (_viewModel.DefaultWidth / 2);
                        else
                            _viewModel.BtnsWidthRequestFooter = (_viewModel.DefaultWidth / 1);

                    }
                    else
                        _viewModel.BtnsWidthRequestFooter = (_viewModel.DefaultWidth / 1);



                    _viewModel.BtnTelefoneIsVisible = false;

                }

            }
            catch (Exception ex)
            {

                Crashes.TrackError(ex);
            }

            return qntLinhas;

        }

        #region Interface
        public void EscondeLoad()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                DialogAlert.HideAndCleanAlert();
                DialogAlert.Destroy();
                DialogAlert = null;

            });
        }

        public void ExibirLoad()
        {
            Device.BeginInvokeOnMainThread(() => ShowAlert(Loader));

        }

        public void BeginRefresh()
        {
            Boolean boolismessage = false;

            if (Application.Current.Properties.ContainsKey("IsMessage"))
            {
                if (Application.Current.Properties["IsMessage"] != null)
                    boolismessage = (Boolean)Application.Current.Properties["IsMessage"];
            }

            if (boolismessage == false)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    PanelLista.BeginRefresh();

                    BuildTemplatePage();

                    ServiceStart();

                });
            }
        }

        public void ServiceStart()
        {

            TokenDataStore tokenDataSource = new TokenDataStore();
            TokenRealm _tokenRealm = null;

            Int32 tempoTransmissao = _app.Configuracao.TempoTransmissaoPadrao;

            Boolean retornoTimer = false;

            if (_boolStartPeriodic == false)
            {

                Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(tempoTransmissao), () =>
                {

                    if (_app.Configuracao.IsLogado == true)
                    {
                        PermissaoService permissao = new PermissaoService();

                        if (_app.Token.IsLocator)
                        {
                            if (permissao.PodeEnviarPanico())
                            {
                                _app.Util.TrackService(true);
                            }
                        }
                    }

                    if (tokenDataSource.Get(1) != null)
                    {
                        _tokenRealm = tokenDataSource.Get(1);

                        if (_tokenRealm.TempoTransmissao > 0)
                        {
                            retornoTimer = false;
                            _boolStartPeriodic = true;
                            ServiceStartPeriodic(_tokenRealm.TempoTransmissao);

                        }

                    }
                    else
                    {
                        retornoTimer = true;
                    }


                    return retornoTimer;

                });
            }

        }


        public void ServiceStartPeriodic(Int32 tempoTransmissao)
        {

            Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(tempoTransmissao), () =>
            {

                if (_app.Configuracao.IsLogado == true)
                {
                    PermissaoService permissao = new PermissaoService();

                    if (_app.Token.IsLocator)
                    {
                        if (permissao.PodeEnviarPanico())
                        {
                            _app.Util.TrackService(true);
                        }
                    }
                }

                return true;

            });

        }


        public void EndRefresh()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                PanelLista.EndRefresh();
                EscondeLoad();
            });
        }


        public void MakeFrameDefault(
            ref Frame paramFrame
            , ref StackLayout paramContent
        )
        {

            paramFrame = DialogAlert.RequireFramePadrao();
            paramContent.Margin = new Thickness(3);
        }

        public void ShowAlert(View paramView)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (DialogAlert != null)
                {
                    DialogAlert.HideAndCleanAlert();
                    DialogAlert.Destroy();
                    DialogAlert = null;
                }
                DialogAlert.ShowAlert(paramView);
            });
        }

        public void Is_True()
        {
            throw new NotImplementedException();
        }

        public void Is_False()
        {
            throw new NotImplementedException();
        }

        #endregion

        public void OpenStreetview(Boolean paramExibe) { }
        public void CloseStreetview(Boolean paramOnAppear) { }

    }
#pragma warning restore CS1998
#pragma warning restore RECS0022
#pragma warning restore CS4014
}