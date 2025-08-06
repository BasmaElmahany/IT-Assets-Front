namespace ItAssetsFront.Models.AssignDevice
{
    public class Return
    {
        public Guid deviceID { get; set; }

        public DateOnly returnDate {  get; set; }

        public string returnStatus { get; set; }

    }
}
