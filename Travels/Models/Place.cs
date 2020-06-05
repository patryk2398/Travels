using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Travels.Models
{
    public class Place
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Country { get; set; }
        [Required]
        public string Locality { get; set; }
        [Required]
        public int Rating { get; set; }
        [Required]
        public string Description { get; set; }
        
        public string Image { get; set; }
        public string UserId { get; set; }
        public string Active { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "Travel Type")]
        public int TravelTypeId { get; set; }

        [ForeignKey("TravelTypeId")]
        public virtual TravelType TravelType { get; set; }
    }
}
