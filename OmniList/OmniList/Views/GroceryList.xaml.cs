using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using OmniList.Helpers;
using OmniList.ViewModels;
using Xamarin.Forms;

namespace OmniList.Views
{
    public partial class GroceryList : ContentPage
    {
        public GroceryList ()
        {
            InitializeComponent();
            BindingContext = new GroceryListViewModel(Navigation);
            GroceryListView.ItemSelected += async (s, e) => await CompleteItem();
        }

        protected override async void OnAppearing ()
        {
            base.OnAppearing();
            await InitializerHelper.Initialize();
            
            var vm = BindingContext as GroceryListViewModel;
            var user = await AuthStore.GetUserFromCache();
            InitializerHelper.Client.CurrentUser = user;
            try
            {
                if (vm != null) await vm.PopulateList();
            }
            catch (Exception e )
            {
                if (e is MobileServiceInvalidOperationException)
                {
                    if (e.Message.ToLower().Contains("unauthorized"))
                    {
                        await Task.Delay(100);
                        Navigation.InsertPageBefore(new LoginPage(), this);
                        await Navigation.PopAsync();
                    }
                }
                var exception = e as MobileServicePushFailedException;
                if (exception?.PushResult.Status == MobileServicePushStatus.CancelledByAuthenticationError)
                {
                    await Task.Delay(100);
                    Navigation.InsertPageBefore(new LoginPage(), this);
                    await Navigation.PopAsync();
                }
            }
                

        }

        private async Task CompleteItem ()
        {
            var vm = BindingContext as GroceryListViewModel;
            if (vm != null) await vm.RemoveItem();
        }

      
    }
}
