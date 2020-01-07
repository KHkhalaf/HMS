using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class Room_Status
    {
        private readonly List<string> Status;
        public Room_Status()
        {
            Status = new List<string>() { "Reserved", "Available", "Maintenance" };
        }

        public string getStatus(int num)
        {
            return Status[num];
        }
    }
}
