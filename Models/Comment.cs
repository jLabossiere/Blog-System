using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public string Email { get; set; }
        public bool Accepted { get; set; }
        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }

    }
}