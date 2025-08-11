namespace ItAssetsFront.Models.AssignDevice
{
    public class DeviceTransfer
    {
        public Guid oldEmpId { get; set; }
        public Guid newEmpId { get; set; }

        public Guid deviceID { get; set; }

        public DateOnly dateOnly { get; set; }
    }
}
