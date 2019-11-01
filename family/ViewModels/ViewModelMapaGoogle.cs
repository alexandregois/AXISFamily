using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using family.Domain.Dto;
using family.Domain.Realm;
using family.Resx;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using family.Domain;
using Microsoft.AppCenter.Crashes;
using System.IO;
using System.Net.Http;
using PCLStorage;

namespace family.ViewModels
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998

    public class ViewModelMapaGoogle
    {
        public Map _mapa { get; set; }

        public ViewModelMapaGoogle(Map paramMapa)
        {
            _mapa = paramMapa;
        }

        public ViewModelMapaGoogle()
        {
        }


        #region CentralizarMapa
        public void CentralizarMapa(
            Double paramLatitude
            , Double paramLontitude
            , Double paramDistancia = 1
            , Boolean paramAnimate = true
        )
        {

            try
            {
                _mapa.MoveToRegion(
                    MapSpan.FromCenterAndRadius(
                        new Position(
                            paramLatitude
                            , paramLontitude
                        )
                        , Distance.FromKilometers(paramDistancia)
                    )
                    , paramAnimate
                );

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

        }

        public void CentralizarMapa(
            Position paramPosition
            , Double paramDistancia = 1
            , Boolean paramAnimate = true
        )
        {

            try
            {
                _mapa.MoveToRegion(
                    MapSpan.FromCenterAndRadius(
                        paramPosition
                        , Distance.FromKilometers(paramDistancia)
                    )
                    , paramAnimate
                );

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

        }

        public void CentralizarMapa(
            Bounds paramBounds
            , Boolean paramAnimate = true
        )
        {
            try
            {
                _mapa.MoveToRegion(MapSpan.FromBounds(paramBounds), paramAnimate);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

        }
        #endregion

        #region Geocode

        public async Task<LatLong> FindPositionByAddress(String paramEndereco)
        {
            LatLong latLng = null;
            try
            {
                Geocoder coder = new Geocoder();
                IEnumerable<Position> lstPosition =
                    await coder.GetPositionsForAddressAsync(paramEndereco);

                if (lstPosition != null && lstPosition.Count() > 0)
                {
                    Position pos = lstPosition.FirstOrDefault();

                    latLng = new LatLong(pos.Latitude, pos.Longitude);
                }
            }
            catch (Exception ex)
            {

                Crashes.TrackError(ex);
            }

            return latLng;
        }

        public async Task<String> FindAddressByPosition(Double paramLatitude, Double paramLongitude)
        {
            String ret = null;
            try
            {
                Geocoder coder = new Geocoder();

                Position pos = new Position(paramLatitude, paramLongitude);

                IEnumerable<string> lstPosition = await coder.GetAddressesForPositionAsync(pos);

                if (lstPosition != null)
                {
                    ret = lstPosition.First();

                    if (ret.Length > 0)
                        ret = ret.Replace("\n", "-");
                }
                else
                {
                    ret = AppResources.AddressNotFound;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

                ret = AppResources.AddressNotFound;
            }

            return ret;
        }

        public Circle MontaMapaPinAncora(
            double paramLatitude
            , double paramLongitude
            , int paramRaio
        )
        {
            Circle circle = null;

            try
            {
                Position posi = new Position(paramLatitude, paramLongitude);
                Pin pinPosicao = new Pin
                {
                    Type = PinType.Place,
                    Position = posi,
                    Label = AppResources.Anchor,
                    Icon = BitmapDescriptorFactory.FromBundle("pin_ancora.png"),
                    IsDraggable = false

                };

                circle = new Circle();
                circle.Center = posi;
                circle.Radius = Distance.FromMeters(paramRaio);

                circle.StrokeColor = Color.FromRgba(0, 145, 179, 204); // #0091B3 80%
                circle.StrokeWidth = 1f;
                circle.FillColor = Color.FromRgba(0, 146, 178, 89); // #0091B3 35%
                circle.Tag = pinPosicao.Label; // Can set any object

                _mapa.Circles.Add(circle);
                _mapa.Pins.Add(pinPosicao);
                CentralizarMapa(posi);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return circle;
        }


        #endregion

        public void LimpaMapa()
        {
            try
            {

                ClearPins();
                ClearCircle();

                if (_mapa.Polylines != null)
                    _mapa.Polylines.Clear();

                if (_mapa.Polygons != null)
                    _mapa.Polygons.Clear();

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public void ClearPins()
        {
            if (_mapa.Pins != null)
                _mapa.Pins.Clear();
        }

        public void ClearCircle()
        {

            if (_mapa.Circles != null)
                _mapa.Circles.Clear();
        }

        public Pin MontaMapaPin(
            PosicaoUnidadeRastreadaRealm UltimaPosicao
            , String Icone = null
        )
        {

            Pin pinPosicao;

            try
            {


                if (Icone == String.Empty || Icone == null)
                {
                    //if (UltimaPosicao.IconUrl != null)
                    //    Icone = UltimaPosicao.IconUrl;
                    //else
                    Icone = UltimaPosicao.Icone;
                }


                //HttpClient client = new HttpClient();
                //client.MaxResponseContentBufferSize = 256000;
                //Stream stream = await client.GetStreamAsync(Icone);

                //Image imgMapa = new Image();
                //imgMapa.Source = ImageSource.FromStream(() => stream);

                //StreamReader reader = new StreamReader(stream);

                //byte[] bytedata = System.Text.Encoding.Unicode.GetBytes(reader.ReadToEnd());

                //SaveImage(bytedata, "iconMapa.png", null);

                //String strPath = LoadImage(bytedata, "iconMapa.png", null).Result;



                Position _position = new Position(
                UltimaPosicao.Latitude
                , UltimaPosicao.Longitude
                );

                pinPosicao = new Pin
                {
                    Type = PinType.Place,
                    Position = _position,
                    Icon = BitmapDescriptorFactory.FromBundle(Icone),
                    Label = UltimaPosicao.StringDataEvento_Posicao,
                    IsVisible = true
                };


                _mapa.Pins.Add(pinPosicao);


            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                pinPosicao = new Pin();

            }
            return pinPosicao;
        }

        public Pin MontaMapaPinPontoControle(
            PosicaoUnidadeRastreadaRealm UltimaPosicao
            , PontoControle pontoControle
            , String Icone = null
        )
        {

            Pin pinPosicao;

            try
            {
                if (Icone == String.Empty || Icone == null)
                    Icone = UltimaPosicao.Icone;

                Position _position = new Position(
                    UltimaPosicao.Latitude
                    , UltimaPosicao.Longitude
                );

                pinPosicao = new Pin
                {
                    Type = PinType.Place,
                    Position = _position,
                    Icon = BitmapDescriptorFactory.FromBundle(Icone),
                    Label = pontoControle.NomePonto,
                    IsVisible = true
                };

                _mapa.Pins.Add(pinPosicao);


            }
            catch (Exception ex)
            {
                pinPosicao = new Pin();
                Crashes.TrackError(ex);

            }

            return pinPosicao;
        }


        public Pin MontaMapaPinAndCentraliza(
            PosicaoUnidadeRastreadaRealm UltimaPosicao
            , String Icone = null
        )
        {

            Pin pinPosicao;

            try
            {

                pinPosicao = MontaMapaPin(
                    UltimaPosicao
                    , Icone
                );

                CentralizarMapa(pinPosicao.Position, 1);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                pinPosicao = new Pin();

            }
            return pinPosicao;
        }

        public Pin MontaMapaPinUltimaPosicaoAndCentraliza(
            PosicaoUnidadeRastreadaRealm UltimaPosicao
        )
        {
            Pin pinPosicao;

            try
            {

                pinPosicao = MontaMapaPin(
                    UltimaPosicao
                    , null
                );

                CentralizarMapa(pinPosicao.Position, 1);

                //if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
                //    _mapa.SelectedPin = pinPosicao;


            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                pinPosicao = new Pin();

            }
            return pinPosicao;
        }

        public void MontaMapaListaHistorico(List<PosicaoHistorico> paramHistorico)
        {

            try
            {
                Polyline polyline = new Polyline()
                {
                    StrokeColor = Color.Red,
                    StrokeWidth = 2f
                };
                String iconePrincipal = "pin_historico_reduzido.png";
                Pin pinPosicao;

                PosicaoHistorico item;
                List<Position> lstPosition = new List<Position>();
                for (Int32 i = 0; i < paramHistorico.Count; i++)
                {
                    item = paramHistorico[i];
                    Position position = new Position(item.Latitude.Value, item.Longitude.Value);

                    pinPosicao = new Pin
                    {
                        Type = PinType.Place,
                        Position = position,
                        Label = item.StringDataEvento,
                        Icon = BitmapDescriptorFactory.FromBundle(iconePrincipal)
                    };


                    lstPosition.Add(position);
                    _mapa.Pins.Add(pinPosicao);

                    polyline.Positions.Add(position);

                }

                if (paramHistorico.Count > 1)
                {
                    _mapa.Polylines.Add(polyline);

                }

                CentralizarMapa(Bounds.FromPositions(lstPosition));

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }
        }

        public Circle DrawCircle(
            double paramLatitude
            , double paramLongitude
            , int paramTolerancia
        )
        {
            Circle circle = null;
            try
            {
                circle = new Circle();
                circle.Center = new Position(
                    paramLatitude
                    , paramLongitude
                );
                circle.Radius = Distance.FromMeters(Convert.ToDouble(paramTolerancia));

                circle.StrokeColor = Color.FromRgba(0, 145, 179, 204);
                circle.StrokeWidth = 1f;
                circle.FillColor = Color.FromRgba(0, 146, 178, 89);
                circle.Tag = "CIRCLE"; // Can set any object

                _mapa.Circles.Add(circle);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }

            return circle;
        }
    }
#pragma warning restore CS1998
#pragma warning restore RECS0022
#pragma warning restore CS4014
}