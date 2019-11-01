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

using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System.Linq;
using family.ViewModels.InterfaceServices;

namespace family.ViewModels
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
	public class ViewModelPontoControle : ViewModelBase
	{
		private ViewModelMapa _viewModelMapa { get; set; }

		public IPartialViewPontoControle ActualView { get; set; }


		public Int64 IdRastreador { get; set; }
		public PosicaoUnidadeRastreadaRealm ActualPosition { get; set; }

		public CancellationTokenSource _tokensourceAction { get; set; }

		private Circle _pontoControle { get; set; }
		private PontoControle _novoPontoControle { get; set; }

		private ModelPontoControle _mPontoControle;
		public ModelPontoControle MPontoControle
		{
			get
			{
				if (_mPontoControle == null)
				{
					_mPontoControle = new ModelPontoControle();
				}
				return _mPontoControle;
			}
		}

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
				PainelHeight = ContentLoadBoxHeight - value;
				this.Notify("PanelSearchHeight");
			}
		}

		private Boolean _maintainIsVisible;
		public Boolean MaintainIsVisible
		{
			get
			{
				return _maintainIsVisible;
			}
			set
			{
				_maintainIsVisible = value;
				this.Notify("MaintainIsVisible");
			}
		}

		private Boolean _maintainIsEnabled;
		public Boolean MaintainIsEnabled
		{
			get
			{
				return _maintainIsEnabled;
			}
			set
			{
				_maintainIsEnabled = value;
				this.Notify("MaintainIsEnabled");
			}
		}

		private Boolean _panelBotoesNovoPontoIsVisible;
		public Boolean PanelBotoesNovoPontoIsVisible
		{
			get
			{
				return _panelBotoesNovoPontoIsVisible;
			}
			set
			{
				_panelBotoesNovoPontoIsVisible = value;
				this.Notify("PanelBotoesNovoPontoIsVisible");
			}
		}

		private Boolean _panelCalendarIsVisible;
		public Boolean PanelCalendarIsVisible
		{
			get
			{
				return _panelCalendarIsVisible;
			}
			set
			{
				_panelCalendarIsVisible = value;
				this.Notify("PanelCalendarIsVisible");
			}
		}

		private Boolean _panelLabelIsVisible;
		public Boolean PanelLabelIsVisible
		{
			get
			{
				return _panelLabelIsVisible;
			}
			set
			{
				_panelLabelIsVisible = value;
				this.Notify("PanelLabelIsVisible");
			}
		}

		private Boolean _panelCalendarIsEnabled;
		public Boolean PanelCalendarIsEnabled
		{
			get
			{
				return _panelCalendarIsEnabled;
			}
			set
			{
				_panelCalendarIsEnabled = value;
				this.Notify("PanelCalendarIsEnabled");
			}
		}

		private Double _panelBotoesNovoPontoWidth;
		public Double PanelBotoesNovoPontoWidth
		{
			get
			{
				return _panelBotoesNovoPontoWidth;
			}
			set
			{
				_panelBotoesNovoPontoWidth = value;
				this.Notify("PanelBotoesNovoPontoWidth");
			}
		}

		private Double _painelHeight;
		public Double PainelHeight
		{
			get
			{
				return _painelHeight;
			}
			set
			{
				_painelHeight = value;
				ListPontosControleHeight = value;
				this.Notify("PainelHeight");
			}
		}

		#region ListPontosControle
		public ICommand ListPontosControleRefreshCommand { get; set; }
		public ICommand ListPontosControleOnEditCommand { get; set; }
		public ICommand ListPontosControleOnDeleteCommand { get; set; }

		private Double _listPontosControleHeight;
		public Double ListPontosControleHeight
		{
			get
			{
				return _listPontosControleHeight;
			}
			set
			{
				_listPontosControleHeight = value;
				this.Notify("ListPontosControleHeight");
			}
		}

		private Boolean _listPontosControleIsEnabled;
		public Boolean ListPontosControleIsEnabled
		{
			get
			{
				return _listPontosControleIsEnabled;
			}
			set
			{
				_listPontosControleIsEnabled = value;
				this.Notify("ListPontosControleIsEnabled");
			}
		}

		private Boolean _listPontosControleIsRefreshing;
		public Boolean ListPontosControleIsRefreshing
		{
			get
			{
				return _listPontosControleIsRefreshing;
			}
			set
			{
				_listPontosControleIsRefreshing = value;
				ListPontosControleIsEnabled = !value;
				this.Notify("ListPontosControleIsRefreshing");
			}
		}

		private Boolean _listPontosControleIsVisible;
		public Boolean ListPontosControleIsVisible
		{
			get
			{
				return _listPontosControleIsVisible;
			}
			set
			{
				_listPontosControleIsVisible = value;
				this.Notify("ListPontosControleIsVisible");
			}
		}

		private List<PontoControle> _listPontosControleItemsSource;
		public List<PontoControle> ListPontosControleItemsSource
		{
			get
			{
				return _listPontosControleItemsSource;
			}
			set
			{
				_listPontosControleItemsSource = value;
				this.Notify("ListPontosControleItemsSource");
			}
		}
		#endregion

		#region BtnAdicionarPontoControle
		public ICommand BtnAdicionarPontoControleCommand { get; set; }

		private String _btnAdicionarPontoControleText;
		public String BtnAdicionarPontoControleText
		{
			get
			{
				return _btnAdicionarPontoControleText;
			}
			set
			{
				_btnAdicionarPontoControleText = value;
				this.Notify("BtnAdicionarPontoControleText");
			}
		}
		#endregion

		#region BtnCancelar
		public ICommand BtnCancelarCommand { get; set; }

		private String _btnCancelarText;
		public String BtnCancelarText
		{
			get
			{
				return _btnCancelarText;
			}
			set
			{
				_btnCancelarText = value;
				this.Notify("BtnCancelarText");
			}
		}
		#endregion

		#region BtnSalvarText
		public ICommand BtnSalvarCommand { get; set; }

		private String _btnSalvarText;
		public String BtnSalvarText
		{
			get
			{
				return _btnSalvarText;
			}
			set
			{
				_btnSalvarText = value;
				this.Notify("BtnSalvarText");
			}
		}
		#endregion

		#region PainelMaitain
		private Thickness _panelLabelMargin;
		public Thickness PanelLabelMargin
		{
			get
			{
				return _panelLabelMargin;
			}
			set
			{
				_panelLabelMargin = value;
				this.Notify("PanelLabelMargin");
			}
		}

		private String _txtNomePlaceholder;
		public String TxtNomePlaceholder
		{
			get
			{
				return _txtNomePlaceholder;
			}
			set
			{
				_txtNomePlaceholder = value;
				this.Notify("TxtNomePlaceholder");
			}
		}

		private String _txtNomeText;
		public String TxtNomeText
		{
			get
			{
				return _txtNomeText;
			}
			set
			{
				_txtNomeText = value;
				this.Notify("TxtNomeText");
			}
		}

		private Double _panelLabelHeight;
		public Double PanelLabelHeight
		{
			get
			{
				return _panelLabelHeight;
			}
			set
			{
				_panelLabelHeight = value;
				this.Notify("PanelLabelHeight");
			}
		}

		private String _txtEnderecoPlaceholder;
		public String TxtEnderecoPlaceholder
		{
			get
			{
				return _txtEnderecoPlaceholder;
			}
			set
			{
				_txtEnderecoPlaceholder = value;
				this.Notify("TxtEnderecoPlaceholder");
			}
		}

		private String _txtEnderecoText;
		public String TxtEnderecoText
		{
			get
			{
				return _txtEnderecoText;
			}
			set
			{
				_txtEnderecoText = value;
				this.Notify("TxtEnderecoText");
			}
		}

		private Double _txtEnderecoWidth;
		public Double TxtEnderecoWidth
		{
			get
			{
				return _txtEnderecoWidth;
			}
			set
			{
				_txtEnderecoWidth = value;
				this.Notify("TxtEnderecoWidth");
			}
		}

		private Double _btnBuscarEnderecoWidth;
		public Double BtnBuscarEnderecoWidth
		{
			get
			{
				return _btnBuscarEnderecoWidth;
			}
			set
			{
				_btnBuscarEnderecoWidth = value;
				this.Notify("BtnBuscarEnderecoWidth");
			}
		}

		public ICommand BtnBuscarEnderecoCommand { get; set; }

		private String _labelRaioText;
		public String LabelRaioText
		{
			get
			{
				return _labelRaioText;
			}
			set
			{
				_labelRaioText = value;
				this.Notify("LabelRaioText");
			}
		}

		private String _labelMetrosText;
		public String LabelMetrosText
		{
			get
			{
				return _labelMetrosText;
			}
			set
			{
				_labelMetrosText = value;
				this.Notify("LabelMetrosText");
			}
		}

		private Thickness _lineMargin;
		public Thickness LineMargin
		{
			get
			{
				return _lineMargin;
			}
			set
			{
				_lineMargin = value;
				this.Notify("LineMargin");
			}
		}

		private Int32 SliderValueDefault = 300;

		private Int32 _sliderValue;
		public Int32 SliderValue
		{
			get
			{
				return _sliderValue;
			}
			set
			{
				_sliderValue = value;
				this.Notify("SliderValue");
			}
		}

		private Boolean _sliderEnable;
		public Boolean SliderEnable
		{
			get
			{
				return _sliderEnable;
			}
			set
			{
				_sliderEnable = value;
				this.Notify("SliderEnable");
			}
		}

		public ICommand OpenSemanaCommand { get; set; }

		public ICommand CloseSemanaCommand { get; set; }

		#endregion

		private String _imageCheckedSource;
		public String ImageCheckedSource
		{
			get
			{
				return _imageCheckedSource;
			}
			set
			{
				_imageCheckedSource = value;
				this.Notify("ImageCheckedSource");
			}
		}

		#region Calendario

		public List<Byte> ListDiasSelecionados { get; set; }

		public ICommand GetCheckCommand { get; set; }

		public ICommand ClickSegundaCommand { get; set; }
		public ICommand ClickTercaCommand { get; set; }
		public ICommand ClickQuartaCommand { get; set; }
		public ICommand ClickQuintaCommand { get; set; }
		public ICommand ClickSextaCommand { get; set; }
		public ICommand ClickSabadoCommand { get; set; }
		public ICommand ClickDomingoCommand { get; set; }

		private Boolean _isChecked;
		public Boolean IsChecked
		{
			get
			{
				return _isChecked;
			}
			set
			{
				_isChecked = value;
				this.Notify("IsChecked");
			}
		}


		private TimeSpan _textoHoraInicial;
		public TimeSpan TextoHoraInicial
		{
			get
			{
				return _textoHoraInicial;
			}
			set
			{
				_textoHoraInicial = value;
				this.Notify("TextoHoraInicial");
			}
		}

		private TimeSpan _textoHoraFinal;
		public TimeSpan TextoHoraFinal
		{
			get
			{
				return _textoHoraFinal;
			}
			set
			{
				_textoHoraFinal = value;
				this.Notify("TextoHoraFinal");
			}
		}

		private Boolean _segundaIsEnable;
		public Boolean SegundaIsEnable
		{
			get
			{
				return _segundaIsEnable;
			}
			set
			{
				_segundaIsEnable = value;
				this.Notify("SegundaIsEnable");
			}
		}

		private Boolean _tercaIsEnable;
		public Boolean TercaIsEnable
		{
			get
			{
				return _tercaIsEnable;
			}
			set
			{
				_tercaIsEnable = value;
				this.Notify("TercaIsEnable");
			}
		}

		private Boolean _quartaIsEnable;
		public Boolean QuartaIsEnable
		{
			get
			{
				return _quartaIsEnable;
			}
			set
			{
				_quartaIsEnable = value;
				this.Notify("QuartaIsEnable");
			}
		}

		private Boolean _quintaIsEnable;
		public Boolean QuintaIsEnable
		{
			get
			{
				return _quintaIsEnable;
			}
			set
			{
				_quintaIsEnable = value;
				this.Notify("QuintaIsEnable");
			}
		}

		private Boolean _sextaIsEnable;
		public Boolean SextaIsEnable
		{
			get
			{
				return _sextaIsEnable;
			}
			set
			{
				_sextaIsEnable = value;
				this.Notify("SextaIsEnable");
			}
		}

		private Boolean _sabadoIsEnable;
		public Boolean SabadoIsEnable
		{
			get
			{
				return _sabadoIsEnable;
			}
			set
			{
				_sabadoIsEnable = value;
				this.Notify("SabadoIsEnable");
			}
		}

		private Boolean _domingoIsEnable;
		public Boolean DomingoIsEnable
		{
			get
			{
				return _domingoIsEnable;
			}
			set
			{
				_domingoIsEnable = value;
				this.Notify("DomingoIsEnable");
			}
		}


		private Color _segundaFundoColor;
		public Color SegundaFundoColor
		{
			get
			{
				return _segundaFundoColor;
			}
			set
			{
				_segundaFundoColor = value;
				this.Notify("SegundaFundoColor");
			}
		}

		private Color _tercaFundoColor;
		public Color TercaFundoColor
		{
			get
			{
				return _tercaFundoColor;
			}
			set
			{
				_tercaFundoColor = value;
				this.Notify("TercaFundoColor");
			}
		}

		private Color _quartaFundoColor;
		public Color QuartaFundoColor
		{
			get
			{
				return _quartaFundoColor;
			}
			set
			{
				_quartaFundoColor = value;
				this.Notify("QuartaFundoColor");
			}
		}

		private Color _quintaFundoColor;
		public Color QuintaFundoColor
		{
			get
			{
				return _quintaFundoColor;
			}
			set
			{
				_quintaFundoColor = value;
				this.Notify("QuintaFundoColor");
			}
		}

		private Color _sextaFundoColor;
		public Color SextaFundoColor
		{
			get
			{
				return _sextaFundoColor;
			}
			set
			{
				_sextaFundoColor = value;
				this.Notify("SextaFundoColor");
			}
		}

		private Color _sabadoFundoColor;
		public Color SabadoFundoColor
		{
			get
			{
				return _sabadoFundoColor;
			}
			set
			{
				_sabadoFundoColor = value;
				this.Notify("SabadoFundoColor");
			}
		}

		private Color _domingoFundoColor;
		public Color DomingoFundoColor
		{
			get
			{
				return _domingoFundoColor;
			}
			set
			{
				_domingoFundoColor = value;
				this.Notify("DomingoFundoColor");
			}
		}

		private Color _segundaTextColor;
		public Color SegundaTextColor
		{
			get
			{
				return _segundaTextColor;
			}
			set
			{
				_segundaTextColor = value;
				this.Notify("SegundaTextColor");
			}
		}

		private Color _tercaTextColor;
		public Color TercaTextColor
		{
			get
			{
				return _tercaTextColor;
			}
			set
			{
				_tercaTextColor = value;
				this.Notify("TercaTextColor");
			}
		}

		private Color _quartaTextColor;
		public Color QuartaTextColor
		{
			get
			{
				return _quartaTextColor;
			}
			set
			{
				_quartaTextColor = value;
				this.Notify("QuartaTextColor");
			}
		}

		private Color _quintaTextColor;
		public Color QuintaTextColor
		{
			get
			{
				return _quintaTextColor;
			}
			set
			{
				_quintaTextColor = value;
				this.Notify("QuintaTextColor");
			}
		}

		private Color _sextaTextColor;
		public Color SextaTextColor
		{
			get
			{
				return _sextaTextColor;
			}
			set
			{
				_sextaTextColor = value;
				this.Notify("SextaTextColor");
			}
		}

		private Color _sabadoTextColor;
		public Color SabadoTextColor
		{
			get
			{
				return _sabadoTextColor;
			}
			set
			{
				_sabadoTextColor = value;
				this.Notify("SabadoTextColor");
			}
		}

		private Color _domingoTextColor;
		public Color DomingoTextColor
		{
			get
			{
				return _domingoTextColor;
			}
			set
			{
				_domingoTextColor = value;
				this.Notify("DomingoTextColor");
			}
		}

		#endregion


		public ViewModelPontoControle(
			ViewModelMapa viewModelMapa
		) : base(true)
		{
			_viewModelMapa = viewModelMapa;
			IdRastreador = _viewModelMapa.IdRastreador;
			ActualPosition = _viewModelMapa.ActualPosition;

			PanelSearchHeight = 47;


			ListPontosControleIsRefreshing = false;
			ListPontosControleIsVisible = true;
			ListPontosControleRefreshCommand = new Command(this.ListPontosControleRefreshAction);

			OpenSemanaCommand = new Command(this.OpenSemana);
			CloseSemanaCommand = new Command(this.CloseSemana);

			BtnAdicionarPontoControleText = AppResources.AddCheckPoint;
			BtnAdicionarPontoControleCommand = new Command(BtnAdicionarPontoControleAction);


			ClickSegundaCommand = new Command(this.ClickSegunda);
			ClickTercaCommand = new Command(this.ClickTerca);
			ClickQuartaCommand = new Command(this.ClickQuarta);
			ClickQuintaCommand = new Command(this.ClickQuinta);
			ClickSextaCommand = new Command(this.ClickSexta);
			ClickSabadoCommand = new Command(this.ClickSabado);
			ClickDomingoCommand = new Command(this.ClickDomingo);


			//GetCheckCommand = new Command(this.GetCheck);

			ImageCheckedSource = "checkbox_off.png";


			MaintainIsVisible = false;
			MaintainIsEnabled = false;
			PanelBotoesNovoPontoIsVisible = false;

			PanelCalendarIsVisible = false;
			PanelCalendarIsEnabled = false;

			ListDiasSelecionados = new List<Byte>();


			SegundaFundoColor = Color.White;
			TercaFundoColor = Color.White;
			QuartaFundoColor = Color.White;
			QuintaFundoColor = Color.White;
			SextaFundoColor = Color.White;
			SabadoFundoColor = Color.White;
			DomingoFundoColor = Color.White;

			SegundaTextColor = Color.Black;
			TercaTextColor = Color.Black;
			QuartaTextColor = Color.Black;
			QuintaTextColor = Color.Black;
			SextaTextColor = Color.Black;
			SabadoTextColor = Color.Black;
			DomingoTextColor = Color.Black;


			PanelBotoesNovoPontoWidth = (DefaultWidth / (double)2);

			BtnSalvarText = AppResources.Salve;
			BtnCancelarText = AppResources.Cancel;

			BtnSalvarCommand = new Command(this.BtnSalvarAction);
			BtnCancelarCommand = new Command(this.BtnCancelarAction);
			BtnBuscarEnderecoCommand = new Command(this.BtnBuscarEnderecoAction);

			PainelMaintainSize();

			this.DefaultTemplateBuild();
		}

		public override void OnAppearing()
		{

            _viewModelMapa.ActualView.ExibirLoad();

            PageBackgroundColor = _viewModelMapa.ActualPage.ColorBarra;
			PanelSearchBackgroundColor = _viewModelMapa.ActualPage.Color;

			ListPontosControleOnEditCommand = new Command(this.ListPontosControleOnEditAction);
			ListPontosControleOnDeleteCommand = new Command(this.ListPontosControleOnDeleteAction);

			ActualView.BeginRefresh();

		}

		public override void OnDisappearing()
		{
			if (_tokensourceAction != null)
				_tokensourceAction.Cancel();

			_viewModelMapa.ModelMapaGoogle._mapa.CameraIdled -= MapaCameraIdled;
		}

		public override void OnLayoutChanged()
		{
		}

		private void DefaultTemplateBuild()
		{
			_viewModelMapa.PanelTituloLabel_Text = AppResources.Checkpoint;
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

		#region ListPontosControle
		private void ListPontosControleRefreshAction(
			object obj
		)
		{
			ServiceResult<List<PontoControle>> result = new ServiceResult<List<PontoControle>>();
			try
			{
				_viewModelMapa.ModelMapaGoogle.LimpaMapa();

				if (
					ListPontosControleItemsSource != null
					&& ListPontosControleItemsSource.Count > 0
				)
				{
					ActualView.ScrollTop(ListPontosControleItemsSource[0]);
				}

				_tokensourceAction = new CancellationTokenSource();

				Task.Run(async () =>
				{
					try
					{
						result = await MPontoControle.BuscarPontoControle(
							_tokensourceAction.Token
						);

					}
					catch (Exception ex)
					{
						Crashes.TrackError(ex);
						
					}
					finally
					{
						BuscaPontosEndRefresh(result);
					}

				}, _tokensourceAction.Token);
			}
			catch (Exception ex)
			{
				Crashes.TrackError(ex);
				BuscaPontosEndRefresh(result);
			}
		}

		private void MarcaPontos()
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				if (
					ListPontosControleItemsSource != null
					&& ListPontosControleItemsSource.Count > 0
				)
				{
					List<Position> lst = new List<Position>();
					foreach (PontoControle item in ListPontosControleItemsSource)
					{
						PosicaoUnidadeRastreadaRealm posi = new PosicaoUnidadeRastreadaRealm()
						{
							Latitude = item.Latitude,
							Longitude = item.Longitude
						};

						Pin temp = _viewModelMapa.ModelMapaGoogle.MontaMapaPinPontoControle(
							posi
							, item
							, "pin_ponto_controle.png"
						);

						lst.Add(temp.Position);

					}

					_viewModelMapa.ModelMapaGoogle.CentralizarMapa(Bounds.FromPositions(lst));

				}
			});
		}

		private void BuscaPontosEndRefresh(ServiceResult<List<PontoControle>> paramResult)
		{
			try
			{
				ActualView.EndRefresh();
				ListPontosControleIsEnabled = true;
				UpdateToken(paramResult.RefreshToken);

				if (!_tokensourceAction.IsCancellationRequested)
				{
					if (String.IsNullOrWhiteSpace(paramResult.MessageError))
					{
						ListPontosControleItemsSource = paramResult.Data;

						MarcaPontos();

					}
					else
					{
						ShowErrorAlert(paramResult.MessageError);
					}
				}

			}
			catch (Exception ex)
			{
				Crashes.TrackError(ex);
			}
			finally
			{
			}
		}

		public void ListPontosControleItemSelected(
			PontoControle paramPonto
		)
		{
			try
			{
				_viewModelMapa.ModelMapaGoogle.ClearCircle();

				Circle circleTemp = _viewModelMapa.ModelMapaGoogle.DrawCircle(
					paramPonto.Latitude
					, paramPonto.Longitude
					, paramPonto.Tolerancia
				);

				_viewModelMapa.ModelMapaGoogle.CentralizarMapa(
					paramPonto.Latitude
					, paramPonto.Longitude
				);

			}
			catch (Exception ex)
			{
				_messageService.ShowAlertAsync(
					AppResources.ErroSuporte
					, AppResources.Error
				);

				Crashes.TrackError(ex);
			}

		}

		private async void ListPontosControleOnDeleteAction(
			object obj
		)
		{

			ServiceResult<Boolean?> resultService = new ServiceResult<bool?>();
			try
			{
				PontoControle menuItem = ((PontoControle)obj);
				ListPontosControleIsEnabled = false;

				Boolean result = await _messageService.ShowAlertChooseAsync(
					AppResources.ConfirmExclusionCheckPoint
					, AppResources.No
					, AppResources.Yes
					, AppResources.Exclusion
				);

				if (result == true)
				{
					_tokensourceAction = new CancellationTokenSource();
					Task.Run(async () =>
					{
						_viewModelMapa.ActualView.ExibirLoad();

						try
						{
							resultService = await MPontoControle.DeletePontoControle(
								menuItem.IdGeography
								, _tokensourceAction.Token
							);
						}
						catch (Exception ex)
						{
							resultService.MessageError = "Exception";
						}
						finally
						{
							ListPontosControleOnDeleteAction_Finish(resultService);
						}
					}, _tokensourceAction.Token);
				}
				else
				{
					ListPontosControleIsEnabled = true;
				}

			}
			catch (Exception ex)
			{
				resultService.MessageError = "Exception";
				ListPontosControleOnDeleteAction_Finish(resultService);
			}
		}

		private void ListPontosControleOnDeleteAction_Finish(
			ServiceResult<Boolean?> paramResultService
		)
		{
			try
			{
				if (!_tokensourceAction.IsCancellationRequested)
				{
					if (!String.IsNullOrWhiteSpace(paramResultService.MessageError))
					{
						ShowErrorAlert(paramResultService.MessageError);
					}
					else
					{
						_messageService.ShowAlertAsync(
							AppResources.pontocontrole_sucessoexcluido
							, AppResources.Success
						);
						ActualView.BeginRefresh();
					}
				}
			}
			catch (Exception ex)
			{
				//Crashes.TrackError(ex);
			}
			finally
			{
				_viewModelMapa.ActualView.EscondeLoad();
				ListPontosControleIsEnabled = true;
				UpdateToken(paramResultService.RefreshToken);
			}
		}

		private void ListPontosControleOnEditAction(
			object obj
		)
		{
			try
			{
				ControlsNovoPontoControle(true);
				_novoPontoControle = ((PontoControle)obj);

				MontaPontoControleMaintain(
					_novoPontoControle.Latitude
					, _novoPontoControle.Longitude
				);

				TxtNomeText = _novoPontoControle.NomePonto;
				//TxtEnderecoText = _novoPontoControle.Endereco;
				TxtEnderecoText = _novoPontoControle.Descricao;
				SliderValue = _novoPontoControle.Tolerancia;

				if (_novoPontoControle.IsNotificaPontoHorario == true)
				{
					ImageCheckedSource = "checkbox_off.png";
					IsChecked = false;
				}
				else
				{
					ImageCheckedSource = "checkbox_on.png";
					IsChecked = true;
				}


				if (_novoPontoControle.HoraInicial != null)
					TextoHoraInicial = _novoPontoControle.HoraInicial; //_novoPontoControle.HoraInicial.ToString(@"hh\:mm");

				if (_novoPontoControle.HoraFinal != null)
					TextoHoraFinal = _novoPontoControle.HoraFinal; //_novoPontoControle.HoraFinal.ToString(@"hh\:mm");


				PopulaDiaSemana(_novoPontoControle.LstDiasSemana);


				_viewModelMapa.ModelMapaGoogle.CentralizarMapa(
					_novoPontoControle.Latitude
					, _novoPontoControle.Longitude
				);

			}
			catch (Exception ex)
			{
				_messageService.ShowAlertAsync(
					AppResources.ErroSuporte
					, AppResources.Error
				);

				Crashes.TrackError(ex);
			}
		}
		#endregion

		private void PopulaDiaSemana(String LstDiasSemana)
		{
			if (!String.IsNullOrEmpty(LstDiasSemana))
			{
				string[] diasSemana = LstDiasSemana.Split(',');

				ListDiasSelecionados.Clear();

				foreach (string dia in diasSemana)
				{

					switch (dia)
					{
						case "0":
							DomingoFundoColor = Color.Orange;
							ListDiasSelecionados.Add(0);
							break;

						case "1":
							SegundaFundoColor = Color.Orange;
							ListDiasSelecionados.Add(1);
							break;

						case "2":
							TercaFundoColor = Color.Orange;
							ListDiasSelecionados.Add(2);
							break;

						case "3":
							QuartaFundoColor = Color.Orange;
							ListDiasSelecionados.Add(3);
							break;

						case "4":
							QuintaFundoColor = Color.Orange;
							ListDiasSelecionados.Add(4);
							break;

						case "5":
							SextaFundoColor = Color.Orange;
							ListDiasSelecionados.Add(5);
							break;

						case "6":
							SabadoFundoColor = Color.Orange;
							ListDiasSelecionados.Add(6);
							break;

					}
				}
			}
		}

		public void OpenSemana()
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				PanelCalendarIsVisible = true;
				PanelCalendarIsEnabled = true;
				PanelBotoesNovoPontoIsVisible = false;
				PanelLabelIsVisible = false;

			});
		}

		public void CloseSemana()
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				PanelCalendarIsVisible = false;
				PanelBotoesNovoPontoIsVisible = true;
				PanelLabelIsVisible = true;

			});
		}

		private void ControlsNovoPontoControle(
			Boolean paramMaintain
		)
		{
			try
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					_viewModelMapa.ModelMapaGoogle.LimpaMapa();
				});
				ListPontosControleIsEnabled = !paramMaintain;
				ListPontosControleIsVisible = !paramMaintain;

				MaintainIsEnabled = paramMaintain;
				MaintainIsVisible = paramMaintain;
				SliderEnable = paramMaintain;

				PanelLabelIsVisible = paramMaintain;
				PanelBotoesNovoPontoIsVisible = paramMaintain;

				TxtEnderecoText = String.Empty;
				TxtNomeText = String.Empty;
				_novoPontoControle = null;

				if (paramMaintain)
				{
					ListPontosControleHeight = 0;
					SliderValue = SliderValueDefault;

					_viewModelMapa.ModelMapaGoogle._mapa.CameraIdled += MapaCameraIdled;
				}
				else
				{
					_viewModelMapa.ModelMapaGoogle._mapa.CameraIdled -= MapaCameraIdled;

					ListPontosControleHeight = PainelHeight;

				}

			}
			catch (Exception ex)
			{
				//Crashes.TrackError(ex);
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

			_pontoControle = _viewModelMapa.ModelMapaGoogle.DrawCircle(
				paramLatitude
				, paramLongitude
				, SliderValue
			);
		}

		private void BtnAdicionarPontoControleAction(
			object obj
		)
		{
			ControlsNovoPontoControle(true);

			MontaPontoControleMaintain(
				ActualPosition.Latitude
				, ActualPosition.Longitude
			);

			_viewModelMapa.ModelMapaGoogle.CentralizarMapa(
				ActualPosition.Latitude
				, ActualPosition.Longitude
			);
		}

		private void BtnCancelarAction(object obj)
		{
			ControlsNovoPontoControle(false);
			MarcaPontos();
		}

		private void BtnSalvarAction(object obj)
		{

			ServiceResult<Boolean?> result = new ServiceResult<bool?>();
			try
			{

				if (String.IsNullOrEmpty(TxtNomeText))
				{
					_messageService.ShowAlertAsync(
						AppResources.ErrorNameEmpty
						, AppResources.Error
					);
				}
				else
				{
					if (_novoPontoControle == null)
						_novoPontoControle = new PontoControle();

					_novoPontoControle.NomePonto = TxtNomeText;
					_novoPontoControle.Endereco = TxtEnderecoText;
					_novoPontoControle.Tolerancia = Convert.ToInt32(_pontoControle.Radius.Meters);
					_novoPontoControle.Latitude = _pontoControle.Center.Latitude;
					_novoPontoControle.Longitude = _pontoControle.Center.Longitude;


					if (IsChecked == true)
					{
						_novoPontoControle.IsNotificaPontoHorario = false;
						ImageCheckedSource = "checkbox_off.png";
					}
					else
					{
						_novoPontoControle.IsNotificaPontoHorario = true;
						ImageCheckedSource = "checkbox_on.png";
					}


					if (TextoHoraInicial.ToString() != "00:00:00" || TextoHoraFinal.ToString() != "00:00:00")
					{
						if (ValidaHora() == true)
						{
							_novoPontoControle.HoraInicial = TextoHoraInicial;
							_novoPontoControle.HoraFinal = TextoHoraFinal;


							if (ListDiasSelecionados.Count > 0)
								_novoPontoControle.LstDiasSemana = string.Join(",", ListDiasSelecionados);

						}
						else
						{
							return;
						}
					}


					_tokensourceAction = new CancellationTokenSource();

					Task.Run(async () =>
					{
						try
						{
							_viewModelMapa.ActualView.ExibirLoad();

							if (_novoPontoControle.IdGeography != 0)
							{
								result = await MPontoControle.UpdatePontoControle(
									_novoPontoControle
									, _tokensourceAction.Token
								);
							}
							else
							{
								result = await MPontoControle.InsertPontoControle(
									_novoPontoControle
									, _tokensourceAction.Token
								);
							}
						}
						catch (Exception ex)
						{
							Crashes.TrackError(ex);
						}
						finally
						{
							BtnSalvarAction_Finish(result);
						}


					}, _tokensourceAction.Token);
				}

			}
			catch (Exception ex)
			{
				Crashes.TrackError(ex);
				BtnSalvarAction_Finish(result);
			}

		}

		private void BtnSalvarAction_Finish(
			ServiceResult<Boolean?> paramResult
		)
		{
			try
			{
				if (!_tokensourceAction.IsCancellationRequested)
				{

					if (String.IsNullOrWhiteSpace(paramResult.MessageError))
					{
						BtnCancelarAction(null);
						ActualView.BeginRefresh();
					}
					else
					{
						ShowErrorAlert(paramResult.MessageError);
					}
				}
			}
			catch (Exception ex)
			{
				//Crashes.TrackError(ex);
				ShowErrorAlert("Exception");
			}
			finally
			{
				_viewModelMapa.ActualView.EscondeLoad();
				UpdateToken(paramResult.RefreshToken);
			}
		}

		private void PainelMaintainSize()
		{
			PanelLabelMargin = new Thickness(10);
			LineMargin = new Thickness(0, 0, 0, 5);
			PanelLabelHeight = 45;

			TxtNomePlaceholder = AppResources.Name;

			TxtEnderecoPlaceholder = AppResources.Address;

			BtnBuscarEnderecoWidth = 53;

			TxtEnderecoWidth = DefaultWidth - (
				PanelLabelMargin.Left
				+ PanelLabelMargin.Right
				+ BtnBuscarEnderecoWidth
			);

			LabelRaioText = AppResources.Radius;

			SliderValue = SliderValueDefault;
		}

		#region Slider
		public void SliderPontoControle_ValueChanged(
			object sender
			, ValueChangedEventArgs e
		)
		{
			MudaValorMetros(e.NewValue);
		}

		public void SliderPontoControle_ButtonPress(
			object sender
			, EventArgs e
		)
		{
			MudaValorMetros(SliderValue);
		}

		private void MudaValorMetros(Double paramNewValue)
		{
			LabelMetrosText =
				Convert.ToInt32(paramNewValue).ToString()
					   + " " + AppResources.Meters;

			if (_pontoControle != null)
				_pontoControle.Radius = Distance
					.FromMeters(paramNewValue);
		}
		#endregion

		private void BtnBuscarEnderecoAction(object obj)
		{
			try
			{
				if (String.IsNullOrWhiteSpace(TxtEnderecoText))
				{
					_messageService.ShowAlertAsync(
						AppResources.ErrorAddressEmpty
						, AppResources.Error
					);
				}
				else
				{

					_tokensourceAction = new CancellationTokenSource();

					Task.Run(async () =>
					{
						try
						{
							_viewModelMapa.ActualView.ExibirLoad();

							LatLong latLng = await _viewModelMapa.ModelMapaGoogle
								.FindPositionByAddress(TxtEnderecoText);

							if (!_tokensourceAction.IsCancellationRequested)
							{
								if (latLng == null)
								{
									_messageService.ShowAlertAsync(
										AppResources.AddressNotFound
										, AppResources.Error
									);
								}
								else
								{
									Device.BeginInvokeOnMainThread(() =>
									{
										MontaPontoControleMaintain(
											latLng.Latitude
											, latLng.Longitude
										);

										_viewModelMapa.ModelMapaGoogle.CentralizarMapa(
											latLng.Latitude
											, latLng.Longitude
										);

									});
								}
								_viewModelMapa.ActualView.EscondeLoad();
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

					}, _tokensourceAction.Token);
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
		}

		private void ClickSegunda()
		{

			Device.BeginInvokeOnMainThread(() =>
			{
				if (ListDiasSelecionados.Where(x => (byte)x == 1).Count() == 0)
				{
					//ValidaHora();

					ListDiasSelecionados.Add(1);
					SegundaFundoColor = Color.Orange;
				}
				else
				{
					ListDiasSelecionados.Remove(1);
					SegundaFundoColor = Color.White;
				}
			});

		}
		private void ClickTerca()
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				if (ListDiasSelecionados.Where(x => (byte)x == 2).Count() == 0)
				{
					//ValidaHora();

					ListDiasSelecionados.Add(2);
					TercaFundoColor = Color.Orange;
				}
				else
				{
					ListDiasSelecionados.Remove(2);
					TercaFundoColor = Color.White;
				}
			});
		}
		private void ClickQuarta()
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				if (ListDiasSelecionados.Where(x => (byte)x == 3).Count() == 0)
				{
					//ValidaHora();

					ListDiasSelecionados.Add(3);
					QuartaFundoColor = Color.Orange;
				}
				else
				{
					ListDiasSelecionados.Remove(3);
					QuartaFundoColor = Color.White;
				}
			});
		}
		private void ClickQuinta()
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				if (ListDiasSelecionados.Where(x => (byte)x == 4).Count() == 0)
				{
					//ValidaHora();

					ListDiasSelecionados.Add(4);
					QuintaFundoColor = Color.Orange;
				}
				else
				{
					ListDiasSelecionados.Remove(4);
					QuintaFundoColor = Color.White;
				}
			});
		}
		private void ClickSexta()
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				if (ListDiasSelecionados.Where(x => (byte)x == 5).Count() == 0)
				{
					//ValidaHora();

					ListDiasSelecionados.Add(5);
					SextaFundoColor = Color.Orange;
				}
				else
				{
					ListDiasSelecionados.Remove(5);
					SextaFundoColor = Color.White;
				}
			});
		}
		private void ClickSabado()
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				if (ListDiasSelecionados.Where(x => (byte)x == 6).Count() == 0)
				{
					//ValidaHora();

					ListDiasSelecionados.Add(6);
					SabadoFundoColor = Color.Orange;
				}
				else
				{
					ListDiasSelecionados.Remove(6);
					SabadoFundoColor = Color.White;
				}
			});
		}
		private void ClickDomingo()
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				if (ListDiasSelecionados.Where(x => (byte)x == 0).Count() == 0)
				{
					//ValidaHora();

					ListDiasSelecionados.Add(0);
					DomingoFundoColor = Color.Orange;
				}
				else
				{
					ListDiasSelecionados.Remove(0);
					DomingoFundoColor = Color.White;
				}
			});
		}

		public void GetCheck(Boolean obj)
		{
			IsChecked = obj;
		}

		private Boolean ValidaSemana()
		{
			Boolean retorno = true;

			if (SegundaFundoColor == Color.White && TercaFundoColor == Color.White
			  && QuartaFundoColor == Color.White && QuintaFundoColor == Color.White
			  && SextaFundoColor == Color.White && SabadoFundoColor == Color.White
			  && DomingoFundoColor == Color.White)
			{
				retorno = false;
			}

			return retorno;
		}

		private Boolean ValidaHora()
		{

			TimeSpan horaInicial;
			TimeSpan horaFinal;

			Boolean retorno = true;


			if (TextoHoraInicial == TimeSpan.MinValue && TextoHoraFinal == TimeSpan.MinValue)
			{
				_messageService.ShowAlertAsync(
					  AppResources.HoraInicialFinalInvalida
					  , AppResources.Error
				  );

				retorno = false;
			}
			else
			{

				int iTime = TimeSpan.Compare(TextoHoraInicial, TextoHoraFinal);
				if (iTime > -1)
				{
					_messageService.ShowAlertAsync(
					   AppResources.HoraInicialIgualMaior
					   , AppResources.Error
				   );

					retorno = false;
				}
				else
				{

					if (ValidaSemana() == false)
					{
						_messageService.ShowAlertAsync(
							AppResources.SelecioneDiaSemana
							, AppResources.Error
						);

						retorno = false;

					}
				}

			}


			return retorno;

		}


	}
#pragma warning restore CS1998
#pragma warning restore RECS0022
#pragma warning restore CS4014
}