using System;
using System.Collections.Generic;
using family.CustomElements;
using family.Domain.Dto;
using family.Domain.Enum;
using family.Domain.Realm;
using family.Resx;
using family.ViewModels;
using family.ViewModels.InterfaceServices;
using family.Views.Interfaces;
using Xamarin.Forms;
using Plugin.Messaging;

namespace family.Views.Template
{
    public class PanelListaTelefone_ViewCell : ViewCell
    {

        private readonly IMessageService _messageService;
        private readonly INavigationService _navigationService;

        public IListaUnidadeRastreada _view { get; set; }

        private ViewModelListaTelefone _viewModel { get; set; }

        protected App _app => (Application.Current as App);

        public PanelListaTelefone_ViewCell(
            Object paramContext
         )
        {

            _viewModel = (ViewModelListaTelefone)paramContext;
            _view = _viewModel._view;


            this._messageService =
                DependencyService.Get<IMessageService>();

            this._navigationService =
                DependencyService.Get<INavigationService>();
        }

        protected override void OnBindingContextChanged()
        {
            try
            {
                base.OnBindingContextChanged();

                TelefoneContatoDto paramTelefone = (TelefoneContatoDto)BindingContext;
                Double margin = 15;

                #region Grid
                Grid boxPrincipal = new Grid();
                boxPrincipal.WidthRequest = _app.ScreenWidth;
                boxPrincipal.ColumnSpacing = 0;
                boxPrincipal.RowSpacing = 0;
                boxPrincipal.VerticalOptions = LayoutOptions.FillAndExpand;
                boxPrincipal.HorizontalOptions = LayoutOptions.FillAndExpand;


                StackLayout stackFull = new StackLayout();
                stackFull.Orientation = StackOrientation.Vertical;

                if (paramTelefone != null)
                {
                    boxPrincipal.ColumnDefinitions = new ColumnDefinitionCollection();
                    ColumnDefinition col01 = new ColumnDefinition()
                    {
                        Width = GridLength.Star
                    };

                    ColumnDefinition col03 = new ColumnDefinition()
                    {
                        Width = GridLength.Auto
                    };

                    ColumnDefinition col02;

                    col02 = new ColumnDefinition()
                    {
                        Width = GridLength.Auto
                    };

                    boxPrincipal.ColumnDefinitions.Add(col01);
                    boxPrincipal.ColumnDefinitions.Add(col02);
                    boxPrincipal.ColumnDefinitions.Add(col03);


                    boxPrincipal.RowDefinitions = new RowDefinitionCollection();
                    boxPrincipal.RowDefinitions.Add(new RowDefinition()
                    {
                        Height = GridLength.Star
                    });
                    #endregion

                    Image ImageUnidade = new Image()
                    {
                        Source = "ic_telefone_off.png",
                        WidthRequest = 35
                    };

                    Int32 intMarginLeft = 10;

                    if (Device.RuntimePlatform == Device.iOS)
                        intMarginLeft = 10;


                    StackLayout boxImageUnidade = new StackLayout()
                    {
                        Margin = new Thickness(intMarginLeft, 10, 25, 0),
                        Spacing = 0,
                        Orientation = StackOrientation.Horizontal,
                        VerticalOptions = LayoutOptions.Center
                    };

                    if (Device.RuntimePlatform == Device.iOS)
                        boxImageUnidade.Margin = new Thickness(intMarginLeft, 5, 25, 0);


                    boxImageUnidade.Children.Add(ImageUnidade);

                    boxPrincipal.Children.Add(boxImageUnidade, 0, 0);


                    #region Texto
                    StackLayout boxTexto = new StackLayout()
                    {
                        Margin = new Thickness(55, 11, 15, 15),
                        Spacing = 0,
                        Orientation = StackOrientation.Vertical,
                        WidthRequest = col02.Width.Value,
                        VerticalOptions = LayoutOptions.Center
                    };

                    Label labelUnidadeRastreada = new Label()
                    {
                        Text = paramTelefone.TelefoneContato,
                        TextColor = Color.White,
                        Margin = new Thickness(0, 0, 0, 2),
                        LineBreakMode = LineBreakMode.TailTruncation,
                        HorizontalTextAlignment = TextAlignment.Start
                    };

                    Label labelDataEvento = new Label()
                    {
                        Text = paramTelefone.Descricao,
                        TextColor = Color.White,
                        Margin = new Thickness(0, 0, 0, 2),
                        LineBreakMode = LineBreakMode.TailTruncation,
                        HorizontalTextAlignment = TextAlignment.Start
                    };

                    boxTexto.Children.Add(labelUnidadeRastreada);
                    boxTexto.Children.Add(labelDataEvento);

                    boxTexto.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = new Command(Box_Tap),
                        CommandParameter = paramTelefone,
                        NumberOfTapsRequired = 1

                    });


                    boxPrincipal.Children.Add(boxTexto, 0, 0);
                    #endregion


                    StackLayout stackline = new StackLayout();
                    stackline.HeightRequest = 1;
                    stackline.WidthRequest = _app.ScreenWidth;
                    stackline.BackgroundColor = Color.LightGray;


                    stackFull.Children.Add(boxPrincipal);
                    stackFull.Children.Add(stackline);

                }


                View = stackFull;

            }
            catch (Exception ex)
            {

            }
        }

        private void Box_Tap(object obj)
        {
            if (obj != null)
            {
                TelefoneContatoDto tempObjeto = obj as TelefoneContatoDto;

                if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android)
                {
                    if (_app._countClick == 0)
                    {
                        _app._countClick += 1;

                        _view.ExibirLoad();

                        Device.OpenUri(new Uri(String.Format("tel:{0}", tempObjeto.TelefoneContato)));

                        _view.EscondeLoad();

                    }
                    else
                        return;
                }
                else
                {

                    var phoneCallTask = Plugin.Messaging.CrossMessaging.Current.PhoneDialer;
                    if (phoneCallTask.CanMakePhoneCall)
                        phoneCallTask.MakePhoneCall(tempObjeto.TelefoneContato);

                    //Device.OpenUri(new Uri(String.Format("tel:{0}", tempObjeto.TelefoneContato)));

                }


            }
        }


    }
}


