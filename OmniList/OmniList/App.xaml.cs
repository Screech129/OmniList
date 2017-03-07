using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmniList.Views;
using Xamarin.Forms;
using OmniList.Helpers;

namespace OmniList
{
  
    public partial class App
    {
        public static bool LoggedIn { get; set; }

        public static string AppName => "OmniList";

       
        public App ()
        {
            
            InitializeComponent();
            if (!AuthStore.IsUserLoggedIn())
            {
                MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                MainPage = new NavigationPage(new GroceryList());
            }


        }

        protected override void OnStart ()
        {
            // Handle when your app starts
          
        }

        protected override void OnSleep ()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume ()
        {
            // Handle when your app resumes
        }
    }
}
