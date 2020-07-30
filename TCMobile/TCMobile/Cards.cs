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
using Microcharts.Forms;
using Microcharts;
using SkiaSharp;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace TCMobile
{
    class Cards
    {
        public async Task<bool> buildCourseCard(string courseid, string coursetitle, string coursedescription, StackLayout container,EventHandler downloadClicked, EventHandler launchCourse,string duedate, string enddate)
        {
            DateTime today = DateTime.Today;
            DateTime expiry = DateTime.Parse(enddate);
            bool expired = today >= expiry;

            MaterialFrame frame;
            StackLayout layout;
            // let's try to sync all the courses
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                API api = new API();
                Device.BeginInvokeOnMainThread(async () => await api.SyncCourse(courseid));
            }
            // DownloadButton launchBtn;
            ActivityIndicator spinner;
           // Image marquee;
            StackLayout marqueeContainer;
            // See if there is a course record so we can display the proper navigation (download, launch etc...)
            Models.Record courseRecord = await App.Database.GetCourseByID(courseid);
            CachedImage marquee = BuildMarquee(courseid,true);

            // wrap the course Marquee in a stack layout so we have more control with the layout
            marqueeContainer = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
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

            Grid titleGrid = new Grid()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = 0,
                Margin = 15
            };
            if (courseRecord != null && courseRecord.Downloaded != false)
            {
                titleGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
                titleGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                titleGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });
            }
            else
            {
                titleGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
                titleGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

            }
            string duedateText = "";
            //ChartView chartView = null;
            if (!String.IsNullOrEmpty(duedate))
            {
                DateTime dt = Convert.ToDateTime(duedate);
                duedateText = "Due: " + String.Format("{0:ddd, MMM d, yyyy}", dt);
            }
            float perc_complete = 0;
            float perc_incomplete = 0;
            if(courseRecord != null && courseRecord.Downloaded == true) {
                if (courseRecord.ProgressMeasure != null && courseRecord.ProgressMeasure != "")
                {
                   perc_complete = (float)Math.Round(float.Parse(courseRecord.ProgressMeasure), 0, MidpointRounding.AwayFromZero);
                   perc_incomplete = 100 - perc_complete;
                    
                }
                else { 
                    perc_complete = (courseRecord != null && courseRecord.CompletionStatus.ToLower() == "completed") ?  100 :(courseRecord != null) ? 50 : 0;
                    perc_incomplete = (courseRecord != null && courseRecord.CompletionStatus.ToLower() != "completed") ? (courseRecord != null) ? 50 : 0 : 100;
                }
           
            }
            Label dueDate = new Label
            {
                Text = duedateText
            };

            string htmlText = @"<html>
                                    <head>
                                        <meta name='viewport' content='width=device-width; height=device-height; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;'/>                                    
                                        <style type='text/css'>
                                             body{font-family:Segoe UI, Helvetica Neue,'Lucida Sans Unicode', Skia, sans-serif;
                                                    border:0px;padding:0px;margin:0px;
                                                    background-color:transparent;
                                                    overflow:hidden;
                                                    height:100%;
                                                }
                                        </style>    
                                    </head>
                                    <body><div style='min-height:50px;'>" + HttpUtility.HtmlDecode(coursedescription) + "</div></body></html>";
            var description = new CustomWebview
            {
               HeightRequest = 50,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Source = new HtmlWebViewSource
                {
                    Html = htmlText
                },
                Style = (Style)Application.Current.Resources["descriptionWebView"]

            };

           // ViewCell webViewViewCell = new ViewCell();
            Grid webViewGrid = new Grid();
            webViewGrid.Children.Add(description);

            spinner = new ActivityIndicator
            {
                IsVisible = false,
                Style = (Style)Application.Current.Resources["spinnerStyle"],
                HeightRequest = 20
            };
            Grid btnGrid = new Grid()
            {
                HorizontalOptions = LayoutOptions.Center
            };
          
            btnGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50)});
            btnGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20)});
            btnGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80)});
              

            Label lbl = new Label()
            {
                Text = (courseRecord == null || courseRecord.Downloaded == false) ? "download" :
                        (courseRecord.CompletionStatus.ToLower() == "completed") ? "review" :
                        (courseRecord.CMI == "") ? "open" : "resume",
                HorizontalOptions = LayoutOptions.Center
            };
            DownloadImageButton downloadBtn = BuildImageDownload(courseid, courseRecord,spinner,lbl);
            downloadBtn.CourseID = courseid;
            

            

            StackLayout cardFooter = new StackLayout
            {
                Padding = new Thickness(16, 0, 16, 8),
                ClassId = "course_" + courseid
            };
            
            DownloadImageButton launchBtn = BuildImageLaunch(courseid, courseRecord, spinner,lbl);
            downloadBtn.LaunchButton = launchBtn;
            launchBtn.Clicked += launchCourse;
            downloadBtn.Clicked += downloadClicked;
            if (!expired)
            {
                btnGrid.Children.Add(downloadBtn, 0, 0);
                btnGrid.Children.Add(launchBtn, 0, 0);
                btnGrid.Children.Add(spinner, 0, 0);
                btnGrid.Children.Add(lbl, 0, 1);
            }
           
            //titleGrid.Children.Add(title, 0, 0);


            if (courseRecord != null && courseRecord.Downloaded != false)
            {                
                Doughnut doughnut = new Doughnut();
                Grid doughnutContainer = doughnut.CompletionChart("Completed", perc_complete, perc_incomplete);
                titleGrid.Children.Add(doughnutContainer,1,0);
            }
            cardBody.Children.Add(titleGrid);
            cardBody.Children.Add(dueDate);
            //string x = (courseRecord == null || courseRecord.Deleted == "true") ? "download" :
            //             (courseRecord.CompletionStatus.ToLower() == "completed") ? "review" :
            //             (courseRecord.CMI == "") ? "open" : "resume";
            
            cardBody.Children.Add(webViewGrid);
            layout.Children.Add(title);
            layout.Children.Add(marqueeContainer);
            layout.Children.Add(cardBody);
            layout.Children.Add(cardFooter);
            //cardFooter.Children.Add(launchBtn);
            // cardFooter.Children.Add(downloadBtn);
            //cardFooter.Children.Add(lbl);
            cardFooter.Children.Add(btnGrid);
            //cardFooter.Children.Add(spinner);
            frame.Content = layout;
            container.Children.Add(frame);

            return true;
        }

        public async Task<Models.LPDBRecord> getMap(string id)
        {
            Courses m = new Courses();
            return await m.GetActivityMap(id);
        }
        // adding LP Marquee //
        public async void  buildLPCard(string id, string lptitle, string lpdescription,FlexLayout LP, EventHandler detailsClicked)
        {
            StudentActivityMap map = null;
            MaterialFrame frame;
            StackLayout layout;
            Label status = new Label();
            LearningPath lpObj = new LearningPath();
            int completionPercent = await lpObj.GetCompletion(id);
            Models.LPDBRecord lp = await getMap(id);
            if(lp.LPMap != "")
            {
                map = JsonConvert.DeserializeObject<StudentActivityMap>(lp.LPMap);
                if(map != null)
                {
                    LP learningPath = new LP();
                    Models.statusObject completionStatus = learningPath.lpStatus(map.Objective);

                    status.Text = completionStatus.completion;
                }
                else
                {
                    status.Text = "not started";
                }
            }
            else
            {
                status.Text = "not started";
            }
            frame = new MaterialFrame
            {
                HasShadow = true,
                Padding = new Thickness(0, 0, 0, 0),
                Margin = new Thickness(0, 8, 0, 24),
                CornerRadius = 0
            };
            CachedImage marquee = BuildMarquee(id, false);
            marquee.HorizontalOptions = LayoutOptions.StartAndExpand;
            // need to add doughnut
            Doughnut doughnut = new Doughnut();
            int percentIncomplete = 100 - completionPercent;
            Grid doughnutContainer = doughnut.CompletionChart("Completed", completionPercent, percentIncomplete);
            /** add a grid to put the html view in ****/
            Grid HtmlContainer = new Grid()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = 0,
                Margin = 0,
                MinimumHeightRequest = 100
            };
            HtmlContainer.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            HtmlContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });


            Label lbl = new Label()
            {
                Text = "more info",
                HorizontalOptions = LayoutOptions.Center
            };

            DownloadImageButton moreBtn = new DownloadImageButton
            {
                //Text = "more",
                Source = "outline_info_black_48.png",
                //Style = (Style)Application.Current.Resources["buttonStyle"],
                ClassId = id,
                CourseID = id,
                BackgroundColor = Color.Transparent,
                BorderColor = Color.Transparent,
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
                                                    height:100%;
                                                }
                                        </style>    
                                    </head>
                                    <body><div style='min-height:50px;'>" + HttpUtility.HtmlDecode(lpdescription) + "</div></body></html>";

            CustomWebview description = new CustomWebview
            {
               HeightRequest = 50,
                WidthRequest = 2,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Source = new HtmlWebViewSource
                {
                    Html = htmlText
                },
                Style = (Style)Application.Current.Resources["descriptionWebView"]

            };
            HtmlContainer.Children.Add(description, 0, 0);
            Grid btnGrid = new Grid()
            {
                HorizontalOptions = LayoutOptions.Center
            };
            btnGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            btnGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
            btnGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });

            btnGrid.Children.Add(moreBtn, 0, 0);
            btnGrid.Children.Add(lbl, 0, 1);
            cardFooter.Children.Add(doughnutContainer);
            cardFooter.Children.Add(btnGrid);
            cardBody.Children.Add(title);
            cardBody.Children.Add(marquee);
           
            cardBody.Children.Add(status);
            cardBody.Children.Add(HtmlContainer);
            layout.Children.Add(cardBody);
            layout.Children.Add(cardFooter);
            frame.Content = layout;


            LP.Children.Add(frame);
        }
        /************* Learning Path Cards *************
         * 
         * 
         *****************************************************/
        public async  Task<bool>buildObjectiveCard(Objective obj, StackLayout container, string lpid,int count)
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
                HeightRequest = 40,
                Style = (Style)Application.Current.Resources["headerStyle"]
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
                IsVisible = (count > 1) ? false : true,
                Opacity = (count > 1) ? 0 : 100


            };

             // used to expand accordion
            AccordionButton AcBtn = new AccordionButton
            {
                Image = "chevron_down.png",
                Padding = new Thickness(0, 0, 0, 0),
                Margin = new Thickness(0, 0, 0, 0),
                WidthRequest = 20,
                HeightRequest = 20,
                BackgroundColor = Color.Transparent,
                BorderColor = Color.Transparent
            };

            AcBtn.ContentFrame = cardBody;
            AcBtn.FrameOpen = false;
            AcBtn.Clicked += AcBtn_Clicked;


            
            StackLayout frameContainer = new StackLayout();
            foreach (Activity act in obj.Activities.Activity)
            {
                // chart for each activity in the objective
                ChartView chartView = null;
                float perc_complete = 0;
                float perc_incomplete = 0;
               
                
            
            Models.Record courseRecord = await App.Database.GetCourseByID(act.CourseID);
               if(courseRecord == null)
                {
                   
                        Models.Record rec = new Models.Record();
                        rec.CourseID = act.CourseID;
                        // find the course name
                        rec.CourseName = act.Name;
                        rec.Version = "1";
                        rec.CourseDescription = "";
                        rec.CompletionStatus = "Not Started";
                        rec.SuccessStatus = "";
                        rec.Score = "";
                        rec.Deleted = "false";
                        rec.Downloaded = false;
                        rec.EndDate = "";
                        rec.DueDate = "";
                        rec.LP = lpid;
                        rec.Objective = obj.id;
                        rec.CMI = "";
                        App.LocalFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                        await App.Database.SaveItemAsync(rec);

                        perc_complete = (rec.CompletionStatus.ToLower() == "completed") ? 100 : (rec.CompletionStatus.ToLower() != "not started") ? 50 : 0;
                        perc_incomplete = (rec.CompletionStatus.ToLower() == "completed") ? 0 : (rec.CompletionStatus.ToLower() != "not started") ? 50 : 100;


                }
                else if(String.IsNullOrEmpty(courseRecord.Objective) || String.IsNullOrEmpty(courseRecord.LP))
                {
                   
                    courseRecord.Objective = obj.id;
                    courseRecord.LP = lpid;

                    
                    await App.Database.SaveItemAsync(courseRecord);
                }

                if (courseRecord != null)
                {
                    if (courseRecord.ProgressMeasure != null && courseRecord.ProgressMeasure != "")
                    {
                        perc_complete = float.Parse(courseRecord.ProgressMeasure) * 100;
                        perc_incomplete = 100 - perc_complete;

                    }
                    else
                    {
                        perc_complete = (courseRecord.CompletionStatus.ToLower() == "completed") ? 100 : (courseRecord.CompletionStatus.ToLower() == "unknown" || courseRecord.CompletionStatus.ToLower() == "attempted") ? 50 : 0;
                        perc_incomplete = (courseRecord.CompletionStatus.ToLower() == "not started") ? 100 : (courseRecord.CompletionStatus.ToLower() == "completed") ? 100 : 50;
                    }
                
                }


                

                Doughnut doughnut = new Doughnut();
                
                Grid doughnutGrid = doughnut.CompletionChart("complete", perc_complete, perc_incomplete);

                //*****************************************************
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
                    Text = act.Name,
                    Style = (Style)Application.Current.Resources["subHeaderStyle"],
                    VerticalTextAlignment = TextAlignment.Center,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };
                ActivityIndicator spinner = new ActivityIndicator
                {
                    IsVisible = false,
                    Style = (Style)Application.Current.Resources["spinnerStyle"],
                    HeightRequest = 20
                };

                Label lbl = new Label()
                {
                    Text = (courseRecord == null || courseRecord.Downloaded == false) ? "download" :
                        (courseRecord.CompletionStatus.ToLower() == "completed") ? "review" :
                       (courseRecord.CMI == "") ? "open" : "resume",
                    HorizontalOptions = LayoutOptions.Center
                };

                // Create the two buttons that get swapped //
                DownloadImageButton launchBtn = BuildImageLaunch(act.CourseID, courseRecord, null,lbl);
                DownloadImageButton downloadBtn = BuildImageDownload(act.CourseID, courseRecord,spinner,lbl);
                launchBtn.HorizontalOptions = LayoutOptions.Center;
                downloadBtn.HorizontalOptions = LayoutOptions.Center;
                launchBtn.VerticalOptions = LayoutOptions.EndAndExpand;
                downloadBtn.VerticalOptions = LayoutOptions.EndAndExpand;
                // Button Grid
                Grid btnGrid = new Grid()
                {
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.EndAndExpand
                };
                btnGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
                btnGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
                btnGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });

                btnGrid.Children.Add(launchBtn, 0, 0);
                btnGrid.Children.Add(downloadBtn, 0, 0);
                btnGrid.Children.Add(spinner, 0, 0);
                btnGrid.Children.Add(lbl, 0, 1);

                Courses c = new Courses();
                downloadBtn.VerticalOptions = LayoutOptions.EndAndExpand;
                downloadBtn.Clicked += c.DownloadClicked;
                launchBtn.Clicked += c.launchCourse;
                launchBtn.VerticalOptions = LayoutOptions.EndAndExpand;
                downloadBtn.LaunchButton = launchBtn;
                downloadBtn.CourseID = act.CourseID;
                // add the image
                CachedImage marquee = BuildMarquee(act.CourseID,false);
                marquee.HorizontalOptions = LayoutOptions.StartAndExpand;

                activityContainer.Children.Add(coursetitle,0,0);
                // activityContainer.Children.Add(chartView, 1, 0);
                activityContainer.Children.Add(doughnutGrid, 1, 0);
                Grid.SetColumnSpan(coursetitle, 2);
                activityContainer.Children.Add(marquee,0,1);
                activityContainer.Children.Add(btnGrid, 1, 1);
                frameContainer.Children.Add(activityContainer);
            }
            
           
            cardBody.Children.Add(frameContainer);
            layout.Children.Add(objectiveTitle,0,0);
           
            layout.Children.Add(AcBtn, 1, 0);
            layout.Children.Add(cardBody,0,1);
            Grid.SetColumnSpan(cardBody, 2);
            container.Children.Add(layout);
            return true;

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
                LoadingPlaceholder = "loading.png",
                ErrorPlaceholder = "placeholder.png",
                Source = Constants.BlobLocation + "/coursecontent/" + id + "/courselogo.gif"

            };

            return marquee;
        }

        public DownloadImageButton BuildImageDownload(string id, Models.Record courseRecord, ActivityIndicator spinner, Label txt)
        {
            DownloadImageButton downloadBtn = new DownloadImageButton
            {
                ///Text = "download",
                Source = "baseline_cloud_download_black_48.png",
                //Style = (Style)Application.Current.Resources["buttonStyle"],
                ClassId = id,
                Spinner = spinner,
                BackgroundColor = Color.Transparent,
                BorderColor = Color.Transparent,
                BtnLabel = txt,
                IsVisible = (courseRecord == null) ? true : (courseRecord.Deleted == "true" || courseRecord.Downloaded == false) ? true : false
            };

            return downloadBtn;
        }

        public DownloadButton BuildDownload(string id, Models.Record courseRecord, ActivityIndicator spinner)
        {
            DownloadButton downloadBtn = new DownloadButton
            {
                Text = "download",
                Image = "baseline_cloud_download_black_48.png",
                Style = (Style)Application.Current.Resources["buttonStyle"],
                
                ClassId = id,
                Spinner = spinner,
                IsVisible = (courseRecord == null) ? true : (courseRecord.Deleted == "false") ? false : true
            };

            return downloadBtn;
        }

        public DownloadButton BuildLaunch(string id, Models.Record courseRecord, ActivityIndicator spinner)
        {
            DownloadButton downloadBtn = new DownloadButton
            {
                Text = (courseRecord == null) ? "open" :
                        (courseRecord.CompletionStatus.ToLower() == "completed") ? "review" :
                        (courseRecord.CMI == "") ? "open" : "resume",
                Image = "baseline_launch_black_48.png",
                Style = (Style)Application.Current.Resources["buttonStyle"],
                ClassId = id,
                Spinner = spinner,
                CourseID = id,
                IsVisible = (courseRecord == null) ? false : (courseRecord.Deleted == "false" && courseRecord.Downloaded == true) ? true : false,
            };

            return downloadBtn;
        }

        public DownloadImageButton BuildImageLaunch(string id, Models.Record courseRecord, ActivityIndicator spinner, Label txt)
        {
            DownloadImageButton downloadBtn = new DownloadImageButton
            {
                Source = "baseline_launch_black_48.png",
                ClassId = id,
                Spinner = spinner,
                CourseID = id,
                BackgroundColor = Color.Transparent,
                BorderColor = Color.Transparent,
                BtnLabel = txt,
                IsVisible = (courseRecord == null) ? false : (courseRecord.Deleted == "false" && courseRecord.Downloaded == true) ? true : false,

            };

            return downloadBtn;
        }
    }
}
