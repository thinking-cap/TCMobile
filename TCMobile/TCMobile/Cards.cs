using System;
using System.Collections.Generic;
using System.Text;
using Xamarin;
using Xamarin.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using FFImageLoading.Forms;
using TCMobile.CustomControls;
using System.Reflection;
using System.IO;
using Expandable;

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
           // Image marquee;
            StackLayout marqueeContainer;
            
          


                Models.Record courseRecord = await App.Database.GetCourseByID(courseid);
            CachedImage marquee = new CachedImage(){
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                MinimumWidthRequest = Constants.deviceWidth,
                WidthRequest = Constants.deviceWidth,
                Margin = new Thickness(0, 0, 0, 0),          
                CacheDuration = TimeSpan.FromDays(30),
                DownsampleToViewSize = true,
                RetryCount = 0,
                RetryDelay = 250,
                LoadingPlaceholder = "placeholder.png",
                ErrorPlaceholder = "placeholder.png",
                Source = Constants.BlobLocation + "/coursecontent/" + courseid + "/courselogo.gif"

            };


            marqueeContainer = new StackLayout
            {
            };

          
            Image marqueePlaceholder = new Image
            {
                Source = ImageSource.FromResource("TCMobile.Images.placeholder.png")

            };

            marqueeContainer.Children.Add(marquee);
           // marqueeContainer.Children.Add(marqueePlaceholder);
            marqueePlaceholder.SetBinding(Image.IsVisibleProperty, "IsLoading");
            marqueePlaceholder.BindingContext = marquee;
            

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
            layout.Children.Add(marqueeContainer);
            layout.Children.Add(cardBody);
            layout.Children.Add(cardFooter);
            cardFooter.Children.Add(launchBtn);
            cardFooter.Children.Add(downloadBtn);
            cardFooter.Children.Add(spinner);
            frame.Content = layout;
            container.Children.Add(frame);

            return true;
        }


        public void buildLPCard(string id, string lptitle, string lpdescription,StackLayout LP, EventHandler detailsClicked)
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
            Button moreBtn;
            moreBtn = new Button
            {
                Text = "more",
                Image = "launch_w.png",
                Style = (Style)Application.Current.Resources["buttonStyle"],
                ClassId = id

            };
            moreBtn.Clicked += detailsClicked;

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

            StackLayout cardFooter = new StackLayout
            {
                Padding = new Thickness(16, 0, 16, 8),
                ClassId = "course_" + id
            };

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
            cardFooter.Children.Add(moreBtn);
            cardBody.Children.Add(title);
            cardBody.Children.Add(description);
            layout.Children.Add(cardBody);
            layout.Children.Add(cardFooter);
            frame.Content = layout;


            LP.Children.Add(frame);
        }

        public void buildObjectiveCard(Objective obj, StackLayout container)
        {
            MaterialFrame frame;
            StackLayout layout;
            // overall card layout //
            frame = new MaterialFrame
            {
                HasShadow = true,
                Padding = new Thickness(0, 0, 0, 0),
                Margin = new Thickness(0, 8, 0, 24),
                CornerRadius = 0
            };

            // objective title //
            Label objectiveTitle = new Label
            {
                Text = obj.Name
            };

            // container for the content in the card
            layout = new StackLayout
            {

            };

            Frame cardBody = new Frame
            {
                Padding = new Thickness(16, 0, 16, 0),
                ClassId = "course_" + obj.id,
                VerticalOptions = LayoutOptions.FillAndExpand
               
            };

            StackLayout cardHeader = new StackLayout
            {
                Padding = new Thickness(16, 0, 16, 0),
                ClassId = "course_" + obj.id,
            };

            cardHeader.Children.Add(objectiveTitle);
            AccordionButton AcBtn = new AccordionButton
            {
                Text = "Hide Show"
            };

            cardHeader.Children.Add(AcBtn);
            AcBtn.ContentFrame= cardBody;
            cardBody.IsVisible = false;
            AcBtn.Clicked += AcBtn_Clicked;


            
            StackLayout frameContainer = new StackLayout();
            foreach (Activity act in obj.Activities.Activity)
            {
                StackLayout activityContainer = new StackLayout
                {
                    Padding = new Thickness(16, 0, 16, 0),
                    ClassId = "course_" + act.id,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };
                Label coursetitle = new Label
                {
                    Text = act.Name
                };
                activityContainer.Children.Add(coursetitle);
                frameContainer.Children.Add(activityContainer);
            }
            cardBody.Content = frameContainer;
            frame.Content = layout;
            layout.Children.Add(cardHeader);
            layout.Children.Add(cardBody);
            container.Children.Add(frame);

        }

        private void AcBtn_Clicked(object sender, EventArgs e)
        {
            AccordionButton btn = (AccordionButton)sender;
            Frame f = btn.ContentFrame;
            if (f.IsVisible)
                f.IsVisible = false;
            else
                f.IsVisible = true;
            string s = "test";
        }
    }
}
