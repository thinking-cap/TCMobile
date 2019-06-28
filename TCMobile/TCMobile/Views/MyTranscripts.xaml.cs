using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TCMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MyTranscripts : ContentPage
	{
		public MyTranscripts ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            buildTranscripts();
        }

        public async void buildTranscripts()
        {
            Courses.Children.Clear();
            Courses c = new Courses();
            List<Models.Record> courses = await c.CheckForCourses();


            
            if (courses.Count() > 0)
            {
                foreach (Models.Record course in courses)
                {
                    Frame card;
                    Models.Record courseRecord = await App.Database.GetCourseByID(course.CourseID);
                    card = new MaterialFrame
                    {
                        ClassId = "course_" + course.CourseID
                    };
                    Label title = new Label
                    {
                        Text = course.CourseName,
                        Style = (Style)Application.Current.Resources["headerStyle"]
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
                                    <body>" + HttpUtility.HtmlDecode(course.CourseDescription) + "</body></html>";
                    var description = new CustomWebview
                    {
                        HeightRequest = 300,
                        Source = new HtmlWebViewSource
                        {
                            Html = htmlText
                        },
                        Style = (Style)Application.Current.Resources["descriptionWebView"]

                    };

                    //Label description = new Label
                    //{
                    //    Text = course.CourseDescription,
                    //    Style = (Style)Application.Current.Resources["textStyle"]
                    //};

                    
                    string completion = (course.CompletionStatus == "") ? (course.CompletionStatus == "unknown") ? "In Progress" : "Not Attempted" : course.CompletionStatus;
                    string success = (course.SuccessStatus == "" || course.SuccessStatus == "unknown") ? "" : "/" + course.SuccessStatus;

                    string score;
                    if(course.ScoreRaw != "")
                    {
                        int raw = Convert.ToInt32(course.ScoreRaw);
                        int max = Convert.ToInt32(course.ScoreMax);
                        int min = Convert.ToInt32(course.ScoreMin);
                        int scaled = (raw - min) / (max - min) * 100;
                        score = scaled.ToString();
                    }
                    else {
                        score = (course.Score == "") ? "" : "  " + Math.Round(Double.Parse(course.Score) * 100).ToString() + "%";
                    }
                     
                    Label status = new Label
                    {
                        Text = completion + success + score,
                        Style = (Style)Application.Current.Resources["headerStyle"]
                    };





                    StackLayout layout = new StackLayout();
                    card.Content = layout;
                    layout.Children.Add(title);
                    layout.Children.Add(status);
                    layout.Children.Add(description);

                    Courses.Children.Add(card);
                }
            }
            else
            {
                Label message = new Label
                {
                    Text = "You have not started any courses on this device.",
                    Style = (Style)Application.Current.Resources["headerStyle"]
                };
                Courses.Children.Add(message);
            }
        }

    }
}