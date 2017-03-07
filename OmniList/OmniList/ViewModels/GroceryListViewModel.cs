using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.WindowsAzure.MobileServices;
using OmniList.Helpers;
using OmniList.Models;
using OmniList.Views;
using Xamarin.Forms;

namespace OmniList.ViewModels
{
    public class GroceryListViewModel: INotifyPropertyChanged
    {
        private readonly INavigation navigation;
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly DbHelper dbHelper = new DbHelper();

        public string User => InitializerHelper.Client.CurrentUser.UserId;

        private bool removed;
        public bool Removed
        {
            get
            {
                return removed;
            }
            set
            {
                removed = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Removed)));
            }

        }

   private bool isRefreshing;
        public bool IsRefreshing
        {
            get
            {
                return isRefreshing;
            }
            set
            {
                isRefreshing = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRefreshing)));

            }

        }

        public ObservableCollection<GroupingHelper<string, Grocery>> GroceriesGrouped { get; set; } =
            new ObservableCollection<GroupingHelper<string, Grocery>>();

        private ObservableCollection<Grocery> groceryCollection;
        public ObservableCollection<Grocery> GroceryCollection
        {
            get
            {
                return groceryCollection;
            }
            set
            {
                groceryCollection = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GroceryCollection)));
            }
        }

        

        private Grocery selectedItem;

        public Grocery SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedItem)));
            }
        }

        public ICommand AddItem { get; protected set; }

        public ICommand OnRefreshing { get; protected set; }

        public GroceryListViewModel(INavigation navigation)
        {
            AddItem = new Command(async () => await AddItemToList());
            OnRefreshing = new Command(async () => await RefreshList());
            this.navigation = navigation;
        }

        public async Task PopulateList ()
        {
          
                await RefreshList();
                var groceryList = await dbHelper.Get<Grocery>();
                var categoryList = await dbHelper.Get<Category>();
                GroceryCollection = new ObservableCollection<Grocery>(groceryList.Where(y => y.Removed == false && y.UserId == User));
                var sorted =
                    GroceryCollection.OrderBy(gc => gc.Name)
                                     .GroupBy(gc => gc.CategoryId)
                                     .Select(gc => new GroupingHelper<string, Grocery>(categoryList.FirstOrDefault(c => c.Id == gc.Key).Name, gc))
                                     .ToList();

                GroceriesGrouped.Clear();
                foreach (var item in sorted)
                {
                    GroceriesGrouped.Add(item);
                }
           
        }


        public async Task AddItemToList ()
        {
            await navigation.PushAsync(new AddEditGrocery());         
        }

        public async Task RemoveItem ()
        {
            selectedItem.Removed = true;
            await dbHelper.Update(selectedItem);
            await PopulateList();
        }

        public async Task RefreshList()
        {
           
                await dbHelper.Sync<Grocery>();
                IsRefreshing = false;
           
           
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
