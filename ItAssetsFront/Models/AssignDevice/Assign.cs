namespace ItAssetsFront.Models.AssignDevice
{
    public class Assign
    {
         public Guid deviceID { get; set; }
         public Guid employeeID { get; set; }
         public Guid assignDate { get; set; }

         public string  deviceStatus { get; set; }
         public int qty { get; set; }
    }
}
