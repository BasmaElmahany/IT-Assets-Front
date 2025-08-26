using System.ComponentModel.DataAnnotations;

namespace ItAssetsFront.Models.AssignDevice
{
    public class Assign
    {
        [Required]
        public Guid deviceID { get; set; }

        [Required]
        public Guid employeeID { get; set; }

        [Required]
        public DateOnly assignDate { get; set; }

        [Required]
        public string deviceStatus { get; set; }

    }
}
