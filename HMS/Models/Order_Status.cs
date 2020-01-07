using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class Order_Status
    {
        private readonly List<string> Status;
        public Order_Status()
        {
            Status = new List<string>() {"SHIPPING", "DELIVERED", "PENDING","DONE"};
        }

        public string getType(int num)
        {
            return Status[num];
        }
    }
}
