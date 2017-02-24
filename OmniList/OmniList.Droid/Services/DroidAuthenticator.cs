using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.WindowsAzure.MobileServices;
using OmniList.Droid.Services;
using OmniList.Helpers;
using Xamarin.Auth;

[assembly: Xamarin.Forms.Dependency(typeof(DroidAuthenticator))]
namespace OmniList.Droid.Services
{
    public class DroidAuthenticator: IAuthenticate
    {
        private Context context;
        public void Init (Context context)
        {
            this.context = context;
        }

        public async Task LoginAsync (MobileServiceClient client, MobileServiceAuthenticationProvider provider)
        {
            await client.LoginAsync(context, provider);

        }

        public async Task LogoutAsync ()
        {
            AuthStore.DeleteTokenCache();
            await InitializerHelper.Client.LogoutAsync();
        }
    }
}