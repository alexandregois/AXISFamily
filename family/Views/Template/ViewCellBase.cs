using System;
using Xamarin.Forms;

namespace family.Views.Template
{
	public class ViewCellBase : ViewCell
	{

		public static readonly BindableProperty SelectedBackgroundColorProperty =
			BindableProperty.Create(
				"SelectedBackgroundColor"
				, typeof(Color)
				, typeof(ViewCellBase)
				, Color.Default
			);

		public Color SelectedBackgroundColor
		{
			get { return (Color)GetValue(SelectedBackgroundColorProperty); }
			set { SetValue(SelectedBackgroundColorProperty, value); }
		}

		public ViewCellBase(Color paramColor)
		{
			SelectedBackgroundColor = paramColor;
		}
	}

}
