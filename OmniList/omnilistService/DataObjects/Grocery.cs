﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Tables;

namespace omnilistService.DataObjects
{
    public class Grocery:EntityData
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public bool Removed { get; set; }
        public string CategoryId { get; set; }
        public virtual Category Category { get; set; }

    }
}