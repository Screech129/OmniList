using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Tables;
using Microsoft.WindowsAzure.MobileServices;

namespace omnilistService.DataObjects
{
    public class Category:EntityData
    {
        public Category()
        {
            Groceries = new List<Grocery>();
        }
        public string Name { get; set; }
        public virtual ICollection< Grocery> Groceries { get; set; }

    }
}