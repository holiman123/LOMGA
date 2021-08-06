using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LOMGAxam;
using LOMGAxam.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Graphics.Drawables;
using Android.Text;
using Android.Content.Res;

[assembly: ExportRenderer(typeof(MyEntry), typeof(MyEntryRenderer))]
namespace LOMGAxam.Droid
{
    [Obsolete]
    class MyEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            GradientDrawable gd = new GradientDrawable();
            gd.SetColor(Android.Graphics.Color.Transparent);
            gd.SetShape(ShapeType.Rectangle);
            gd.SetCornerRadius(12);
            gd.SetStroke(3, Android.Graphics.Color.Rgb(196, 196, 196));


            IntPtr IntPtrtextViewClass = JNIEnv.FindClass(typeof(TextView));
            IntPtr mCursorDrawableResProperty = JNIEnv.GetFieldID(IntPtrtextViewClass, "mCursorDrawableRes", "I");

            // my_cursor is the xml file name which we defined above
            JNIEnv.SetField(Control.Handle, mCursorDrawableResProperty, Resource.Drawable.MyEntryXML);

            this.Control.InputType = InputTypes.TextVariationVisiblePassword;
            this.Control.SetPadding(10, 0, 0, 0);
            this.Control?.SetBackgroundDrawable(gd);
        }
    }
}