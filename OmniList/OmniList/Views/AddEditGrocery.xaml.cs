using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmniList.ViewModels;
using Xamarin.Forms;

namespace OmniList.Views
{
    public partial class AddEditGrocery : ContentPage
    {
        public AddEditGrocery ()
        {
            InitializeComponent();
            BindingContext = new AddEditGroceryViewModel(Navigation);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
        }

        private void VisualElement_OnFocused(object sender, FocusEventArgs e)
        {
            var vm = BindingContext as AddEditGroceryViewModel;
            vm?.SetCategory(vm.Name);
        }
    }
}
