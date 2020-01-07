using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class RoomType
    {
        private readonly List<string> Types;
        public RoomType()
        {
            Types = new List<string>() { "Single Bed","Double Bed","Wifi","Deluxe","Luxury"};
        }

        public string getType(int num)
        {
             return Types[num]; 
        }

    }
}
