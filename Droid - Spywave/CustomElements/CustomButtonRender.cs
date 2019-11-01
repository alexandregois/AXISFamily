using System;
using Android.Content;
using Android.Graphics;
using family.CustomElements;
using family.Droid.CustomElements;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Button), typeof(CustomButtonRender))]
namespace family.Droid.CustomElements
{
    public class CustomButtonRender : ButtonRenderer
    {

        public CustomButtonRender(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {

            try
            {
           
                base.OnElementChanged(e);

                CustomButton customButton = e.NewElement as CustomButton;

                Android.Widget.Button thisButton = this.Control as Android.Widget.Button;

                //            thisButton. = 0;


                //            if (e.OldElement == null)
                //            {
                //                if (this.Control == null)
                //                {
                //                    Android.Widget.Button nativeControl = this.CreateNativeControl();
                //                    nativeControl.SetOnClickListener((Android.Views.View.IOnClickListener) ButtonRenderer.ButtonClickListener.Instance.Value);
                //                    nativeControl.Tag = (Java.Lang.Object) this;
                //                    this.SetNativeControl(nativeControl);
                //                    this._textColorSwitcher = new TextColorSwitcher(nativeControl.TextColors);
                //                    nativeControl.AddOnAttachStateChangeListener((Android.Views.View.IOnAttachStateChangeListener) this);
                //                }
                //            }
                //            else if (this._drawableEnabled)
                //            {
                //                this._drawableEnabled = false;
                //                this._backgroundDrawable.Reset();
                //                this._backgroundDrawable = (ButtonDrawable) null;
                //            }
                //            base.UpdateAll();


            }
            catch (Exception)
            {

            }
        }

    }
}