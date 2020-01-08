using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class Service
    {
        [DisplayName("Service Id")]
        public int Id { get; set; }
        [DisplayName("Service Type")]
        public string Service_Type { get; set; }
        [Required]
        [DisplayName("Room No")]
        public int Room_NO { get; set; }
        public string Status { get; set; }
        [DisplayName("Date Order")]
        public DateTime Service_Date { get; set; }
        [Required]
        public string Description { get; set; }
        [DisplayName("Cost")]
        public int Cost { get; set; }
        [Required]
        [DisplayName("Table NO")]
        public int Table_No { get; set; }
        public virtual Account User { get; set; }
        public virtual Food food { get; set; }
        public virtual Drink drink { get; set; }
    }
}
