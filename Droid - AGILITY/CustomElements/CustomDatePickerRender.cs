using Android.Content;
using family.CustomElements;
using family.Droid.CustomElements;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(DatePicker), typeof(CustomDatePickerRender))]
namespace family.Droid.CustomElements
{
	public class CustomDatePickerRender: DatePickerRenderer
	{
		private CustomDatePicker _date;

		public CustomDatePickerRender(Context context)
			: base(context)
		{
		}
		protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
		{
			base.OnElementChanged(e);
			_date = e.NewElement as CustomDatePicker;

			if(_date != null)
			{
				_date.DateSelected += (object sender, DateChangedEventArgs eDateSelected) => {
					if(eDateSelected.OldDate != eDateSelected.NewDate)
						_date.OnDonePressed();
				};
			}

		}
	}
}
