using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Newtonsoft.Json.Linq;
using OmniList.Models;

namespace OmniList.Helpers
{
    public class MobileServiceSyncHandler : IMobileServiceSyncHandler
    {
        public Task OnPushCompleteAsync (MobileServicePushCompletionResult result)
        {
            foreach (var error in result.Errors)
            {
                Debug.WriteLine(error);
            }

            return Task.FromResult(0);
        }

        public async Task<JObject> ExecuteTableOperationAsync (IMobileServiceTableOperation operation)
        {
            JObject result = null;
            MobileServicePreconditionFailedException conflictError = null;
            Debug.WriteLine("Beginning Sync");
            do
            {
                try
                {
                    result = await operation.ExecuteAsync();
                }
                catch (MobileServicePreconditionFailedException e)
                {
                    conflictError = e;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }

                if (conflictError != null)
                {

                    JObject serverItem = conflictError.Value;

                    if (serverItem == null)
                    {
                        serverItem = (JObject)(await operation.Table.LookupAsync((string)operation.Item[MobileServiceSystemColumns.Id]));
                    }


                    serverItem[MobileServiceSystemColumns.Version] = operation.Item[MobileServiceSystemColumns.Version];
                }
            } while (conflictError != null);

            return result;
        }
    }
}
