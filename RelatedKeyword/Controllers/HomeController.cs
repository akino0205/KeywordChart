using Microsoft.AspNetCore.Mvc;
using RelatedKeyword.Models;
using RelatedKeyword.Services;
using System.Diagnostics;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {        
        private readonly ILogger<HomeController> _logger;
        private readonly SearchService _naverSearchService;

        public HomeController(ILogger<HomeController> logger, SearchService naverSearchService)
        {
            _logger = logger;
            _naverSearchService = naverSearchService;
        }

        public IActionResult Index()
        {
            NaverSearchModel model = new NaverSearchModel()
            { HistoryKeywords = _naverSearchService.GetSearchHistory()};
            return View(model);
        }
        public async Task<IActionResult> Result(string searchKeyword)
        {
            NaverSearchModel model = new();
            if (!String.IsNullOrEmpty(searchKeyword))
                model = (_naverSearchService.SearchKeyword(searchKeyword)).Result;

            return View("Index", model);
        }

        public IActionResult Privacy()
        {
            return View();
        }       
    }
}