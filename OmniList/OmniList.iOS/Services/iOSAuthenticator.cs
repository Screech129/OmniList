using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using OmniList.Helpers;
using OmniList.iOS.Services;
using UIKit;
using Xamarin.Auth;

[assembly: Xamarin.Forms.Dependency(typeof(iOSAuthenticator))]
namespace OmniList.iOS.Services
{
    public class iOSAuthenticator:IAuthenticate
    {
        public UIViewController RootView => UIApplication.SharedApplication.KeyWindow.RootViewController;
        public async Task LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider)
        {
            await client.LoginAsync(RootView, provider);
        }

        public AccountStore GetAccountStore ()
        {
            return AccountStore.Create();
        }

        public async Task LogoutAsync ()
        {
            AuthStore.DeleteTokenCache();
            await InitializerHelper.Client.LogoutAsync();
        }
    }
}
