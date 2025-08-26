namespace ItAssetsFront.Models.AssignDevice
{
    public class Return
    { 
        public Guid id { get; set; }
        public Guid deviceID { get; set; }

        public DateOnly returnDate {  get; set; }

        public string returnStatus { get; set; }
        public string WhyReturn { get; set; }
    }
}
