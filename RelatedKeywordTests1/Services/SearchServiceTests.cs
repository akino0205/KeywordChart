using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RelatedKeyword.Models;
using RelatedKeyword.Services;
using RelatedKeyword.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RelatedKeyword.Services.Tests
{
    [TestClass()]
    public class SearchServiceTests
    {
        private readonly SearchService _search;
        
        public SearchServiceTests()
        {            
            _search = Utilities.MockSearchService();
        }
        #region Search test
        [TestMethod()]
        public void SearchKeywordTest()
        {
            var result = _search.SearchKeyword("나이키").Result;
            Assert.IsNotNull(result.ColumnChartModel);
            Assert.IsNotNull(result.ColumnChartModel.SeriesJsonData);
            Assert.AreNotEqual(0, result.ColumnChartModel.SeriesData.Count);
            Assert.IsNotNull(result.PieChartModel);
            Assert.IsNotNull(result.PieChartModel.SeriesJsonData);
            Assert.AreNotEqual(0, result.PieChartModel.SeriesData.Count);
        }

        [TestMethod()]
        public void SearchRelatedKeywordInfoTest()
        {
            var result = _search.SearchRelatedKeywordInfo("나이키").Result;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.KeywordList);
            Assert.AreNotEqual(0, result.KeywordList.Count);            
        }
        #endregion Search test
        #region Search history test
        [TestMethod()]
        public void GetSearchHistoryTest()
        {
            List<string> result = _search.GetSearchHistory();
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);
            foreach(string item in result)
            {
                Assert.IsNotNull(item);
                Assert.AreNotEqual("", item);
            }
        }
        [TestMethod()]
        public void HasUserKeyTest()
        {
            Assert.AreEqual(true, _search.HasUserKey());
        }
        #endregion Search history test
    }
}