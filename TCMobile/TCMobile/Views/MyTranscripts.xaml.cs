using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TCMobile.CustomControls;
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

                    Grid chartGrid = new Grid()
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Padding = 0,
                        Margin = 0
                    };

                    chartGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
                    chartGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    chartGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });



                    string completion = (course.CompletionStatus == "") ? (course.CompletionStatus == "unknown") ? "In Progress" : "Not Attempted" : course.CompletionStatus;
                    string success = (course.SuccessStatus == "" || course.SuccessStatus == "unknown") ? "" : "/" + course.SuccessStatus;

                    string score;
                    float score_a;
                    float score_b;
                    float perc_complete;
                    float perc_incomplete;
                    if (String.IsNullOrEmpty(course.ProgressMeasure))
                    {
                        perc_complete = (course.CompletionStatus == "") ? 0 : (course.CompletionStatus == "Completed") ? 100 : 50;
                        perc_incomplete = 100 - perc_complete;
                    }
                    else
                    {
                        perc_complete = float.Parse(courseRecord.ProgressMeasure) * 100;
                        perc_incomplete = 100 - perc_complete;
                    }
               
                    
                    if (course.ScoreRaw != "")
                    {
                        int raw = Convert.ToInt32(course.ScoreRaw);
                        int max = Convert.ToInt32(course.ScoreMax);
                        int min = Convert.ToInt32(course.ScoreMin);
                        int scaled = (raw - min) / (max - min) * 100;
                        score = scaled.ToString();
                        score_a = scaled;
                        score_b = (scaled < 100) ? 100 - score_a : 0;
                    }
                    else {
                        score = (course.Score == "") ? "" : "  " + Math.Round(Double.Parse(course.Score) * 100).ToString() + "%";
                        score_a = (float)Math.Round(Double.Parse(course.Score) * 100);
                        score_b = (Math.Round(Double.Parse(course.Score) * 100) < 100) ? 100 - score_a : 0;
                    }

                   

                    Doughnut doughnut = new Doughnut();
                    Grid doughnutContainer = doughnut.CompletionChart("Score", score_a, score_b);

                    Label status = new Label
                    {
                        Text = completion + success,
                        Style = (Style)Application.Current.Resources["headerStyle"]
                    };

                    Doughnut completeDoughnut = new Doughnut();
                    Grid completeDoughnutContainer = completeDoughnut.CompletionChart("Complete", perc_complete, perc_incomplete);


                    StackLayout layout = new StackLayout();
                    card.Content = layout;
                    layout.Children.Add(title);
                    doughnutContainer.VerticalOptions = LayoutOptions.CenterAndExpand;
                    completeDoughnutContainer.VerticalOptions = LayoutOptions.CenterAndExpand;
                    doughnutContainer.HorizontalOptions = LayoutOptions.CenterAndExpand;
                    completeDoughnutContainer.HorizontalOptions = LayoutOptions.CenterAndExpand;
                    chartGrid.Children.Add(doughnutContainer, 0, 0);
                    chartGrid.Children.Add(completeDoughnutContainer, 1, 0);
                    layout.Children.Add(chartGrid);
                   // layout.Children.Add(completeDoughnutContainer);
                   // layout.Children.Add(doughnutContainer);
                    //layout.Children.Add(description);

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