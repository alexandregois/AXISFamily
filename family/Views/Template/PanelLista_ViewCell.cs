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
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace family.Views.Template
{
    public class PanelLista_ViewCell : ViewCell
    {

        private readonly IMessageService _messageService;
        private readonly INavigationService _navigationService;

        public IListaUnidadeRastreada _view { get; set; }

        private ViewModelListaUnidadeRastreada _viewModel { get; set; }

        protected App _app => (Application.Current as App);

        public PanelLista_ViewCell(
            Object paramContext
         )
        {

            _viewModel = (ViewModelListaUnidadeRastreada)paramContext;
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

                PosicaoUnidadeRastreadaRealm paramUnidadeRastreada = (PosicaoUnidadeRastreadaRealm)BindingContext;
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

                if (paramUnidadeRastreada != null)
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

                    Int32 intMarginLeft = 10;
                    double doubleScala = 1.0;

                    Thickness thicknessMargin = new Thickness(intMarginLeft, 10, 0, 0);



                    if (!String.IsNullOrEmpty(paramUnidadeRastreada.IconePadrao))
                    {
                        if (_app.ScreenHeight < 510)
                        {
                            doubleScala = 2.1;
                            intMarginLeft = 17;
                            thicknessMargin = new Thickness(intMarginLeft, 10, 15, 0);
                        }
                        else
                        {
                            doubleScala = 3.5;
                            intMarginLeft = 25;
                            thicknessMargin = new Thickness(intMarginLeft, 10, 10, 0);
                        }

                    }

                    Image ImageUnidade = new Image()
                    {
                        Source = paramUnidadeRastreada.PathImage,
                        Scale = doubleScala
                    };

                    if (Device.RuntimePlatform == Device.iOS)
                        intMarginLeft = 10;


                    StackLayout boxImageUnidade = new StackLayout()
                    {
                        Margin = thicknessMargin,
                        Spacing = 0,
                        Orientation = StackOrientation.Horizontal,
                        VerticalOptions = LayoutOptions.Center,
                        HeightRequest = 35
                    };

                    if (Device.RuntimePlatform == Device.iOS)
                        boxImageUnidade.Margin = new Thickness(21, 5, 0, 0);


                    boxImageUnidade.Children.Add(ImageUnidade);

                    boxPrincipal.Children.Add(boxImageUnidade, 0, 0);


                    #region Texto
                    StackLayout boxTexto = new StackLayout()
                    {
                        Margin = new Thickness(63, 11, 15, 15),
                        Spacing = 0,
                        Orientation = StackOrientation.Vertical,
                        WidthRequest = col02.Width.Value,
                        VerticalOptions = LayoutOptions.Center
                    };


                    if (Device.RuntimePlatform == Device.iOS)
                        boxTexto.Margin = new Thickness(69, 11, 15, 15);



                    Label labelUnidadeRastreada = new Label()
                    {
                        Text = paramUnidadeRastreada.IdentificacaoFinal,
                        TextColor = Color.White,
                        Margin = new Thickness(0, 0, 0, 2),
                        LineBreakMode = LineBreakMode.TailTruncation,
                        HorizontalTextAlignment = TextAlignment.Start
                    };

                    Label labelDataEvento = new Label()
                    {
                        Text = paramUnidadeRastreada.StringDataEvento_Posicao,
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
                        CommandParameter = paramUnidadeRastreada,
                        NumberOfTapsRequired = 1

                    });


                    boxPrincipal.Children.Add(boxTexto, 0, 0);
                    #endregion

                    #region Image

                    String sourceImage = String.Empty;


                    //Muda icone de ignicao
                    if (paramUnidadeRastreada.Ignicao != null)
                    {
                        Boolean boolIgnicao = paramUnidadeRastreada.Ignicao.Value;

                        if (boolIgnicao)
                        {
                            sourceImage = "ic_ignition_on.png";
                        }

                        if (!boolIgnicao)
                        {
                            sourceImage = "ic_ignition_off.png";
                        }
                    }


                    Image ImageIgnicao = new Image()
                    {
                        Source = sourceImage,
                        HeightRequest = 15,
                        WidthRequest = 16,
                        Margin = new Thickness(0, 0, margin, 0),
                        Opacity = 1,
                        VerticalOptions = LayoutOptions.Center
                    };


                    Image ImageAlerta = new Image()
                    {
                        Source = "ic_alerta.png",
                        HeightRequest = 15,
                        WidthRequest = 16,
                        Margin = new Thickness(0, 0, margin, 0),
                        Opacity = 1,
                        VerticalOptions = LayoutOptions.Center
                    };

                    ImageAlerta.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = new Command(ImageAlerta_Tap),
                        CommandParameter = paramUnidadeRastreada,
                        NumberOfTapsRequired = 1
                    });


                    StackLayout stack = new StackLayout();
                    stack.Orientation = StackOrientation.Horizontal;
                    stack.HorizontalOptions = LayoutOptions.End;

                    stack.Children.Add(ImageIgnicao);

                    if (paramUnidadeRastreada.ListaViolacaoRegra != null && paramUnidadeRastreada.ListaViolacaoRegra.Count > 0)
                        stack.Children.Add(ImageAlerta);


                    boxPrincipal.Children.Add(stack, 2, 0);


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

            PosicaoUnidadeRastreadaRealm tempPosicao = obj as PosicaoUnidadeRastreadaRealm;

            if (obj != null)
            {
                try
                {

                    if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android)
                    {
                        if (_app._countClick == 0)
                        {

                            _app._countClick += 1;

                            _view.ExibirLoad();

                            _viewModel.NavegateMapa(
                            EnumPage.UltimaPosicao
                            , tempPosicao.IdRastreador
                            );

                            _view.EscondeLoad();

                        }
                        else
                            return;
                    }
                    else
                    {

                        _viewModel.NavegateMapa(
                        EnumPage.UltimaPosicao
                        , tempPosicao.IdRastreador
                        );


                    }

                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);

                }

            }
        }

        private async void ImageAlerta_Tap(object obj)
        {
            String answer;

            List<String> alertas = new List<String>();

            if (obj != null)
            {
                PosicaoUnidadeRastreadaRealm tempPosicao = obj as PosicaoUnidadeRastreadaRealm;


                foreach (var item in tempPosicao.ListaViolacaoRegra)
                {
                    alertas.Add(item.NomeRegra);
                }


                answer = await
                    this._messageService.ShowMessageAsync(
                        AppResources.Alert
                        , AppResources.Cancel
                        , null
                        , alertas.ToArray()
                    );
            }
        }

    }
}


