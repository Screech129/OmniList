using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Newtonsoft.Json.Linq;
using OmniList.Models;
using Plugin.Connectivity;

namespace OmniList.Helpers
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class DbHelper
    {
        private static MobileServiceClient Client => InitializerHelper.Client;
        private bool initialized = true;

        public DbHelper ()
        {
        }

        public async Task Refresh<T> ()
        {
            try
            {
                if (!initialized)
                {
                    await Initialize<T>();
                }
                if (CrossConnectivity.Current.IsConnected)
                {

                    await Client.GetSyncTable<T>().PullAsync(null, null, null, CancellationToken.None);
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

        public async Task Initialize<T> ()
        {

            await InitializerHelper.Initialize();

            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    try
                    {
                        await Client.GetSyncTable<T>().PullAsync(null, null, null, CancellationToken.None);

                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                        throw;
                    }
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

        public async Task Insert<T> (T item)
        {
            if (!initialized)
            {
                await Initialize<T>();
            }
            try
            {
                await Client.GetSyncTable<T>().InsertAsync(item);
                await Refresh<T>();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw;
            }

        }

        public async Task<List<T>> Get<T> ()
        {
            if (!initialized)
            {
                await Initialize<T>();
            }

            return await Client.GetSyncTable<T>().ToListAsync();
        }

        public async Task DeleteItem<T> (T item)
        {

            if (!initialized)
            {
                await Initialize<T>();
            }
            await Client.GetSyncTable<T>().DeleteAsync(item);
            await Refresh<T>();
        }

        public async Task Update<T> (T item)
        {
            try
            {
                if (!initialized)
                {
                    await Initialize<T>();
                }
                await Client.GetSyncTable<T>().UpdateAsync(item);
                await Refresh<T>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
