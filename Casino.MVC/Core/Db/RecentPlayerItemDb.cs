using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasinoMVC.Core
{
    public class RecentPlayerItemDb
    {
        public int Id { get; set; }
        public int ItemId { get; set; }

        public Guid UserId { get; set; }

        public DateTime Time { get; set; }
    }
}
