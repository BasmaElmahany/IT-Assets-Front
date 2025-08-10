using ItAssetsFront.Models.DeviceModels;
using ItAssetsFront.Models.OfficeModels;

namespace ItAssetsFront.Models.AssignOffice
{
    public class getAllOfficeAssigned
    {
        public Guid id { get; set; }

        public Guid deviceID { get; set; }

        public Guid OffID { get; set; }

        public DateOnly assignDate { get; set; }
         
        public string deviceStatus { get; set; }

        public DateOnly? returnDate { get; set; }

        public string? returnStatus { get; set; }

        public int qty { get; set; }

        public Device device { get; set; }

        public Office office { get; set; }
    }
}
