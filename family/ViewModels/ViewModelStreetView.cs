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
using Xamarin.Forms.PlatformConfiguration;
using XLabs.Platform;
using XLabs.Platform.Device;
using Plugin.DeviceInfo;

namespace family.ViewModels
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
    public class ViewModelStreetView : ViewModelBase, IStreetView
    {

        public App _app => (Application.Current as App);

        public CancellationTokenSource _tokensourceAction { get; set; }
        public CancellationTokenSource _tokensourceDelay { get; set; }

        private ICrossPlataformUtil _util { get; set; }

        #region StreetView

        public WebView WVStreet { get; set; }

        Boolean IsBusy = false;
        public Boolean _isBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                this.Notify("IsBusy");
            }
        }

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

        Double _heightStreetView;
        public Double HeightStreetView
        {
            get
            {
                return _heightStreetView;
            }
            set
            {
                _heightStreetView = value;
                this.Notify("HeightStreetView");
            }
        }

        Double _widthStreetView;
        public Double WidthStreetView
        {
            get
            {
                return _widthStreetView;
            }
            set
            {
                _widthStreetView = value;
                this.Notify("WidthStreetView");
            }
        }

        #endregion

        public bool _isRefreshing { get; private set; }

        public ViewModelStreetView(Double paramLatitude, Double paramLongitude, Boolean paramExibe
        ) : base(true)
        {
            MontaStreetView(paramLatitude, paramLongitude, paramExibe);
        }

        public override void OnAppearing()
        {

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
        public void ExibirLoad()
        {

        }

        public void MontaStreetView(Double paramLatitude, Double paramLongitude, Boolean paramExibe)
        {
            WVStreet = new WebView();

            Device.BeginInvokeOnMainThread(async () =>
            {
                IsBusy = true;

                try
                {

                    String mapsKey = "AIzaSyBw3Voldg8_kywqtlXmqoqxF_3VbUXi2ws";

                    //String mapsKey = "AIzaSyAUAr2_d4LHFYlMB4EHnt-aBY-KGxu4o7k";

                    String url = String.Empty;

                    HeightStreetView = _app.ScreenHeight;
                    WidthStreetView = _app.ScreenWidth;

                    String versao = CrossDeviceInfo.Current.Version;

                    versao = versao.Substring(0, 1);

                    int intVersao = Convert.ToInt32(versao);


                    #region Android

                    if (intVersao >= 8)
                    {

                        if (_app.ScreenHeight < 550)
                        {
                            url =
                                "https://maps.googleapis.com/maps/api/streetview?size="
                                + 400 + "x"
                                              + 600 + "&location="
                                              + paramLatitude.ToString() + ","
                                              + paramLongitude.ToString()
                                              + "&key=" + mapsKey;

                        }
                        else
                        {
                            url = "http://maps.google.com/maps?q=&layer=tc&cbll=" + paramLatitude.ToString().Replace(",", ".") + ","
                                              + paramLongitude.ToString().Replace(",", ".") + "&cbp=0,0,0,0,0&key=" + mapsKey;

                        }

                    }
                    else
                    {

                        if (_app.ScreenHeight < 550)
                        {
                            url =
                                "https://maps.googleapis.com/maps/api/streetview?size="
                                + 360 + "x"
                                              + 540 + "&location="
                                              + paramLatitude.ToString() + ","
                                              + paramLongitude.ToString()
                                              + "&key=" + mapsKey;

                        }
                        else
                        {
                            url = "http://maps.google.com/maps?q=&layer=tc&cbll=" + paramLatitude.ToString().Replace(",", ".") + ","
                                              + paramLongitude.ToString().Replace(",", ".") + "&cbp=0,0,0,0,0&key=" + mapsKey;

                        }

                    }



                    //if (intVersao >= 8)
                    //{

                    //    url = "https://maps.googleapis.com/maps/api/streetview?size=400x600&location=" + paramLatitude.ToString().Replace(",", ".") + "," +
                    //    paramLongitude.ToString().Replace(",", ".") + "&fov=80&heading=70&pitch=0&key=" + mapsKey;

                    //}
                    //else
                    //{

                    //    url = "https://maps.googleapis.com/maps/api/streetview?size=360x540&location=" + paramLatitude.ToString().Replace(",", ".") + "," +
                    //        paramLongitude.ToString().Replace(",", ".") + "&fov=80&heading=70&pitch=0&key=" + mapsKey;

                    //}


                    #endregion
                    

                    //url = "https://www.google.com/maps/embed/v1/streetview?key=" + mapsKey + "&location=" + paramLatitude.ToString().Replace(",", ".") + ","
                    //                          + paramLongitude.ToString().Replace(",", ".") + "&heading=210&pitch=10&fov=35";


                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        url =
                            "https://maps.googleapis.com/maps/api/streetview?size="
                            + 400 + "x"
                            + 600 + "&location="
                            + paramLatitude.ToString() + ","
                                                         + paramLongitude.ToString()
                                                         + "&key=" + mapsKey;
                    }


                    _contentStreetView = url;

                    if (_wVStreetSource != _contentStreetView)
                    {
                        WVStreetSource = _contentStreetView;
                        _wVStreetSource = _contentStreetView;
                    }

                    WVStreet.Source = url;

                    ExibeStreetView = paramExibe;
                    IsBusy = false;

                }

                catch
                {
                    ShowErrorAlert("Exception");
                }

            });

        }

    }
#pragma warning restore CS1998
#pragma warning restore RECS0022
#pragma warning restore CS4014
}