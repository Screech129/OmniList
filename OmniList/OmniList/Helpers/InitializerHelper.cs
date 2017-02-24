using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using OmniList.Models;
using Xamarin.Forms;

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

        public static async Task<bool> LoginAsync(MobileServiceAuthenticationProvider provider)
        {
            await Initialize();
            var authenticator = DependencyService.Get<IAuthenticate>();
            try
            {
                await authenticator.LoginAsync(Client, provider);
                var user = Client.CurrentUser;
                AuthStore.CacheAuthenticationToken(user);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return false;
            }
        }

        private static List<AppServiceIdentity> identities = null;

        public static async Task<AppServiceIdentity> GetIdentityAsync ()
        {
            if (Client.CurrentUser?.MobileServiceAuthenticationToken == null)
            {
                throw new InvalidOperationException("Not Authenticated");
            }

            if (identities == null)
            {
                identities = await Client.InvokeApiAsync<List<AppServiceIdentity>>("/.auth/me");
            }

            if (identities.Count > 0)
                return identities[0];
            return null;
        }

      
    }
}
