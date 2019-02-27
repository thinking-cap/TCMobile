using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


            StackLayout layout;
            if (courses.Count() > 0)
            {
                foreach (Models.Record course in courses)
                {
                    Models.Record courseRecord = await App.Database.GetCourseByID(course.CourseID);
                    layout = new StackLayout
                    {
                        Spacing = 1,
                        ClassId = "course_" + course.CourseID
                    };
                    Label title = new Label
                    {
                        Text = course.CourseName,
                        Style = (Style)Application.Current.Resources["headerStyle"]
                    };
                    Label description = new Label
                    {
                        Text = course.CourseDescription,
                        Style = (Style)Application.Current.Resources["textStyle"]
                    };
                    string completion = (course.CompletionStatus == "" || course.CompletionStatus == "unknown") ? "Not Attempted" : course.CompletionStatus;
                    string success = (course.SuccessStatus == "" || course.SuccessStatus == "unknown") ? "" : "/" + course.SuccessStatus;
                    string score = (course.Score == "") ? "" : "  " + course.Score + "%";
                    Label status = new Label
                    {
                        Text = completion + success + score,
                        Style = (Style)Application.Current.Resources["headerStyle"]
                    };







                    layout.Children.Add(title);
                    layout.Children.Add(status);
                    layout.Children.Add(description);

                    Courses.Children.Add(layout);
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