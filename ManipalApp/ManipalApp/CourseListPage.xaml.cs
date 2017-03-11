using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ManipalApp
{
    public partial class CourseListPage : ContentPage
    {
        public CourseListPage()
        {
            InitializeComponent();

            BindCourseList();
        }

        private async void BindCourseList()
        {
            var listOfCourses = await GetAllCoursesAsync();

            if (listOfCourses != null)
                CourseListView.ItemsSource = listOfCourses;
        }

        private async Task<List<Course>> GetAllCoursesAsync()
        {
            HttpClient client = new HttpClient();

            var uri = new Uri("http://manipalapiapp.azurewebsites.net/courses");

            try
            {
                var response = await client.GetAsync(uri);

                var result = await response.Content.ReadAsStringAsync();

                var courseList = JsonConvert.DeserializeObject<List<Course>>(result);

                if (courseList != null && courseList.Count > 0)
                    return courseList;

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return null;
            }
        }

        private async void CourseListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedItem = (sender as ListView).SelectedItem;

            await Navigation.PushAsync(new CourseDetailPage
            {
                BindingContext = selectedItem
            });
        }
    }
}
