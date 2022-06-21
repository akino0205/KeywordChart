using System;
using System.Collections.Generic;

#nullable disable

namespace RelatedKeyword.Models
{
    public partial class Userinfo
    {
        public int UserKey { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string Ip { get; set; }
        public DateTime? EditDate { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
