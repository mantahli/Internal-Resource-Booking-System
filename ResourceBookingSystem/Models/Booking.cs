using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResourceBookingSystem.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("Resource")]
        public int ResourceId { get; set; }
        public Resource Resource { get; set; }

        [Required(ErrorMessage = "Start time is required")]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "End time is required")]
        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "Booked by is required")]
        [Display(Name = "Booked By")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string BookedBy { get; set; }

        [Required(ErrorMessage = "Purpose is required")]
        [StringLength(500, ErrorMessage = "Purpose cannot exceed 500 characters")]
        public string Purpose { get; set; }
    }
}