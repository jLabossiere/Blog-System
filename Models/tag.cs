﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }
    }
}