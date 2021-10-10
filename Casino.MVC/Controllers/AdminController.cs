using Casino.Crawler;
using CasinoMVC.Core;
using CasinoMVC.Data;
using CasinoMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasinoMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly ApplicationDbContext _context;

        public AdminController(ILogger<AdminController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Scrape()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ScrapeAsync(AdminScraperModel input)
        {
            var crawler = new ItemScraper();
            var result = await crawler.PopulateDatabaseAsync(_context, input.PageAmount, input.StartingPageIndex, input.Rarity);

            return View("ScrapeResult", result);
        }
    }
}
