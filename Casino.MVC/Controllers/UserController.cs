using CasinoMVC.Core;
using CasinoMVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using CasinoMVC.Data;

namespace CasinoMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public UserController(SignInManager<ApplicationUser> signInManager,
           UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> SignIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(InputLoginModel input)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(input.Email, input.Password, input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View();
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(InputRegisterModel input)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = input.Email, Email = input.Email, Balance = 0f };
                var result = await _userManager.CreateAsync(user, input.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
           
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> SellAsync(int id, string previouslink = null)
        {
            previouslink = previouslink ?? Url.Content("~/");
            var user = await _userManager.GetUserAsync(User);
            if(!user.OwnedItemIds.Contains(id))
            {
                return RedirectToAction("Index","Home");
            }
            var item = await _context.DotaItems.FindAsync(id);
            if(item is null)
            {
                return RedirectToAction("Index", "Home");
            }

            user.OwnedItemIds.Remove(id);
            user.Balance += item.Price;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return LocalRedirect(previouslink);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> SellAllAsync(string previouslink = null)
        {
            previouslink = previouslink ?? Url.Content("~/");
            var user = await _userManager.GetUserAsync(User);

            foreach (var id in user.OwnedItemIds)
            {
                var item = await _context.DotaItems.FindAsync(id);
                user.Balance += item.Price;
            }

            user.OwnedItemIds.Clear();
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return LocalRedirect(previouslink);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CurrentAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var items = new List<DotaItemModel>();
            foreach (var id in user.OwnedItemIds)
            {
                items.Add(await _context.DotaItems.FindAsync(id));
            }
            return View(items);
        }
    }
}
