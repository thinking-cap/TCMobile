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
	public partial class MyCourses : ContentPage
	{
		public MyCourses ()
		{
			InitializeComponent ();
            CheckForCourses();

		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CheckForCourses();
        }

        private async void CheckForCourses()
        {
            Courses c = new Courses();
            List<Models.Record> courses = await c.CheckForCourses();
            if (courses.Count > 0)
                CourseList.ItemsSource = courses;
        }

        private async void OpenCourseClick(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            string id = button.ClassId;

            string test = await Courses.openCourse(id, Navigation);
        }

        private async void RemoveCourseClick(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            string id = button.ClassId;
            Models.Record course = await App.Database.GetCourseByID(id);
            await App.Database.DeleteItemAsync(course);
        }

       


	}
}