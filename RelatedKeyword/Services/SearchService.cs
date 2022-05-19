using Microsoft.Extensions.Options;
using RelatedKeyword.Models;
using System.Security.Cryptography;
using System.Text;

namespace RelatedKeyword.Services
{
    public class SearchService
    {
        private readonly NaverSearchAPISettings _naverSettings;
        private readonly HttpClient _client;
        private readonly UserContext _userContext;
        private readonly string _mbKey = "temp";
        public SearchService(IOptions<NaverSearchAPISettings> naverSettings, HttpClient client, UserContext userContext)
        {
            _naverSettings = naverSettings.Value;
            _client = client;
            _userContext = userContext;
        }
        #region Search
        public async Task<NaverSearchModel> SearchRelatedKeywordInfo(string keyword)
        {
            NaverSearchModel result = null; 
            _client.BaseAddress = new Uri($"{_naverSettings.SiteUrl}{_naverSettings.Resource}");
            var timestamp = getTimestamp().ToString();
            var signature = generateSignature(timestamp, "GET", _naverSettings.Resource);
            var param = getParam(keyword);
            _client.DefaultRequestHeaders.Add("X-API-KEY", _naverSettings.AccessLicense);
            _client.DefaultRequestHeaders.Add("X-Customer", _naverSettings.CUSTOMER_ID);
            _client.DefaultRequestHeaders.Add("X-Timestamp", timestamp);
            _client.DefaultRequestHeaders.Add("X-Signature", signature);
            var httpResponseMessage = await _client.GetAsync($"{_naverSettings.SiteUrl}{_naverSettings.Resource}?{param}");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                result = await httpResponseMessage.Content.ReadFromJsonAsync<NaverSearchModel>();
                SaveSearchHistory(keyword, result);
                result.HistoryKeywords = GetSearchHistory();
            }
            return result;
        }
        private long getTimestamp()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }
        private string generateSignature(string timestamp, string method, string resource)
        {
            HMACSHA256 HMAC = new HMACSHA256(Encoding.UTF8.GetBytes(_naverSettings.SecretKey));
            return Convert.ToBase64String(HMAC.ComputeHash(Encoding.UTF8.GetBytes(timestamp + "." + method + "." + resource)));
        }
        private string getParam(string keyword)
        {
            var paramDic = new Dictionary<string, string>();
            paramDic.Add("siteId", "");
            paramDic.Add("biztpId","");
            paramDic.Add("hintKeywords", keyword);
            paramDic.Add("event","");
            paramDic.Add("month","");
            paramDic.Add("showDetail","1");
            //key=value&key2=value2
            return String.Join("&", paramDic.Select(kvp => kvp.Key + "=" + kvp.Value));
        }
        #endregion Search
        #region search history
        //TODO : 사용자에 따라 가져오기
        public List<string> GetSearchHistory()
        => _userContext.Searchhistories.Any(a => a.UserKey.Equals(_mbKey))
            ? _userContext.Searchhistories.Where(w => w.UserKey.Equals(_mbKey)).OrderByDescending(o => o.Date).Take(10).Select(s => s.Keyword).ToList()
            : null;
        private void SaveSearchHistory(string keyword, NaverSearchModel data)
        {
            if (data is null) return;

            List<Searchresult> results = new();
            try
            {
                foreach (var info in data.KeywordList.Select((item, index) => new { Item = item, Idx = index }))
                {
                    results.Add(new Searchresult()
                    {
                        Seq = info.Idx,
                        ResultKey = DateTime.Now.ToString(),
                        RelatedKeyword = info.Item.RelKeyword,
                        PcCnt = info.Item.monthlyPcQcCntInt,
                        MobileCnt = info.Item.monthlyMobileQcCntInt,
                        SumCnt = info.Item.monthlyQcCnt
                    });
                }
                _userContext.Searchhistories.Add(new Searchhistory()
                {
                    UserKey = _mbKey, //TODO : 사용자키 넣기
                    Keyword = keyword,
                    Result = data.ToString(),
                    Date = DateTime.Now,
                    Searchresults = results
                });
                _userContext.SaveChanges();
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        #endregion search history
    }
}
