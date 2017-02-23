using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.WindowsAzure.MobileServices;
using OmniList.Helpers;
using OmniList.UWP;
using Xamarin.Auth;

namespace OmniList.UWP
{

    public sealed partial class MainPage
    {
        public MainPage ()
        {
            this.InitializeComponent();
            LoadApplication(new OmniList.App());
        }

      
       
    }
}
