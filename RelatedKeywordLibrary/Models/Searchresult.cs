using System;
using System.Collections.Generic;

#nullable disable

namespace RelatedKeywordLibrary.Models
{
    public partial class Searchresult
    {
        public int Seq { get; set; }
        public string ResultKey { get; set; }
        public string RelatedKeyword { get; set; }
        public int PcCnt { get; set; }
        public int MobileCnt { get; set; }
        public int SumCnt { get; set; }
        public int HistoryIndex { get; set; }

        public virtual Searchhistory HistoryIndexNavigation { get; set; }
    }
}
