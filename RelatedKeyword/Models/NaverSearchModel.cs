namespace RelatedKeyword.Models
{
    public class NaverSearchModel
    {
        public List<NaverSearchInfo> KeywordList { get; set; }
    }
    public class NaverSearchInfo
    {
        public string RelatedKeyword { get; set; }
    }
}
