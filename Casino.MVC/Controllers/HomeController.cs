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
            var dBitems = _context.RecentPlayerItems.OrderByDescending(x => x.Time).Take(14);
            var items = new List<DotaOwnerItemModel>();
            foreach (var record in dBitems)
            {
                var item = await _context.DotaItems.FindAsync(record.ItemId);
                items.Add(new DotaOwnerItemModel(item)
                {
                    OwnerId = record.UserId,
                    OwnerName = (await _context.Users.FindAsync(record.UserId)).UserName
                });
            }
            var toret = new IndexItemsModel
            {
                Chests = _context.Chests.ToList(),
                RecentItems = items
            };
            return View(toret);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("404")]
        public IActionResult PageNotFound()
        {
            return View("404");
        }
    }
}
