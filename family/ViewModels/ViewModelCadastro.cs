using System;
using family.Model;
using family.ViewModels.Base;
using family.ViewModels.InterfaceServices;
using family.Views.Interfaces;
using Xamarin.Forms;
using family.Resx;
using System.Threading.Tasks;
using family.Domain.Dto;
using family.Domain;
using System.Threading;

using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace family.ViewModels
{
	#pragma warning disable CS4014
	#pragma warning disable RECS0022
	public class ViewModelCadastro : ViewModelBase
	{
		public ILoader _view { get; set; }

		private Double _panelLogoHeight;
		public Double PanelLogoHeight
		{
			get
			{
				return _panelLogoHeight;
			}
			set
			{
				_panelLogoHeight = value;
				this.Notify("PanelLogoHeight");
			}
		}

		private ImageSource _logoSource;
		public ImageSource LogoSource
		{
			get
			{
				return _logoSource;
			}
			set
			{
				_logoSource = value;
				this.Notify("LogoSource");
			}
		}

		private Double _logoHeight;
		public Double LogoHeight
		{
			get
			{
				return _logoHeight;
			}
			set
			{
				_logoHeight = value;
				this.Notify("LogoHeight");
			}
		}

		private Double _logoWidth;
		public Double LogoWidth
		{
			get
			{
				return _logoWidth;
			}
			set
			{
				_logoWidth = value;
				this.Notify("LogoWidth");
			}
		}

		private Double _entryHeight;
		public Double EntryHeight
		{
			get
			{
				return _entryHeight;
			}
			set
			{
				_entryHeight = value;
				this.Notify("EntryHeight");
			}
		}

		private Double _entryWidth;
		public Double EntryWidth
		{
			get
			{
				return _entryWidth;
			}
			set
			{
				_entryWidth = value;
				this.Notify("EntryWidth");
			}
		}

		private ImageSource _buttonCadastrarSource;
		public ImageSource ButtonCadastrarSource
		{
			get
			{
				return _buttonCadastrarSource;
			}
			set
			{
				_buttonCadastrarSource = value;
				this.Notify("ButtonCadastrarSource");
			}
		}

		private Double _ButtonCadastrarWidth;
		public Double ButtonCadastrarWidth
		{
			get
			{
				return _ButtonCadastrarWidth;
			}
			set
			{
				_ButtonCadastrarWidth = value;
				this.Notify("ButtonCadastrarWidth");
			}
		}

		public Command ButtonCadastrarCommand { get; set; }

		private Thickness _scrollViewMargin;
		public Thickness ScrollViewMargin
		{
			get
			{
				return _scrollViewMargin;
			}
			set
			{
				_scrollViewMargin = value;
				this.Notify("ScrollViewMargin");
			}
		}

		private Double _panelHeight;
		public Double PanelHeight
		{
			get
			{
				return _panelHeight;
			}
			set
			{
				_panelHeight = value;
				this.Notify("PanelHeight");
			}
		}

		private Double _txtSenhaWidth;
		public Double TxtSenhaWidth
		{
			get
			{
				return _txtSenhaWidth;
			}
			set
			{
				_txtSenhaWidth = value;
				this.Notify("TxtSenhaWidth");
			}
		}

		private String _txtNome;
		public String TxtNome
		{
			get
			{
				return _txtNome;
			}
			set
			{
				_txtNome = value;
				this.Notify("TxtNome");
			}
		}

		private String _txtCep;
		public String TxtCep
		{
			get
			{
				return _txtCep;
			}
			set
			{
				_txtCep = value;
				this.Notify("TxtCep");
			}
		}

		private String _txtTelefone;
		public String TxtTelefone
		{
			get
			{
				return _txtTelefone;
			}
			set
			{
				_txtTelefone = value;
				this.Notify("TxtTelefone");
			}
		}

		private String _txtEmail;
		public String TxtEmail
		{
			get
			{
				return _txtEmail;
			}
			set
			{
				_txtEmail = value;
				this.Notify("TxtEmail");
			}
		}

		private String _txtSenha;
		public String TxtSenha
		{
			get
			{
				return _txtSenha;
			}
			set
			{
				_txtSenha = value;
				this.Notify("TxtSenha");
			}
		}

		private String _txtConfirmarSenha;
		public String TxtConfirmarSenha
		{
			get
			{
				return _txtConfirmarSenha;
			}
			set
			{
				_txtConfirmarSenha = value;
				this.Notify("TxtConfirmarSenha");
			}
		}

		public ViewModelCadastro()
		{

			ScrollViewMargin = new Thickness(0, _app.Util.GetHeightStatusBar(), 0, 0);
			PanelHeight = DefaultHeight - ScrollViewMargin.Top;

			PanelLogoHeight = 240;

			LogoSource = ImageSource.FromFile("logo_Completo.png");
			LogoHeight = 210;
			LogoWidth = 160;

			EntryHeight = 53;
			EntryWidth = DefaultWidth - 60;

			ButtonCadastrarWidth = 53;

			TxtSenhaWidth = EntryWidth - ButtonCadastrarWidth;
			ButtonCadastrarSource = ImageSource.FromFile("ic_login.png");
			ButtonCadastrarCommand = new Command(this.Cadastrar);

		}

		public override void OnAppearing()
		{
		}

		public override void OnDisappearing()
		{
		}

		public override void OnLayoutChanged()
		{
		}

		private async void Cadastrar()
		{
			try
			{
				_view.ExibirLoad();

				String erroMessage = validaForm();

				if (String.IsNullOrWhiteSpace(erroMessage))
				{
					await Task.Run(async () =>
					{

						try
						{
							Boolean isAssocia = false;
							ModelUsuario modelUsuario = new ModelUsuario();
							ViewModelMapaGoogle mapa = new ViewModelMapaGoogle();

							String cep = TxtCep.Substring(0, 5) + "-" + TxtCep.Substring(5);
							LatLong _latLong = await mapa.FindPositionByAddress(cep);

							if(_latLong != null)
							{

								CancellationTokenSource tokenSource = new CancellationTokenSource();
								ServiceResult<Int32?> result = await modelUsuario.Cadastrar(
									TxtNome
									, TxtCep
									, TxtTelefone
									, TxtEmail.ToLower()
									, TxtSenha
									, _latLong
									, isAssocia
									, tokenSource.Token
								);

								if(result.IsValid)
								{
									_messageService.ShowAlertAsync(
										AppResources.CadastrarUsuarioSucesso
										, AppResources.Success
									);

									_app.ObjetoTransferencia = new Usuario()
									{
										Email = TxtEmail,
										Senha = TxtSenha
									};

									_navigationService.Voltar();

								}
								else
								{
									ShowErrorAlert(result.MessageError);
									_view.EscondeLoad();

								}
							}
							else
							{
								_messageService.ShowAlertAsync(
									AppResources.ErrorCepInvalid
									, AppResources.Error
								);
								_view.EscondeLoad();
							}

						}
						catch(Exception ex)
						{
							_messageService.ShowAlertAsync(
								AppResources.ErroSuporte
                                , AppResources.Error
							);

                            Crashes.TrackError(ex);

                            _view.EscondeLoad();
						}

					});
				}
				else
				{
					_messageService.ShowAlertAsync(
						erroMessage
						, AppResources.Error
					);
					_view.EscondeLoad();
				}

			}
			catch(Exception ex)
			{
				_messageService.ShowAlertAsync(
					AppResources.ErroSuporte
                    , AppResources.Error
				);

                Crashes.TrackError(ex);

                _view.EscondeLoad();
			}
		}

		private String validaForm()
		{
			String error = "";

			if (String.IsNullOrWhiteSpace(TxtNome))
			{
				error += AppResources.ErrorNameEmpty + Environment.NewLine;
			}

			if (String.IsNullOrWhiteSpace(TxtCep))
			{
				error += AppResources.ErrorCepEmpty + Environment.NewLine;
			}
			else if (TxtCep.Length != 8)
			{
				error += AppResources.ErrorCepInvalid + Environment.NewLine;
			}

			if (String.IsNullOrWhiteSpace(TxtTelefone))
			{
				error += AppResources.ErrorTelefoneEmpty + Environment.NewLine;
			}

			ModelEmail _modelEmail = new ModelEmail();
			if (String.IsNullOrWhiteSpace(TxtEmail))
			{
				error += AppResources.ErrorEmailEmpty + Environment.NewLine;
			}
			else if(_modelEmail.IsEmailValido(TxtEmail.ToLower()))
			{
				error += AppResources.ErrorEmailInvalid + Environment.NewLine;
			}

			if (String.IsNullOrWhiteSpace(TxtConfirmarSenha) 
			         || String.IsNullOrWhiteSpace(TxtSenha))
			{
				error += AppResources.ErrorPasswordsEmpty + Environment.NewLine;
			}
			else if (TxtSenha != TxtConfirmarSenha)
			{
				error += AppResources.PasswordsNotMatch + Environment.NewLine;
			}

			return error;
		}

	}
	#pragma warning restore CS4014
	#pragma warning restore RECS0022
}
