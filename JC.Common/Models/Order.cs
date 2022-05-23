using System.Collections.Generic;
using System.Linq;

namespace JC.Common.Models
{
    public class Order
    {
        public Customer Customer { get; set; }

        public List<Item> Items { get; set; }

        public decimal OrderTotal => Enumerable.Sum(Items.Select(x => x.Price * x.Quantity));
    }
}
