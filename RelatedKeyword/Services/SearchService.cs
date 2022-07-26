using Microsoft.Extensions.Options;
using RelatedKeyword.Models;
using RelatedKeyword.Models.Chart;
using RelatedKeywordLibrary.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace RelatedKeyword.Services
{
    public class SearchService
    {
        private readonly NaverSearchAPISettings _naverSettings;
        private readonly HttpClient _client;
        private readonly UserContext _userContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int _topCount = 10;

        public SearchService(IOptions<NaverSearchAPISettings> naverSettings, HttpClient client, 
            UserContext userContext, IHttpContextAccessor httpContextAccessor)
        {
            _naverSettings = naverSettings.Value;
            _client = client;
            _userContext = userContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<NaverSearchModel> SearchKeyword(string keyword)
        {
            NaverSearchModel result = await SearchRelatedKeywordInfo(keyword);
            FillPieChartModel(ref result);
            FillColumnChartModel(ref result);
            return result;
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
        #region Search history
        //TODO : 사용자에 따라 가져오기
        public List<string> GetSearchHistory()
        {
            var userkeyFormIP = GetUserKeyFromIp();
            return HasUserKey(userkeyFormIP)
            ? _userContext.Searchhistories.Where(w => w.UserKey.Equals(userkeyFormIP))
            .OrderByDescending(o => o.Date).Take(10).Select(s => s.Keyword).ToList()
            : null;
        }
        public bool HasUserKey(int userkeyFormIP)
         => _userContext.Searchhistories.Any(a => a.UserKey.Equals(userkeyFormIP));
        public int GetUserKeyFromIp()
        {
            var clientIP = GetIP();
            return _userContext.Userinfos.Any(a => a.Ip.Equals(clientIP))
               ? _userContext.Userinfos.Where(w => w.Ip.Equals(clientIP)).FirstOrDefault()?.UserKey ?? 0
               : CreateUserKeyWithIP(clientIP);
        }
        private string GetIP()
         => _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress.ToString();
        private int CreateUserKeyWithIP(string clientIP)
        {
            _userContext.Userinfos.Add(new Userinfo()
            {
                Ip = clientIP,
                CreateDate = DateTime.Now
            });
            _userContext.SaveChanges();
            return _userContext.Userinfos.Where(w => w.Ip.Equals(clientIP)).FirstOrDefault()?.UserKey ?? 0;
        }
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
                    UserKey = GetUserKeyFromIp(), //TODO : 사용자키 넣기
                    Keyword = keyword,
                    Result = data.ToString(),
                    Date = DateTime.Now,
                    Searchresults = results
                });
                _userContext.SaveChanges();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        #endregion Search history
        #region Search Chart
        private void FillPieChartModel(ref NaverSearchModel model)
        {
            PieChartModel pie = new();
            //차트 제목
            pie.Title = $"Top{_topCount} 연관검색어 비중";
            var topItems = model.KeywordList?.OrderByDescending(o => o.monthlyQcCnt).Take(GetTakeCount(model));
            decimal topTotalCount = topItems.Sum(s => s.monthlyQcCnt);
            var dataModel = new List<PieChartSeriesDataModel>();
            foreach (var item in topItems)
            {
                dataModel.Add(new() { 
                    Name = $"{item.RelKeyword} : {item.monthlyQcCnt}건",
                    Y = item.monthlyQcCnt/ topTotalCount * 100m
                });
            }
            pie.SeriesData = dataModel;
            pie.SeriesJsonData = GetJsonData(dataModel);
            model.PieChartModel = pie;
        }
        private void FillColumnChartModel(ref NaverSearchModel model)
        {
            StackedColumnDataModel col = new();
            //차트 제목
            col.Title = $"Top{_topCount} PC/모바일 연관검색어 건수";
            var topItems = model.KeywordList?.OrderByDescending(o => o.monthlyQcCnt).Take(GetTakeCount(model));

            //x축 종류
            col.xAxisCategories = topItems.Select(s => s.RelKeyword).ToList();
            col.xAxisCategoriesJsonData = GetJsonData(col.xAxisCategories);

            var dataModel = new List<StackedColumnSeriesModel>();
            dataModel.Add(new()
            {
                Name = "모바일",
                Data = topItems.Select(s => s.monthlyMobileQcCntInt).ToList()
            });
            dataModel.Add(new()
            {
                Name = "PC",
                Data = topItems.Select(s => s.monthlyPcQcCntInt).ToList()
            });

            col.SeriesData = dataModel;
            col.SeriesJsonData = GetJsonData(dataModel);
            model.ColumnChartModel = col;
        }
        private int GetTakeCount(NaverSearchModel model)
         => model.KeywordList.Count > _topCount ? _topCount : model.KeywordList.Count;
        private string GetJsonData(object data)
         => JsonSerializer.Serialize(data, new JsonSerializerOptions
         {
             Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
         });
        #endregion Search Chart
    }
}
