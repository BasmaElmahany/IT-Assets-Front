namespace ItAssetsFront.Models.AssignOffice
{
    public class ReturndevOff
    {
        public Guid id { get; set; }
        public Guid deviceID { get; set; }
        public DateOnly returnDate { get; set; }
        public string returnStatus { get; set; }
    }
}
