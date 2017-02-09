using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Mobile.Server;

namespace omnilistService.DataObjects
{
    public class Grocery:EntityData
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public Guid UserId { get; set; }
        public bool Removed { get; set; }

    }
}