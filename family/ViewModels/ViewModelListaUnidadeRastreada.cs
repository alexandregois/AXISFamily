using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using family.Domain.Dto;
using family.Domain.Enum;
using family.Domain.Realm;
using family.Model;
using family.Resx;
using family.Services.ServiceRealm;
using family.ViewModels.Base;
using family.Views.Interfaces;
using Xamarin.Forms;

using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.InAppBilling.Abstractions;
using Plugin.InAppBilling;

namespace family.ViewModels
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
    public class ViewModelListaUnidadeRastreada : ViewModelBase
    {
        public IListaUnidadeRastreada _view { get; set; }

        private Boolean _enviaPanico { get; set; }

        public PosicaoDataStore posicaoStore { get; set; }

        public ICommand BtnPanicoCommand { get; set; }
        public ICommand BtnDeviceCommand { get; set; }
        public ICommand BtnConviteCommand { get; set; }
        public ICommand BtnConfiguracaoCommand { get; set; }

        public ICommand BtnTelefoneCommand { get; set; }

        public CancellationTokenSource Tokensource { get; set; }

        private ServiceResult<RetornoSolicitacaoRastreamentoDto> resultRastreamento { get; set; }



        private bool _btnEnabled;
        public bool BtnEnabled
        {
            get
            {
                return _btnEnabled;
            }
            set
            {
                _btnEnabled = value;
                this.Notify("BtnEnabled");
            }
        }

        private Double _defaultButtonHeight;
        public Double DefaultButtonHeight
        {
            get
            {
                return _defaultButtonHeight;
            }
            set
            {
                _defaultButtonHeight = value;
                this.Notify("DefaultButtonHeight");
            }
        }

        public ICommand PanelLista_RefreshCommand { get; set; }

        private bool _panelLista_IsRefreshing;
        public bool PanelLista_IsRefreshing
        {
            get
            {
                return _panelLista_IsRefreshing;
            }
            set
            {
                _panelLista_IsRefreshing = value;
                this.Notify("PanelLista_IsRefreshing");
            }
        }

        private bool _panelLista_Enabled;
        public bool PanelLista_Enabled
        {
            get
            {
                return _panelLista_Enabled;
            }
            set
            {
                _panelLista_Enabled = value;
                this.Notify("PanelLista_Enabled");
            }
        }


        private Double _panelLista_RowHeight;
        public Double PanelLista_RowHeight
        {
            get
            {
                return _panelLista_RowHeight;
            }
            set
            {
                _panelLista_RowHeight = value;
                this.Notify("PanelLista_RowHeight");
            }
        }


        private List<PosicaoUnidadeRastreadaRealm> _panelLista_ItemsSource
        = new List<PosicaoUnidadeRastreadaRealm>();
        public List<PosicaoUnidadeRastreadaRealm> PanelLista_ItemsSource
        {
            get
            {
                return _panelLista_ItemsSource;
            }
            set
            {
                _panelLista_ItemsSource = value;
                this.Notify("PanelLista_ItemsSource");
            }
        }

        private ModelUnidadeRastreada _modelUnidadeRastreada;
        public ModelUnidadeRastreada ModelUnidadeRastreada
        {
            get
            {
                if (_modelUnidadeRastreada == null)
                    _modelUnidadeRastreada = new ModelUnidadeRastreada();
                return _modelUnidadeRastreada;
            }
        }

        private ModelTelefone _modelTelefone;
        public ModelTelefone ModelTelefone
        {
            get
            {
                if (_modelTelefone == null)
                    _modelTelefone = new ModelTelefone();
                return _modelTelefone;
            }
        }

        private bool _btnPanicoIsVisible = false;
        public bool BtnPanicoIsVisible
        {
            get
            {
                return _btnPanicoIsVisible;
            }
            set
            {
                _btnPanicoIsVisible = value;
                this.Notify("BtnPanicoIsVisible");
            }
        }

        private bool _btnAddDispositivoIsVisible = false;
        public bool BtnAddDispositivoIsVisible
        {
            get
            {
                return _btnAddDispositivoIsVisible;
            }
            set
            {
                _btnAddDispositivoIsVisible = value;
                this.Notify("BtnAddDispositivoIsVisible");
            }
        }

        private bool _btnPodeConvidarIsVisible = false;
        public bool BtnPodeConvidarIsVisible
        {
            get
            {
                return _btnPodeConvidarIsVisible;
            }
            set
            {
                _btnPodeConvidarIsVisible = value;
                this.Notify("BtnPodeConvidarIsVisible");
            }
        }

        private bool _btnTelefoneIsVisible = false;
        public bool BtnTelefoneIsVisible
        {
            get
            {
                return _btnTelefoneIsVisible;
            }
            set
            {
                _btnTelefoneIsVisible = value;
                this.Notify("BtnTelefoneIsVisible");
            }
        }

        private Double _btnsWidthRequest;
        public Double BtnsWidthRequest
        {
            get
            {
                return _btnsWidthRequest;
            }
            set
            {
                _btnsWidthRequest = value;
                this.Notify("BtnsWidthRequest");
            }
        }

        private Double _btnsWidthRequestFooter;
        public Double BtnsWidthRequestFooter
        {
            get
            {
                return _btnsWidthRequestFooter;
            }
            set
            {
                _btnsWidthRequestFooter = value;
                this.Notify("BtnsWidthRequestFooter");
            }
        }


        public ViewModelListaUnidadeRastreada()
        {
            this.DefaultTemplateBuild();

            VoltarVisible = false;

            this.BtnEnabled = true;

            this.DefaultButtonHeight = 64;

            this.BtnPanicoCommand = new Command(this.PanicoAction);
            this.BtnDeviceCommand = new Command(this.DeviceAction);
            //this.BtnConviteCommand = new Command(this.ConviteAction);
            this.BtnConfiguracaoCommand = new Command(this.ConfiguracaoAction);
            this.BtnTelefoneCommand = new Command(this.TelefoneAction);

            PanelLista_Enabled = false;

            this.PanelLista_RowHeight = 70;
            this.PanelLista_RefreshCommand = new Command(this.PanelLista_RefreshAction);

            posicaoStore = new PosicaoDataStore();

        }

        public async Task<InAppBillingProduct> ObterInformacaoProdutoLojaAsync()
        {
            InAppBillingProduct product = null;

            if (!CrossInAppBilling.IsSupported)
                return product;

            try
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                ModelUsuario modelUsuario = new ModelUsuario();

                ServiceResult<Boolean> resultCompra = new ServiceResult<bool>();
                resultCompra = await modelUsuario.RetornaCompra_Realizada(tokenSource.Token);

                //BOTÃO CONTRATAR
                if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
                {

                    if (resultCompra.Data == null || resultCompra.Data == false)
                    { 

                        IInAppBilling billing = CrossInAppBilling.Current;
                        Boolean connected = await CrossInAppBilling.Current.ConnectAsync(ItemType.InAppPurchase);

                        if (!connected)
                        {
                            throw new Exception("Não foi possível conecta com InAppPurchase.");
                        }

                        IEnumerable<InAppBillingProduct> items = await billing.GetProductInfoAsync(ItemType.InAppPurchase,
                            new string[] { "br.com.systemsat.family.fullaccess" });

                        foreach (var item in items)
                        {
                            this.Notify("Name: " + item.Name + " Value: " + item.MicrosPrice);
                            product = item;
                        }


                        ContratarAction();

                    }
                    else
                    {

                        ServiceResult<Boolean> resultBool = new ServiceResult<Boolean>();

                        ModelUsuario model = new ModelUsuario();
                        CancellationTokenSource tokenSource1 = new CancellationTokenSource();
                        resultRastreamento = await model.RastrearDispositivo(
                                _app.Util.GetEmailLogadoFromPrefs()
                                , _app.Token.IdAplicativo
                                , _app.Util.GetIdentifier()
                                , tokenSource1.Token
                            );


                        //Application.Current.Properties["SolicitacaoRastreamentoEmAndamento"] = null;
                        Application.Current.Properties["SolicitacaoRastreamentoEmAndamento"] = true;

                        resultBool.Data = true; //resultRastreamento.Data.Aplicativo.IsLocator;

                        ViewModelConfiguracao viewModelConfiguracao = new ViewModelConfiguracao();
                        if (resultRastreamento != null && resultRastreamento.Data.Aplicativo != null)
                        {
                            viewModelConfiguracao.ChangeTempoTransmissao(resultRastreamento.Data.Aplicativo);
                        }

                        _view.BuildTemplatePage();

                        DevicePositivo_Finish(resultBool);

                    }
                    

                }

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                await CrossInAppBilling.Current.DisconnectAsync();
            }

            return product;
        }

        private async void ContratarAction()
        {

            var productId = new string[] { "br.com.systemsat.family.fullaccess" };
            var billing = CrossInAppBilling.Current;

            billing.InTestingMode = false;

            ServiceResult<Boolean> resultBool = new ServiceResult<Boolean>();

            try
            {

                var connected = await billing.ConnectAsync();//ItemType.InAppPurchase, productId);
                if (!connected)
                    //Couldn't connect to billing
                    return;

                //var items = await billing.GetProductInfoAsync(ItemType.InAppPurchase, productId);
                //var item = items.First();

                //var responseApple = await billing.ConsumePurchaseAsync((ItemType.InAppPurchase, productId);


                //try to purchase item
                var purchase = await billing.PurchaseAsync(productId[0], ItemType.InAppPurchase, Guid.NewGuid().ToString());
                if (purchase == null)
                {
                    //Not purchased, may also throw excpetion to catch
                    resultBool.Data = false;
                }
                else
                {
                    CancellationTokenSource tokenSource = new CancellationTokenSource();
                    ModelUsuario modelUsuario = new ModelUsuario();

                    ServiceResult<Boolean> resultCompra = new ServiceResult<bool>();
                    resultCompra = await modelUsuario.Grava_CompraRealizada(tokenSource.Token);
                    
                    ModelUsuario model = new ModelUsuario();
                    CancellationTokenSource tokenSource1 = new CancellationTokenSource();
                    resultRastreamento = await model.RastrearDispositivo(
                            _app.Util.GetEmailLogadoFromPrefs()
                            , _app.Token.IdAplicativo
                            , _app.Util.GetIdentifier()
                            , tokenSource1.Token
                        );


                    //Application.Current.Properties["SolicitacaoRastreamentoEmAndamento"] = null;
                    Application.Current.Properties["SolicitacaoRastreamentoEmAndamento"] = true;

                    resultBool.Data = true; //resultRastreamento.Data.Aplicativo.IsLocator;


                    ViewModelConfiguracao viewModelConfiguracao = new ViewModelConfiguracao();
                    if (resultRastreamento != null && resultRastreamento.Data.Aplicativo != null)
                    {
                        viewModelConfiguracao.ChangeTempoTransmissao(resultRastreamento.Data.Aplicativo);
                    }

                }

            }
            catch (InAppBillingPurchaseException purchaseEx)
            {
                //Quando dá cancelar na tela que solicita a senha da store

                _messageService.ShowAlertAsync(
                    purchaseEx.Message
                    , AppResources.Error
                );

                resultBool.Data = false;

            }
            catch (Exception ex)
            {
                //Debug.WriteLine("Issue connecting: " + ex);

                _messageService.ShowAlertAsync(
                    ex.Message
                    , AppResources.Error
                );

                resultBool.Data = false;

            }
            finally
            {

                //Disconnect, it is okay if we never connected, this will never throw an exception
                await billing.DisconnectAsync();

                _view.BuildTemplatePage();

                DevicePositivo_Finish(resultBool);
            }


        }

        public override void OnAppearing()
        {
            try
            {

                this.DefaultTemplateBuild();

                this.BtnEnabled = true;

                if (Tokensource != null)
                    Tokensource.Cancel();

                Tokensource = new CancellationTokenSource();

                TimeSpan loopExpress = TimeSpan.FromSeconds(1);

                PanelLista_ItemsSource = posicaoStore.List().ToList();


                if (
                    PanelLista_ItemsSource != null
                    && PanelLista_ItemsSource.Count > 0
                   )
                {
                    PosicaoUnidadeRastreadaRealm posi = PanelLista_ItemsSource[0];

                    if (
                        (DateTime.UtcNow - posi.DataEvento).TotalSeconds
                        > _app.Token.TempoTransmissao
                    )
                    {
                        Loop(loopExpress);
                    }
                    else
                    {
                        PanelLista_Enabled = true;
                        Loop(_app.Configuracao.LoopTimeSpan);
                    }
                }
                else
                {
                    Loop(loopExpress);
                }

            }
            catch (Exception ex)
            {

                //throw;
            }

        }

        public override void OnDisappearing()
        {
            Tokensource.Cancel(false);
        }

        public override void OnLayoutChanged()
        {
            //throw new NotImplementedException();
        }

        private void DefaultTemplateBuild()
        {
            VoltarVisible = false;

            PageColor = Color.FromHex("#3FC0EF");
            ImageSourceProperty = "ic_busca.png";
            ImageWidthProperty = 30;

            Button refreshButton = new Button()
            {
                Image = "ic_refresh.png",
                Command = new Command(Refresh_Tap),
                HeightRequest = 53,
                WidthRequest = 55,
                Margin = new Thickness(0),
                BorderRadius = 0,
                BorderWidth = 0,
                BorderColor = Color.Transparent,
                BackgroundColor = Color.Transparent
            };

            BoxRightContent = refreshButton;

            Label labelTemp = PanelTituloLabel_Titulo();
            labelTemp.Text = AppResources.Select;
            labelTemp.HeightRequest = _app.DefaultTemplateHeightNavegationBar;
            BoxMiddleContent = labelTemp;

        }

        private void Refresh_Tap(object arg)
        {
            if (!PanelLista_IsRefreshing)
            {

                Tokensource.Cancel();
                Tokensource = new CancellationTokenSource();
                PanelLista_Enabled = false;
                _view.BeginRefresh();
            }

        }

        private void TelefoneAction(object obj)
        {
            _navigationService.NavigateToTelefone();
            //Device.OpenUri(new Uri(String.Format("tel:{0}", "21969364042")));
        }

        private void ConfiguracaoAction(object obj)
        {
            BtnEnabled = false;
            PanelLista_Enabled = false;
            _navigationService.NavigateToConfiguracao();
        }

        #region ListaUnidadeRastreada
        private async Task Loop(TimeSpan paramTimeSpan)
        {
            //Device.BeginInvokeOnMainThread(async () =>
            //{
            try
            {
                await Task.Delay(paramTimeSpan, Tokensource.Token);

                if (!Tokensource.IsCancellationRequested)
                    Refresh_Tap(null);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }

            //});
        }

        private void PanelLista_RefreshAction(object obj)
        {
            try
            {

                Task.Run(async () =>
                {
                    try
                    {

                        ServiceResult<List<PosicaoUnidadeRastreada>> result
                        = await ModelUnidadeRastreada.GetUnidadeRastreada(Tokensource.Token);

                        if (!Tokensource.IsCancellationRequested)
                        {
                            UpdateToken(result.RefreshToken);
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                if (String.IsNullOrWhiteSpace(result.MessageError))
                                {
                                    posicaoStore.Clean();
                                    posicaoStore.Add(result.Data);
                                    PanelLista_ItemsSource = posicaoStore.List().ToList();

                                    _app._countClick = 0;
                                }
                                else
                                {
                                    String erroMessage;
                                    try
                                    {
                                        erroMessage =
                                            AppResources
                                                .ResourceManager
                                                .GetString(result.MessageError);

                                    }
                                    catch (Exception ex)
                                    {

                                        Crashes.TrackError(ex);
                                    }

                                }

                                _view.EndRefresh();

                                PanelLista_Enabled = true;

                                if (!Tokensource.IsCancellationRequested)
                                {
                                    Tokensource = new CancellationTokenSource();
                                    Loop(_app.Configuracao.LoopTimeSpan);
                                }

                            });

                        }
                    }
                    catch (Exception ex)
                    {
                        //_messageService.ShowAlertAsync(
                        //	AppResources.ErroSuporte
                        //	, AppResources.Error
                        //);

                        _view.EndRefresh();

                        Crashes.TrackError(ex);

                    }
                }, Tokensource.Token);
            }
            catch (Exception ex)
            {
                //_messageService.ShowAlertAsync(
                //	AppResources.ErroSuporte
                //	, AppResources.Error
                //);

                _view.EndRefresh();

                Crashes.TrackError(ex);
            }
        }

        public void NavegateMapa(
            EnumPage paramPartialPage
            , int paramIdRastreador
        )
        {
            BtnEnabled = false;
            _navigationService.NavigateToMapa(
                paramPartialPage
                , paramIdRastreador
            );
        }
        #endregion

        //#region Convite
        //private void ConviteAction(object obj)
        //{
        //	throw new NotImplementedException();
        //}
        //#endregion

        #region SeRastrear
        private void DeviceAction(object obj)
        {
            try
            {

                this.BtnEnabled = false;

                Frame novoFrame = new Frame();
                StackLayout content = new StackLayout();

                _view.MakeFrameDefault(ref novoFrame, ref content);

                Thickness margin = new Thickness(0, 0, 0, 10);
                Label titulo = new Label()
                {
                    Text = AppResources.TrackDevice,
                    FontSize = 18,
                    FontAttributes = FontAttributes.Bold,
                    Margin = margin
                };
                content.Children.Add(titulo);

                Label textConteudo = new Label()
                {
                    FontSize = 16,
                    Margin = margin
                };


                Application.Current.Properties["IsMessage"] = true;


                Task.Run(async () =>
            {
                _view.ExibirLoad();
                ServiceResult<MensagemSistemaDto> result = new ServiceResult<MensagemSistemaDto>();

                CancellationTokenSource tokenSource = new CancellationTokenSource();

                try
                {
                    ModelKeepAlive modelKeepAlive = new ModelKeepAlive();

                    if (_app.Token.IsUsuarioAdminPadrao)
                    {
                        result = await modelKeepAlive.RetornaMensagemSistema(6, tokenSource.Token);
                        if (result != null && result.Data != null)
                            textConteudo.Text = result.Data.Texto;  //AppResources.TrackDeviceAdmin;
                    }
                    else
                    {
                        result = await modelKeepAlive.RetornaMensagemSistema(7, tokenSource.Token);
                        if (result != null && result.Data != null)
                            textConteudo.Text = result.Data.Texto;  //AppResources.TrackDeviceNotAdmin;
                    }


                    content.Children.Add(textConteudo);

                    StackLayout contentAction = new StackLayout();
                    contentAction.Orientation = StackOrientation.Horizontal;
                    contentAction.WidthRequest = novoFrame.WidthRequest;
                    contentAction.HeightRequest = 40;

                    Double larguraPedida = (contentAction.WidthRequest / 2) - 10;

                    Button positiveButton = new Button()
                    {
                        BorderColor = Color.Transparent,
                        BorderWidth = 0,
                        BorderRadius = 0,
                        Command = new Command(DevicePositivo),
                        Text = "OK",
                        WidthRequest = larguraPedida,
                        HeightRequest = contentAction.HeightRequest,
                        Margin = new Thickness(0, 0, 10, 0)
                    };
                    contentAction.Children.Add(positiveButton);

                    Button negativeButton = new Button()
                    {
                        BorderColor = Color.Transparent,
                        BorderWidth = 0,
                        BorderRadius = 0,
                        Command = new Command(DeviceNegativo),
                        Text = AppResources.Cancel,
                        WidthRequest = larguraPedida,
                        HeightRequest = contentAction.HeightRequest,
                        Margin = new Thickness(0)
                    };
                    contentAction.Children.Add(negativeButton);

                    content.Children.Add(contentAction);

                    novoFrame.Content = content;

                    _view.ShowAlert(novoFrame);

                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }

            });


            }
            catch (Exception)
            {
                this.BtnEnabled = true;
            }
        }

        public void DevicePositivo()
        {

            Application.Current.Properties["IsMessage"] = false;

            resultRastreamento = new ServiceResult<RetornoSolicitacaoRastreamentoDto>();

            ServiceResult<Boolean> resultBool = new ServiceResult<Boolean>();

            try
            {
                _view.ExibirLoad();


                Task.Run(async () =>
                {
                    try
                    {

                        if (Device.RuntimePlatform == Device.iOS)
                        {
                            ObterInformacaoProdutoLojaAsync();
                        }
                        else
                        {

                            ModelUsuario model = new ModelUsuario();
                            CancellationTokenSource tokenSource = new CancellationTokenSource();
                            resultRastreamento = await model.RastrearDispositivo(
                                    _app.Util.GetEmailLogadoFromPrefs()
                                    , _app.Token.IdAplicativo
                                    , _app.Util.GetIdentifier()
                                    , tokenSource.Token
                                );


                            //Application.Current.Properties["SolicitacaoRastreamentoEmAndamento"] = null;
                            Application.Current.Properties["SolicitacaoRastreamentoEmAndamento"] = true;

                            resultBool.Data = true; //resultRastreamento.Data.Aplicativo.IsLocator;


                            ViewModelConfiguracao viewModelConfiguracao = new ViewModelConfiguracao();
                            if (resultRastreamento != null && resultRastreamento.Data.Aplicativo != null)
                            {
                                viewModelConfiguracao.ChangeTempoTransmissao(resultRastreamento.Data.Aplicativo);
                            }

                            _view.BuildTemplatePage();

                            DevicePositivo_Finish(resultBool);
                        }

                    }
                    catch (Exception ex)
                    {
                        if (ex.ToString().IndexOf("Realms") < 0)
                        {
                            resultRastreamento.IsValid = false;
                            resultRastreamento.MessageError = "Exception";
                        }

                        Crashes.TrackError(ex);

                    }
                    finally
                    {

                    }

                });

            }
            catch (Exception ex)
            {
                if (ex.ToString().IndexOf("Realms") < 0)
                {
                    resultRastreamento.IsValid = false;
                    resultRastreamento.MessageError = "Exception";
                    DevicePositivo_Finish(resultBool);

                }

                Crashes.TrackError(ex);
            }
        }

        private async void DevicePositivo_Finish(ServiceResult<Boolean> paramResult)
        {
            try
            {
                _view.EscondeLoad();
                UpdateToken(paramResult.RefreshToken);
                if (String.IsNullOrWhiteSpace(paramResult.MessageError))
                {

                    Task.Run(async () =>
                    {
                        try
                        {
                            _messageService.ShowAlertAsync(resultRastreamento.Data.Mensagem,
                           AppResources.RastrearDispositivoSucesso);


                            //Executa o KeepAlive
                            ViewModelConfiguracao viewModelConfiguracao = new ViewModelConfiguracao();
                            viewModelConfiguracao.CheckKeepAlive();


                            _view.BuildTemplatePage();

                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }

                    });


                }
                else
                {
                    ShowErrorAlert(paramResult.MessageError);
                }

                BtnEnabled = true;
            }
            catch (Exception ex)
            {
                if (ex.ToString().IndexOf("Realms") < 0)
                {
                    ShowErrorAlert("Exception");
                }
            }
        }

        public void DeviceNegativo(object sender)
        {
            Application.Current.Properties["IsMessage"] = false;

            BtnEnabled = true;
            _view.EscondeLoad();
        }
        #endregion

        #region Panico
        private void PanicoAction(object obj)
        {
            BtnEnabled = false;
            Frame novoFrame = new Frame();
            StackLayout content = new StackLayout();

            _view.MakeFrameDefault(ref novoFrame, ref content);

            Thickness margin = new Thickness(0, 0, 0, 10);
            Label titulo = new Label()
            {
                Text = AppResources.PanicAlert,
                FontSize = 18,
                FontAttributes = FontAttributes.Bold,
                Margin = margin
            };
            content.Children.Add(titulo);

            Label text = new Label()
            {
                FontSize = 16,
                Text = AppResources.PanicAlertSendExplanation,
                Margin = margin
            };
            content.Children.Add(text);

            StackLayout contentAction = new StackLayout();
            contentAction.Orientation = StackOrientation.Horizontal;
            contentAction.WidthRequest = novoFrame.WidthRequest;
            contentAction.HeightRequest = 40;

            Double larguraPedida = (contentAction.WidthRequest / 2) - 10;

            Button positiveButton = new Button()
            {
                BorderColor = Color.Transparent,
                BorderWidth = 0,
                BorderRadius = 0,
                Text = AppResources.Send,
                WidthRequest = larguraPedida,
                HeightRequest = contentAction.HeightRequest,
                Margin = new Thickness(0, 0, 10, 0)
            };

            Button negativeButton = new Button()
            {
                BorderColor = Color.Transparent,
                BorderWidth = 0,
                BorderRadius = 0,
                Command = new Command(PanicoNegativo),
                Text = AppResources.Cancel,
                WidthRequest = larguraPedida,
                HeightRequest = contentAction.HeightRequest,
                Margin = new Thickness(0)
            };

            positiveButton.Command = new Command(
                () => PanicoPositivo(
                    positiveButton
                    , negativeButton
                    , negativeButton.Text
                    , contentAction.WidthRequest
                )
            );


            negativeButton.Command = new Command(
                //this.PanelLista_RefreshAction
                PanicoNegativo
            );


            contentAction.Children.Add(positiveButton);
            contentAction.Children.Add(negativeButton);

            content.Children.Add(contentAction);

            novoFrame.Content = content;

            _view.ShowAlert(novoFrame);
        }

        public void PanicoPositivo(
            Button paramPositiveButton
            , Button paramNegativeButton
            , String paramNegativeButtonText
            , Double paramLargura
        )
        {

            //Enviar alerta de pânico
            _enviaPanico = true;
            Int32 tempo = 5;
            paramNegativeButton.WidthRequest = paramLargura;
            paramNegativeButton.Text = paramNegativeButtonText + " " + tempo + "s";

            paramPositiveButton.IsVisible = false;


            Application.Current.Properties["IsMessage"] = true;


            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                tempo--;
                if (_enviaPanico)
                {
                    paramNegativeButton.Text = paramNegativeButtonText + " " + tempo + "s";

                    paramNegativeButton.Command = new Command(
                        //this.PanelLista_RefreshAction
                        PanicoNegativo
                    );


                    if (tempo == 0)
                    {
                        PanicoPositivo_Action();
                    }
                    else
                    {
                        return true;
                    }
                }

                return false;
            });
            BtnEnabled = true;

        }

        public async Task PanicoPositivo_Action()
        {
            try
            {
                Task.Run(async () =>
                {
                    Application.Current.Properties["IsMessage"] = false;

                    ModelUsuario modelUsuario = new ModelUsuario();
                    await modelUsuario.EnviaPanico();

                    _view.EscondeLoad();
                    Refresh_Tap(null);

                });
            }
            catch (Exception ex)
            {
                if (ex.ToString().IndexOf("Realms") < 0)
                {
                    _messageService.ShowAlertAsync(
                    AppResources.ErroSuporte
                    , AppResources.Error
                    );
                }

                _view.EscondeLoad();

                Crashes.TrackError(ex);

            }

        }

        public void PanicoNegativo(object sender)
        {
            Application.Current.Properties["IsMessage"] = false;

            BtnEnabled = true;
            _enviaPanico = false;
            Refresh_Tap(null);
        }
        #endregion


    }
#pragma warning restore CS1998
#pragma warning restore RECS0022
#pragma warning restore CS4014
}