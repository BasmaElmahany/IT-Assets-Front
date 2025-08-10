using ItAssetsFront.Models.DeviceModels;
using ItAssetsFront.Models.EmployeeModels;

namespace ItAssetsFront.Models.AssignDevice
{
    public class GetAllAssigned
    {
        public Guid id {  get; set; }

        public Guid deviceID { get; set; }

        public Guid employeeID { get; set; }

        public DateOnly assignDate {  get; set; }

        public string deviceStatus { get; set; }

        public DateOnly ? returnDate { get; set; }

        public string? returnStatus { get; set; }

        public int qty { get; set; }

        public Device device { get; set; } 
        
        public GetAllEmployee employee { get; set; }

    }
}
