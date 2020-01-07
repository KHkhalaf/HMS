using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class RoomViewModel
    {
        public int Id { get; set; }
        [DisplayName("Room Number")]
        public int number { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public int Charges { get; set; }
        [Required]
        public string Status { get; set; }
        public string image { get; set; }
        public RoomViewModel()
        {
            this.Status = (new Room_Status()).getStatus(1);
        }
    }   
}
