using System;
using family.ViewModels.Base;
using family.Resx;
using Xamarin.Forms;
using family.Views.Interfaces;
using family.Domain.Realm;
using System.Threading.Tasks;
using System.Threading;
using family.Model;
using family.Domain.Dto;
using System.Windows.Input;
using family.Services.ServiceRealm;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms.GoogleMaps;
using family.CrossPlataform;

namespace family.ViewModels
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
	public class ViewModelBloqueio : ViewModelBase
	{
		private ViewModelMapa _viewModelMapa { get; set; }

        private ICrossPlataformUtil _util { get; set; }

        public IPartialView ActualView { get; set; }

		public Int64 IdRastreador { get; set; }
		public PosicaoUnidadeRastreadaRealm ActualPosition { get; set; }

		public CancellationTokenSource _tokensourceAction { get; set; }

		private StatusComandoDto _statusComando { get; set; }

		private ModelComando _mComando;
		public ModelComando MComando
		{
			get
			{
				if (_mComando == null)
					_mComando = new ModelComando();
				return _mComando;
			}
		}

		public ICommand AtivarBoxCommand { get; set; }

		private String _labelPassWordText;
		public String LabelPassWordText
		{
			get
			{
				return _labelPassWordText;
			}
			set
			{
				_labelPassWordText = value;
				this.Notify("LabelPassWordText");
			}
		}

		private String _ativarBoxText;
		public String AtivarBoxText
		{
			get
			{
				return _ativarBoxText;
			}
			set
			{
				_ativarBoxText = value;
				this.Notify("AtivarBoxText");
			}
		}

		private String _txtSenhaPlaceHolder;
		public String TxtSenhaPlaceHolder
		{
			get
			{
				return _txtSenhaPlaceHolder;
			}
			set
			{
				_txtSenhaPlaceHolder = value;
				this.Notify("TxtSenhaPlaceHolder");
			}
		}

		private Color _scrollViewColor;
		public Color ScrollViewColor
		{
			get
			{
				return _scrollViewColor;
			}
			set
			{
				_scrollViewColor = value;
				this.Notify("ScrollViewColor");
			}
		}

		private Color _labelButtonBackgroundColor;
		public Color LabelButtonBackgroundColor
		{
			get
			{
				return _labelButtonBackgroundColor;
			}
			set
			{
				_labelButtonBackgroundColor = value;
				this.Notify("LabelButtonBackgroundColor");
			}
		}

		private String _labelStatusText;
		public String LabelStatusText
		{
			get
			{
				return _labelStatusText;
			}
			set
			{
				_labelStatusText = AppResources.Status + ": " + value;
				this.Notify("LabelStatusText");
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
				_labelIgnicaoText = AppResources.Ignition + ": " + value;
				this.Notify("LabelIgnicaoText");
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

		private Thickness _marginSeparationDefault;
		public Thickness MarginSeparationDefault
		{
			get
			{
				return _marginSeparationDefault;
			}
			set
			{
				_marginSeparationDefault = value;
				this.Notify("MarginSeparationDefault");
			}
		}

		private Thickness _boxTotalMargin;
		public Thickness BoxTotalMargin
		{
			get
			{
				return _boxTotalMargin;
			}
			set
			{
				_boxTotalMargin = value;
				this.Notify("BoxTotalMargin");
			}
		}

		private Thickness _labelIgnicaoMargin;
		public Thickness LabelIgnicaoMargin
		{
			get
			{
				return _labelIgnicaoMargin;
			}
			set
			{
				_labelIgnicaoMargin = value;
				this.Notify("LabelIgnicaoMargin");
			}
		}

		private Thickness _labelVelocidadeMargin;
		public Thickness LabelVelocidadeMargin
		{
			get
			{
				return _labelVelocidadeMargin;
			}
			set
			{
				_labelVelocidadeMargin = value;
				this.Notify("LabelVelocidadeMargin");
			}
		}

		private Double _labelTwoColumnWidthRequest;
		public Double LabelTwoColumnWidthRequest
		{
			get
			{
				return _labelTwoColumnWidthRequest;
			}
			set
			{
				_labelTwoColumnWidthRequest = value;
				this.Notify("LabelTwoColumnWidthRequest");
			}
		}

		private Boolean _ativarBoxIsEnabled = false;
		public Boolean AtivarBoxIsEnabled
		{
			get
			{
				return _ativarBoxIsEnabled;
			}
			set
			{
				_ativarBoxIsEnabled = value;
				this.Notify("AtivarBoxIsEnabled");
			}
		}

		private Boolean _txtSenhaIsEnabled = false;
		public Boolean TxtSenhaIsEnabled
		{
			get
			{
				return _txtSenhaIsEnabled;
			}
			set
			{
				_txtSenhaIsEnabled = value;
				this.Notify("TxtSenhaIsEnabled");
			}
		}

		private String _txtSenhaText;
		public String TxtSenhaText
		{
			get
			{
				return _txtSenhaText;
			}
			set
			{
				_txtSenhaText = value;
				this.Notify("TxtSenhaText");
			}
		}

		public ViewModelBloqueio(
			ViewModelMapa viewModelMapa
		) : base(true)
		{
			_viewModelMapa = viewModelMapa;
			IdRastreador = _viewModelMapa.IdRastreador;
			ActualPosition = _viewModelMapa.ActualPosition;

			LabelPassWordText = AppResources.BlockPassword;
			//AtivarBoxText = AppResources.Wait;
			TxtSenhaPlaceHolder = AppResources.Password;
			MarginSeparationDefault = new Thickness(0, 10, 0, 0);
			BoxTotalMargin = new Thickness(23, 0);

			LabelIgnicaoMargin = new Thickness(0, 0, 10, 0);
			LabelVelocidadeMargin = new Thickness(LabelIgnicaoMargin.Right, 0, 0, 0);

			LabelTwoColumnWidthRequest = DefaultWidth
				- (
					BoxTotalMargin.Left
					+ BoxTotalMargin.Right
					+ LabelIgnicaoMargin.Right
					+ LabelVelocidadeMargin.Left
				);

			TxtSenhaIsEnabled = false;
			AtivarBoxIsEnabled = false;

			AtivarBoxCommand = new Command(this.AtivarBoxActive);

			this.DefaultTemplateBuild();
		}

		private async Task Loop()
		{

			TimeSpan loopExpress = TimeSpan.FromSeconds(30);

			//if (Device.RuntimePlatform == Device.iOS)
			//	loopExpress = TimeSpan.FromSeconds(60);

			_tokensourceAction.Cancel();
			_tokensourceAction = new CancellationTokenSource();

			try
			{
				await Task.Delay(loopExpress, _tokensourceAction.Token);

				if (!_tokensourceAction.IsCancellationRequested)
					Device.BeginInvokeOnMainThread(() =>
					{
						MontaPainel();

					});

			}
			catch (Exception)
			{
			}

		}

		public override void OnAppearing()
		{

			_viewModelMapa.ActualView.ExibirLoad();

			MontaPainel();

			TxtSenhaIsEnabled = false;
			AtivarBoxIsEnabled = false;

			ScrollViewColor = _viewModelMapa.ActualPage.ColorBarra;
			LabelButtonBackgroundColor = _viewModelMapa.ActualPage.Color;

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
			_viewModelMapa.PanelTituloLabel_Text = AppResources.Block;
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

		private void MontaPainel()
		{
			ActualPosition = _viewModelMapa.ActualPosition;

            _viewModelMapa.ModelMapaGoogle.LimpaMapa();
            _viewModelMapa.ModelMapaGoogle.MontaMapaPinUltimaPosicaoAndCentraliza(_viewModelMapa.ActualPosition);

            LabelIgnicaoText = (ActualPosition.Ignicao.Value ? AppResources.On : AppResources.Off);
			LabelVelocidadeText = ActualPosition.StringVelocidade;
			BuscaStatusComando();
		}

		private async void BuscaStatusComando()
		{
			try
			{
				Int32 idUnidadeRastreada = ActualPosition.IdUnidadeRastreada;
				Int32 idRastreadorUnidadeRastreada = ActualPosition.IdRastreadorUnidadeRastreada;
				Int32 idRastreador = ActualPosition.IdRastreador;
				_tokensourceAction = new CancellationTokenSource();
				_viewModelMapa.ActualView.ExibirLoad();
				Task.Run(async () =>
				{
					try
					{
						_viewModelMapa.ActualView.ExibirLoad();
						ServiceResult<StatusComandoDto> result = await MComando.GetStatusBloqueio(
							idRastreadorUnidadeRastreada
							, idRastreador
							, _tokensourceAction.Token
						);

						FinalizaConstrucaoPainel(result);

					}
					catch (Exception ex)
					{
						_messageService.ShowAlertAsync(
							AppResources.ErroSuporte
							, AppResources.Error
						);

						Crashes.TrackError(ex);

						_viewModelMapa.ActualView.EscondeLoad();
					}


				}, _tokensourceAction.Token);
			}
			catch (Exception ex)
			{
				_messageService.ShowAlertAsync(
					AppResources.ErroSuporte
					, AppResources.Error
				);

				Crashes.TrackError(ex);

				AtivarBoxText = AppResources.Send;
				AtivarBoxIsEnabled = true;
				TxtSenhaIsEnabled = true;
				LabelStatusText = "";
				TxtSenhaText = String.Empty;

				_viewModelMapa.ActualView.EscondeLoad();

			}
			finally
			{
				
				if (Device.RuntimePlatform == Device.iOS)
					await Task.Delay(3000);
				
				_viewModelMapa.ActualView.EscondeLoad();
				Loop();
			}
		}

		private void FinalizaConstrucaoPainel(
			ServiceResult<StatusComandoDto> paramResult
		)
		{
			UpdateToken(paramResult.RefreshToken);
			if (!_tokensourceAction.IsCancellationRequested)
			{
				try
				{

					TxtSenhaText = String.Empty;


					if (String.IsNullOrWhiteSpace(paramResult.MessageError))
					{
						if (paramResult.Data != null)
						{

							_statusComando = paramResult.Data;


							if (_statusComando.IsBloqueado)
							{
								AtivarBoxText = AppResources.Unlock;
								//LabelStatusText = AppResources.Blocked;
								LabelStatusText = _statusComando.Descricao;
							}
							else
							{
								AtivarBoxText = AppResources.Block;
								//LabelStatusText = AppResources.Unlocked;
								LabelStatusText = _statusComando.Descricao;
							}


							TxtSenhaIsEnabled = _statusComando.PodeMandarComando;
							AtivarBoxIsEnabled = _statusComando.PodeMandarComando;

							if (_statusComando != null && _statusComando.IdStatusComando != null)
							{
								if (_statusComando.IdStatusComando == 1 || _statusComando.IdStatusComando == 3)
								{
									AtivarBoxText = AppResources.CancelarComando;
									TxtSenhaIsEnabled = true;
									AtivarBoxIsEnabled = true;

									LabelStatusText = "Aguardando";

								}
							}

                            PosicaoDataStore posicaoData = new PosicaoDataStore();
                            PosicaoUnidadeRastreadaRealm posi = posicaoData.Get(IdRastreador);

                            _viewModelMapa.ModelMapaGoogle.LimpaMapa();
                            _viewModelMapa.ModelMapaGoogle.MontaMapaPinUltimaPosicaoAndCentraliza(posi);


                        }
						else
						{
							AtivarBoxText = AppResources.Send;
							AtivarBoxIsEnabled = true;
							TxtSenhaIsEnabled = true;
							LabelStatusText = "";
							TxtSenhaText = String.Empty;
						}
					}
					else
					{
						ShowErrorAlert(paramResult.MessageError);

						AtivarBoxIsEnabled = true;
						TxtSenhaIsEnabled = true;

						return;

					}

				}
				catch (Exception ex)
				{
					_messageService.ShowAlertAsync(
						paramResult.MessageError
						, AppResources.Error
					);

					Crashes.TrackError(ex);

					_viewModelMapa.ActualView.EscondeLoad();

				}
				finally
				{
					_viewModelMapa.ActualView.EscondeLoad();
				}
			}
		}

		private void AtivarBoxActive(
			object obj
		)
		{
			if (String.IsNullOrWhiteSpace(TxtSenhaText))
			{
				_messageService.ShowAlertAsync(
					AppResources.PassworPlease
					, AppResources.Alert
				);
			}
			else
			{
				_tokensourceAction = new CancellationTokenSource();
				TxtSenhaIsEnabled = false;
				AtivarBoxIsEnabled = false;
				AtivarBoxText = AppResources.Wait;

				Task.Run(async () =>
				{
					try
					{
						_viewModelMapa.ActualView.ExibirLoad();

						if (_statusComando != null)
						{
							if (!_statusComando.IsBloqueado)
							{

								Boolean envia = await _messageService.ShowAlertChooseAsync(
									AppResources.BlockAlert
									, AppResources.No
									, AppResources.Yes
									, AppResources.Alert
								);

								EnviarComandoAcao(envia);

							}
							else
							{
								EnviarComandoAcao(true);
							}


							if (_statusComando.IdStatusComando == 1 || _statusComando.IdStatusComando == 3)
							{
								LabelStatusText = "Aguardando";
								EnviarComandoCancelar(true);
							}
						}

					}
					catch (Exception ex)
					{
						_messageService.ShowAlertAsync(
							AppResources.ErroSuporte
							, AppResources.Error
						);

						Crashes.TrackError(ex);

						_viewModelMapa.ActualView.EscondeLoad();
					}

				}, _tokensourceAction.Token);
			}
		}


		public async Task EnviarComandoAcao(
			Boolean paramEnvia
		)
		{
			if (!_tokensourceAction.IsCancellationRequested)
			{
				ServiceResult<StatusComandoDto> comando = new ServiceResult<StatusComandoDto>();
				try
				{
					if (paramEnvia)
					{
						PosicaoDataStore posicaoData = new PosicaoDataStore();
						PosicaoUnidadeRastreadaRealm posi = posicaoData.Get(IdRastreador);

						comando = await MComando.ComandoBloqueio(
							posi.IdRastreador
							, posi.OrdemRastreador
							, posi.IdRastreadorUnidadeRastreada
							, TxtSenhaText
							, !_statusComando.IsBloqueado
							, _tokensourceAction.Token
						);
					}
				}
				catch
				{
					comando.MessageError = "Exception";
				}
				finally
				{

					FinalizaEnvioComando(comando);
				}
			}
		}

		public async Task EnviarComandoCancelar(
			Boolean paramEnvia
		)
		{
			if (!_tokensourceAction.IsCancellationRequested)
			{
				ServiceResult<Boolean> comando = new ServiceResult<Boolean>();
				try
				{
					if (paramEnvia)
					{
						PosicaoDataStore posicaoData = new PosicaoDataStore();
						PosicaoUnidadeRastreadaRealm posi = posicaoData.Get(IdRastreador);

						comando = await MComando.ComandoCancelar(
							_statusComando.IdComandoLog.Value
							, _tokensourceAction.Token
						);
					}
				}
				catch
				{
					comando.MessageError = "Exception";
				}
				finally
				{

					AtivarBoxText = AppResources.Block;
					AtivarBoxIsEnabled = true;
					TxtSenhaIsEnabled = true;
					TxtSenhaText = String.Empty;
				}
			}
		}


		private void FinalizaEnvioComando(
			ServiceResult<StatusComandoDto> paramComando
		)
		{


			UpdateToken(paramComando.RefreshToken);
			if (!_tokensourceAction.IsCancellationRequested)
			{
				try
				{
					if (String.IsNullOrWhiteSpace(paramComando.MessageError))
					{
						_statusComando.IdStatusComando = paramComando.Data.IdStatusComando;

					}
					else
					{
						ShowErrorAlert(paramComando.MessageError);
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
				finally
				{

					FinalizaConstrucaoPainel(paramComando);

					_viewModelMapa.ActualView.EscondeLoad();
				}
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