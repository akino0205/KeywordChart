using Microsoft.AspNetCore.Mvc;
using RelatedKeyword.Models;
using RelatedKeyword.Services;
using System.Diagnostics;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NaverSearchService _naverSearchService;

        public HomeController(ILogger<HomeController> logger, NaverSearchService naverSearchService)
        {
            _logger = logger;
            _naverSearchService = naverSearchService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Result(string searchKeyword)
        {
            NaverSearchModel model = new();
            if (!String.IsNullOrEmpty(searchKeyword))
            {
                model = (_naverSearchService.SearchRelatedKeywordInfo(searchKeyword)).Result;
            }

            return View("Index", model);
        }

        public IActionResult Privacy()
        {
            return View();
        }       
    }
}