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

        [Required]
        public List<postDevice> Devices { get; set; } = new List<postDevice>();
    }
}
