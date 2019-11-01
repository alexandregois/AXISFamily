using family.CustomElements;
using family.iOS.CustomElements;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Slider), typeof(CustomSliderRender))]
namespace family.iOS.CustomElements
{
	public class CustomSliderRender : SliderRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
		{

            try
            {

                base.OnElementChanged(e);

                CustomSlider customButton = e.NewElement as CustomSlider;

                UISlider thisButton = Control as UISlider;
                thisButton.TouchDown += delegate
                {
                    customButton.OnPressed();
                };

                thisButton.TouchUpInside += delegate
                {
                    customButton.OnReleased();
                };

            }
            catch (System.Exception ex)
            {
                return;
            }

		}
	}
}
