using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class Food
    {
        public int Id { get; set; }
        [DisplayName("Food Name")]
        public string FoodName { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string image { get; set; }
    }
}
