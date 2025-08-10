using System.ComponentModel.DataAnnotations;

namespace ItAssetsFront.Models.AssignOffice
{
    public class OfficeAssign
    {
        [Required]
        public Guid deviceID { get; set; }

        [Required]
        public Guid OfficeID { get; set; }

        [Required]
        public DateOnly assignDate { get; set; }

        [Required]
        public string deviceStatus { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int qty { get; set; }
    }
}
