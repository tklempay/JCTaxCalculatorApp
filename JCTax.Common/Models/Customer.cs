using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.Common.Models
{
    public class Customer
    {
        public Location HomeAddress { get; set; }
        public Location MailingAddress { get; set; }
        public Location BillingAddress { get; set; }
        public List<Order> OrderList { get; set; }
    }
}
