using Android.Content;
using family.CustomElements;
using family.Droid.CustomElements;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Label), typeof(CustomLabelRender))]
namespace family.Droid.CustomElements
{
	public class CustomLabelRender : LabelRenderer
	{
		public CustomLabelRender(Context context)
			: base(context)
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged(e);

			CustomLabel customLabel = e.NewElement as CustomLabel;

			if(customLabel != null)
			{
				if(customLabel.MaxLines.HasValue)
				{
					Control.SetMaxLines(customLabel.MaxLines.Value);

					Control.Ellipsize = Android.Text.TextUtils.TruncateAt.End;
				}
			}
		}
	}
}
