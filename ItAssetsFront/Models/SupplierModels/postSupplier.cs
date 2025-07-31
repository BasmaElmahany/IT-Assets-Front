using System.ComponentModel.DataAnnotations;

namespace ItAssetsFront.Models.SupplierModels
{
    public class postSupplier
    {
        [Display(Name = "Supplier Name")]
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string name { get; set; }


        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^(010|011|012|015)[0-9]{8}$", ErrorMessage = "Enter a valid Egyptian phone number")]
        public string phoneNumber { get; set; }
    }
}
