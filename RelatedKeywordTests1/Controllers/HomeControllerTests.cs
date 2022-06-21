using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RelatedKeyword.Services;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using RelatedKeyword.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using RelatedKeyword.Tests;

namespace RelatedKeyword.Controllers.Tests
{
    /*
     * ASP.NET MVC 애플리케이션에 대한 단위 테스트 만들기(C#)
     * https://docs.microsoft.com/ko-kr/aspnet/mvc/overview/older-versions-1/unit-testing/creating-unit-tests-for-asp-net-mvc-applications-cs
     */
    /// <summary>
    /// View - Controller Unit Test
    /// </summary>
    [TestClass()]
    public class HomeControllerTests
    {
        private readonly HomeController _constroller;
        private readonly SearchService _naverSearchService;
        public HomeControllerTests()
        {
            _naverSearchService = Utilities.MockSearchService();
            _constroller = new HomeController(_naverSearchService);
        }
        [TestMethod()]
        public void IndexViewTest()
        {
            var result = _constroller.Index() as ViewResult;
            Assert.IsNull(result.ViewName);
        }

        [TestMethod()]
        public async Task ResultViewTest()
        {
            var result = (await _constroller.Result("")) as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }
    }
}