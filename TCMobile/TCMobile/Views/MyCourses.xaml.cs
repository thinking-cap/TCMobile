using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;

namespace TCMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MyCourses : ContentPage
	{

        private Pages.LoadingPage _loadingPage;
		public MyCourses ()
		{
			InitializeComponent ();
            _loadingPage = new Pages.LoadingPage("Loading");
            
            CheckForCourses();

		}

        private async void openPopup()
        {
            await PopupNavigation.Instance.PushAsync(_loadingPage);
        }

        private async void closePopup()
        {
            await PopupNavigation.Instance.RemovePageAsync(_loadingPage);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CheckForCourses();
        }

        private async void CheckForCourses()
        {
            openPopup();
            Courses c = new Courses();
            List<Models.Record> courses = await c.CheckForCourses();
            if (courses.Count > 0)
                CourseList.ItemsSource = courses;
            closePopup();
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