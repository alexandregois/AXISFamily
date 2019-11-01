using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using family.CrossPlataform;
using family.Domain.Dto;
using family.Domain.Enum;
using family.Domain.Realm;
using family.Resx;
using family.Services.ServiceRealm;
using family.ViewModels.Base;
using family.Views.Interfaces;
using family.Views.PartialViews;
using family.Views.Services;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace family.ViewModels
{
	public class ViewModelMapa : ViewModelBase
	{

		public Color CorInativa = Color.FromHex("#FF363636");

		public MenuMapa ActualPage { get; set; }

		public Int32 IdUnidadeRastreada { get; set; }

		public Int32 IdRastreador { get; set; }
		public PosicaoUnidadeRastreadaRealm ActualPosition { get; set; }

		public PosicaoDataStore PosicaoDataStore { get; set; }

		public IMapa ActualView { get; set; }
		public IPartialView ActualPartialView { get; set; }

		public Dictionary<EnumPage, MenuMapa> ListMenuMapa { get; set; }
		public StackLayout LayoutMenuMapa { get; set; }

		public ViewModelMapaGoogle ModelMapaGoogle
		{
			get
			{
				return ActualView.GetModelMapaGoogle();
			}
		}

		private Color _contentLoadBoxBackgroundColor;
		public Color ContentLoadBoxBackgroundColor
		{
			get
			{
				return _contentLoadBoxBackgroundColor;
			}
			set
			{
				_contentLoadBoxBackgroundColor = value;
				this.Notify("ContentLoadBoxBackgroundColor");
			}
		}
        
        #region Box Menu
        public ICommand BoxMenuCommand { get; set; }

		public ICommand BoxMenuHistorico { get; set; }

		public ICommand BoxMenuUltimaPosicao { get; set; }

		public ICommand BoxMenuAncora { get; set; }

		public ICommand BoxMenuManutencao { get; set; }

		public ICommand BoxMenuPontoControle { get; set; }

		public ICommand BoxMenuBloqueio { get; set; }

		private Color _ultimaPosicaoBackgroundColor;
		public Color UltimaPosicaoBackgroundColor
		{
			get
			{
				return _ultimaPosicaoBackgroundColor;
			}
			set
			{
				_ultimaPosicaoBackgroundColor = value;
				this.Notify("UltimaPosicaoBackgroundColor");
			}
		}

		private Double _ultimaPosicaoOpacity;
		public Double UltimaPosicaoOpacity
		{
			get
			{
				return _ultimaPosicaoOpacity;
			}
			set
			{
				_ultimaPosicaoOpacity = value;
				this.Notify("UltimaPosicaoOpacity");
			}
		}

        private Boolean _exibeStreet;
        public Boolean ExibeStreet
        {
            get
            {
                return _exibeStreet;
            }
            set
            {
                _exibeStreet = value;
                this.Notify("ExibeStreet");
            }
        }

        private Boolean _exibeMapa;
        public Boolean ExibeMapa
        {
            get
            {
                return _exibeMapa;
            }
            set
            {
                _exibeMapa = value;
                this.Notify("ExibeMapa");
            }
        }

        private Boolean _ultimaPosicaoEnabled;
		public Boolean UltimaPosicaoEnabled
		{
			get
			{
				return _ultimaPosicaoEnabled;
			}
			set
			{
				_ultimaPosicaoEnabled = value;
				this.Notify("UltimaPosicaoEnabled");
			}
		}

		private Color _historicoBackgroundColor;
		public Color HistoricoBackgroundColor
		{
			get
			{
				return _historicoBackgroundColor;
			}
			set
			{
				_historicoBackgroundColor = value;
				this.Notify("HistoricoBackgroundColor");
			}
		}

		private Double _historicoOpacity;
		public Double HistoricoOpacity
		{
			get
			{
				return _historicoOpacity;
			}
			set
			{
				_historicoOpacity = value;
				this.Notify("HistoricoOpacity");
			}
		}

		private Boolean _historicoEnabled;
		public Boolean HistoricoEnabled
		{
			get
			{
				return _historicoEnabled;
			}
			set
			{
				_historicoEnabled = value;
				this.Notify("HistoricoEnabled");
			}
		}

		private Boolean _historicoVisible;
		public Boolean HistoricoVisible
		{
			get
			{
				return _historicoVisible;
			}
			set
			{
				_historicoVisible = value;
				this.Notify("HistoricoVisible");
			}
		}

		private Color _pontoControleBackgroundColor;
		public Color PontoControleBackgroundColor
		{
			get
			{
				return _pontoControleBackgroundColor;
			}
			set
			{
				_pontoControleBackgroundColor = value;
				this.Notify("PontoControleBackgroundColor");
			}
		}

		private Double _pontoControleOpacity;
		public Double PontoControleOpacity
		{
			get
			{
				return _pontoControleOpacity;
			}
			set
			{
				_pontoControleOpacity = value;
				this.Notify("PontoControleOpacity");
			}
		}

		private Boolean _pontoControleEnabled;
		public Boolean PontoControleEnabled
		{
			get
			{
				return _pontoControleEnabled;
			}
			set
			{
				_pontoControleEnabled = value;
				this.Notify("PontoControleEnabled");
			}
		}

		private Boolean _pontoControleVisible;
		public Boolean PontoControleVisible
		{
			get
			{
				return _pontoControleVisible;
			}
			set
			{
				_pontoControleVisible = value;
				this.Notify("PontoControleVisible");
			}
		}

		private Color _ancoraBackgroundColor;
		public Color AncoraBackgroundColor
		{
			get
			{
				return _ancoraBackgroundColor;
			}
			set
			{
				_ancoraBackgroundColor = value;
				this.Notify("AncoraBackgroundColor");
			}
		}

		private Double _ancoraOpacity;
		public Double AncoraOpacity
		{
			get
			{
				return _ancoraOpacity;
			}
			set
			{
				_ancoraOpacity = value;
				this.Notify("AncoraOpacity");
			}
		}

		private Boolean _ancoraEnabled;
		public Boolean AncoraEnabled
		{
			get
			{
				return _ancoraEnabled;
			}
			set
			{
				_ancoraEnabled = value;
				this.Notify("AncoraEnabled");
			}
		}

		private Boolean _ancoraVisible;
		public Boolean AncoraVisible
		{
			get
			{
				return _ancoraVisible;
			}
			set
			{
				_ancoraVisible = value;
				this.Notify("AncoraVisible");
			}
		}

		private Color _manutencaoBackgroundColor;
		public Color ManutencaoBackgroundColor
		{
			get
			{
				return _manutencaoBackgroundColor;
			}
			set
			{
				_manutencaoBackgroundColor = value;
				this.Notify("ManutencaoBackgroundColor");
			}
		}

		private Boolean _manutencaoEnabled;
		public Boolean ManutencaoEnabled
		{
			get
			{
				return _manutencaoEnabled;
			}
			set
			{
				_manutencaoEnabled = value;
				this.Notify("ManutencaoEnabled");
			}
		}

		private Double _manutencaoOpacity;
		public Double ManutencaoOpacity
		{
			get
			{
				return _manutencaoOpacity;
			}
			set
			{
				_manutencaoOpacity = value;
				this.Notify("ManutencaoOpacity");
			}
		}

		private Boolean _manutencaoVisible;
		public Boolean ManutencaoVisible
		{
			get
			{
				return _manutencaoVisible;
			}
			set
			{
				_manutencaoVisible = value;
				this.Notify("ManutencaoVisible");
			}
		}


		private Color _bloqueioBackgroundColor;
		public Color BloqueioBackgroundColor
		{
			get
			{
				return _bloqueioBackgroundColor;
			}
			set
			{
				_bloqueioBackgroundColor = value;
				this.Notify("BloqueioBackgroundColor");
			}
		}

		private Double _bloqueioOpacity;
		public Double BloqueioOpacity
		{
			get
			{
				return _bloqueioOpacity;
			}
			set
			{
				_bloqueioOpacity = value;
				this.Notify("BloqueioOpacity");
			}
		}

		private Boolean _bloqueioEnabled;
		public Boolean BloqueioEnabled
		{
			get
			{
				return _bloqueioEnabled;
			}
			set
			{
				_bloqueioEnabled = value;
				this.Notify("BloqueioEnabled");
			}
		}

		private Boolean _bloqueioVisible;
		public Boolean BloqueioVisible
		{
			get
			{
				return _bloqueioVisible;
			}
			set
			{
				_bloqueioVisible = value;
				this.Notify("BloqueioVisible");
			}
		}

		#endregion

		#region Cabeçalho
		private String _panelTituloLabel_Text;
		public String PanelTituloLabel_Text
		{
			get
			{
				return _panelTituloLabel_Text;
			}
			set
			{
				_panelTituloLabel_Text = value;
				this.Notify("PanelTituloLabel_Text");
			}
		}

		private String _panelSubTituloLabel_Text;
		public String PanelSubTituloLabel_Text
		{
			get
			{
				return _panelSubTituloLabel_Text;
			}
			set
			{
				_panelSubTituloLabel_Text = value;
				this.Notify("PanelSubTituloLabel_Text");
			}
		}

        #endregion
                

        private EnumPage openPage;

		public ViewModelMapa(
			EnumPage paramOpenPage
			, int paramIdRastreador
		) : base(true)
		{
			IdRastreador = paramIdRastreador;

			openPage = paramOpenPage;

			this.DefaultTemplateBuild();

			//BoxMenuCommand = new Command();
			BoxMenuHistorico = new Command(MenuClickHistorico);

			BoxMenuAncora = new Command(MenuClickAncora);
			BoxMenuPontoControle = new Command(MenuClickPontoControle);
			BoxMenuUltimaPosicao = new Command(MenuClickUltimaPosicao);

			BoxMenuManutencao = new Command(MenuClickManutencao);
			BoxMenuBloqueio = new Command(MenuClickBloqueio);

			HistoricoVisible = false;
			PontoControleVisible = false;
			AncoraVisible = false;
			BloqueioVisible = false;
			ManutencaoVisible = false;

			UltimaPosicaoOpacity = 1;
			HistoricoOpacity = 1;
			PontoControleOpacity = 1;
			AncoraOpacity = 1;
			BloqueioOpacity = 1;
			ManutencaoOpacity = 1;

			GetPosition();


            ExibeMapa = true;
            ExibeStreet = false;


            ListMenuMapa = MontaMenuMapaObj();

        }

		public override void OnAppearing()
		{
			try
			{

				HistoricoVisible = true;
				PontoControleVisible = true;
				AncoraVisible = true;
				BloqueioVisible = true;
				ManutencaoVisible = true;

				if (ActualPosition == null)
					GetPosition();

				if (ListMenuMapa == null)
					ListMenuMapa = MontaMenuMapaObj();

				ActualPage = ListMenuMapa[openPage];

				Navegacao(ActualPage);

			}
			catch (Exception ex)
			{
                Crashes.TrackError(ex);
            }

		}

		public override void OnDisappearing()
		{
		}

		public override void OnLayoutChanged()
		{
		}

		private void GetPosition()
		{
			PosicaoDataStore = new PosicaoDataStore();
			ActualPosition = PosicaoDataStore.Get(IdRastreador);
			IdUnidadeRastreada = ActualPosition.IdUnidadeRastreada;
		}

		#region MenuMapa
		public Dictionary<EnumPage, MenuMapa> MontaMenuMapaObj()
		{
			Dictionary<EnumPage, MenuMapa> ListMenuMapa = new Dictionary<EnumPage, MenuMapa>();

			try
			{

				PermissaoService permissao = new PermissaoService();

				#region Última Posição
				MenuMapa ultimaPosicaoMapa = new MenuMapa()
				{
					Color = Color.FromHex("#FF90AD2A"),
					ColorBarra = Color.FromHex("#FFc3df5c"),
					ColorLoad = Color.FromHex("#FFc3df5c"),
					ColorStatusBar = Color.FromHex("#FF5f7e00"),
					Identificacao = AppResources.CurrentLocation,
					Icone = "ic_localizacao.png",
					Pagina = EnumPage.UltimaPosicao,
					IconeLargura = 22,
					IconeAltura = 30
				};
				ListMenuMapa.Add(ultimaPosicaoMapa.Pagina, ultimaPosicaoMapa);
				#endregion

				if (permissao.PodeVisualizarHistorico())
				{
					MenuMapa historico = new MenuMapa()
					{
						Color = Color.FromHex("#FF9259A0"),
						ColorBarra = Color.FromHex("#FFc487d1"),
						ColorLoad = Color.FromHex("#FFc487d1"),
						ColorStatusBar = Color.FromHex("#FF632d71"),
						Identificacao = AppResources.Historic,
						Icone = "ic_historico.png",
						Pagina = EnumPage.HistoricoPosicao,
						IconeLargura = 30,
						IconeAltura = 30
					};

					HistoricoVisible = true;

					ListMenuMapa.Add(historico.Pagina, historico);
				}
				else
				{
					HistoricoVisible = false;
					HistoricoOpacity = 0;
					HistoricoBackgroundColor = Color.Transparent;
					HistoricoEnabled = false;
				}

				if (permissao.PodeManterPontoControle())
				{
					MenuMapa pontoControle = new MenuMapa()
					{
						Color = Color.FromHex("#FF293F6E"),
						ColorBarra = Color.FromHex("#FF58699d"),
						ColorLoad = Color.FromHex("#FF58699d"),
						ColorStatusBar = Color.FromHex("#FF001942"),
						Identificacao = AppResources.Checkpoint,
						Icone = "ic_ponto_controle.png",
						Pagina = EnumPage.PontoControle,
						IconeLargura = 30,
						IconeAltura = 30
					};

					PontoControleVisible = true;

					ListMenuMapa.Add(pontoControle.Pagina, pontoControle);
				}
				else
				{
					PontoControleVisible = false;
					PontoControleOpacity = 0;
					PontoControleBackgroundColor = Color.Transparent;
					PontoControleEnabled = false;
				}

				if (permissao.PodeManterAncora())
				{

					MenuMapa ancora = new MenuMapa()
					{
						Color = Color.FromHex("#FF0091B3"),
						ColorBarra = Color.FromHex("#FF57c1e5"),
						ColorLoad = Color.FromHex("#FF57c1e5"),
						ColorStatusBar = Color.FromHex("#FF006383"),
						Identificacao = AppResources.Anchor,
						Icone = "ic_ancora.png",
						Pagina = EnumPage.Ancora,
						IconeLargura = 34,
						IconeAltura = 30
					};

					AncoraVisible = true;

					ListMenuMapa.Add(ancora.Pagina, ancora);
				}
				else
				{
					AncoraVisible = false;
					AncoraOpacity = 0;
					AncoraBackgroundColor = Color.Transparent;
					AncoraEnabled = false;
				}

				if (permissao.PodeManterBloqueio())
				{
					switch (ActualPosition.IdTipoUnidadeRastreada)
					{
						case (byte)EnumTipoUnidadeRastreada.Carga:
						case (byte)EnumTipoUnidadeRastreada.Veiculo:

							MenuMapa bloqueio = new MenuMapa()
							{
								Color = Color.FromHex("#FFE3851A"),
								ColorBarra = Color.FromHex("#FFffb54e"),
								ColorLoad = Color.FromHex("#FFffb54e"),
								ColorStatusBar = Color.FromHex("#FFab5700"),
								Identificacao = AppResources.Block,
								Icone = "ic_bloq.png",
								Pagina = EnumPage.Bloqueio,
								IconeLargura = 34,
								IconeAltura = 30
							};

							BloqueioVisible = true;

							ListMenuMapa.Add(bloqueio.Pagina, bloqueio);
							break;
						default:
							BloqueioVisible = false;
							BloqueioOpacity = 0;
							BloqueioBackgroundColor = Color.Transparent;
							BloqueioEnabled = false;
							break;
					}
				}
				else
				{
					BloqueioVisible = false;
					BloqueioOpacity = 0;
					BloqueioBackgroundColor = Color.Transparent;
					BloqueioEnabled = false;
				}

				if (permissao.PodeManterManutencao())
				{
					if (ActualPosition.IdTipoUnidadeRastreada == (byte)EnumTipoUnidadeRastreada.Veiculo)
					{

						MenuMapa manutencao = new MenuMapa()
						{
							Color = Color.FromHex("#2f7dc1"),
							ColorBarra = Color.FromHex("#2f7dc1"),
							ColorLoad = Color.FromHex("#2f7dc1"),
							ColorStatusBar = Color.FromHex("#005190"),
							Identificacao = AppResources.Maintenance,
							Icone = "ic_manutencao.png",
							Pagina = EnumPage.Manutencao,
							IconeLargura = 34,
							IconeAltura = 30
						};

						ManutencaoVisible = true;

						ListMenuMapa.Add(manutencao.Pagina, manutencao);

					}
					else
					{
						ManutencaoVisible = false;
						ManutencaoOpacity = 0;
						ManutencaoBackgroundColor = Color.Transparent;
						ManutencaoEnabled = false;
					}
				}
				else
				{
					ManutencaoVisible = false;
					ManutencaoOpacity = 0;
					ManutencaoBackgroundColor = Color.Transparent;
					ManutencaoEnabled = false;
				}

			}
			catch (Exception)
			{

			}

			return ListMenuMapa;
		}

		private void MenuClickHistorico()
		{
			MenuClick(2);
		}

		private void MenuClickUltimaPosicao()
		{
			MenuClick(1);
		}

		private void MenuClickAncora()
		{
			MenuClick(4);
		}

		private void MenuClickBloqueio()
		{
			MenuClick(5);
		}

		private void MenuClickPontoControle()
		{
			MenuClick(3);
		}

		private void MenuClickManutencao()
		{
			MenuClick(6);
		}

		private void MenuClick(
			Int32 paramPagina
		)
		{

            Task.Delay(3000);

				try
				{

					MenuMapa pagina;
					switch (Convert.ToInt32(paramPagina))
					{
						case (Int32)EnumPage.Ancora:
							pagina = ListMenuMapa[EnumPage.Ancora];
							break;
						case (Int32)EnumPage.Bloqueio:
							pagina = ListMenuMapa[EnumPage.Bloqueio];
							break;
						case (Int32)EnumPage.HistoricoPosicao:
							pagina = ListMenuMapa[EnumPage.HistoricoPosicao];
							break;
						case (Int32)EnumPage.PontoControle:
							pagina = ListMenuMapa[EnumPage.PontoControle];
							break;
						case (Int32)EnumPage.Manutencao:
							pagina = ListMenuMapa[EnumPage.Manutencao];
							break;
						default:
							pagina = ListMenuMapa[EnumPage.UltimaPosicao];
							break;
					}

					if (pagina.Pagina != ActualPage.Pagina)
					{
						ActualPartialView.OnDisappearing();
						ModelMapaGoogle.LimpaMapa();
						Navegacao(pagina);
					}

				}
				catch (Exception ex)
				{


				}

		}

		private void DesativaMenu(Boolean paramIsEnabled = false)
		{
			foreach (KeyValuePair<EnumPage, MenuMapa> item in ListMenuMapa)
			{
				switch (item.Key)
				{
					case EnumPage.Ancora:
						AncoraOpacity = 0.2;
						AncoraBackgroundColor = Color.Transparent;
						AncoraEnabled = paramIsEnabled;
						break;
					case EnumPage.Bloqueio:
						BloqueioOpacity = 0.2;
						BloqueioBackgroundColor = Color.Transparent;
						BloqueioEnabled = paramIsEnabled;
						break;
					case EnumPage.HistoricoPosicao:
						HistoricoOpacity = 0.2;
						HistoricoBackgroundColor = Color.Transparent;
						HistoricoEnabled = paramIsEnabled;
						break;
					case EnumPage.PontoControle:
						PontoControleOpacity = 0.2;
						PontoControleBackgroundColor = Color.Transparent;
						PontoControleEnabled = paramIsEnabled;
						break;
					case EnumPage.Manutencao:
						ManutencaoOpacity = 0.2;
						ManutencaoBackgroundColor = Color.Transparent;
						ManutencaoEnabled = paramIsEnabled;
						break;
					case EnumPage.UltimaPosicao:
						UltimaPosicaoOpacity = 0.2;
						UltimaPosicaoBackgroundColor = Color.Transparent;
						UltimaPosicaoEnabled = paramIsEnabled;
						break;
				}
			}

		}

		private void Navegacao(MenuMapa paramPagina)
		{

			DesativaMenu(true);

			if (ActualPartialView != null)
				ActualPartialView.OnDisappearing();

			#region Muda Cores
			PageColor = paramPagina.Color;
			ActualView.ChangeColor(
				paramPagina.ColorStatusBar
				, paramPagina.ColorLoad
			);

			ImageSourceProperty = paramPagina.Icone;
			ImageWidthProperty = paramPagina.IconeAltura;
			#endregion

			switch (paramPagina.Pagina)
			{
				case EnumPage.HistoricoPosicao:
					HistoricoEnabled = true;
					HistoricoOpacity = 1;
					HistoricoBackgroundColor = paramPagina.Color;
					ActualPartialView = new PartialViewHistorico(this);
					break;
				case EnumPage.PontoControle:
					PontoControleEnabled = true;
					PontoControleOpacity = 1;
					PontoControleBackgroundColor = paramPagina.Color;
					ActualPartialView = new PartialViewPontoControle(this);
					break;
				case EnumPage.Ancora:
					AncoraEnabled = true;
					AncoraOpacity = 1;
					AncoraBackgroundColor = paramPagina.Color;
					ActualPartialView = new PartialViewAncora(this);
					break;
				case EnumPage.Bloqueio:
					BloqueioEnabled = true;
					BloqueioOpacity = 1;
					BloqueioBackgroundColor = paramPagina.Color;
					ActualPartialView = new PartialViewBloqueio(this);
					break;
				case EnumPage.Manutencao:
					ManutencaoEnabled = true;
					ManutencaoOpacity = 1;
					ManutencaoBackgroundColor = paramPagina.Color;
					ActualPartialView = new PartialViewManutencao(this);
					break;
				default:
					UltimaPosicaoEnabled = false;
					UltimaPosicaoOpacity = 1;
					UltimaPosicaoBackgroundColor = paramPagina.Color;
					ActualPartialView = new PartialViewUltimaPosicao(this);
					break;
			}

			ContentLoadBoxBackgroundColor = paramPagina.Color;

			ActualView.AddPartialPage((ContentView)ActualPartialView);

			ActualPage = paramPagina;

			ActualPartialView.OnAppearing();

			ActualView.EscondeLoad();

		}
		#endregion

		private void DefaultTemplateBuild()
		{
			VoltarVisible = true;

			StackLayout stackTopo = new StackLayout()
			{
				Orientation = StackOrientation.Vertical,
				HeightRequest = _app.DefaultTemplateHeightNavegationBar,
				Margin = 0,
				Spacing = 0
			};

			Label labelTitulo = PanelTituloLabel_Titulo();
			labelTitulo.HeightRequest = stackTopo.HeightRequest / (double)2;
			labelTitulo.VerticalTextAlignment = TextAlignment.End;

			Label labelSubTitulo = new Label()
			{
				TextColor = Color.White,
				FontSize = 12,
				Margin = new Thickness(0),
				VerticalTextAlignment = TextAlignment.Start,
				LineBreakMode = LineBreakMode.TailTruncation,
				HeightRequest = labelTitulo.HeightRequest
			};

			labelTitulo.SetBinding(
				Label.TextProperty
				, new TemplateBinding("Parent.BindingContext.PanelTituloLabel_Text")
			);

			labelSubTitulo.SetBinding(
				Label.TextProperty
				, new TemplateBinding("Parent.BindingContext.PanelSubTituloLabel_Text")
			);

			stackTopo.Children.Add(labelTitulo);
			stackTopo.Children.Add(labelSubTitulo);
			BoxMiddleContent = stackTopo;

		}

    }
}
