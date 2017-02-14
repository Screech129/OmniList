using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Microsoft.WindowsAzure.MobileServices;
using OmniList.Helpers;

namespace OmniList.Droid
{
    [Activity(Label = "OmniList", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IAuthenticate
    {
        private MobileServiceUser user;
        protected override void OnCreate (Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            App.Init((IAuthenticate)this);

            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();


            LoadApplication(new App());
        }

        public async Task<bool> Authenticate()
        {
            var success = false;
            var message = string.Empty;

            try
            {
                await InitializerHelper.Initialize();
                user = await InitializerHelper.Client.LoginAsync(this, MobileServiceAuthenticationProvider.Google);
                if (user != null)
                {
                    message = string.Format($"Your are now signed in as {user.UserId}");
                    success = true;
                }
            }
            catch (Exception e)
            {
                message = e.Message;
                
            }

            var builder = new AlertDialog.Builder(this);
            builder.SetMessage(message);
            builder.SetTitle("Sign-in result");
            builder.Create().Show();

            return success;
        }
    }
}

