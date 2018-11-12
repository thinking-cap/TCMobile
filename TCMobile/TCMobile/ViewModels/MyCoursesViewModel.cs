using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace TCMobile.ViewModels
{
	public class MyCoursesViewModel : ContentView
	{
		public MyCoursesViewModel ()
		{
			Content = new StackLayout {
				Children = {
					new Label { Text = "My Courses" }
				}
			};
		}
	}
}