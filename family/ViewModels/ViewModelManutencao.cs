using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using family.Domain;
using family.Domain.Realm;
using family.Resx;
using family.ViewModels.Base;
using family.Views.Interfaces;
using Xamarin.Forms;
using family.Domain.Dto;
using family.Model;
using Xamarin.Forms.GoogleMaps;
using family.Services.ServiceRealm;
using family.ViewModels.InterfaceServices;
using Microsoft.AppCenter.Crashes;

namespace family.ViewModels
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
    public class ViewModelManutencao : ViewModelBase
    {
        private ViewModelMapa _viewModelMapa { get; set; }

        private ModelManutencao _modelManutencao { get; set; }

        public IPartialView ActualView { get; set; }

        protected readonly IMessageService _messageService;

        public Int64 IdRastreador { get; set; }
        public PosicaoUnidadeRastreadaRealm ActualPosition { get; set; }

        public CancellationTokenSource _tokensourceAction { get; set; }
        public CancellationTokenSource _tokensourceDelay { get; set; }

        private Color _pageBackgroundColor;
        public Color PageBackgroundColor
        {
            get
            {
                return _pageBackgroundColor;
            }
            set
            {
                _pageBackgroundColor = value;
                this.Notify("PageBackgroundColor");
            }
        }

        private Color _panelSearchBackgroundColor;
        public Color PanelSearchBackgroundColor
        {
            get
            {
                return _panelSearchBackgroundColor;
            }
            set
            {
                _panelSearchBackgroundColor = value;
                this.Notify("PanelSearchBackgroundColor");
            }
        }

        #region PainelMaitain

        private String _labelOdometro;
        public String LabelOdometro
        {
            get
            {
                return _labelOdometro;
            }
            set
            {
                _labelOdometro = value;
                this.Notify("LabelOdometro");
            }
        }

        private String _trocaOleo;
        public String TrocaOleo
        {
            get
            {
                return _trocaOleo;
            }
            set
            {
                _trocaOleo = value;
                this.Notify("TrocaOleo");
            }
        }

        private String _proximaRevisao;
        public String ProximaRevisao
        {
            get
            {
                return _proximaRevisao;
            }
            set
            {
                _proximaRevisao = value;
                this.Notify("ProximaRevisao");
            }
        }

        private String _rodizioPneu;
        public String RodizioPneu
        {
            get
            {
                return _rodizioPneu;
            }
            set
            {
                _rodizioPneu = value;
                this.Notify("RodizioPneu");
            }
        }

        private DateTime _minData;
        public DateTime MinData
        {
            get
            {
                return _minData;
            }
            set
            {
                _minData = value;
                this.Notify("MinData");
            }
        }

        private DateTime _maxData;
        public DateTime MaxData
        {
            get
            {
                return _maxData;
            }
            set
            {
                _maxData = value;
                this.Notify("MaxData");
            }
        }

        private String _seguroData;
        public String SeguroData
        {
            get
            {
                return _seguroData;
            }
            set
            {
                _seguroData = value;
                this.Notify("SeguroData");
            }
        }

        private DateTime _seguroDataEdicao;
        public DateTime SeguroDataEdicao
        {
            get
            {
                return _seguroDataEdicao;
            }
            set
            {
                _seguroDataEdicao = value;
                this.Notify("SeguroDataEdicao");
            }
        }

        private Boolean _edicaoIsVisible;
        public Boolean EdicaoIsVisible
        {
            get
            {
                return _edicaoIsVisible;
            }
            set
            {
                _edicaoIsVisible = value;
                this.Notify("EdicaoIsVisible");
            }
        }

        private Boolean _visualizacaoIsVisible;
        public Boolean VisualizacaoIsVisible
        {
            get
            {
                return _visualizacaoIsVisible;
            }
            set
            {
                _visualizacaoIsVisible = value;
                this.Notify("VisualizacaoIsVisible");
            }
        }

        private Double _botoesWidth;
        public Double BotoesWidth
        {
            get
            {
                return _botoesWidth;
            }
            set
            {
                _botoesWidth = value;
                this.Notify("BotoesWidth");
            }
        }

        #region BtnEditarManutencao
        public ICommand BtnEditarManutencaoCommand { get; set; }

        public ICommand TxtValueNumberCommand { get; set; }

        private String _btnEditarManutencaoText;
        public String BtnEditarManutencaoText
        {
            get
            {
                return _btnEditarManutencaoText;
            }
            set
            {
                _btnEditarManutencaoText = value;
                this.Notify("BtnEditarManutencaoText");
            }
        }

        private Boolean _btnEditarManutencaoIsVisible;
        public Boolean BtnEditarManutencaoIsVisible
        {
            get
            {
                return _btnEditarManutencaoIsVisible;
            }
            set
            {
                _btnEditarManutencaoIsVisible = value;
                this.Notify("BtnEditarManutencaoIsVisible");
            }
        }

        #endregion

        #region BtnSalvarManutencao
        public ICommand BtnSalvarManutencaoCommand { get; set; }
        public ICommand BtnCancelarManutencaoCommand { get; set; }

        private String _btnSalvarManutencaoText;
        public String BtnSalvarManutencaoText
        {
            get
            {
                return _btnSalvarManutencaoText;
            }
            set
            {
                _btnSalvarManutencaoText = value;
                this.Notify("BtnSalvarManutencaoText");
            }
        }

        private Boolean _btnSalvarManutencaoIsVisible;
        public Boolean BtnSalvarManutencaoIsVisible
        {
            get
            {
                return _btnSalvarManutencaoIsVisible;
            }
            set
            {
                _btnSalvarManutencaoIsVisible = value;
                this.Notify("BtnSalvarManutencaoIsVisible");
            }
        }

        #endregion


        #endregion

        public ViewModelManutencao(
            ViewModelMapa viewModelMapa
        ) : base(true)
        {

            _viewModelMapa = viewModelMapa;
            ActualPosition = _viewModelMapa.ActualPosition;
            IdRastreador = _viewModelMapa.IdRastreador;

            MinData = DateTime.Now.ToLocalTime().AddMonths(-6);
            MaxData = DateTime.Now.ToLocalTime().AddMonths(+1);

            BotoesWidth = DefaultWidth / 2;

            this._messageService =
                    DependencyService.Get<IMessageService>();

            _modelManutencao = new ModelManutencao();


            BtnEditarManutencaoCommand = new Command(EditarManutencaoCommand);

            BtnSalvarManutencaoCommand = new Command(SalvarManutencaoCommand);

            BtnCancelarManutencaoCommand = new Command(CancelarEdicao);

            TxtValueNumberCommand = new Command(ValueNumberCommand);

            MontaPaniel();

            this.DefaultTemplateBuild();

        }

        private void ValueNumberCommand(object obj)
        {
            throw new NotImplementedException();
        }

        private void MaintainManutencao()
        {
            ServiceResult<ManutencaoBasicaDto> result = new ServiceResult<ManutencaoBasicaDto>();

            try
            {

                CancelarEdicao();

                if (ActualPosition.Odometro != null)
                    LabelOdometro = ActualPosition.Odometro.ToString();
                else
                    LabelOdometro = "";

                _tokensourceAction = new CancellationTokenSource();

                Task.Run(async () =>
                {
                    try
                    {

                        ManutencaoBasicaDto paramManutencao = new ManutencaoBasicaDto();

                        paramManutencao.IdUnidadeRastreada = _viewModelMapa.IdUnidadeRastreada;

                        if (!String.IsNullOrEmpty(TrocaOleo))
                            paramManutencao.TrocaOleo = Convert.ToInt32(TrocaOleo);

                        if (!String.IsNullOrEmpty(ProximaRevisao))
                            paramManutencao.ProximaRevisao = Convert.ToInt32(ProximaRevisao);

                        if (!String.IsNullOrEmpty(RodizioPneu))
                            paramManutencao.RodizioPneu = Convert.ToInt32(RodizioPneu);

                        if (SeguroDataEdicao != null)
                            if ((SeguroDataEdicao.Day - DateTime.Now.ToLocalTime().Day) > 0)
                                paramManutencao.StringValidadeSeguro = SeguroDataEdicao.ToString("yyyy/MM/dd hh:mm:ss");


                        result = await _modelManutencao.UpdateManutencao(paramManutencao,
                            _tokensourceAction.Token
                        );

                        if (result.Data.TrocaOleo != null)
                            TrocaOleo = result.Data.TrocaOleo.ToString();

                        if (result.Data.ProximaRevisao != null)
                            ProximaRevisao = result.Data.ProximaRevisao.ToString();

                        if (result.Data.RodizioPneu != null)
                            RodizioPneu = result.Data.RodizioPneu.ToString();

                        if (result.Data.ValidadeSeguro != null)
                            SeguroData = String.Format(
                            "{0:dd/MM/yyyy}"
                            , result.Data.ValidadeSeguro);


                        _messageService.ShowAlertAsync(
                 AppResources.MaintenanceSave
                 , AppResources.Maintenance);


                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                    finally
                    {
                        //BuscaPontosEndRefresh(result);
                    }

                }, _tokensourceAction.Token);


                _viewModelMapa.ModelMapaGoogle.LimpaMapa();
                _viewModelMapa.ModelMapaGoogle.MontaMapaPinUltimaPosicaoAndCentraliza(ActualPosition);


            }
            catch (Exception ex)
            {

                Crashes.TrackError(ex);
            }
        }

        private void CancelarEdicao()
        {
            VisualizacaoIsVisible = true;
            EdicaoIsVisible = false;

            BtnSalvarManutencaoIsVisible = false;
            BtnEditarManutencaoIsVisible = true;

            BtnEditarManutencaoText = AppResources.Edit;
            BtnSalvarManutencaoText = AppResources.Save;
        }

        private void SalvarManutencaoCommand(object obj)
        {
            //Device.BeginInvokeOnMainThread(() =>
            //{
            MaintainManutencao();
            //});
        }

        private void EditarManutencaoCommand(object obj)
        {
            //Device.BeginInvokeOnMainThread(() =>
            //{
            VisualizacaoIsVisible = false;
            EdicaoIsVisible = true;
            //});
        }

        public override void OnAppearing()
        {

            _viewModelMapa.ActualView.ExibirLoad();

            PageBackgroundColor = _viewModelMapa.ActualPage.ColorBarra;
            PanelSearchBackgroundColor = _viewModelMapa.ActualPage.Color;

            this.DefaultTemplateBuild();

            VisualizacaoIsVisible = true;

            //CreateTokens();

            MontaPaniel();

        }

        public override void OnDisappearing()
        {
            //if (_tokensourceAction != null)
            //    _tokensourceAction.Cancel();

            _viewModelMapa.ModelMapaGoogle._mapa.CameraIdled -= MapaCameraIdled;
        }

        public override void OnLayoutChanged()
        {
        }

        private void DefaultTemplateBuild()
        {
            _viewModelMapa.PanelTituloLabel_Text = AppResources.Maintenance;
            _viewModelMapa.PanelSubTituloLabel_Text = ActualPosition.IdentificacaoFinal;

            Button refreshButton = new Button()
            {
                HeightRequest = 35,
                WidthRequest = 35,

                Margin = new Thickness(0, 0, 0, 0),
                BorderRadius = 0,
                BorderWidth = 0,
                BorderColor = Color.Transparent,
                BackgroundColor = Color.Transparent
            };

            Button menuMapaButton = new Button()
            {
                Image = "ic_menu.png",
                Command = new Command(MapaTipos),
                HeightRequest = 35,
                WidthRequest = 35,
                Margin = new Thickness(0, 0, 11, 0),
                BorderRadius = 0,
                BorderWidth = 0,
                BorderColor = Color.Transparent,
                BackgroundColor = Color.Transparent
            };

            if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
            {
                menuMapaButton.Margin = new Thickness(7, 0, 7, 0);
            }


            StackLayout stackLayout = new StackLayout();
            stackLayout.WidthRequest = 79;
            stackLayout.HeightRequest = 53;
            stackLayout.Orientation = StackOrientation.Horizontal;

            stackLayout.Children.Add(refreshButton);

            stackLayout.Children.Add(menuMapaButton);
            _viewModelMapa.BoxRightContent = stackLayout;

        }

        public async void MapaTipos()
        {
            //String answer;
            //answer = await _messageService.ShowMessageAsync(
            //    AppResources.StreetView
            //    , AppResources.Cancel
            //    , null
            //    , new string[]{
            //    AppResources.ExibeStreetView
            //}
            //);

            //if (answer != AppResources.Cancel)
            //{
            //    OpenStreetView();
            //}

            String answer;
            answer = await _messageService.ShowMessageAsync(
                AppResources.Mapa
                , AppResources.Cancel
                , null
                , new string[]{
                AppResources.MapaPadrao,
                AppResources.Trafego,
                AppResources.Satelite,
                AppResources.Terreno,
                AppResources.Hibrido
            }
            );



            if (answer == AppResources.Trafego)
            {
                this._viewModelMapa.ActualView.mapaPosicao.MapType = MapType.Street;
                this._viewModelMapa.ActualView.mapaPosicao.IsTrafficEnabled = true;
            }
            else if (answer == AppResources.Satelite)
            {
                this._viewModelMapa.ActualView.mapaPosicao.MapType = MapType.Satellite;
                this._viewModelMapa.ActualView.mapaPosicao.IsTrafficEnabled = false;
            }
            else if (answer == AppResources.Terreno)
            {
                this._viewModelMapa.ActualView.mapaPosicao.MapType = MapType.Terrain;
                this._viewModelMapa.ActualView.mapaPosicao.IsTrafficEnabled = false;
            }
            else if (answer == AppResources.Hibrido)
            {
                this._viewModelMapa.ActualView.mapaPosicao.MapType = MapType.Hybrid;
                this._viewModelMapa.ActualView.mapaPosicao.IsTrafficEnabled = false;
            }
            else if (answer == AppResources.MapaPadrao)
            {
                this._viewModelMapa.ActualView.mapaPosicao.MapType = MapType.Street;
                this._viewModelMapa.ActualView.mapaPosicao.IsTrafficEnabled = false;
            }



        }

        public async void MapaCameraIdled(
            object sender
            , CameraIdledEventArgs e
        )
        {
            MontaPontoControleMaintain(
                e.Position.Target.Latitude
                , e.Position.Target.Longitude
            );
        }

        private void MontaPontoControleMaintain(
            Double paramLatitude
            , Double paramLongitude
        )
        {
            _viewModelMapa.ModelMapaGoogle.LimpaMapa();

            PosicaoUnidadeRastreadaRealm posi = new PosicaoUnidadeRastreadaRealm()
            {
                Latitude = paramLatitude,
                Longitude = paramLongitude
            };

            _viewModelMapa.ModelMapaGoogle.MontaMapaPin(
                posi
                , "pin_ponto_controle.png"
            );
        }

        public void MontaPaniel()
        {

            ServiceResult<ManutencaoBasicaDto> result = new ServiceResult<ManutencaoBasicaDto>();

            try
            {

                VisualizacaoIsVisible = true;
                EdicaoIsVisible = false;

                BtnSalvarManutencaoIsVisible = false;
                BtnEditarManutencaoIsVisible = true;

                BtnEditarManutencaoText = AppResources.Edit;
                BtnSalvarManutencaoText = AppResources.Save;


                if (ActualPosition.Odometro != null)
                    LabelOdometro = ActualPosition.Odometro.ToString();
                else
                    LabelOdometro = "";

                _tokensourceAction = new CancellationTokenSource();

                Task.Run(async () =>
                {
                    try
                    {
                        ManutencaoBasicaDto paramManutencao = new ManutencaoBasicaDto();

                        paramManutencao.IdUnidadeRastreada = _viewModelMapa.IdUnidadeRastreada;

                        result = await _modelManutencao.ListaManutencao(paramManutencao,
                            _tokensourceAction.Token
                        );

                        if (result.Data.TrocaOleo != null)
                            TrocaOleo = result.Data.TrocaOleo.ToString();

                        if (result.Data.ProximaRevisao != null)
                            ProximaRevisao = result.Data.ProximaRevisao.ToString();

                        if (result.Data.RodizioPneu != null)
                            RodizioPneu = result.Data.RodizioPneu.ToString();

                        if (result.Data.ValidadeSeguro != null)
                        {
                            SeguroData = String.Format(
                            "{0:dd/MM/yyyy}"
                            , result.Data.ValidadeSeguro);

                            SeguroDataEdicao = result.Data.ValidadeSeguro.Value;
                        }
                        else
                            SeguroDataEdicao = DateTime.Now.ToLocalTime();

                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                    finally
                    {
                        //BuscaPontosEndRefresh(result);
                    }

                }, _tokensourceAction.Token);


                _viewModelMapa.ModelMapaGoogle.LimpaMapa();
                _viewModelMapa.ModelMapaGoogle.MontaMapaPinUltimaPosicaoAndCentraliza(ActualPosition);


            }
            catch (Exception ex)
            {

                Crashes.TrackError(ex);
            }

        }

        private void CreateTokens()
        {
            if (_tokensourceDelay != null)
                _tokensourceDelay.Cancel();
            _tokensourceDelay = new CancellationTokenSource();

            if (_tokensourceAction != null)
                _tokensourceAction.Cancel();

            _tokensourceAction = new CancellationTokenSource();
        }


    }
#pragma warning restore CS1998
#pragma warning restore RECS0022
#pragma warning restore CS4014
}