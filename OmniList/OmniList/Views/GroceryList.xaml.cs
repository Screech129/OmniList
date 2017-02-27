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
        public GroceryList ()
        {
            InitializeComponent();
            GroceryListView.ItemSelected += async (s, e) => await CompleteItem();
        }

        protected override async void OnAppearing ()
        {
            base.OnAppearing();
            await InitializerHelper.Initialize();
            
            var vm = BindingContext as GroceryListViewModel;
            var user = await AuthStore.GetUserFromCache();
            InitializerHelper.Client.CurrentUser = user; 
                if (vm != null) await vm.PopulateList();

        }

        private async Task CompleteItem ()
        {
            var vm = BindingContext as GroceryListViewModel;
            if (vm != null) await vm.RemoveItem();
        }

      
    }
}
