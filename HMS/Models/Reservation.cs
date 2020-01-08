using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        [DataType(DataType.Text)]
        public string Description { get; set; }
        public DateTime DateIn { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOut { get; set; }
        public int Customer_id { get; set; }
        public int Room_id { get; set; }
        public virtual Account Customer{get;set; }
        public virtual RoomViewModel Room { get; set; }
    }
}
