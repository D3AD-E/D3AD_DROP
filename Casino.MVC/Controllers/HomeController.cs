using Casino.Crawler;
using CasinoMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CasinoMVC.Data;
using CasinoMVC.Core;

namespace CasinoMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> IndexAsync()
        {
            //Crawler crawler = new Crawler();
            //await crawler.PopulateDatabase(_context, 3, 6);
            
            var toret = new IndexItemsModel
            {
                Chests = _context.Chests.Select(x => new ChestModel(x, _context)).ToList()
            };
            //var dotaItems = items.Cast<DotaItemModel>().ToList();
            return View(toret);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
