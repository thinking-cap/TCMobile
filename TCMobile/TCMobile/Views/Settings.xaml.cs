using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TCMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Settings : ContentPage
	{
		public Settings ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            buildSettings();
        }

        public async void buildSettings()
        {
            Container.Children.Clear();
            Courses c = new Courses();
            List<Models.Record> courses = await c.CheckForCourses();

            foreach (Models.Record course in courses)
            {
                if (course.Deleted == "false")
                {
                    StackLayout layout;
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

                    Button delete = new Button
                    {
                        Text = "delete",
                        Image = "delete.png",
                        Style = (Style)Application.Current.Resources["buttonStyle"],
                        ClassId = course.CourseID
                    };

                    delete.Clicked += removeCoursePackgeAsync;

                    layout.Children.Add(title);
                    layout.Children.Add(delete);
                    Container.Children.Add(layout);
                }
            }
        }

        public async void removeCoursePackgeAsync(Object Sender, EventArgs args)
        {
            Button button = (Button)Sender;
            string id = button.ClassId;
            StackLayout item = (StackLayout)button.Parent;
            Models.Record course = await App.Database.GetCourseByID(id);
            course.Deleted = "true";
            int del = await App.Database.SaveItemAsync(course);
            Container.Children.Remove(item);
            // let's delete the downloaded files  to clear up space //
            string courseindex = "Courses/" + id ;
            string localFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string coursePath = Path.GetDirectoryName(Path.Combine(localFolder, courseindex));
            System.IO.DirectoryInfo di = new DirectoryInfo(coursePath);
            foreach (FileInfo file in di.EnumerateFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.EnumerateDirectories())
            {
                dir.Delete(true);
            }

        }
     
        private void Wifi_Toggled(object sender, ToggledEventArgs e)
        {

            App.Current.Properties["WiFi"] = e.Value.ToString();
            Constants.WifiOnly = e.Value.ToString();
            App.Current.SavePropertiesAsync();
        }
    }
}