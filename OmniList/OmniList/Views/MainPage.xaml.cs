using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using OmniList.Helpers;
using OmniList.ViewModels;
using Xamarin.Forms;

namespace OmniList.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage ()
        {
            InitializeComponent();
        }

        private async void LoginButton_OnClicked (object sender, EventArgs e)
        {
            var btn = (Button)sender;
            MobileServiceAuthenticationProvider provider = MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory;
            switch (btn.Text)
            {
                case "Google":
                     provider = MobileServiceAuthenticationProvider.Google;
                    break;
                case "New Account":
                     provider = MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory;
                    break;
                case "Facebook":
                    provider = MobileServiceAuthenticationProvider.Facebook;
                    break;

            }
            if (await InitializerHelper.LoginAsync(provider))
            {
                await Task.Delay(100);
                Navigation.InsertPageBefore(new NavigationPage(new GroceryList()), this);
                await Navigation.PopAsync();
            }


        }
    }
}
