using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TCMobile.CustomControls
{
    public class AccordionButton : Button
    {
        public static readonly BindableProperty Frame =
                BindableProperty.Create("ContentFrame", typeof(StackLayout), typeof(AccordionButton));

        public static readonly BindableProperty Open =
            BindableProperty.Create("FrameOpen", typeof(Boolean), typeof(AccordionButton));

        public StackLayout ContentFrame
        {
            get { return (StackLayout)GetValue(Frame); }
            set { SetValue(Frame, value); }
        }

        public Boolean FrameOpen
        {
            get { return (Boolean)GetValue(Open); }
            set { SetValue(Open, value); }
        }

    }
}
