using ItAssetsFront.Models.CategoryModels;
using ItAssetsFront.Models.OfficeModels;

namespace ItAssetsFront.Models.DeviceRequest
{
    public class Request
    {
        public Guid id { get; set; }

        public Guid categoryID { get; set; }

        public getAllCategories category {  get; set; } 

        public string deviceName { get; set; }

        public int deviceCount { get; set; }

        public Guid officeId { get; set; }

        public Office  office { get; set; }

        public DateOnly date {  get; set; }
    }
}
