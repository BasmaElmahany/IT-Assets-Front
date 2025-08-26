using System.ComponentModel.DataAnnotations;

namespace ItAssetsFront.Models.DeviceModels
{
    public class EditDevice
    {
        public Guid id { get; set; }
        [Required(ErrorMessage = "Device name is required")]
        [Display(Name = "Device Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Serial Number is required")]
        [Display(Name = "Serial Number")]
        public string SerialNumber { get; set; }

        [Display(Name = "Device Photo")]
        public IFormFile Photo { get; set; }

        public string ExistingPhotoUrl { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [Display(Name = "Device Status")]
        public string Status { get; set; }
        public string Spex { get; set; }

        public int Warranty { get; set; }

        [Required(ErrorMessage = "Brand ID is required")]
        [Display(Name = "Brand")]
        public Guid BrandId { get; set; }

        [Required(ErrorMessage = "Category ID is required")]
        [Display(Name = "Category")]
        public Guid CategoryID { get; set; }

        [Required(ErrorMessage = "Supplier ID is required")]
        [Display(Name = "Supplier")]
        public Guid? SupplierID { get; set; }

        [Display(Name = "Is Faulty")]
        public bool IsFaulty { get; set; }

        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be 0 or greater")]
        [Display(Name = "Quantity")]
        public double price { get; set; }
    }
}
