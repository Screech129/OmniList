using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using OmniList.Models;

namespace OmniList.Helpers
{
    public class InitializerHelper
    {
        public static MobileServiceClient Client;
        public static async Task Initialize ()
        {
            try
            {
                Client = new MobileServiceClient("https://omnilist.azurewebsites.net");
                
                var store = new MobileServiceSQLiteStore("localstore.db");
                store.DefineTable<Grocery>();

                await Client.SyncContext.InitializeAsync(store);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }


        }
    }
}
