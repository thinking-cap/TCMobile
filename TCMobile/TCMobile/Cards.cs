using System;
using System.Collections.Generic;
using System.Text;
using Xamarin;
using Xamarin.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace TCMobile
{
    class Cards
    {
        public async Task<bool> buildCourseCard(string courseid, string coursetitle, string coursedescription, StackLayout container,EventHandler downloadClicked, EventHandler launchCourse)
        {
            MaterialFrame frame;
            StackLayout layout;
            Button downloadBtn;
            Button launchBtn;
            ActivityIndicator spinner;
            Image marquee;
            Models.Record courseRecord = await App.Database.GetCourseByID(courseid);
            marquee = new Image
            {
                // lets cache the image so we for 72 hours //
                Source = new UriImageSource
                {
                    Uri = new Uri(Constants.BlobLocation + "/coursecontent/" + courseid + "/courselogo.gif"),
                    CacheValidity = new TimeSpan(72, 0, 0),
                    CachingEnabled = true
                }

            };
            frame = new MaterialFrame
            {
                HasShadow = true,
                Padding = new Thickness(0, 0, 0, 0),
                Margin = new Thickness(0, 8, 0, 24),
                CornerRadius = 0

            };

            layout = new StackLayout
            {
                ClassId = "course_" + courseid

            };

            StackLayout cardBody = new StackLayout
            {
                Padding = new Thickness(16, 0, 16, 0),
                ClassId = "course_" + courseid,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            Label title = new Label
            {
                Text = coursetitle,
                Style = (Style)Application.Current.Resources["headerStyle"]
            };

            //HtmlLabel description = new HtmlLabel
            //{
            //    Text = coursedescription
            //};

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
                                    <body>" + HttpUtility.HtmlDecode(coursedescription) + "</body></html>";
            var description = new CustomWebview
            {
                HeightRequest = 300,
                Source = new HtmlWebViewSource
                {
                    Html = htmlText
                },
                Style = (Style)Application.Current.Resources["descriptionWebView"]

            };






            downloadBtn = new Button
            {
                Text = "download",
                Image = "download.png",
                Style = (Style)Application.Current.Resources["buttonStyle"],
                ClassId = courseid,
                IsVisible = (courseRecord == null) ? true : (courseRecord.Deleted == "false") ? false : true
            };

            spinner = new ActivityIndicator
            {
                IsVisible = false,
                Style = (Style)Application.Current.Resources["spinnerStyle"],
                HeightRequest = 20
            };

            StackLayout cardFooter = new StackLayout
            {
                Padding = new Thickness(16, 0, 16, 8),
                ClassId = "course_" + courseid
            };

            launchBtn = new Button
            {
                Text = (courseRecord == null) ? "open" :
                        (courseRecord.CompletionStatus.ToLower() == "completed") ? "review" :
                        (courseRecord.CMI == "") ? "open" : "resume",
                IsVisible = (courseRecord == null) ? false : (courseRecord.Deleted == "false") ? true : false,
                Image = "launch_w.png",
                Style = (Style)Application.Current.Resources["buttonStyle"],
                ClassId = courseid

            };
            launchBtn.Clicked += launchCourse;
            downloadBtn.Clicked += downloadClicked;
            cardBody.Children.Add(title);
            cardBody.Children.Add(description);
            layout.Children.Add(marquee);
            layout.Children.Add(cardBody);
            layout.Children.Add(cardFooter);
            cardFooter.Children.Add(launchBtn);
            cardFooter.Children.Add(downloadBtn);
            cardFooter.Children.Add(spinner);
            frame.Content = layout;
            container.Children.Add(frame);

            return true;
        }


        public void buildLPCard(string id, string lptitle, string lpdescription,StackLayout LP)
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
