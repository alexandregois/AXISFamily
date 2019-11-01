using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using family.Domain.Dto;
using family.Domain.Realm;
using family.Model;
using family.Resx;
using family.Services.ServiceRealm;
using family.ViewModels.Base;
using family.Views.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace family.ViewModels
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
	public class ViewModelAncora : ViewModelBase
	{
		private ViewModelMapa _viewModelMapa { get; set; }
		public PosicaoDataStore PosicaoStore { get; set; }
		public IPartialView ActualView { get; set; }

		public Int64 IdRastreador { get; set; }
		public PosicaoUnidadeRastreadaRealm ActualPosition { get; set; }

		private Circle _pontoControle { get; set; }

		public ICommand AtivarBoxCommand { get; set; }

		private ModelAncora _model;
		public ModelAncora Model
		{
			get
			{
				if(_model == null)
				{
					_model = new ModelAncora();
				}
				return _model;
			}
		}


		public CancellationTokenSource _tokensourceAction { get; set; }

		private String _labelEnderecoText = "";
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

		private String _ativarBoxText = "";
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

		private String _labelTxtMetrosText;
		public String LabelTxtMetrosText
		{
			get
			{
				return _labelTxtMetrosText;
			}
			set
			{
				_labelTxtMetrosText = value;
				this.Notify("LabelTxtMetrosText");
			}
		}

		private String _labelAnchorRadiusText;
		public String LabelAnchorRadiusText
		{
			get
			{
				return _labelAnchorRadiusText;
			}
			set
			{
				_labelAnchorRadiusText = value;
				this.Notify("LabelAnchorRadiusText");
			}
		}

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


		public ViewModelAncora(
			ViewModelMapa viewModelMapa
		) : base(true)
		{
			_viewModelMapa = viewModelMapa;
			PosicaoStore = new PosicaoDataStore();
			IdRastreador = _viewModelMapa.IdRastreador;

			LabelAnchorRadiusText = AppResources.Radius;

			ActualPosition = _viewModelMapa.ActualPosition;

			AtivarBoxCommand = new Command(this.AtivarBoxAction);

			SliderEnable = true;

		}

		public override void OnAppearing()
		{

            _viewModelMapa.ActualView.ExibirLoad();

            this.DefaultTemplateBuild();

			MontaPaniel();

		}

		public override void OnDisappearing()
		{
		}

		public override void OnLayoutChanged()
		{
		}

		public void MontaPaniel()
		{
			_viewModelMapa.ModelMapaGoogle.LimpaMapa();

			_viewModelMapa.ModelMapaGoogle.MontaMapaPinAndCentraliza(ActualPosition);

			if(ActualPosition.Ancora_Latitude.HasValue)
			{
				MontaPaniel_Desativar();
			}
			else
			{
				MontaPaniel_Ativar();
			}

		}

		private void DefaultTemplateBuild()
		{
			_viewModelMapa.PanelTituloLabel_Text = AppResources.Anchor;
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

        private void MontaPaniel_Ativar()
		{
			LabelEnderecoText = ActualPosition.Endereco;
			AtivarBoxText = AppResources.Enable;

			SliderEnable = true;

			SliderValue = 300;

			_pontoControle = _viewModelMapa.ModelMapaGoogle.MontaMapaPinAncora(
				ActualPosition.Latitude
				, ActualPosition.Longitude
				, SliderValue
			);

		}

		private void MontaPaniel_Desativar()
		{
			AtivarBoxText = AppResources.Disable;

			SliderEnable = false;
			SliderValue = ActualPosition.Ancora_Tolerancia.Value;

			LabelEnderecoText = AppResources.GettingAddress;

			Double latitude = ActualPosition.Ancora_Latitude.Value;
			Double longitude = ActualPosition.Ancora_Longitude.Value;

			Task.Run(async () => {
				try
				{
					String endereco = await _viewModelMapa.ModelMapaGoogle.FindAddressByPosition(
						latitude
						, longitude
					);
					LabelEnderecoText = endereco;
				} catch(Exception) { }
			});

			_pontoControle = _viewModelMapa.ModelMapaGoogle.MontaMapaPinAncora(
				latitude
				, longitude
				, SliderValue
			);

			if(
				ActualPosition.Latitude != _pontoControle.Center.Latitude
				&& ActualPosition.Longitude != _pontoControle.Center.Longitude
			)
			{
				List<Position> lst = new List<Position>();
				lst.Add(
					new Position(
						ActualPosition.Latitude
						, ActualPosition.Longitude
					)
				);
				lst.Add(_pontoControle.Center);

				Bounds bound = Bounds.FromPositions(lst);

				_viewModelMapa.ModelMapaGoogle.CentralizarMapa(
					bound
					, false
				);
			}

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
			LabelTxtMetrosText = 
				Convert.ToInt32(paramNewValue).ToString() 
				       + " " + AppResources.Meters;
			
			if(_pontoControle != null)
				_pontoControle.Radius = Distance
					.FromMeters(paramNewValue);
		}
		#endregion

		private void AtivarBoxAction(
			object obj
		)
		{

			try
			{
				_viewModelMapa.ActualView.ExibirLoad();
				_tokensourceAction = new CancellationTokenSource();

				Double? latitude = ActualPosition.Ancora_Latitude;
				Int32 id = ActualPosition.IdUnidadeRastreada;
				Int32 idRastreador = ActualPosition.IdRastreador;

				Task.Run(async () => {
					if (latitude == null)
					{
						AtivaAncora(
							id
							, idRastreador
						);
					}
					else
					{
						DesativaAncora(
							id
							, idRastreador
						);
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
            }

		}

		private async Task AtivaAncora(
			Int32 paramId
			, Int32 paramIdRastreador
		)
		{
			try
			{
				Double latitude = _pontoControle.Center.Latitude;
				Double longitude = _pontoControle.Center.Longitude;
				Int32 raio = Convert.ToInt32(SliderValue);

				ServiceResult<AncoraAtivacaoDto> result = await Model.AtivarAncora(
					latitude
					, longitude
					, raio
					, paramId
					, _tokensourceAction.Token
				);

				UpdateToken(result.RefreshToken);

				TerminaServicoAncora(
					result.MessageError
					, paramIdRastreador
					, latitude
					, longitude
					, raio
				);
			}
			catch(Exception ex)
			{
				_messageService.ShowAlertAsync(
					AppResources.ErroSuporte
                    , AppResources.Error
				);

                Crashes.TrackError(ex);
            }
		}

		private async Task DesativaAncora(
			Int32 paramId
			, Int32 paramIdRastreador
		)
		{
			try
			{
				ServiceResult<Int32?> result = await Model.DesativarAncora(
					paramId
					, _tokensourceAction.Token
				);

				UpdateToken(result.RefreshToken);

				TerminaServicoAncora(
					result.MessageError
					, paramIdRastreador
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

		private void TerminaServicoAncora(
			String paramMessageError
			, Int32 paramIdRastreador
			, Double? paramLatitude = null
			, Double? paramLongitude = null
			, Int32? paramRaio = null
		)
		{
			if(!_tokensourceAction.IsCancellationRequested)
			{
				if(String.IsNullOrWhiteSpace(paramMessageError))
				{
					PosicaoDataStore posicao = new PosicaoDataStore();
					posicao.UpdateAncora(
						paramLatitude
						, paramLongitude
						, paramRaio
						, paramIdRastreador
					);

					String message = "";
					if(paramLatitude == null)
					{
						message = AppResources.AnchorDisabled;
					}
					else
					{
						message = AppResources.AnchorCreated;
					}

					_messageService.ShowAlertAsync(
						message
						, AppResources.Success
					);

					Device.BeginInvokeOnMainThread(() =>
					{
						ActualPosition = posicao.Get(paramIdRastreador);

						MontaPaniel();
					});

				}
				else
				{
					ShowErrorAlert(paramMessageError);
				}

				_viewModelMapa.ActualView.EscondeLoad();
			}
		}


	}
	#pragma warning restore CS1998
	#pragma warning restore RECS0022
	#pragma warning restore CS4014
}