using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TCMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LearningPaths : ContentPage
	{
        public TCMobile.LPS lp;
        public LearningPaths ()
		{
			InitializeComponent ();
            loadLearningPaths();
           
        }

        async void loadLearningPaths()
        {
            CredentialsService credentials = new CredentialsService();
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                lp = await Courses.GetLearningPaths(credentials.HomeDomain, credentials.UserID);
                buildLPS(lp.LearningPaths);

            }
        }

        public void buildLPS(List<LearningPath> lp)
        {
            if (lp != null)
            {
                foreach (LearningPath l in lp)
                {
                    buildLPCard(l.id, l.title, l.description);
                }
            }
        }

        public void buildLPCard(string id, string lptitle, string lpdescription)
        {
            MaterialFrame frame;
            StackLayout layout;
            frame = new MaterialFrame
            {
                HasShadow = true,
                Padding = new Thickness(0, 0, 0, 0),
                Margin = new Thickness(0, 8, 0, 24),
                CornerRadius = 0
            };

            layout = new StackLayout
            {

            };

            StackLayout cardBody = new StackLayout
            {
                Padding = new Thickness(16, 0, 16, 0),
                ClassId = "course_" + id,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            Label title = new Label
            {
                Text = lptitle,
                Style = (Style)Application.Current.Resources["headerStyle"]
            };
            // html description

            string htmlText = @"<html>
                                    <head>
                                        <meta name='viewport' content='width=device-width; height=device-height; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;'/>                                    
                                        <style type='text/css'>
                                             body{font-family:Segoe UI, Helvetica Neue,'Lucida Sans Unicode', Skia, sans-serif;
                                                    border:0px;padding:0px;margin:0px;
                                                    background-color:transparent;
                                                    overflow:hidden;
                                                }
                                        </style>    
                                    </head>
                                    <body>" + HttpUtility.HtmlDecode(lpdescription) + "</body></html>";
            CustomWebview description = new CustomWebview
            {
                HeightRequest = 300,
                Source = new HtmlWebViewSource
                {
                    Html = htmlText
                },
                Style = (Style)Application.Current.Resources["descriptionWebView"]

            };

            cardBody.Children.Add(title);
            cardBody.Children.Add(description);
            layout.Children.Add(cardBody);
            frame.Content = layout;


            LP.Children.Add(frame);
        }
    }
}