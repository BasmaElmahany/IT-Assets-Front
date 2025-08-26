using System.ComponentModel.DataAnnotations;

namespace ItAssetsFront.Models.DeviceModels
{
    public class BulkDeviceRequest
    {
        [Required(ErrorMessage = "Brand is required")]
        public Guid BrandId { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public Guid CategoryID { get; set; }

        public Guid? SupplierID { get; set; }

        // Shared fields
        [Required]
        public string Name { get; set; }

        public string Spex { get; set; }

        public string Status { get; set; }

        public IFormFile? Photo { get; set; }

        public int? Warranty { get; set; }

        public decimal? Price { get; set; }

        public bool IsFaulty { get; set; } = false;

        public bool IsAvailable { get; set; } = true;

        // Only Serial Numbers will be different
        [Required]
        public List<postDevice> Devices { get; set; } = new List<postDevice>();
    }
}
