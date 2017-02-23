using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using Xamarin.Auth;
using Xamarin.Forms;

namespace OmniList.Helpers
{
    public class AuthStore
    {
        private static string TokenKeyName = "token";

        public static void CacheAuthenticationToken(MobileServiceUser user)
        {
            var account = new Account(user.UserId);

            account.Properties.Add(TokenKeyName,user.MobileServiceAuthenticationToken);
            GetAccountStore().Save(account, App.AppName);

            Debug.WriteLine($"Cached auth token: {user.MobileServiceAuthenticationToken}");
        }
        public static async Task<MobileServiceUser> GetUserFromCache ()
        {
            var accounts = GetAccountStore().FindAccountsForService(App.AppName);

            if (accounts == null)
            {
                return null;
            }
            try
            {
                foreach (var acct in accounts)
                {
                    string token;

                    if (acct.Properties.TryGetValue("token", out token))
                    {
                        if (!IsTokenExpired(token))
                        {
                            Debug.WriteLine("TEST");
                            await InitializerHelper.Initialize();
                            InitializerHelper.Client.CurrentUser = new MobileServiceUser(acct.Username)
                            {
                                MobileServiceAuthenticationToken = token
                            };
                            return InitializerHelper.Client.CurrentUser;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
            

            return null;
        }

        public static bool IsUserLoggedIn()
        {
            var accounts = GetAccountStore().FindAccountsForService(App.AppName);

            if (accounts == null)
            {
                return false;
            }
            try
            {
                foreach (var acct in accounts)
                {
                    string token;

                    if (acct.Properties.TryGetValue("token", out token))
                    {
                        if (!IsTokenExpired(token))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }

            return false;
        }

        public static void DeleteTokenCache ()
        {
            var accountStore = GetAccountStore();
            var account = accountStore.FindAccountsForService(App.AppName).FirstOrDefault();
            if (account != null)
            {
                accountStore.Delete(account, App.AppName);
            }
        }

        private static AccountStore GetAccountStore()
        {
            
            return DependencyService.Get<IAuthenticate>().GetAccountStore();
        }

        private static bool IsTokenExpired (string token)
        {
            // Get just the JWT part of the token (without the signature).
            var jwt = token.Split(new Char[] { '.' })[1];

            // Undo the URL encoding.
            jwt = jwt.Replace('-', '+').Replace('_', '/');
            switch (jwt.Length % 4)
            {
                case 0: break;
                case 2: jwt += "=="; break;
                case 3: jwt += "="; break;
                default:
                    throw new ArgumentException("The token is not a valid Base64 string.");
            }

            // Convert to a JSON String
            var bytes = Convert.FromBase64String(jwt);
            string jsonString = UTF8Encoding.UTF8.GetString(bytes, 0, bytes.Length);

            // Parse as JSON object and get the exp field value,
            // which is the expiration date as a JavaScript primative date.
            JObject jsonObj = JObject.Parse(jsonString);
            var exp = Convert.ToDouble(jsonObj["exp"].ToString());

            // Calculate the expiration by adding the exp value (in seconds) to the
            // base date of 1/1/1970.
            DateTime minTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var expire = minTime.AddSeconds(exp);
            return (expire < DateTime.UtcNow);
        }
    }
}
