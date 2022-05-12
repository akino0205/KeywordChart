using Microsoft.Extensions.Options;
using RelatedKeyword.Models;
using System.Security.Cryptography;
using System.Text;

namespace RelatedKeyword.Services
{
    public class NaverSearchService
    {
        private readonly NaverSearchAPISettings _naverSettings;
        private readonly HttpClient _client;
        public NaverSearchService(IOptions<NaverSearchAPISettings> naverSettings, HttpClient client)
        {
            _naverSettings = naverSettings.Value;
            _client = client;

        }
        public async Task<NaverSearchModel> SearchRelatedKeywordInfo(string keyword)
        {
            NaverSearchModel response = null; 
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
                response = await httpResponseMessage.Content.ReadFromJsonAsync<NaverSearchModel>();

            return response;
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
    }
}
