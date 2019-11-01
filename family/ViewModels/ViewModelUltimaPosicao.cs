using System;
using System.Threading;
using System.Threading.Tasks;
using family.CrossPlataform;
using family.Domain.Dto;
using family.Domain.Enum;
using family.Domain.Realm;
using family.Model;
using family.Resx;
using family.Services.ServiceRealm;
using family.ViewModels.Base;
using family.Views.Interfaces;
using Plugin.Battery;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using System.Net;
using XLabs.Enums;
using XLabs.Forms.Controls;
using Microsoft.AppCenter.Crashes;

namespace family.ViewModels
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
	public class ViewModelUltimaPosicao : ViewModelBase
	{
		private ViewModelMapa _viewModelMapa { get; set; }

		private ViewModelStreetView _viewModelStreetView { get; set; }

		public PosicaoDataStore PosicaoStore { get; set; }

		public IPartialView ActualView { get; set; }

		public IListaUnidadeRastreada _view { get; set; }

		public App _app => (Application.Current as App);

		public CancellationTokenSource _tokensourceAction { get; set; }
		public CancellationTokenSource _tokensourceDelay { get; set; }

		public Int64 IdRastreador { get; set; }
		public PosicaoUnidadeRastreadaRealm ActualPosition { get; set; }

		private ICrossPlataformUtil _util { get; set; }

		private Boolean _isStreetView = false;

		private String _labelDataPosicaoText;
		public String LabelDataPosicaoText
		{
			get
			{
				return _labelDataPosicaoText;
			}
			set
			{
				_labelDataPosicaoText = value;
				this.Notify("LabelDataPosicaoText");
			}
		}

		private String _labelEnderecoText;
		public String LabelEnderecoText
		{
			get
			{
				return _labelEnderecoText;
			}
			set
			{
				_labelEnderecoText = value;
				this.Notify("LabelEnderecoText");
			}
		}

		private String _labelVelocidadeText;
		public String LabelVelocidadeText
		{
			get
			{
				return _labelVelocidadeText;
			}
			set
			{
				_labelVelocidadeText = value;
				this.Notify("LabelVelocidadeText");
			}
		}

		private Boolean _ignicaoBoxVisible = false;
		public Boolean IgnicaoBoxVisible
		{
			get
			{
				return _ignicaoBoxVisible;
			}
			set
			{
				_ignicaoBoxVisible = value;
				this.Notify("IgnicaoBoxVisible");
			}
		}

		private String _labelIgnicaoText;
		public String LabelIgnicaoText
		{
			get
			{
				return _labelIgnicaoText;
			}
			set
			{
				_labelIgnicaoText = value;
				this.Notify("LabelIgnicaoText");
			}
		}

		private Boolean _gpsBoxVisible = false;
		public Boolean GpsBoxVisible
		{
			get
			{
				return _gpsBoxVisible;
			}
			set
			{
				_gpsBoxVisible = value;
				this.Notify("GpsBoxVisible");
			}
		}

		private String _labelGpsText;
		public String LabelGpsText
		{
			get
			{
				return _labelGpsText;
			}
			set
			{
				_labelGpsText = value;
				this.Notify("LabelGpsText");
			}
		}

		private Boolean _sinalBoxVisible = false;
		public Boolean SinalBoxVisible
		{
			get
			{
				return _sinalBoxVisible;
			}
			set
			{
				_sinalBoxVisible = value;
				this.Notify("SinalBoxVisible");
			}
		}

		private String _labelSinalText;
		public String LabelSinalText
		{
			get
			{
				return _labelSinalText;
			}
			set
			{
				_labelSinalText = value;
				this.Notify("LabelSinalText");
			}
		}

		private Boolean _bateriaBoxVisible = false;
		public Boolean BateriaBoxVisible
		{
			get
			{
				return _bateriaBoxVisible;
			}
			set
			{
				_bateriaBoxVisible = value;
				this.Notify("BateriaBoxVisible");
			}
		}

		private String _labelBateriaIgnicaoText;
		public String LabelBateriaIgnicaoText
		{
			get
			{
				return _labelBateriaIgnicaoText;
			}
			set
			{
				_labelBateriaIgnicaoText = value;
				this.Notify("LabelBateriaIgnicaoText");
			}
		}

		#region StreetView

		String _contentStreetView = null;
		String _wVStreetSource = null;
		public String WVStreetSource
		{
			get
			{
				return _wVStreetSource;
			}
			set
			{
				_wVStreetSource = value;
				this.Notify("WVStreetSource");
			}
		}

		Boolean _exibeStreetView = false;
		public Boolean ExibeStreetView
		{
			get
			{
				return _exibeStreetView;
			}
			set
			{
				_exibeStreetView = value;
				this.Notify("ExibeStreetView");
			}
		}

		#endregion


		public bool _isRefreshing { get; private set; }

		public ViewModelUltimaPosicao(
			ViewModelMapa viewModelMapa
		) : base(true)
		{

			_viewModelMapa = viewModelMapa;
			PosicaoStore = new PosicaoDataStore();
			IdRastreador = _viewModelMapa.IdRastreador;

			_util = DependencyService.Get<ICrossPlataformUtil>();
                        
            MontaPaniel();

			this.DefaultTemplateBuild();

        }

		public override void OnAppearing()
		{
			this.DefaultTemplateBuild();

			CreateTokens();

            MontaPaniel();

		}

		public override void OnDisappearing()
		{
			if (_tokensourceDelay != null)
				_tokensourceDelay.Cancel();

			if (_tokensourceAction != null)
				_tokensourceAction.Cancel();
		}

		public override void OnLayoutChanged()
		{
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

		public void MontaPaniel()
		{
			try
			{

				ActualPosition = PosicaoStore.Get(IdRastreador);
				_app.ActualPosition = ActualPosition;


                if (!String.IsNullOrWhiteSpace(ActualPosition.Endereco))
				{
					LabelEnderecoText = ActualPosition.Endereco;
				}
				else
				{
					LabelEnderecoText = AppResources.GettingAddress;

					Task.Run(async () =>
					{
						try
						{
							PosicaoUnidadeRastreadaRealm tempPosition = PosicaoStore.Get(IdRastreador);

							String endereco = await _viewModelMapa.ModelMapaGoogle.FindAddressByPosition(
								Convert.ToDouble(tempPosition.Latitude)
								, Convert.ToDouble(tempPosition.Longitude)
							);

							if (!String.IsNullOrWhiteSpace(endereco))
							{
								PosicaoStore.UpdateEndereco(IdRastreador, endereco);
							}

							LabelEnderecoText = endereco;
						}
						catch (Exception) { }
					});
				}

				LabelDataPosicaoText = AppResources.Date + " " + String.Format(
						"{0:dd/MM/yyyy HH:mm:ss}"
						, ActualPosition.DataEvento.ToLocalTime());

				String velocidade = "0";
				if (ActualPosition.Velocidade.HasValue)
				{
					velocidade = ActualPosition.Velocidade.Value.ToString();

					if (velocidade != "0" && velocidade.Length > 3)
						velocidade = ActualPosition.Velocidade.Value.ToString("n2");
				}
				LabelVelocidadeText = velocidade;


				if (ActualPosition.Ignicao != null)
				{
					IgnicaoBoxVisible = ActualPosition.Ignicao.HasValue;

					if (IgnicaoBoxVisible)
					{
						String batIgnicao = "Desligado";

						if (ActualPosition.Ignicao.Value)
							batIgnicao = "Ligado";

						LabelIgnicaoText = batIgnicao;
					}
				}


				GpsBoxVisible = ActualPosition.GPSValido.HasValue;
				if (GpsBoxVisible)
				{
					String gpsLabel = "Inválido";
					if (ActualPosition.GPSValido.Value)
						gpsLabel = "Válido";

					LabelGpsText = gpsLabel;
				}

				if (
					ActualPosition.SinalGPRS.HasValue
					&& ActualPosition.SinalGPRS.Value > 0
				   )
				{
					LabelSinalText = ActualPosition.SinalGPRS.Value.ToString();
					SinalBoxVisible = true;
				}

				//String statusBateria = "Status: " + CrossBattery.Current.Status.ToString();
				//String statusBateria = CrossBattery.Current.Status.ToString();

				String bateria = null;
				switch (ActualPosition.IdTipoUnidadeRastreada)
				{
					case (Byte)EnumTipoUnidadeRastreada.Pessoa:
						if (
							ActualPosition.BateriaBackup.HasValue
							&& ActualPosition.BateriaBackup.Value > 0
						)
						{
							bateria = String.Format("{0:n2}", ActualPosition.BateriaBackup.Value);
							//ActualPosition.BateriaBackup.Value.ToString();
						}
						break;
					default:
						if (
							ActualPosition.BateriaPrincipal.HasValue
							&& ActualPosition.BateriaPrincipal.Value > 0
						   )
						{
							bateria = String.Format("{0:n2}", ActualPosition.BateriaPrincipal.Value);
							//ActualPosition.BateriaPrincipal.Value.ToString();
						}
						break;
				}

                String strMedidaBateria = String.Empty;
                if (ActualPosition.IdUnidadeMedidaBateriaPrincipal == 1)
                    strMedidaBateria = " v";
                else
                    strMedidaBateria = " %";


                if (!String.IsNullOrWhiteSpace(bateria))
				{
					LabelBateriaIgnicaoText = bateria + strMedidaBateria; // + "  " + AppResources.ResourceManager.GetString(statusBateria);

					BateriaBoxVisible = true;
				}

				_viewModelMapa.ModelMapaGoogle.LimpaMapa();
				//_viewModelMapa.ModelMapaGoogle.MontaMapaPinUltimaPosicaoAndCentraliza(ActualPosition);


				_app.selectPinUltimaPosicao = _viewModelMapa.ModelMapaGoogle.MontaMapaPinUltimaPosicaoAndCentraliza(ActualPosition);


				Loop();


			}
			catch (Exception ex) { }

		}

		private void DefaultTemplateBuild()
		{
			_viewModelMapa.PanelTituloLabel_Text = AppResources.CurrentLocation;
			_viewModelMapa.PanelSubTituloLabel_Text = ActualPosition.IdentificacaoFinal;

			Button refreshButton = new Button()
			{
				Image = "ic_refresh.png",
				Command = new Command(Refresh_Tap),
				HeightRequest = 35,
				WidthRequest = 35,

				//HeightRequest = 53,
				//WidthRequest = 55,

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
			else
			{
				menuMapaButton.HeightRequest = 35;
				menuMapaButton.WidthRequest = 35;
			}


			StackLayout stackLayout = new StackLayout();
			stackLayout.WidthRequest = 79;
			stackLayout.HeightRequest = 53;
			stackLayout.Orientation = StackOrientation.Horizontal;

			stackLayout.Children.Add(refreshButton);

			////Sem Botao StreetView
			//_viewModelMapa.BoxRightContent = refreshButton;

			//Com Botao StreetView
			stackLayout.Children.Add(menuMapaButton);
			_viewModelMapa.BoxRightContent = stackLayout;

		}

		private void OpenWaze()
		{
			String paramEndereco = ActualPosition.Endereco;
			String paramLatitude = ActualPosition.Latitude.ToString().Replace(",", ".");
			String paramLongitude = ActualPosition.Longitude.ToString().Replace(",", ".");

			String paramURL = "https://waze.com/ul?q=" + paramEndereco + "&ll="
				+ paramLatitude + "," + paramLongitude + "&navigate=yes";

			//String paramURL = "https://waze.com/ul?ll=" + paramLatitude + "," + paramLongitude + "&navigate=yes";

			_util.OpenWaze(paramURL.Replace(" ", "%20"));

		}

		private void Refresh_Tap(object arg)
		{
			if (!_isRefreshing)
			{
				_tokensourceDelay.Cancel();
				RefreshAction();
			}
		}


		#region Loop

		private async Task Loop()
		{

			try
			{

				_isRefreshing = false;

				await Task.Delay(
					_app.Configuracao.LoopTimeSpan
					, _tokensourceDelay.Token
				);

				if (!_isRefreshing)
					RefreshAction();

			}
			catch (Exception ex) { }

		}

		private void RefreshAction()
		{


            //Executa o KeepAlive
            ViewModelConfiguracao viewModelConfiguracao = new ViewModelConfiguracao();
            viewModelConfiguracao.CheckKeepAlive();



            ServiceResult<PosicaoUnidadeRastreada> result = new ServiceResult<PosicaoUnidadeRastreada>();
			try
			{
				_isRefreshing = true;

				_viewModelMapa.ActualView.ExibirLoad();

				Int32 idRastreadorUnidadeRastreada = ActualPosition.IdRastreadorUnidadeRastreada;

				Task.Run(async () =>
				{
					try
					{
						ModelPosicao _model = new ModelPosicao();

						result = await _model.BuscarUltimaPosicao(
							idRastreadorUnidadeRastreada
							, _tokensourceAction.Token
						);
					}
					catch (Exception ex)
					{
						Crashes.TrackError(ex);
					}
					finally
					{
						RefreshAction_Finish(result);
					}
				}, _tokensourceAction.Token);
			}
			catch (Exception ex)
			{
				if (ex.ToString().IndexOf("Realms") < 0)
				{
					Crashes.TrackError(ex);
					RefreshAction_Finish(result);
				}
			}
		}


		private void RefreshAction_Finish(
			ServiceResult<PosicaoUnidadeRastreada> paramResult
		)
		{
			if (!_tokensourceAction.IsCancellationRequested)
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					try
					{

						if (String.IsNullOrWhiteSpace(paramResult.MessageError))
						{
							PosicaoDataStore posicao = new PosicaoDataStore();
							PosicaoUnidadeRastreadaRealm novaPosicao
							= new PosicaoUnidadeRastreadaRealm();
							novaPosicao.Transform(paramResult.Data);

							posicao.CreateUpadate(novaPosicao);

							ActualPosition = novaPosicao;

							MontaPaniel();

						}
						else
						{
							ShowErrorAlert(paramResult.MessageError);
						}

					}
					catch (Exception ex)
					{
						if (ex.ToString().IndexOf("Realms") < 0)
						{
							ShowErrorAlert("Exception");
						}
					}
					finally
					{
						RetornaStatusAtivo();
						UpdateToken(paramResult.RefreshToken);
					}
				});
			}
		}

		private void RetornaStatusAtivo()
		{

			_viewModelMapa.ActualView.EscondeLoad();
			if (!_tokensourceAction.IsCancellationRequested)
			{
				CreateTokens();

				Loop();
			}

		}
		#endregion

		public async void MapaTiposCheck()
		{

			Frame novoFrame = new Frame();
			StackLayout content = new StackLayout();
			content.Orientation = StackOrientation.Vertical;


			_view.MakeFrameDefault(ref novoFrame, ref content);

			Thickness margin = new Thickness(0, 0, 0, 10);
			Label titulo = new Label()
			{
				Text = AppResources.Mapa,
				FontSize = 18,
				FontAttributes = FontAttributes.Bold,
				Margin = margin
			};
			content.Children.Add(titulo);


			StackLayout contentMapaStreet = new StackLayout();
			contentMapaStreet.Orientation = StackOrientation.Horizontal;

            XLabs.Forms.Controls.CheckBox chkMapaStreet = new XLabs.Forms.Controls.CheckBox();
			chkMapaStreet.DefaultText = AppResources.MapaPadrao;

			content.Children.Add(contentMapaStreet);

			novoFrame.Content = content;

			_view.ShowAlert(novoFrame);


		}

		public async void MapaTipos()
		{


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
				AppResources.Hibrido,
				AppResources.StreetView
			}
			);



			if (answer == AppResources.Trafego)
			{
				_viewModelMapa.ActualView.CloseStreetview(true);
				this._viewModelMapa.ActualView.mapaPosicao.MapType = MapType.Street;
				this._viewModelMapa.ActualView.mapaPosicao.IsTrafficEnabled = true;
			}
			else if (answer == AppResources.Satelite)
			{
				_viewModelMapa.ActualView.CloseStreetview(true);
				this._viewModelMapa.ActualView.mapaPosicao.MapType = MapType.Satellite;
				this._viewModelMapa.ActualView.mapaPosicao.IsTrafficEnabled = false;
			}
			else if (answer == AppResources.Terreno)
			{
				_viewModelMapa.ActualView.CloseStreetview(true);
				this._viewModelMapa.ActualView.mapaPosicao.MapType = MapType.Terrain;
				this._viewModelMapa.ActualView.mapaPosicao.IsTrafficEnabled = false;
			}
			else if (answer == AppResources.Hibrido)
			{
				_viewModelMapa.ActualView.CloseStreetview(true);
				this._viewModelMapa.ActualView.mapaPosicao.MapType = MapType.Hybrid;
				this._viewModelMapa.ActualView.mapaPosicao.IsTrafficEnabled = false;
			}
			else if (answer == AppResources.MapaPadrao)
			{
				_viewModelMapa.ActualView.CloseStreetview(true);
				this._viewModelMapa.ActualView.mapaPosicao.MapType = MapType.Street;
				this._viewModelMapa.ActualView.mapaPosicao.IsTrafficEnabled = false;
			}
			else if (answer == AppResources.StreetView)
			{
                //_viewModelMapa.ActualView.OpenStreetview(true);
                OpenStreetView(true);
            }


		}

		private void OpenStreetView(Boolean paramExibe)
		{
            //_navigationService.NavigateToStreetView(ActualPosition.Latitude, ActualPosition.Longitude, paramExibe);

            _util.OpenStreet(ActualPosition.Latitude.ToString().Replace(",", "."), ActualPosition.Longitude.ToString().Replace(",", "."));
		}

	}
#pragma warning restore CS1998
#pragma warning restore RECS0022
#pragma warning restore CS4014
}