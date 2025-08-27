using System.ComponentModel.DataAnnotations;

namespace ItAssetsFront.Models.DeviceModels
{
    public class BulkDeviceRequest
    {

        [Required]
        public Guid BrandId { get; set; }

        [Required]
        public Guid CategoryID { get; set; }

        public Guid? SupplierID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Spex { get; set; }
        public string Status { get; set; }
        public IFormFile? Photo { get; set; }
        public int Warranty { get; set; }
        public double Price { get; set; }
        public bool IsFaulty { get; set; } = false;
        public bool IsAvailable { get; set; } = true;

        // ✅ Only serial numbers differ
        [Required]
        public List<DeviceSerialRequest> Devices { get; set; } = new();
    }
}
