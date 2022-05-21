using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.Common.Models
{
    public class Order
    {
        public Customer Customer { get; set; }

        public List<Item> Items { get; set; }
    }
}
