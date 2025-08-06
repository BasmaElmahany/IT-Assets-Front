using System.ComponentModel.DataAnnotations;

namespace ItAssetsFront.Models.EmployeeModels
{
    public class GetAllEmployee
    {
        public Guid id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [Display(Name = "Full Name")]
        public string name { get; set; }

        [Required(ErrorMessage = "Position is required")]
        [StringLength(50, ErrorMessage = "Position cannot exceed 100 characters")]
        [Display(Name = "Job Position")]
        public string position { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email Address")]
        public string email { get; set; }
    }
}
