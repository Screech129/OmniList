using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using OmniList.Helpers;
using OmniList.UWP.Services;
using Xamarin.Auth;

[assembly:Xamarin.Forms.Dependency(typeof(UWPAuthenticator))]
namespace OmniList.UWP.Services
{
    public class UWPAuthenticator:IAuthenticate
    {
        public async Task LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider)
        {
            await client.LoginAsync(provider);
        }

        public async Task LogoutAsync ()
        {
            AuthStore.DeleteTokenCache();
            await InitializerHelper.Client.LogoutAsync();
        }
    }
}
