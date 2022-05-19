namespace RelatedKeyword.Models
{
    public class NaverSearchModel
    {
        /// <summary>
        /// 검색어 이력
        /// </summary>
        public List<string> HistoryKeywords { get; set; }
        public List<NaverSearchInfo> KeywordList { get; set; }
    }
    public class NaverSearchInfo
    {
        /// <summary>
        /// 연관키워드
        /// </summary>
        public string RelKeyword { get; set; }
        /// <summary>
        /// 월간검색수 - 모바일
        /// </summary>
        public object monthlyMobileQcCnt { get; set; }
        public int monthlyMobileQcCntInt { get => monthlyMobileQcCnt.ToString().Contains("<") ? 10 : int.Parse(monthlyMobileQcCnt.ToString()); }
        /// <summary>
        /// 월간검색수 - PC
        /// </summary>
        public object monthlyPcQcCnt { get; set; }
        public int monthlyPcQcCntInt { get => monthlyPcQcCnt.ToString().Contains("<") ? 10 : int.Parse(monthlyPcQcCnt.ToString()); }
        /// <summary>
        /// 월간검색수 - PC + 모바일
        /// </summary>
        public int monthlyQcCnt { get => monthlyMobileQcCntInt + monthlyPcQcCntInt; }
    }
}
