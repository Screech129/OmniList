﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using OmniList.Models;
using Xamarin.Auth;

namespace OmniList.Helpers
{
    public interface IAuthenticate
    {
        Task LoginAsync (MobileServiceClient client, MobileServiceAuthenticationProvider provider);
        //Task LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider, User user);

        //Task<AppServiceIdentity> GetIdentityAsync();

        AccountStore GetAccountStore ();

        Task LogoutAsync ();


    }
}