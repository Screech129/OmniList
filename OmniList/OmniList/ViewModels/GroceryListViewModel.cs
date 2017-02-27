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
using Xamarin.Forms;

namespace OmniList.ViewModels
{
    public class GroceryListViewModel: INotifyPropertyChanged
    {
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

        private string newItem;
        public string NewItem
        {
            get
            {
                return newItem;
            }
            set
            {
                newItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NewItem)));

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

        public GroceryListViewModel()
        {
            AddItem = new Command(async () => await AddItemToList());
            OnRefreshing = new Command(async () => await RefreshList());
        }

        public async Task PopulateList ()
        {

            await RefreshList();
            var groceryList = await dbHelper.Get<Grocery>();
            GroceryCollection = new ObservableCollection<Grocery>(groceryList.Where(x => x.Removed == false));
        }


        public async Task AddItemToList ()
        {
            var item = new Grocery()
            {
                Removed = false,
                Name = NewItem,
                CategoryId = "a11654fd-3652-45fc-9dfc-14202054ccca",
                UserId = User

            };
            try
            {
                await dbHelper.Insert(item);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw;
            }
           
            await PopulateList();
        }

        public async Task RemoveItem ()
        {
            selectedItem.Removed = true;
            await dbHelper.Update(selectedItem);
            await PopulateList();
        }

        public async Task RefreshList()
        {
            await dbHelper.Refresh<Grocery>();
            IsRefreshing = false;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
