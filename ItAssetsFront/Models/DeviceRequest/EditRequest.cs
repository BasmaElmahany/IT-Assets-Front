namespace ItAssetsFront.Models.DeviceRequest
{
    public class EditRequest
    {  
        public Guid id {  get; set; }
        public Guid categoryID { get; set; }
        public string deviceName { get; set; }
        public int deviceCount { get; set; }
        public Guid officeId { get; set; }
        public DateOnly date { get; set; }
    }
}
