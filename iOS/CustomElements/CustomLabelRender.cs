using family.CustomElements;
using family.iOS.CustomElements;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Label), typeof(CustomLabelRender))]
namespace family.iOS.CustomElements
{
	public class CustomLabelRender: LabelRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged(e);



			CustomLabel customLabel = e.NewElement as CustomLabel;

			if (Control != null)
			{
				UILabel label = Control;

				if(customLabel != null)
				{
					if(customLabel.MaxLines.HasValue)
						label.Lines = customLabel.MaxLines.Value;
				}
			}
		}
	}
}
