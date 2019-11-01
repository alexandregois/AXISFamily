using System;
using family.ViewModels.Base;
using family.Views.Interfaces;
using family.Resx;
using family.Domain.Realm;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using System.Threading.Tasks;
using family.Model;
using family.Domain.Dto;
using System.Threading;
using family.CustomElements;

using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.Battery;

namespace family.ViewModels
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
    public class ViewModelHistorico : ViewModelBase
    {
        private ViewModelMapa _viewModelMapa { get; set; }
        public Int64 IdRastreador { get; set; }
        public IPartialViewHistorico ActualView { get; set; }

        public PosicaoUnidadeRastreadaRealm ActualPosition { get; set; }

        public CancellationTokenSource _tokensourceAction { get; set; }

        private Boolean _isPage { get; set; }
        private Int32 _pageNumber { get; set; } = 0;
        private Int32 _pageSize { get; set; } = 30;
        private Int32 _total { get; set; } = 0;

        private Pin _selectedPosition { get; set; }

        public App _app => (Application.Current as App);

        private ModelPosicao _mPosicao;
        public ModelPosicao MPosicao
        {
            get
            {
                if (_mPosicao == null)
                {
                    _mPosicao = new ModelPosicao();
                }
                return _mPosicao;
            }
        }

        private Double _panelSearchHeight;
        public Double PanelSearchHeight
        {
            get
            {
                return _panelSearchHeight;
            }
            set
            {
                _panelSearchHeight = value;
                ListViewPosicoesHeight = ContentLoadBoxHeight - _panelSearchHeight;
                this.Notify("PanelSearchHeight");
            }
        }

        private String _loadMoreText;
        public String LoadMoreText
        {
            get
            {
                return _loadMoreText;
            }
            set
            {
                _loadMoreText = value;
                this.Notify("LoadMoreText");
            }
        }

        private DateTime _dataFiltroDate;
        public DateTime DataFiltroDate
        {
            get
            {
                return _dataFiltroDate;
            }
            set
            {
                _dataFiltroDate = value;
                this.Notify("DataFiltroDate");
            }
        }

        private Double _imageButtonWidthRequest;
        public Double ImageButtonWidthRequest
        {
            get
            {
                return _imageButtonWidthRequest;
            }
            set
            {
                _imageButtonWidthRequest = value;
                this.Notify("ImageButtonWidthRequest");
            }
        }

        private String _localizadoDetalhesText;
        public String LocalizadoDetalhesText
        {
            get
            {
                return _localizadoDetalhesText;
            }
            set
            {
                _localizadoDetalhesText = AppResources.Address + ": " + value;
                this.Notify("LocalizadoDetalhesText");
            }
        }

        #region ListViewPosicoes
        public ICommand ListViewPosicoesRefreshCommand { get; set; }
        public ICommand PaginarHistoricoCommand { get; set; }
        public ICommand ListViewPosicoesDetalhesCommand { get; set; }

		public ICommand PaginarHistoricoCommandRight { get; set; }
		public ICommand PaginarHistoricoCommandLeft { get; set; }


        private Double _listViewPosicoesHeight;
        public Double ListViewPosicoesHeight
        {
            get
            {
                return _listViewPosicoesHeight;
            }
            set
            {
                _listViewPosicoesHeight = value;
                this.Notify("ListViewPosicoesHeight");
            }
        }

        private Boolean _listViewPosicoesIsEnabled;
        public Boolean ListViewPosicoesIsEnabled
        {
            get
            {
                return _listViewPosicoesIsEnabled;
            }
            set
            {
                _listViewPosicoesIsEnabled = value;
                this.Notify("ListViewPosicoesIsEnabled");
            }
        }

        private Boolean _listViewPosicoesIsRefreshing;
        public Boolean ListViewPosicoesIsRefreshing
        {
            get
            {
                return _listViewPosicoesIsRefreshing;
            }
            set
            {
                _listViewPosicoesIsRefreshing = value;
                this.Notify("ListViewPosicoesIsRefreshing");
            }
        }

        private List<PosicaoHistorico> _listViewPosicoesItemsSource;
        public List<PosicaoHistorico> ListViewPosicoesItemsSource
        {
            get
            {
                return _listViewPosicoesItemsSource;
            }
            set
            {
                _listViewPosicoesItemsSource = value;
                this.Notify("ListViewPosicoesItemsSource");
            }
        }
        #endregion

        #region ImageSetaDireita
        private Double _imageSetaDireitaButtonOpacity;
        public Double ImageSetaDireitaButtonOpacity
        {
            get
            {
                return _imageSetaDireitaButtonOpacity;
            }
            set
            {
                _imageSetaDireitaButtonOpacity = value;
                this.Notify("ImageSetaDireitaButtonOpacity");
            }
        }

        private Boolean _imageSetaDireitaButtonIsEnabled;
        public Boolean ImageSetaDireitaButtonIsEnabled
        {
            get
            {
                return _imageSetaDireitaButtonIsEnabled;
            }
            set
            {
                _imageSetaDireitaButtonIsEnabled = value;
                this.Notify("ImageSetaDireitaButtonIsEnabled");
            }
        }
        #endregion

        #region ImageSetaEsquerda
        private Double _imageSetaEsquerdaButtonOpacity;
        public Double ImageSetaEsquerdaButtonOpacity
        {
            get
            {
                return _imageSetaEsquerdaButtonOpacity;
            }
            set
            {
                _imageSetaEsquerdaButtonOpacity = value;
                this.Notify("ImageSetaEsquerdaButtonOpacity");
            }
        }

        private Boolean _imageSetaEsquerdaButtonIsEnabled;
        public Boolean ImageSetaEsquerdaButtonIsEnabled
        {
            get
            {
                return _imageSetaEsquerdaButtonIsEnabled;
            }
            set
            {
                _imageSetaEsquerdaButtonIsEnabled = value;
                this.Notify("ImageSetaEsquerdaButtonIsEnabled");
            }
        }
        #endregion

        #region LupaButton
        private Boolean _lupaButtonIsEnabled;
        public Boolean LupaButtonIsEnabled
        {
            get
            {
                return _lupaButtonIsEnabled;
            }
            set
            {
                _lupaButtonIsEnabled = value;
                this.Notify("LupaButtonIsEnabled");
            }
        }

        private Double _lupaButtonOpacity;
        public Double LupaButtonOpacity
        {
            get
            {
                return _lupaButtonOpacity;
            }
            set
            {
                _lupaButtonOpacity = value;
                this.Notify("LupaButtonOpacity");
            }
        }
        #endregion

        public ViewModelHistorico(
            ViewModelMapa viewModelMapa
        ) : base(true)
        {
            _viewModelMapa = viewModelMapa;
            IdRastreador = _viewModelMapa.IdRastreador;

            ActualPosition = _viewModelMapa.ActualPosition;

            LoadMoreText = AppResources.LoadMorePositions;

            PanelSearchHeight = 53;

            ImageButtonWidthRequest = 40;

            LupaButtonIsEnabled = false;
            ListViewPosicoesIsRefreshing = false;
            ListViewPosicoesRefreshCommand = new Command(this.ListViewPosicoesRefreshAction);
            PaginarHistoricoCommand = new Command(this.PaginarHistoricoAction);

			PaginarHistoricoCommandRight = new Command(this.PaginarHistoricoActionRight);
			PaginarHistoricoCommandLeft = new Command(this.PaginarHistoricoActionLeft);

            ListViewPosicoesDetalhesCommand = new Command(this.ListViewPosicoesOnDetalhes);
            this.DefaultTemplateBuild();


            DesativaTodosBotoes();

        }

        public override void OnAppearing()
        {
            ActualView.BeginRefresh();
        }

        public override void OnDisappearing()
        {
            if (_tokensourceAction != null)
                _tokensourceAction.Cancel();
        }

        public override void OnLayoutChanged()
        {
        }

        private void DefaultTemplateBuild()
        {
            _viewModelMapa.PanelTituloLabel_Text = AppResources.Historic;

            Button lupaButton = new Button()
            {
                HeightRequest = 53,
                WidthRequest = 53,
                Image = "ic_busca.png",
                BackgroundColor = Color.Transparent,
                BorderColor = Color.Transparent,
                Margin = new Thickness(-7,0,0,0),
                BorderWidth = 0,
                BorderRadius = 0,
                IsEnabled = false,
                Command = new Command(LupaClick)
            };

            Button menuMapaButton = new Button()
            {
                Image = "ic_menu.png",
                Command = new Command(MapaTipos),
                HeightRequest = 35,
                WidthRequest = 35,
                Margin = new Thickness(-11, 0, 0, 0),
                BorderRadius = 0,
                BorderWidth = 0,
                BorderColor = Color.Transparent,
                BackgroundColor = Color.Transparent
            };

            lupaButton.SetBinding(
                Button.IsEnabledProperty
                , new TemplateBinding("Parent.BindingContext.ActualPartialView.BindingContext.LupaButtonIsEnabled")
            );

            lupaButton.SetBinding(
                Button.OpacityProperty
                , new TemplateBinding("Parent.BindingContext.ActualPartialView.BindingContext.LupaButtonOpacity")
            );

			if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
			{
				menuMapaButton.Margin = new Thickness(7, 0, 7, 0);
			}

			StackLayout stackLayout = new StackLayout();            
            stackLayout.WidthRequest = 79;
            stackLayout.HeightRequest = 53;

            stackLayout.Orientation = StackOrientation.Horizontal;

            stackLayout.Children.Add(lupaButton);

            ////Sem Botao StreetView
            //_viewModelMapa.BoxRightContent = refreshButton;

            //Com Botao MapaTipos
            stackLayout.Children.Add(menuMapaButton);
            _viewModelMapa.BoxRightContent = stackLayout;


            //_viewModelMapa.BoxRightContent = lupaButton;

            DateTime dataTemp = ActualPosition.DataEvento.LocalDateTime;

            DataFiltroDate = new DateTime(
                dataTemp.Year
                , dataTemp.Month
                , dataTemp.Day
            );
            MudaDataTitulo();

        }

        public void DataFiltro_DateSelected(
            object sender
            , EventArgs e
        )
        {
            if (!ListViewPosicoesIsRefreshing)
            {
                MudaDataTitulo();

                _isPage = false;

                ActualView.BeginRefresh();
            }
            LupaButtonIsEnabled = true;
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

        public void DataFiltro_Unfocused(
            object sender
            , FocusEventArgs e
        )
        {
            LupaButtonIsEnabled = true;
        }

        private void MudaDataTitulo()
        {
            String dataLabel = DataFiltroDate.Date.ToString("dd/MM/yyyy") + " 00:00 às 23:59";

            _viewModelMapa.PanelSubTituloLabel_Text = dataLabel;
        }

        private void LupaClick()
        {
            LupaButtonIsEnabled = false;
            ActualView.DataFiltroFocus();
        }

        private void ListViewPosicoesRefreshAction(object obj)
        {
            try
            {
                DesativaTodosBotoes();
                Boolean fazRequisicao = false;
                Boolean isReverse = false;
                Int32 stratRowIndex = (_pageNumber * _pageSize);
                DateTime InitialPeriod;
                DateTime FinalPeriod;

                if (_isPage)
                {
                    if (_pageNumber < 0)
                    {
                        if (DataFiltroDate.Date != DateTime.Now.Date)
                        {
                            DataFiltroDate = DataFiltroDate.Date.AddDays((double)1);
                            MudaDataTitulo();
                            _pageNumber = 0;
                            stratRowIndex = 0;
                            isReverse = true;
                            fazRequisicao = true;
                        }
                    }
                    else
                    {
                        if (stratRowIndex >= _total)
                        {
                            DataFiltroDate = DataFiltroDate.Date.AddDays((double)-1);
                            MudaDataTitulo();
                            _pageNumber = 0;
                            stratRowIndex = 0;
                        }

                        fazRequisicao = true;
                    }
                }
                else
                {
                    fazRequisicao = true;
                    _pageNumber = 0;
                    stratRowIndex = 0;
                }

                InitialPeriod = DataFiltroDate;
                FinalPeriod = new DateTime(
                    InitialPeriod.Year
                    , InitialPeriod.Month
                    , InitialPeriod.Day
                    , 23
                    , 59
                    , 59
                );

                if (fazRequisicao)
                {
                    BuscarHistorico(
                        InitialPeriod
                        , FinalPeriod
                        , stratRowIndex
                        , isReverse
                    );
                }
                else
                {
                    EndRefresh_Final();
                }

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

                Crashes.TrackError(ex);

                LupaButtonIsEnabled = true;
            }
        }

        private void BuscarHistorico(
            DateTime paramInitialPeriod
            , DateTime paramFinalPeriod
            , Int32 paramStratRowIndex
            , Boolean paramIsReverse
        )
        {
            try
            {
                _viewModelMapa.ModelMapaGoogle.LimpaMapa();

                if (
                    ListViewPosicoesItemsSource != null
                    && ListViewPosicoesItemsSource.Count > 0
                )
                {
                    ActualView.ScrollTop(ListViewPosicoesItemsSource[0]);
                }

                Int32 idUnidadeRastreada = ActualPosition.IdUnidadeRastreada;
                Byte ordemRastreador = ActualPosition.OrdemRastreador;

                _tokensourceAction = new CancellationTokenSource();

                Task.Run(async () =>
                {
                    ServiceResult<List<PosicaoHistorico>> result = new ServiceResult<List<PosicaoHistorico>>();
                    try
                    {
                        result = await MPosicao.BuscarHistoricoPosicao(
                            idUnidadeRastreada
                            , ordemRastreador
                            , paramInitialPeriod.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss")
                            , paramFinalPeriod.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss")
                            , paramStratRowIndex
                            , _pageSize
                            , paramIsReverse
                            , _tokensourceAction.Token
                        );
                    }
                    catch (Exception ex)
                    {
                        if (ex.ToString().IndexOf("Realms") < 0)
                        {
                            Crashes.TrackError(ex);
                        }
                    }
                    finally
                    {
						if (Device.RuntimePlatform == Device.iOS)
							await Task.Delay(3000);

                        EndRefresh(
                            paramIsReverse
                            , result
                        );
                    }

                }, _tokensourceAction.Token);
            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
        }

        private void EndRefresh(
            Boolean paramIsReverse
            , ServiceResult<List<PosicaoHistorico>> paramResult
        )
        {
            try
            {
                UpdateToken(paramResult.RefreshToken);
                if (!_tokensourceAction.IsCancellationRequested)
                {
                    try
                    {
                        if (String.IsNullOrWhiteSpace(paramResult.MessageError))
                        {
                            ListViewPosicoesItemsSource = paramResult.Data;

                            if (
                                paramResult.Data != null
                                && paramResult.Data.Count > 0
                            )
                            {
                                PosicaoHistorico frist = paramResult.Data[0];

                                _total = frist.Total;

                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    _viewModelMapa
                                        .ModelMapaGoogle
                                        .MontaMapaListaHistorico(paramResult.Data);
                                });

                                if (paramIsReverse)
                                {
                                    _pageNumber = (_total / _pageSize);
                                }

                            }
                            else
                            {
                                _total = 0;
                            }



                        }
                        else
                        {
                            ShowErrorAlert(paramResult.MessageError);
                        }
                    }
                    catch (Exception ex)
                    { }
                    finally
                    {
                        EndRefresh_Final();
                    }
                }
            }
            catch (Exception ex)
            {
                //_messageService.ShowAlertAsync(
                //	AppResources.ErroSuporte
                //                , AppResources.Error
                //);

                Crashes.TrackError(ex);

                EndRefresh_Final();
            }
            finally
            {
            }
        }

        private void EndRefresh_Final()
        {
            AtivaTodosBotoes();
            ActualView.EndRefresh();
            if (
                ListViewPosicoesItemsSource != null
                && ListViewPosicoesItemsSource.Count > 0
            )
            {
                PosicaoHistorico frist = ListViewPosicoesItemsSource[0];

                ActualView.SelectedItem(frist);
            }
        }

        private void AtivaTodosBotoes()
        {
            if (DataFiltroDate.Date == DateTime.Now.Date)
            {
                if (_pageNumber != 0)
                {
                    ImageSetaDireitaButtonOpacity = 1;
                    ImageSetaDireitaButtonIsEnabled = true;
                }
                else
                {
                    ImageSetaDireitaButtonOpacity = 0.5;
                    ImageSetaDireitaButtonIsEnabled = false;
                }
            }
            else
            {
                ImageSetaDireitaButtonOpacity = 1;
                ImageSetaDireitaButtonIsEnabled = true;
            }

            ImageSetaEsquerdaButtonOpacity = 1;
            ImageSetaEsquerdaButtonIsEnabled = true;

            LupaButtonOpacity = 1;
            LupaButtonIsEnabled = true;

            ListViewPosicoesIsEnabled = true;
        }

        private void DesativaTodosBotoes()
        {
            ImageSetaDireitaButtonOpacity = 0.5;
            ImageSetaDireitaButtonIsEnabled = false;

            ImageSetaEsquerdaButtonOpacity = 0.5;
            ImageSetaEsquerdaButtonIsEnabled = false;

            LupaButtonOpacity = 0.5;
            LupaButtonIsEnabled = false;

            ListViewPosicoesIsEnabled = false;
        }

        public void ListViewPosicoesItemSelected(
            PosicaoHistorico paramPosicao
        )
        {
            if (_selectedPosition != null)
            {
                _selectedPosition.Position = new Position(
                    paramPosicao.Latitude.Value
                    , paramPosicao.Longitude.Value
                );

                _viewModelMapa.ModelMapaGoogle.CentralizarMapa(
                    paramPosicao.Latitude.Value
                    , paramPosicao.Longitude.Value
                );

            }
            else
            {
                String iconePrincipal = "";
                switch (paramPosicao.IdTipoUnidadeRastreada)
                {
                    case 2:
                        iconePrincipal = "pin_historico_cel.png";
                        break;
                    default:
                        iconePrincipal = "pin_historico_carro.png";
                        break;
                }

                PosicaoUnidadeRastreadaRealm posi = new PosicaoUnidadeRastreadaRealm()
                {
                    Latitude = paramPosicao.Latitude.Value,
                    Longitude = paramPosicao.Longitude.Value,
                    DataEvento = paramPosicao.DataEvento.Value
                };

                _selectedPosition = _viewModelMapa.ModelMapaGoogle.MontaMapaPinAndCentraliza(
                    posi
                    , iconePrincipal
                );
            }

        }

        public void ListViewPosicoesOnDetalhes(
            object sender
        )
        {
            try
            {
                PosicaoHistorico paramPosicao = (PosicaoHistorico)sender;
                _viewModelMapa.ActualView.ExibirLoad();

                _tokensourceAction = new CancellationTokenSource();

                DesativaTodosBotoes();

                Int32 idUnidadeRastreada = ActualPosition.IdUnidadeRastreada;
                Task.Run(async () =>
                {
                    ServiceResult<PosicaoUnidadeRastreada> result = new ServiceResult<PosicaoUnidadeRastreada>();
                    try
                    {
                        result = await MPosicao.BuscarDetalhePosicao(
                            paramPosicao.IdPosicao
                            , idUnidadeRastreada
                            , paramPosicao.OrdemRastreador
                            , _tokensourceAction.Token
                        );

                        if (result.Data.Latitude == null)
                        {
                            result.Data.Latitude = paramPosicao.Latitude;
                            result.Data.Longitude = paramPosicao.Longitude;
                        }

                        if (result.Data.DataEvento == null || result.Data.DataEvento == DateTime.MinValue)
                            result.Data.DataEvento = paramPosicao.DataEvento;

                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                    finally
                    {
						
                        ListViewPosicoesOnDetalhes_Finaliza(
                            result
                        );
                    }
                }, _tokensourceAction.Token);
            }
            catch (Exception ex)
            {
                _viewModelMapa.ActualView.EscondeLoad();

                //_messageService.ShowAlertAsync(
                //	AppResources.ErroSuporte
                //                , AppResources.Error
                //);

                Crashes.TrackError(ex);

                AtivaTodosBotoes();
            }
        }

        private void ListViewPosicoesOnDetalhes_Finaliza(
            ServiceResult<PosicaoUnidadeRastreada> paramResult
        )
        {
            try
            {
                UpdateToken(paramResult.RefreshToken);
                if (!_tokensourceAction.IsCancellationRequested)
                {
                    if (String.IsNullOrWhiteSpace(paramResult.MessageError))
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {

                            PosicaoUnidadeRastreada posi = paramResult.Data;

                            var grid = new Grid();
                            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });
                            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(200) });
                            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });


                            Frame frame = _viewModelMapa.ActualView.DialogAlert.RequireFramePadrao();
                            frame.Margin = new Thickness(0);

                            AbsoluteLayout cotentPai = new AbsoluteLayout()
                            {
                                Margin = new Thickness(0),
                                HeightRequest = (_app.DefaultTemplateHeightContent * 0.8)
                            };


                            Label detalhes = new Label()
                            {
                                Text = AppResources.Details,
                                HorizontalTextAlignment = TextAlignment.Center,
                                WidthRequest = frame.WidthRequest,
                                Margin = new Thickness(0),
                                HeightRequest = 30,
                                FontSize = 20,
                                FontAttributes = FontAttributes.Bold
                            };
                            cotentPai.Children.Add(detalhes);


                            StackLayout stackLine = new StackLayout()
                            {
                                Margin = new Thickness(0, 35, 0, 0),
                                HeightRequest = 1,
                                WidthRequest = frame.WidthRequest,
                                BackgroundColor = Color.Black

                            };
                            cotentPai.Children.Add(stackLine);


                            AbsoluteLayout.SetLayoutFlags(
                                detalhes
                                , AbsoluteLayoutFlags.PositionProportional
                            );

                            AbsoluteLayout.SetLayoutBounds(
                                detalhes
                                , new Rectangle(
                                    0
                                    , 0
                                    , AbsoluteLayout.AutoSize
                                    , AbsoluteLayout.AutoSize
                                )
                            );

                            Button okButton = new Button()
                            {
                                Text = "OK",
                                Command = new Command(OkButtonCommand),
                                WidthRequest = frame.WidthRequest,
                                HeightRequest = 30,
                                Margin = new Thickness(0),
                                BorderColor = Color.Transparent,
                                BackgroundColor = Color.Transparent,
                                BorderWidth = 0
                            };

                            if (Device.RuntimePlatform == Device.Android)
                            {
                                okButton.HeightRequest += 10;
                            }

                            cotentPai.Children.Add(okButton);

                            AbsoluteLayout.SetLayoutFlags(
                                okButton
                                , AbsoluteLayoutFlags.PositionProportional
                            );

                            AbsoluteLayout.SetLayoutBounds(
                                okButton
                                , new Rectangle(
                                    0
                                    , 1f
                                    , AbsoluteLayout.AutoSize
                                    , AbsoluteLayout.AutoSize
                                )
                            );

                            double alturaMaxima = cotentPai.HeightRequest - (
                                detalhes.HeightRequest
                                + okButton.HeightRequest
                                + 10
                            );

                            ScrollView scrollContent = new ScrollView()
                            {
                                Margin = new Thickness(
                                    0
                                    , (detalhes.HeightRequest + 5)
                                    , 0
                                    , 0
                                ),
                                HeightRequest = alturaMaxima
                            };

                            StackLayout content = new StackLayout()
                            {
                                Margin = new Thickness(0, 5, 0, 0),
                                Spacing = 10
                            };

                            Label localizacao = new Label();

                            localizacao.SetBinding(
                                Label.TextProperty
                                , new Binding(
                                    "LocalizadoDetalhesText"
                                    , BindingMode.Default
                                    , null
                                    , null
                                    , null
                                    , this
                                )

                            );
                            content.Children.Add(localizacao);

                            if (!String.IsNullOrWhiteSpace(posi.Endereco))
                            {
                                LocalizadoDetalhesText = posi.Endereco;
                            }
                            else
                            {
                                LocalizadoDetalhesText = AppResources.GettingAddress;

                                _tokensourceAction = new CancellationTokenSource();
                                Task.Run(async () =>
                                {


                                    String strEndereco = await _viewModelMapa
                                        .ModelMapaGoogle
                                        .FindAddressByPosition(
                                            posi.Latitude.Value
                                            , posi.Longitude.Value
                                        );

                                    LocalizadoDetalhesText = strEndereco;

                                }, _tokensourceAction.Token);

                            }


                            if (posi.Velocidade.HasValue)
                            {
                                Label velociadade = new Label()
                                {
                                    Text = String.Format(
                                        "{0}: {1}"
                                        , AppResources.Speed
                                        , posi.StringVelocidade
                                    )
                                };
                                content.Children.Add(velociadade);
                            }

                            if (posi.DataEvento.HasValue)
                            {
                                Label dataGps = new Label()
                                {
                                    Text = String.Format(
                                        "{0}: {1}"
                                        , AppResources.DatePosition
                                        , posi.StringDataEvento
                                    )
                                };
                                content.Children.Add(dataGps);
                            }

                            if (posi.DataAtualizacao.HasValue)
                            {
                                Label dataAtualizacao = new Label()
                                {
                                    Text = String.Format(
                                        "{0}: {1}"
                                        , AppResources.DateUpdate
                                        , posi.StringDataAtualizacao
                                    )
                                };
                                content.Children.Add(dataAtualizacao);
                            }

                            if (!String.IsNullOrWhiteSpace(posi.Evento))
                            {
                                Label evento = new Label()
                                {
                                    Text = String.Format(
                                        "{0}: {1}"
                                        , AppResources.Event
                                        , posi.Evento
                                    )
                                };
                                content.Children.Add(evento);
                            }

                            if (posi.Ignicao.HasValue)
                            {
                                Label ignicao = new Label();
                                String ligadoText = (posi.Ignicao.Value ? AppResources.On : AppResources.Off);

                                ignicao.Text = String.Format(
                                    "{0}: {1}"
                                    , AppResources.Ignition
                                    , ligadoText
                                );
                                content.Children.Add(ignicao);
                            }

                            if (posi.GPSValido.HasValue)
                            {
                                Label gps = new Label();

                                String gpsText = (posi.GPSValido.Value ? AppResources.Valid : AppResources.Invalid);

                                gps.Text = String.Format(
                                    "GPS {0}"
                                    , gpsText
                                );

                                content.Children.Add(gps);
                            }

                            if (posi.SinalGPRS.HasValue)
                            {
                                Label gprs = new Label()
                                {
                                    Text = String.Format(
                                        "{0}: {1}"
                                        , AppResources.GprsSignal
                                        , posi.SinalGPRS.Value
                                    )
                                };
                                content.Children.Add(gprs);
                            }

                            if (posi.Odometro.HasValue)
                            {
                                Label odometro = new Label()
                                {
                                    Text = String.Format(
                                        "{0}: {1}"
                                        , AppResources.Odometer
                                        , posi.Odometro.Value
                                    )
                                };
                                content.Children.Add(odometro);
                            }

                            if (posi.BateriaPrincipal.HasValue)
                            {
                                Label bateria;

                                bateria = new Label()
                                {
                                    Text = String.Format(
                                        "{0}: {1} %"
                                        , AppResources.Battery
                                        , posi.BateriaPrincipal.Value
                                    )
                                };

                                content.Children.Add(bateria);
                            }

                            scrollContent.Content = content;

                            cotentPai.Children.Add(scrollContent);

                            frame.Content = cotentPai;

                            _viewModelMapa.ActualView.EscondeLoad();
                            _viewModelMapa.ActualView.ShowAlert(frame);
                        });

                    }
                    else
                    {

                        _viewModelMapa.ActualView.EscondeLoad();
                        ShowErrorAlert(paramResult.MessageError);
                    }


                    AtivaTodosBotoes();
                }
            }
            catch (Exception ex)
            {
                _viewModelMapa.ActualView.EscondeLoad();
                //_messageService.ShowAlertAsync(
                //	AppResources.ErroSuporte
                //                , AppResources.Error
                //);

                Crashes.TrackError(ex);
                AtivaTodosBotoes();
            }
        }

        private void OkButtonCommand(object obj)
        {
            _viewModelMapa.ActualView.EscondeLoad();
        }

        private void PaginarHistoricoAction(
            object paramPasso
        )
        {
            try
            {
                if (!ListViewPosicoesIsRefreshing)
                {
                    _pageNumber += Convert.ToInt32(paramPasso);

                    _isPage = true;

                    ActualView.BeginRefresh();
                }

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

        }

		private void PaginarHistoricoActionRight(
			object paramPasso
		)
		{
			try
			{
				if (!ListViewPosicoesIsRefreshing)
				{
					_pageNumber += -1;

					_isPage = true;

					ActualView.BeginRefresh();
				}

			}
			catch (Exception ex)
			{
				Crashes.TrackError(ex);
			}

		}

		private void PaginarHistoricoActionLeft(
			object paramPasso
		)
		{
			try
			{
				if (!ListViewPosicoesIsRefreshing)
				{
					_pageNumber += 1;

					_isPage = true;

					ActualView.BeginRefresh();
				}

			}
			catch (Exception ex)
			{
				Crashes.TrackError(ex);
			}

		}
    }
#pragma warning restore CS1998
#pragma warning restore RECS0022
#pragma warning restore CS4014
}