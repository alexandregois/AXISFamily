using System;
using Xamarin.Forms;

namespace family.CustomElements
{
	public class CustomSlider : Slider
	{
		public event EventHandler Pressed;
		public event EventHandler Released;
		public event EventHandler ButtonPress;
		public Color Color { get; set; }

		public virtual void OnPressed()
		{
			Pressed?.Invoke(this, EventArgs.Empty);
		}

		public virtual void OnReleased()
		{
			Released?.Invoke(this, EventArgs.Empty);
		}

		public void OnButtonPress()
		{
			ButtonPress?.Invoke(this, EventArgs.Empty);
		}
	}
}
