using RelatedKeyword.Models.Chart;

namespace RelatedKeyword.Models
{
    public class NaverSearchModel
    {
        /// <summary>
        /// 검색어 이력
        /// </summary>
        public List<string> HistoryKeywords { get; set; }
        public List<NaverSearchInfo> KeywordList { get; set; }
        /// <summary>
        /// 연관검색어 비중(원차트)
        /// </summary>
        public PieChartModel PieChartModel { get; set; } = new();
        public StackedColumnDataModel ColumnChartModel { get; set; } = new();
    }
    public class NaverSearchInfo
    {
        /// <summary>
        /// 연관키워드
        /// </summary>
        public string RelKeyword { get; set; }
        /// <summary>
        /// 계산된 월간검색수 - 기타 수 계산을 위함.
        /// </summary>
        public int monthlyDirectQcCntInt { get; set; }
        /// <summary>
        /// 월간검색수 - 모바일
        /// </summary>
        public object monthlyMobileQcCnt { get; set; }
        public int monthlyMobileQcCntInt { 
            get => monthlyMobileQcCnt is null ? 0 : 
                monthlyMobileQcCnt.ToString().Contains("<") ? 10 : int.Parse(monthlyMobileQcCnt.ToString()); 
        }
        /// <summary>
        /// 월간검색수 - PC
        /// </summary>
        public object monthlyPcQcCnt { get; set; }
        public int monthlyPcQcCntInt { 
            get => monthlyPcQcCnt is null ? 0 : 
                monthlyPcQcCnt.ToString().Contains("<") ? 10 : int.Parse(monthlyPcQcCnt.ToString()); 
        }
        /// <summary>
        /// 월간검색수 - PC + 모바일
        /// </summary>
        public int monthlyQcCnt { get => monthlyDirectQcCntInt + monthlyMobileQcCntInt + monthlyPcQcCntInt; }
    }
}
