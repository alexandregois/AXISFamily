using family.CustomElements;
using family.ViewModels;
using family.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace family.Views
{
    public partial class ViewStreet : ContentPage, IStreetView
    {
        public ViewModelStreetView _viewModel { get; set; }   
       
        public ViewStreet(Double paramLatitude, Double paramLongitude, Boolean paramExibe)
        {
            InitializeComponent();

            _viewModel = new ViewModelStreetView(paramLatitude, paramLongitude, paramExibe
            );

            this.BindingContext = _viewModel;

            Device.BeginInvokeOnMainThread(async () =>
            {
                WVStreet.Source = _viewModel.WVStreet.Source;
            });

        }

        public void OnAppearing()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                WVStreet.Source = _viewModel.WVStreet.Source;
            });
        }

        public void OnDisappearing()
        {
            throw new NotImplementedException();
        }

        public void ExibirLoad()
        {
            throw new NotImplementedException();
        }
    }
}