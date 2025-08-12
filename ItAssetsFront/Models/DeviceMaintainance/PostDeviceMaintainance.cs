namespace ItAssetsFront.Models.DeviceMaintainance
{
    public class PostDeviceMaintainance
    {
        public Guid deviceID { get; set; }

        public string description { get; set; }

        public DateOnly date { get; set; }

        public bool isComplete { get; set; }
    }
}
