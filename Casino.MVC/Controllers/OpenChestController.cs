using CasinoMVC.Core;
using CasinoMVC.Data;
using CasinoMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasinoMVC.Controllers
{
    public class OpenChestController : Controller
    {
        private readonly ILogger<OpenChestController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OpenChestController(UserManager<ApplicationUser> userManager, ILogger<OpenChestController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int id)
        {
            var dbChest = await _context.Chests.FindAsync(id);
            var chest = new ChestModel(dbChest);
            await chest.Initialize(_context.DotaItems);
            var model = new OpenChestModel { Chest = chest };

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Open(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var dbChest = await _context.Chests.FindAsync(id);

            if (user.Balance <= dbChest.Price)
            {
                var chest = new ChestModel(dbChest);
                await chest.Initialize(_context.DotaItems);
                return View("Index", new OpenChestModel { Chest = chest });
            }

            user.Balance -= dbChest.Price;

            var model = await GetWinningModelAsync(dbChest);

            user.OwnedItemIds ??= new();
            user.OwnedItemIds.Add(model.WinningItem.Id);
            user.OpenedChestAmount++;
            _context.Users.Update(user);
            var recentEntry = new RecentPlayerItemDb
            {
                User = user,
                ItemId = model.WinningItem.Id,
                Time = DateTime.Now
            };
            await _context.RecentPlayerItems.AddAsync(recentEntry);
            await _context.SaveChangesAsync();

            return View("Index", model);
        }

        private async Task<OpenChestModel> GetWinningModelAsync(ChestDbItem dbChest)
        {
            var chest = new ChestModel(dbChest);
            await chest.Initialize(_context.DotaItems);
            var model = new OpenChestModel { Chest = chest };

            var itemRandom = new Randomizer<int>(chest.ItemIds);
            var random = new Random();
            var winningIndex = random.Next(15, 35);
            int maxItemCount = winningIndex + 4;

            model.WinningIndex = winningIndex;
            for (int i = 0; i < maxItemCount; i++)  //its actually 1 less -1 0 1 => 0 1 2
            {
                var itemId = itemRandom.Next();
                var item = await _context.DotaItems.FindAsync(itemId);
                model.RouletteItems.Add(item);
                if (i == winningIndex)
                    model.WinningItem = item;
            }

            return model;
        }
    }
}
