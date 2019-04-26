using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TCMobile
{
    class Styles
    {
        public void LabelColour(string name,string color)
        {
            var textColour = new Style(typeof(Label))
            {
                Class = name,
                Setters =
                        {
                            new Setter
                            {
                                Property = Label.TextColorProperty,
                                Value = Color.FromHex(color)
                            }
                        }
            };

            App.Current.Resources.Add(textColour);
        }        
    }
}
