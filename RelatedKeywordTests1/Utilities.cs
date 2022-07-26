using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using RelatedKeyword.Models;
using RelatedKeyword.Services;
using RelatedKeywordLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RelatedKeyword.Tests
{
    public class Utilities
    {

        public static SearchService MockSearchService()
        {
            var naverSettingsOptions = MocknaverSettingsOptions();
            var client = new HttpClient();
            var userContext = GetUserContext();
            var httpContextAccessor = MockHttpContextAccessor();
            return new SearchService(naverSettingsOptions, client, userContext, httpContextAccessor);
        }
        public static IOptions<NaverSearchAPISettings> MocknaverSettingsOptions()
        {
            NaverSearchAPISettings naverSettings = new NaverSearchAPISettings()
            {
                SiteUrl = "https://api.naver.com",
                Resource = "/keywordstool",
                CUSTOMER_ID = "2551511",
                AccessLicense = "010000000084e259bc2fb1b3ac6147cb5ac02bb791232e17a7ac020339783b877bfdad90be",
                SecretKey = "AQAAAACfhGte7JdWdUQc5fyaARy5A3RmDs+00ACO3NO4smNDGw=="
            };
            var mockIOption = new Mock<IOptions<NaverSearchAPISettings>>();
            mockIOption.Setup(ap => ap.Value).Returns(naverSettings);
            return mockIOption.Object;
        }

        public static UserContext GetUserContext()
        {
            var contextOptions = new DbContextOptionsBuilder<UserContext>()
                .UseMySQL("Server=localhost;User=norah;Database=userdb;Port=3306;Password=userNorah20@@;")
                .Options;

            return new UserContext(contextOptions);
        }
        public static IHttpContextAccessor MockHttpContextAccessor()
        {
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(req => req.HttpContext.Connection.RemoteIpAddress)
                .Returns(IPAddress.Parse("::1"));
            return mockHttpContextAccessor.Object;
        }
    }
}
