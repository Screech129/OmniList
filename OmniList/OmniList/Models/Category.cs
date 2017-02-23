using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniList.Models
{
    public class Category
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public ICollection<Grocery> Groceries { get; set; }

        [Version]
        public string AzureVersion { get; set; }
        [CreatedAt]
        public DateTimeOffset? AzureCreatedDate { get; set; }

        [UpdatedAt]
        public DateTimeOffset? AzureUpdatedDate { get; set; }

      
    }
}
