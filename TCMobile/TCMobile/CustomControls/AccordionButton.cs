using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TCMobile.CustomControls
{
    public class AccordionButton : Button
    {
        public static readonly BindableProperty Frame =
                BindableProperty.Create("ContentFrame", typeof(Frame), typeof(AccordionButton));

        public Frame ContentFrame
        {
            get { return (Frame)GetValue(Frame); }
            set { SetValue(Frame, value); }
        }

    }
}
