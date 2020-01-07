using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class ServiceType
    {
        private readonly List<string> Types;
        public ServiceType()
        {
            Types = new List<string>() { "Foods", "Drinks", "Clean Room", "Clean Clothes", "Table Reservation" };
        }

        public string getType(int num)
        {
            return Types[num];
        }
    }
}
