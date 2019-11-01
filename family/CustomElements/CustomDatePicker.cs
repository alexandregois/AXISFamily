using System;
using Xamarin.Forms;

namespace family.CustomElements
{
	public class CustomDatePicker : DatePicker
	{

		public event EventHandler DonePress;

		public virtual void OnDonePressed()
		{
			DonePress?.Invoke(this, EventArgs.Empty);
		}
	}
}
