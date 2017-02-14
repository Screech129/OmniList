using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using OmniList.Models;
using Plugin.Connectivity;

namespace OmniList.Helpers
{
    public class DbHelper
    {
        private static MobileServiceClient Client => InitializerHelper.Client;
        private bool initialized = true;
        private static IMobileServiceSyncTable<Grocery> ToDoTable => Client.GetSyncTable<Grocery>();

        public DbHelper ()
        {
        }

        public async Task Refresh ()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    await ToDoTable.PullAsync(null, null, null, CancellationToken.None);
                }
                else
                {
                    Debug.WriteLine("Currently off line will try next refresh...");

                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw;
            }
        }

        public async Task Initialize ()
        {

            await InitializerHelper.Initialize();

            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    await ToDoTable.PullAsync(null, null, null, CancellationToken.None);
                }
                else
                {
                    Debug.WriteLine("Currently off line will try next refresh...");
                }

            }
            catch (Exception e)
            {
                initialized = false;
                Debug.WriteLine(e.ToString());
                throw;
            }
            initialized = true;
        }

        public async Task Insert (Grocery item)
        {
           if (!initialized)
                {
                    await Initialize();
                }
                await ToDoTable.InsertAsync(item);
                await Refresh();
        }

        public async Task<List<Grocery>> Get ()
        {
            if (!initialized)
            {
                await Initialize();
            }
            return await ToDoTable.ToListAsync();
        }

        public async Task DeleteItem (Grocery item)
        {
            if (!initialized)
            {
                await Initialize();
            }
            await ToDoTable.DeleteAsync(item);
            await Refresh();
        }

        public async Task Update (Grocery item)
        {
            try
            {
                if (!initialized)
                {
                    await Initialize();
                }
                item.Removed = !item.Removed;
                await ToDoTable.UpdateAsync(item);
                await Refresh();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
