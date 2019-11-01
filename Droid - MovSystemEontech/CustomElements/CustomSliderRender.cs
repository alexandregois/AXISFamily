using Android.Content;
using Android.Graphics;
using Android.Views;
using family.CustomElements;
using family.Droid.CustomElements;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(Slider), typeof(CustomSliderRender))]
namespace family.Droid.CustomElements
{
	public class CustomSliderRender : SliderRenderer
	{
		public CustomSliderRender(Context context)
			: base(context)
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
		{
			base.OnElementChanged(e);

			CustomSlider customButton = e.NewElement as CustomSlider;

			Android.Widget.SeekBar thisSeekBar = Control as Android.Widget.SeekBar;

			PorterDuffColorFilter color = 
				new PorterDuffColorFilter(customButton.Color.ToAndroid(), PorterDuff.Mode.SrcIn);

			thisSeekBar.ProgressDrawable.SetColorFilter(color);
			thisSeekBar.Thumb.SetColorFilter(color);

			thisSeekBar.Touch += (object sender, TouchEventArgs args) =>
			{
				//System.Diagnostics.Debug.WriteLine(args.Event.Action);
				switch (args.Event.Action)
				{
					case MotionEventActions.Up:
						customButton.OnReleased();
						break;

				}
				args.Handled = false;
			};
		}

	}
}