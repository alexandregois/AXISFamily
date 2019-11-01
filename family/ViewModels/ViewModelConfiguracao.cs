using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using family.CrossPlataform;
using family.Domain;
using family.Domain.Dto;
using family.Domain.Enum;
using family.Domain.Realm;
using family.Model;
using family.Resx;
using family.Services.ServiceRealm;
using family.ViewModels.Base;
using family.ViewModels.InterfaceServices;
using family.Views;
using family.Views.Interfaces;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;
using Newtonsoft.Json;
using Plugin.InAppBilling;
using Plugin.InAppBilling.Abstractions;
using Plugin.LocalNotifications;
using Xamarin.Forms;

namespace family.ViewModels
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
    public class ViewModelConfiguracao : ViewModelBase
    {
        public ILoader _view { get; set; }

        private IMessage _messageToast;
        protected readonly INavigationService _navigationService;
        public static string BuyProductId = "br.com.systemsat.family.fullaccess";
        public IListaUnidadeRastreada _viewListaUnidadeRastreada { get; set; }

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

        private String _email;
        public String Email
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
        private String _contratarButtonText;
        public String ContratarButtonText
        {
            get
            {
                return _contratarButtonText;
            }
            set
            {
                _contratarButtonText = value;
                this.Notify("ContratarButtonText");
            }
        }

        private ICommand _contratarButtonCommand;
        public ICommand ContratarButtonCommand
        {
            get
            {
                return _contratarButtonCommand;
            }
            set
            {
                _contratarButtonCommand = value;
                this.Notify("ContratarButtonCommand");
            }
        }


        private bool _btnContratarIsVisible = false;
        public bool BtnContratarIsVisible
        {
            get
            {
                return _btnContratarIsVisible;
            }
            set
            {
                _btnContratarIsVisible = value;
                this.Notify("BtnContratarIsVisible");
            }
        }

        private bool _ativaPainelComprar = false;
        public bool AtivaPainelComprar
        {
            get
            {
                return _ativaPainelComprar;
            }
            set
            {
                _ativaPainelComprar = value;
                this.Notify("AtivaPainelComprar");
            }
        }

        private bool _ativaPainelDeslogar = false;
        public bool AtivaPainelDeslogar
        {
            get
            {
                return _ativaPainelDeslogar;
            }
            set
            {
                _ativaPainelDeslogar = value;
                this.Notify("AtivaPainelDeslogar");
            }
        }

        private Thickness _margemPainelDesconectar;
        public Thickness MargemPainelDesconectar
        {
            get
            {
                return _margemPainelDesconectar;
            }
            set
            {
                _margemPainelDesconectar = value;
                this.Notify("MargemPainelDesconectar");
            }
        }

        private String _desconectarButtonText;
        public String DesconectarButtonText
        {
            get
            {
                return _desconectarButtonText;
            }
            set
            {
                _desconectarButtonText = value;
                this.Notify("DesconectarButtonText");
            }
        }

        private ICommand _desconectarButtonCommand;
        public ICommand DesconectarButtonCommand
        {
            get
            {
                return _desconectarButtonCommand;
            }
            set
            {
                _desconectarButtonCommand = value;
                this.Notify("DesconectarButtonCommand");
            }
        }

        private Double _imageBoxHeight;
        public Double ImageBoxHeight
        {
            get
            {
                return _imageBoxHeight;
            }
            set
            {
                _imageBoxHeight = value;
                this.Notify("ImageBoxHeight");
            }
        }

        private Double _imageBoxWidth;
        public Double ImageBoxWidth
        {
            get
            {
                return _imageBoxWidth;
            }
            set
            {
                _imageBoxWidth = value;
                this.Notify("ImageBoxWidth");
            }
        }

        private Thickness _imageBoxMargin;
        public Thickness ImageBoxMargin
        {
            get
            {
                return _imageBoxMargin;
            }
            set
            {
                _imageBoxMargin = value;
                this.Notify("ImageBoxMargin");
            }
        }

        private Thickness _contentBoxMargin;
        public Thickness ContentBoxMargin
        {
            get
            {
                return _contentBoxMargin;
            }
            set
            {
                _contentBoxMargin = value;
                this.Notify("ContentBoxMargin");
            }
        }

        private Double _contentBoxHeight;
        public Double ContentBoxHeight
        {
            get
            {
                return _contentBoxHeight;
            }
            set
            {
                _contentBoxHeight = value;
                this.Notify("ContentBoxHeight");
            }
        }

        private Double _contentBoxWidth;
        public Double ContentBoxWidth
        {
            get
            {
                return _contentBoxWidth;
            }
            set
            {
                _contentBoxWidth = value;
                this.Notify("ContentBoxWidth");
            }
        }

        public ViewModelConfiguracao()
        {
            this.DefaultTemplateBuild();
            DesconectarButtonCommand = new Command(this.DesconectarAction);

            Email = _app.Util.GetEmailLogadoFromPrefs();

            DesconectarButtonText = AppResources.LogOut;

            ImageBoxMargin = new Thickness(40, 0, 0, 0);
            ImageBoxWidth = 80;
            ImageBoxHeight = 80;

            ContentBoxMargin = new Thickness(5, 0, 0, 0);
            ContentBoxHeight = 115;
            ContentBoxWidth = _app.ScreenWidth - ContentBoxMargin.Left;

        }

        public override void OnAppearing()
        {
            this.DefaultTemplateBuild();
        }

        public override void OnDisappearing()
        {
        }

        public override void OnLayoutChanged()
        {
        }

        private void DefaultTemplateBuild()
        {
            VoltarVisible = true;

            PageColor = Color.FromHex("#f09356");
            ImageSourceProperty = "ic_configuracoes.png";
            ImageWidthProperty = 30;

            Label labelTemp = PanelTituloLabel_Titulo();
            labelTemp.Text = AppResources.Configuration;
            labelTemp.HeightRequest = _app.DefaultTemplateHeightNavegationBar;
            BoxMiddleContent = labelTemp;

        }

        public void Lista_TelefoneAction()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            ModelTelefone modelTelefone = new ModelTelefone();

            try
            {

                Task.Run(async () =>
                {
                    try
                    {

                        ServiceResult<List<TelefoneContatoDto>> result = await modelTelefone.GetTelefone(tokenSource.Token);

                        if (!tokenSource.IsCancellationRequested)
                        {
                            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                            {
                                if (String.IsNullOrWhiteSpace(result.MessageError))
                                {
                                    _app.ListaTelefone = result.Data;
                                }

                                if (!tokenSource.IsCancellationRequested)
                                {
                                    tokenSource = new CancellationTokenSource();
                                }
                            });

                        }
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);

                    }
                }, tokenSource.Token);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }


        public async void CheckKeepAlive()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            ModelKeepAlive modelKeepAlive = new ModelKeepAlive();

            ModelPushNotification classPushNotification = new ModelPushNotification();
            var resultPush = await classPushNotification.GetRegistraPushKey();

            Task.Run(async () =>
            {

                try
                {

                    String strUsuario = null;
                    String strSenha = null;


                    if (Application.Current.Properties.ContainsKey("User"))
                        if (Application.Current.Properties["User"] != null)
                            strUsuario = (string)Application.Current.Properties["User"];

                    if (Application.Current.Properties.ContainsKey("Pass"))
                        if (Application.Current.Properties["Pass"] != null)
                            strSenha = (string)Application.Current.Properties["Pass"];


                    String paramHash = _app.Configuracao.CodigoEmpresa;
                    String paramIdentificacao = _app.Configuracao.Imei;
                    String paramIdSistemaOperacional;


                    if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
                    {
                        paramIdSistemaOperacional =
                            ((byte)EnumSistemaOperacional.iOS).ToString();
                    }
                    else
                    {
                        paramIdSistemaOperacional =
                            ((byte)EnumSistemaOperacional.Android).ToString();
                    }


                    ServiceResult<List<Byte>> parametroKeepAlives = await modelKeepAlive.CheckKeepAlive(tokenSource.Token);

                    Boolean solicitacaoRastreamentoEmAndamento = false;
                    if (Application.Current.Properties.ContainsKey("SolicitacaoRastreamentoEmAndamento"))
                        if (Application.Current.Properties["SolicitacaoRastreamentoEmAndamento"] != null)
                            solicitacaoRastreamentoEmAndamento = (Boolean)Application.Current.Properties["SolicitacaoRastreamentoEmAndamento"];


                    if (parametroKeepAlives.Data != null && parametroKeepAlives.Data.Count > 0)
                    {

                        foreach (var item in parametroKeepAlives.Data)
                        {
                            if (Convert.ToInt32(item) == Convert.ToInt32(EnumKeepAlive.Aplicativo))
                            {

                                ServiceResult<String> result = await modelKeepAlive.GetAplicativoKeepAlive(item, tokenSource.Token);


                                if (result != null && result.Data != null && result.Data != "null")
                                {
                                    AplicativoDto resultAplicativo = JsonConvert.DeserializeObject<AplicativoDto>(result.Data);

                                    if (resultAplicativo != null)
                                    {

                                        if (resultAplicativo.IsDeleted == false)
                                        {

                                            ServiceResult<MensagemSistemaDto> resultMessage = new ServiceResult<MensagemSistemaDto>();
                                            resultMessage = await modelKeepAlive.RetornaMensagemSistema(1, tokenSource.Token);
                                            String message = resultMessage.Data.Texto.ToString().Replace("\"", "");

                                            String[] messageList = message.Split(new[] { ',' });

                                            String messageWrap = String.Empty;

                                            for (int i = 0; i < messageList.Length; i++)
                                            {
                                                if (i == 0)
                                                    messageWrap = messageList[i];
                                                else
                                                    messageWrap = messageWrap + "\n" + messageList[i];
                                            }

                                            CrossLocalNotifications.Current.Show(AppResources.Alert, messageWrap);

                                        }

                                        ChangeTempoTransmissao(resultAplicativo);


                                        //Remove o Objeto do KeepAlive
                                        await modelKeepAlive.AtualizarObjetoKeepAlive(item, tokenSource.Token);

                                    }

                                }

                            }

                            if (Convert.ToInt32(item) == Convert.ToInt32(EnumKeepAlive.SolicitacaoRastreamentoNegada))
                            {

                                ServiceResult<String> resultObjeto = await modelKeepAlive.RetornaObjetoKeepAlive(item, tokenSource.Token);

                                if (resultObjeto != null && resultObjeto.Data != null && resultObjeto.Data != "null")
                                {

                                    String message = resultObjeto.Data.ToString().Replace("\"", "");


                                    Application.Current.Properties["SolicitacaoRastreamentoEmAndamento"] = null;
                                    Application.Current.Properties["SolicitacaoRastreamentoEmAndamento"] = false;


                                    //Remove o Objeto do KeepAlive
                                    await modelKeepAlive.AtualizarObjetoKeepAlive(item, tokenSource.Token);

                                    CrossLocalNotifications.Current.Show(AppResources.Alert, message);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }


            });


        }

        public async void ChangeTempoTransmissao(AplicativoDto paramAplicativo)
        {

            TokenDataStore tokenDataSource = new TokenDataStore();

            TokenRealm _tokenRealm = null;
            TokenRealm _newTokenRealm = null;


            try
            {
                if (tokenDataSource.Get(1) != null)
                {
                    _tokenRealm = tokenDataSource.Get(1);

                    _newTokenRealm = new TokenRealm();


                    if (_tokenRealm.Id != 0)
                        _newTokenRealm.Id = _tokenRealm.Id;

                    if (_tokenRealm.Access_Token != null)
                        _newTokenRealm.Access_Token = _tokenRealm.Access_Token;

                    if (_tokenRealm.expires_in != null)
                        _newTokenRealm.expires_in = _tokenRealm.expires_in;

                    _newTokenRealm.IsUsuarioAdminPadrao = _tokenRealm.IsUsuarioAdminPadrao;

                    if (_tokenRealm.LstFuncao != null)
                        _newTokenRealm.LstFuncao = _tokenRealm.LstFuncao;

                    if (_tokenRealm.IdAplicativo != 0)
                        _newTokenRealm.IdAplicativo = _tokenRealm.IdAplicativo;

                }

                Application.Current.Properties["SolicitacaoRastreamentoEmAndamento"] = null;
                Application.Current.Properties["SolicitacaoRastreamentoEmAndamento"] = paramAplicativo.HasSolicitacaoRastreamento;



                if (paramAplicativo.IsDeleted == true)
                {
                    Util.TrackService(false);

                    Desconectar();
                }
                else
                {

                    if (paramAplicativo.IntervaloComunicacaoMovimento > 0)
                        _newTokenRealm.TempoTransmissao = paramAplicativo.IntervaloComunicacaoMovimento;
                    else
                        _newTokenRealm.TempoTransmissao = _app.Configuracao.TempoTransmissaoPadrao;


                    if (paramAplicativo.IP != null)
                        _newTokenRealm.IP = paramAplicativo.IP;


                    _newTokenRealm.IsLocator = paramAplicativo.IsLocator;


                    if (paramAplicativo.Porta != 0)
                        _newTokenRealm.Porta = paramAplicativo.Porta;

                    if (paramAplicativo.Identificacao != null)
                        _newTokenRealm.Identificacao = Convert.ToInt64(paramAplicativo.Identificacao);


                    tokenDataSource.CreateUpadate(_newTokenRealm);


                    Token _token = new Token();
                    _token.TransformFromRealm(_newTokenRealm);
                    _app.Token = _token;


                    Util.ChangeTempoTransmissao();


                    if (paramAplicativo.IsLocator == true)
                    {
                        try
                        {

                            Util.TrackService(true);


                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }

                    }
                    else
                    {
                        try
                        {

                            Util.TrackService(false);

                            Application.Current.Properties["SolicitacaoRastreamentoEmAndamento"] = null;
                            Application.Current.Properties["SolicitacaoRastreamentoEmAndamento"] = false;


                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                CheckKeepAlive();
                Crashes.TrackError(ex);
            }

        }

        private async void Desconectar()
        {
            try
            {
                _view.ExibirLoad();


                Application.Current.Properties["SolicitacaoRastreamentoEmAndamento"] = null;
                Application.Current.Properties["SolicitacaoRastreamentoEmAndamento"] = false;


                CancellationTokenSource tokenSource = new CancellationTokenSource();
                Task.Run(async () =>
                {


                    Application.Current.Properties["IsLogado"] = null;
                    Application.Current.Properties["IsLogado"] = false;

                    try
                    {
                        ModelPushNotification classPushNotification = new ModelPushNotification();
                        classPushNotification.DeletePushKey(tokenSource.Token);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }

                    try
                    {
                        ModelUsuario modelUsuario = new ModelUsuario();
                        modelUsuario.Deslogar(tokenSource.Token);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }

                    try
                    {
                        _app.Util.DeslogarRest();
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }

                    try
                    {
                        TokenDataStore tokenDb = new TokenDataStore();
                        tokenDb.Clean();
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }


                });

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    _app.MainPage = new NavigationPage(new ViewLogin(_app.isPersonalizado, _app.nameProject));
                });

            }
            catch (Exception ex)
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    _app.MainPage = new NavigationPage(new ViewLogin(_app.isPersonalizado, _app.nameProject));
                });

                Crashes.TrackError(ex);
            }

        }

        private async void DesconectarAction(object obj)
        {
            try
            {
                _view.ExibirLoad();


                //await Push.SetEnabledAsync(false);


                Application.Current.Properties["SolicitacaoRastreamentoEmAndamento"] = null;
                Application.Current.Properties["SolicitacaoRastreamentoEmAndamento"] = false;


                CancellationTokenSource tokenSource = new CancellationTokenSource();
                Task.Run(async () =>
                {

                    Application.Current.Properties["IsLogado"] = null;
                    Application.Current.Properties["IsLogado"] = false;


                    try
                    {
                        ModelPushNotification classPushNotification = new ModelPushNotification();
                        classPushNotification.DeletePushKey(tokenSource.Token);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }

                    try
                    {
                        ModelUsuario modelUsuario = new ModelUsuario();
                        modelUsuario.Deslogar(tokenSource.Token);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }

                    try
                    {
                        _app.Util.DeslogarRest();
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }

                    try
                    {
                        TokenDataStore tokenDb = new TokenDataStore();
                        tokenDb.Clean();
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }


                });

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    _app.MainPage = new NavigationPage(new ViewLogin(_app.isPersonalizado, _app.nameProject));
                });

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    _app.MainPage = new NavigationPage(new ViewLogin(_app.isPersonalizado, _app.nameProject));
                });
            }
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

                    if (resultCompra.Data != null || resultCompra.Data == false)
                    {

                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {

                            ContratarButtonText = AppResources.Purchase;
                            BtnContratarIsVisible = true;
                            ContratarButtonCommand = new Command(this.ContratarAction);
                            MargemPainelDesconectar = new Thickness(0, -25, 0, 0);
                            AtivaPainelComprar = true;
                            AtivaPainelDeslogar = true;

                        });

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

                    }
                    else
                    {
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {

                            BtnContratarIsVisible = false;
                            MargemPainelDesconectar = new Thickness(0, -57, 0, 0);
                            AtivaPainelComprar = false;
                            AtivaPainelDeslogar = true;


                            //_messageService.ShowAlertAsync(
                            //AppResources.ServicoPago
                            //, AppResources.Alert);

                        });

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

        private async void ContratarAction(object obj)
        {

            var productId = new string[] { "br.com.systemsat.family.fullaccess" };
            var billing = CrossInAppBilling.Current;

            billing.InTestingMode = false;

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
                }
                else
                {
                    CancellationTokenSource tokenSource = new CancellationTokenSource();
                    ModelUsuario modelUsuario = new ModelUsuario();

                    ServiceResult<Boolean> resultCompra = new ServiceResult<bool>();
                    resultCompra = await modelUsuario.Grava_CompraRealizada(tokenSource.Token);

                    //if (resultCompra.Data != null && resultCompra.Data == false)
                    //{
                    /*
                    //purchase.State item comprado
                    _messageService.ShowAlertAsync(
                        purchase.State.ToString()
                        , AppResources.Alert);

                    */

                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                    {
                        BtnContratarIsVisible = false;
                        MargemPainelDesconectar = new Thickness(0, -57, 0, 0);
                        AtivaPainelComprar = false;
                        AtivaPainelDeslogar = true;

                    });


                    //}

                }
            }
            catch (InAppBillingPurchaseException purchaseEx)
            {
                //Quando dá cancelar na tela que solicita a senha da store

                _messageService.ShowAlertAsync(
                    purchaseEx.Message
                    , AppResources.Error
                );

            }
            catch (Exception ex)
            {
                //Debug.WriteLine("Issue connecting: " + ex);

                _messageService.ShowAlertAsync(
                    ex.Message
                    , AppResources.Error
                );

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    _app.MainPage = new NavigationPage(new ViewLogin(_app.isPersonalizado, _app.nameProject));
                });

            }
            finally
            {
                //Disconnect, it is okay if we never connected, this will never throw an exception
                await billing.DisconnectAsync();
            }


        }

    }
#pragma warning restore CS1998
#pragma warning restore RECS0022
#pragma warning restore CS4014
}