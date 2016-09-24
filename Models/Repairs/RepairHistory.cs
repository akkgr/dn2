using System;
using System.ComponentModel.DataAnnotations;

namespace cinnamon.api.Models
{
    public class RepairHistory
    {
        [Required]
        public DateTime Occurred { get; set; }

        [Required]
        public RepairStatus Status { get; set; }

        public string Remarks { get; set; }
    }
}