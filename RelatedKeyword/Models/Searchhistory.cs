using System;
using System.Collections.Generic;

#nullable disable

namespace RelatedKeyword.Models
{
    public partial class Searchhistory
    {
        public Searchhistory()
        {
            Searchresults = new HashSet<Searchresult>();
        }

        public int HistoryIndex { get; set; }
        public string UserKey { get; set; }
        public string Keyword { get; set; }
        public string Result { get; set; }
        public DateTime? Date { get; set; }

        public virtual ICollection<Searchresult> Searchresults { get; set; }
    }
}
