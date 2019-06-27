using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMobile.CustomControls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TCMobile.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TCMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Settings : ContentPage
	{
        private string _freeSpace;
        public String FreeSpace 
        {
            get { return _freeSpace; }
            set
            {
                _freeSpace = value;
                OnPropertyChanged();
            }
        }

       


        public Settings ()
		{
			InitializeComponent ();
            BindingContext = this;
        }
        
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            getFreeStorage();
            
            buildSettings();
        }

        public async void getFreeStorage()
        {
            var freeSpace = await DependencyService.Get<iStorage>().GetFreeSpace();
            double FreeGigabytes = Math.Round(freeSpace / 1024.0 / 1024.0 / 1024.0, 2);
            FreeSpace = FreeGigabytes.ToString() + "gbs of free storage";
            
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

                    DownloadImageButton delete = new DownloadImageButton
                    {
                       
                        Source = "outline_remove_circle_outline_black_48.png",
                        ClassId = course.CourseID,
                        BackgroundColor = Color.Transparent,
                        BorderColor = Color.Transparent
                    };

                    delete.Clicked += removeCoursePackgeAsync;
                    /// layout in a grid 

                    Grid btnGrid = new Grid()
                    {
                        HorizontalOptions = LayoutOptions.Center
                    };

                    btnGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
                    btnGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star) });
                    btnGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });
                    btnGrid.Children.Add(title, 0, 0);
                    btnGrid.Children.Add(delete, 1, 0);

                    layout.Children.Add(btnGrid);
                    Container.Children.Add(layout);
                }
            }
        }

        public async void removeCoursePackgeAsync(Object Sender, EventArgs args)
        {
            DownloadImageButton button = (DownloadImageButton)Sender;
            string id = button.ClassId;
            StackLayout item = (StackLayout)button.Parent.Parent;
            Models.Record course = await App.Database.GetCourseByID(id);
            course.Deleted = "true";
            course.Downloaded = false;
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
            await Task.Delay(500).ContinueWith(t => getFreeStorage());

        }
     
        private void Wifi_Toggled(object sender, ToggledEventArgs e)
        {

            App.Current.Properties["WiFi"] = e.Value.ToString();
            Constants.WifiOnly = e.Value.ToString();
            App.Current.SavePropertiesAsync();
        }
    }
}