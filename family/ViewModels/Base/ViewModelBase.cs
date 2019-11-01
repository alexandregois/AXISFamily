using System;
using System.ComponentModel;
using family.Domain.Realm;
using family.Resx;
using family.ViewModels.InterfaceServices;
using Xamarin.Forms;
using family.Domain.Dto;
using family.Services.ServiceRealm;
using family.Domain;
using System.Linq;
using family.Views;

using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace family.ViewModels.Base
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {

        protected App _app => (Application.Current as App);
        protected readonly IMessageService _messageService;
        protected readonly INavigationService _navigationService;

        private Color _pageColor;
        public Color PageColor
        {
            get
            {
                return _pageColor;
            }
            set
            {
                _pageColor = value;
                this.Notify("PageColor");
            }
        }

        private Double _defaultWidth;
        public Double DefaultWidth
        {
            get
            {
                return _defaultWidth;
            }
            set
            {
                _defaultWidth = value;
                this.Notify("DefaultWidth");
            }
        }

        private Double _defaultHeight;
        public Double DefaultHeight
        {
            get
            {
                return _defaultHeight;
            }
            set
            {
                _defaultHeight = value;
                this.Notify("DefaultHeight");
            }
        }

        private Double _defaultHeightContent;
        public Double DefaultHeightContent
        {
            get
            {
                return _defaultHeightContent;
            }
            set
            {
                _defaultHeightContent = value;
                this.Notify("DefaultHeightContent");
            }
        }

        private Double _shadowHeight;
        public Double ShadowHeight
        {
            get
            {
                return _shadowHeight;
            }
            set
            {
                _shadowHeight = value;
                this.Notify("ShadowHeight");
            }
        }

        public Boolean PainelMapaIsVisible { get; set; }

        public Boolean BarraInferiorIsVisible { get; set; }

        public Boolean ExibeStreetViewIsVisible { get; set; }


        public ViewModelBase(
            Boolean paramIsMapaPage = false
        )
        {
            this._messageService =
                    DependencyService.Get<IMessageService>();
            this._navigationService =
                    DependencyService.Get<INavigationService>();

            DefaultWidth = _app.Util.GetScreenWidth();
            DefaultHeight = _app.Util.GetScreenHeight();
            DefaultHeightContent = _app.DefaultTemplateHeightContent;
            ShadowHeight = DefaultHeightContent;
            PageColor = Color.Black;

            VoltarCommand = new Command(this.VoltarAction);

            PainelMapaIsVisible = true;
            BarraInferiorIsVisible = true;
            ExibeStreetViewIsVisible = false;

            if (paramIsMapaPage)
                SizeBoxViewMapa();
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public void Notify(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public abstract void OnAppearing();

        public abstract void OnDisappearing();

        public abstract void OnLayoutChanged();

        public String BuscaTraducaoError(
            String paramMessageError
        )
        {
            String erroMessage;
            try
            {
                erroMessage =
                    AppResources
                        .ResourceManager
                        .GetString(paramMessageError);

                if (String.IsNullOrWhiteSpace(erroMessage))
                    erroMessage = paramMessageError;

            }
            catch (Exception ex)
            {
                erroMessage = AppResources.ErroSuporte;
                Crashes.TrackError(ex);
            }
            return erroMessage;
        }

        public void ShowErrorAlert(
            String paramMessageError
        )
        {
            String erroMessage = BuscaTraducaoError(paramMessageError);

            if (paramMessageError == System.Net.HttpStatusCode.Unauthorized.ToString())
            {
                _navigationService.NavigateToLogin();
            }

            _messageService.ShowAlertAsync(
                erroMessage
                , AppResources.Error
            );
        }

        #region Template
        private Command _voltarCommand;
        public Command VoltarCommand
        {
            get
            {
                return _voltarCommand;
            }
            set
            {
                _voltarCommand = value;
                this.Notify("VoltarCommand");
            }
        }

        private Boolean _voltarVisible;
        public Boolean VoltarVisible
        {
            get
            {
                return _voltarVisible;
            }
            set
            {
                _voltarVisible = value;
                this.Notify("VoltarVisible");
            }
        }

        private ImageSource _imageSourceProperty;
        public ImageSource ImageSourceProperty
        {
            get
            {
                return _imageSourceProperty;
            }
            set
            {
                _imageSourceProperty = value;
                this.Notify("ImageSourceProperty");
            }
        }

        private Double _imageWidthProperty;
        public Double ImageWidthProperty
        {
            get
            {
                return _imageWidthProperty;
            }
            set
            {
                _imageWidthProperty = value;
                this.Notify("ImageWidthProperty");
            }
        }

        private View _boxMiddleContent;
        public View BoxMiddleContent
        {
            get
            {
                return _boxMiddleContent;
            }
            set
            {
                _boxMiddleContent = value;
                this.Notify("BoxMiddleContent");
            }
        }

        private View _boxRightContent;
        public View BoxRightContent
        {
            get
            {
                return _boxRightContent;
            }
            set
            {
                _boxRightContent = value;
                this.Notify("BoxRightContent");
            }
        }

        private void VoltarAction(object obj)
        {
            this._navigationService.Voltar();
        }

        public Label PanelTituloLabel_Titulo()
        {
            return new Label()
            {
                TextColor = Color.White,
                FontSize = 16,
                Margin = new Thickness(0),
                VerticalTextAlignment = TextAlignment.Center,
                LineBreakMode = LineBreakMode.TailTruncation
            };
        }
        #endregion

        #region ViewMapa
        private Double _barraInferiorHeight;
        public Double BarraInferiorHeight
        {
            get
            {
                return _barraInferiorHeight;
            }
            set
            {
                _barraInferiorHeight = value;
                this.Notify("BarraInferiorHeight");
            }
        }

        private Double _painelMapaHeight;
        public Double PainelMapaHeight
        {
            get
            {
                return _painelMapaHeight;
            }
            set
            {
                _painelMapaHeight = value;
                this.Notify("PainelMapaHeight");
            }
        }

        private Double _menuHeight;
        public Double MenuHeight
        {
            get
            {
                return _menuHeight;
            }
            set
            {
                _menuHeight = value;
                this.Notify("MenuHeight");
            }
        }

        private Double _menuItemWidth;
        public Double MenuItemWidth
        {
            get
            {
                return _menuItemWidth;
            }
            set
            {
                _menuItemWidth = value;
                this.Notify("MenuItemWidth");
            }
        }

        private Double _contentLoadBoxHeight;
        public Double ContentLoadBoxHeight
        {
            get
            {
                return _contentLoadBoxHeight;
            }
            set
            {
                _contentLoadBoxHeight = value;
                this.Notify("ContentLoadBoxHeight");
            }
        }

        public void SizeBoxViewMapa()
        {
            BarraInferiorHeight = 300;

            PainelMapaHeight = DefaultHeightContent - BarraInferiorHeight;

            MenuHeight = 53;
            MenuItemWidth = 55;

            ContentLoadBoxHeight = BarraInferiorHeight - MenuHeight;
        }

        #endregion

        #region UpdateToken
        public void UpdateToken(Object paramRefreshToken)
        {
            try
            {
                if (paramRefreshToken != null)
                {
                    TokenDto tempToken = (TokenDto)paramRefreshToken;

                    TokenRealm _tokenRealm = new TokenRealm()
                    {
                        Id = 1,
                        Access_Token = tempToken.Access_Token,
                        IsUsuarioAdminPadrao = tempToken.IsUsuarioAdminPadrao
                    };

                    if (!String.IsNullOrEmpty(tempToken.UrlLogo))
                    {
                        _tokenRealm.UrlLogo = tempToken.UrlLogo;
                    }

                    if (tempToken.LstFuncao != null)
                    {
                        _tokenRealm.LstFuncao = "|" + String.Join("|", tempToken.LstFuncao.ToArray()) + "|";
                    }

                    if (
                        tempToken.TempoTransmissao.HasValue
                        && tempToken.TempoTransmissao.Value > 0
                    )
                    {
                        if (tempToken.TempoTransmissao.Value < _app.Configuracao.TempoTransmissaoPadrao)
                            _tokenRealm.TempoTransmissao = _app.Configuracao.TempoTransmissaoPadrao;
                        else
                            _tokenRealm.TempoTransmissao = tempToken.TempoTransmissao.Value;
                    }
                    else
                    {
                        _tokenRealm.TempoTransmissao = _app.Configuracao.TempoTransmissaoPadrao;
                    }

                    if (tempToken.Aplicativo != null)
                    {
                        _tokenRealm.IP = tempToken.Aplicativo.IP;
                        _tokenRealm.IsLocator = tempToken.Aplicativo.IsLocator;
                        _tokenRealm.IdAplicativo = tempToken.Aplicativo.IdAplicativo;
                        _tokenRealm.Porta = tempToken.Aplicativo.Porta;
                        _tokenRealm.Identificacao = Convert.ToInt64(tempToken.Aplicativo.Identificacao);
                    }

                    TokenDataStore dataSource = new TokenDataStore();

                    dataSource.CreateUpadate(_tokenRealm);

                    Token _token = new Token();
                    _token.TransformFromRealm(_tokenRealm);

                    if (_token.IsLocator != _app.Token.IsLocator)
                    {
                        _app.Util.TrackService(_token.IsLocator);
                        RefazPaginaLista();
                    }
                    else
                    {
                        if (_token.IsLocator)
                        {
                            if (_token.TempoTransmissao != _app.Token.TempoTransmissao)
                            {
                                _app.Util.ChangeTempoTransmissao();
                            }
                        }
                    }

                    _app.Token = _token;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void RefazPaginaLista()
        {
            Page paginaAtual = Application.Current.MainPage.Navigation.NavigationStack.Last();
            if (paginaAtual.GetType() == typeof(ViewListaUnidadeRastreada))
            {
                ViewModelListaUnidadeRastreada atualPage = (ViewModelListaUnidadeRastreada)this;
                atualPage._view.ExibirLoad();
                atualPage._view.BuildTemplatePage();
            }
        }

        #endregion

    }
}
