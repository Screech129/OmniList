using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmniList.Helpers;
using OmniList.ViewModels;
using Xamarin.Forms;

namespace OmniList.Views
{
    public partial class GroceryList : ContentPage
    {
        private bool authenticated = false;
        public GroceryList ()
        {
            InitializeComponent();
            GroceryListView.ItemSelected += async (s, e) => await CompleteItem();
        }

        protected override async void OnAppearing ()
        {
            base.OnAppearing();
            var vm = BindingContext as GroceryListViewModel;
            if (authenticated)
            {
                if (vm != null) await vm.PopulateList();
                LoginButton.IsVisible = false;
            }
        }





        private async Task CompleteItem ()
        {
            var vm = BindingContext as GroceryListViewModel;
            if (vm != null) await vm.RemoveItem();
        }

        private async void LoginButton_OnClicked (object sender, EventArgs e)
        {
            if (App.Authenticator != null)
                authenticated = await App.Authenticator.Authenticate();

            if (authenticated)
            {
                var vm = BindingContext as GroceryListViewModel;
                if (vm != null) await vm.PopulateList();
            }
        }
    }
}
