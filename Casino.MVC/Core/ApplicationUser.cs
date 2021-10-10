using CasinoMVC.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasinoMVC.Core
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public List<int> OwnedItemIds { get; set; }

        public float Balance { get; set; }

        public int OpenedChestAmount { get; set; }

        public ApplicationUser() :base()
        {
            OwnedItemIds = new();
        }
    }
}
