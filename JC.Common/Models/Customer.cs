using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.Common.Models
{
    public class Customer
    {
        public string Name { get; set; }
        public Location HomeAddress { get; set; }
        public Location MailingAddress { get; set; }
        public Location BillingAddress { get; set; }
        public Order Order { get; set; }
    }
}
