using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class InvoiceViewModel
    {
        public int Id { get; set; }
        public Account Customer { get; set; }
        public List<Service> Services { get; set; }
        public List<Reservation> Rooms { get; set; }
        public int Cost { get; set; }
    }
}
