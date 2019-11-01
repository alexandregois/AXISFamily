using System;
using family.CustomElements;
using family.iOS.CustomElements;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(DatePicker), typeof(CustomDatePickerRender))]
namespace family.iOS.CustomElements
{
	public class CustomDatePickerRender : DatePickerRenderer
	{
		private DateTime oldDate { get; set; }

		protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
		{
			base.OnElementChanged(e);

			CustomDatePicker customLabel = e.NewElement as CustomDatePicker;

			if(customLabel != null)
			{
				var toolbar = (UIToolbar)Control.InputAccessoryView;
				var doneBtn = toolbar.Items[1];

				Element.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e2) => {
					if(e2.PropertyName == "IsFocused")
					{
						if(Element.IsFocused)
							oldDate = ((DatePicker)sender).Date;
					}
				};

				doneBtn.Clicked += (object sender, EventArgs e2) => 
				{
					if(oldDate != customLabel.Date)
						customLabel.OnDonePressed();	
				};
			}

		}
	}
}
