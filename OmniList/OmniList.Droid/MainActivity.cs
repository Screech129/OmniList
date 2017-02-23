﻿using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using Microsoft.WindowsAzure.MobileServices;
using OmniList.Droid;
using OmniList.Droid.Services;
using OmniList.Helpers;
using Xamarin.Auth;
using Xamarin.Forms;

namespace OmniList.Droid
{
    [Activity(Label = "OmniList", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        protected override void OnCreate (Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
         
            CurrentPlatform.Init();
            ((DroidAuthenticator) DependencyService.Get<IAuthenticate>()).Init(this);

            LoadApplication(new App());
        }

       
    }
}

