using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace OmniList.Models
{
    public class Grocery
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public Guid UserId { get; set; }
        public bool Removed { get; set; }


        [Version]
        public string AzureVersion { get; set; }

        [CreatedAt]
        public DateTimeOffset? AzureCreatedDate { get; set; }

        [UpdatedAt]
        public DateTimeOffset? AzureUpdatedDate { get; set; }

    }
}
