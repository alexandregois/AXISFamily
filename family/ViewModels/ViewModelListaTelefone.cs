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

namespace family.ViewModels
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
    public class ViewModelListaTelefone : ViewModelBase
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


        private List<TelefoneContatoDto> _panelLista_ItemsSource
        = new List<TelefoneContatoDto>();
        public List<TelefoneContatoDto> PanelLista_ItemsSource
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


        public ViewModelListaTelefone()
        {
            this.DefaultTemplateBuild();

            VoltarVisible = false;

            this.BtnEnabled = true;

            this.DefaultButtonHeight = 64;

            this.BtnConfiguracaoCommand = new Command(this.ConfiguracaoAction);

            PanelLista_Enabled = false;

            this.PanelLista_RowHeight = 70;
            this.PanelLista_RefreshCommand = new Command(this.PanelLista_RefreshAction);

            posicaoStore = new PosicaoDataStore();

            //_view.EscondeLoad();

        }

        public override void OnAppearing()
        {
            this.DefaultTemplateBuild();

            this.BtnEnabled = true;

            if (Tokensource != null)
                Tokensource.Cancel();


            if (_app.ListaTelefone.Count > 0)
            {
                PanelLista_ItemsSource = _app.ListaTelefone;
                PanelLista_Enabled = true;
            }


            Tokensource = new CancellationTokenSource();

            TimeSpan loopExpress = TimeSpan.FromSeconds(1);

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
            VoltarVisible = true;

            PageColor = Color.FromHex("#a3455d"); // "#3FC0EF");
            ImageSourceProperty = "ic_telefone_on.png";
            ImageWidthProperty = 30;

            Button refreshButton = new Button()
            {
                Image = "ic_vazio.png",
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
            labelTemp.Text = AppResources.Telefone;
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

        private void ConfiguracaoAction(object obj)
        {
            BtnEnabled = false;
            PanelLista_Enabled = false;
            _navigationService.NavigateToConfiguracao();
        }

        #region ListaTelefones
        private async Task Loop(TimeSpan paramTimeSpan)
        {

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

        }

        private void PanelLista_RefreshAction(object obj)
        {
            try
            {

                Task.Run(async () =>
                {
                    try
                    {
                        if (_app.ListaTelefone.Count > 0)
                        {
                            PanelLista_ItemsSource = _app.ListaTelefone;
                            PanelLista_Enabled = true;
                        }

                    }
                    catch (Exception ex)
                    {
                        _view.EndRefresh();

                        Crashes.TrackError(ex);

                    }
                }, Tokensource.Token);
            }
            catch (Exception ex)
            {
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