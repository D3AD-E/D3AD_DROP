using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasinoMVC.Models
{
    public class DotaOwnerItemModel : DotaItemModel
    {
        public string OwnerName { get; set; }
        public Guid OwnerId { get; set; }
        public DotaOwnerItemModel(DotaItemModel other) : base(other)
        {

        }
    }
}
