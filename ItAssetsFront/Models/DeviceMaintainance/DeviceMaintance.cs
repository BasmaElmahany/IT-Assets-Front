using ItAssetsFront.Models.DeviceModels;

namespace ItAssetsFront.Models.DeviceMaintainance
{
    public class DeviceMaintance
    {
         public Guid id { get; set; }
         public Guid deviceID { get; set; }

         public Device device { get; set; }

        public string description { get; set; }

        public DateOnly date {  get; set; }

        public bool isComplete { get; set; }
    }
}
