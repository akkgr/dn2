using System;
using System.ComponentModel.DataAnnotations;

namespace cinnamon.api.Models
{
    public class RequestHistory
    {
        [Required]
        public DateTime Occurred { get; set; }

        [Required]
        public string Responsible { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public RequestStatus Status { get; set; }

        public string Remarks { get; set; }

        public int Duration { get; set; }
    }
}