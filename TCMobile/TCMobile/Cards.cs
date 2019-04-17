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
            DownloadImageButton downloadBtn;
            DownloadButton launchBtn;
            ActivityIndicator spinner;
           // Image marquee;
            StackLayout marqueeContainer;
            // See if there is a course record so we can display the proper navigation (download, launch etc...)
            Models.Record courseRecord = await App.Database.GetCourseByID(courseid);
            CachedImage marquee = BuildMarquee(courseid,true);

            // wrap the course Marquee in a stack layout so we have more control with the layout
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



            spinner = new ActivityIndicator
            {
                IsVisible = false,
                Style = (Style)Application.Current.Resources["spinnerStyle"],
                HeightRequest = 20
            };

            downloadBtn = BuildImageDownload(courseid, courseRecord,spinner);
            downloadBtn.CourseID = courseid;
            

            

            StackLayout cardFooter = new StackLayout
            {
                Padding = new Thickness(16, 0, 16, 8),
                ClassId = "course_" + courseid
            };

            launchBtn = new DownloadButton
            {
                Text = (courseRecord == null) ? "open" :
                        (courseRecord.CompletionStatus.ToLower() == "completed") ? "review" :
                        (courseRecord.CMI == "") ? "open" : "resume",
                IsVisible = (courseRecord == null) ? false : (courseRecord.Deleted == "false") ? true : false,
                Image = "launch_w.png",
                Style = (Style)Application.Current.Resources["buttonStyle"],
                ClassId = courseid,
                CourseID = courseid

            };
            downloadBtn.LaunchButton = launchBtn;
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


        public void buildLPCard(string id, string lptitle, string lpdescription,FlexLayout LP, EventHandler detailsClicked)
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
                HorizontalOptions = LayoutOptions.FillAndExpand
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

        public async void buildObjectiveCard(Objective obj, StackLayout container)
        {
           
            Grid layout;
            // overall card layout //
            

            // objective title //
            Label objectiveTitle = new Label
            {
                Margin = new Thickness(0,0,0,0),
                Text = obj.Name,
                VerticalOptions = LayoutOptions.FillAndExpand,
                VerticalTextAlignment = TextAlignment.Center,
                HeightRequest = 40
            };

            // container for the content in the card
            layout = new Grid
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                WidthRequest = Constants.deviceWidth,
                Margin = new Thickness(0,10,0,0)
                
            };
            layout.Padding = new Thickness(16, 0, 16, 0);
            layout.RowDefinitions.Add(new RowDefinition { Height = 40 });
            layout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            layout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            layout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            StackLayout cardBody = new StackLayout
            {
                Padding = new Thickness(0, 0, 0, 0),
                Margin = new Thickness(0),
                ClassId = "course_" + obj.id,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                IsVisible = false,
                Opacity = 0
               
            };


            // used to expand accordion
            AccordionButton AcBtn = new AccordionButton
            {
                Image = "chevron_down.png",
                Padding = new Thickness(0, 0, 0, 0),
                Margin = new Thickness(0, 0, 0, 0),
                WidthRequest = 20,
                HeightRequest = 20
            };

            AcBtn.ContentFrame = cardBody;
            AcBtn.FrameOpen = false;
            AcBtn.Clicked += AcBtn_Clicked;


            
            StackLayout frameContainer = new StackLayout();
            foreach (Activity act in obj.Activities.Activity)
            {
                Models.Record courseRecord = await App.Database.GetCourseByID(act.CourseID);
                Grid activityContainer = new Grid
                {
                    Padding = new Thickness(5, 0, 5, 0),
                    ClassId = "course_" + act.id
                };
                activityContainer.RowDefinitions.Add(new RowDefinition { Height = 50 });
                activityContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                activityContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                activityContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

                Label coursetitle = new Label
                {
                    Text = act.Name
                };
                ActivityIndicator spinner = new ActivityIndicator
                {
                    IsVisible = false,
                    Style = (Style)Application.Current.Resources["spinnerStyle"],
                    HeightRequest = 20
                };

                DownloadButton launchBtn = new DownloadButton
                {
                    Text = (courseRecord == null) ? "open" :
                        (courseRecord.CompletionStatus.ToLower() == "completed") ? "review" :
                        (courseRecord.CMI == "") ? "open" : "resume",
                    IsVisible = (courseRecord == null) ? false : (courseRecord.Deleted == "false") ? true : false,
                    Image = "launch_w.png",
                    Style = (Style)Application.Current.Resources["buttonStyle"],
                    ClassId = act.CourseID,
                    CourseID = act.CourseID

                };


                DownloadImageButton downloadBtn = BuildImageDownload(act.CourseID, courseRecord,spinner);
                Courses c = new Courses();
                downloadBtn.Clicked += c.DownloadClicked;
                launchBtn.Clicked += c.launchCourse;
                downloadBtn.LaunchButton = launchBtn;
                downloadBtn.CourseID = act.CourseID;
                // add the image
                CachedImage marquee = BuildMarquee(act.CourseID,false);


                activityContainer.Children.Add(coursetitle,0,0);
                Grid.SetColumnSpan(coursetitle, 2);
                activityContainer.Children.Add(marquee,0,1);
                activityContainer.Children.Add(downloadBtn,1,1);
                activityContainer.Children.Add(launchBtn, 1, 1);
                activityContainer.Children.Add(spinner, 1, 1);
                frameContainer.Children.Add(activityContainer);
            }
            
           
            cardBody.Children.Add(frameContainer);
            layout.Children.Add(objectiveTitle,0,0);
            layout.Children.Add(AcBtn, 1, 0);
            layout.Children.Add(cardBody,0,1);
            Grid.SetColumnSpan(cardBody, 2);
            container.Children.Add(layout);

        }

        private async void AcBtn_Clicked(object sender, EventArgs e)
        {
            AccordionButton btn = (AccordionButton)sender;
            StackLayout f = btn.ContentFrame;
            await btn.RotateTo((btn.FrameOpen ? 0 : 180), 200, Easing.CubicInOut);
            if (btn.FrameOpen)
            {
                await Task.WhenAll(new List<Task> { f.FadeTo(0, 200, Easing.Linear) });
                f.IsVisible = false;
                f.ForceLayout();
                btn.FrameOpen = false;
            }
            else {
                f.IsVisible = true;
                //await Task.WhenAll(new List<Task> { f.LayoutTo(new Rectangle(f.Bounds.X, f.Bounds.Y, f.Bounds.Width, 300), 500, Easing.CubicOut)});
                await Task.WhenAll(new List<Task> { f.FadeTo(1, 400, Easing.Linear) });
                //f.Opacity = 1;
                 btn.FrameOpen = true;
            }
        }

        /************************************************************************** 
         * BuildMarquee returns a cached image object 
         * It takes 2 params
         * id - CourseID
         * fullscreen - do you want to make a full screen widht or 1/2 width marquee         
         **************************************************************************/
        public CachedImage BuildMarquee(string id,bool fullscreen)
        {
            CachedImage marquee = new CachedImage()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                MinimumWidthRequest = (fullscreen) ? App.ScreenWidth : App.ScreenWidth / 2,
                WidthRequest = (fullscreen) ? App.ScreenWidth : App.ScreenWidth / 2,
                Margin = new Thickness(0, 0, 0, 0),
                CacheDuration = TimeSpan.FromDays(30),
                DownsampleToViewSize = true,
                RetryCount = 0,
                RetryDelay = 250,
                LoadingPlaceholder = "placeholder.png",
                ErrorPlaceholder = "placeholder.png",
                Source = Constants.BlobLocation + "/coursecontent/" + id + "/courselogo.gif"

            };

            return marquee;
        }

        public DownloadImageButton BuildImageDownload(string id, Models.Record courseRecord, ActivityIndicator spinner)
        {
            DownloadImageButton downloadBtn = new DownloadImageButton
            {
                ///Text = "download",
                Source = "download.png",
                Style = (Style)Application.Current.Resources["buttonStyle"],
                ClassId = id,
                Spinner = spinner,
                IsVisible = (courseRecord == null) ? true : (courseRecord.Deleted == "false") ? false : true
            };

            return downloadBtn;
        }

        public DownloadButton BuildDownload(string id, Models.Record courseRecord, ActivityIndicator spinner)
        {
            DownloadButton downloadBtn = new DownloadButton
            {
                Text = "download",
                Image = "download.png",
                Style = (Style)Application.Current.Resources["buttonStyle"],
                ClassId = id,
                Spinner = spinner,
                IsVisible = (courseRecord == null) ? true : (courseRecord.Deleted == "false") ? false : true
            };

            return downloadBtn;
        }
    }
}
