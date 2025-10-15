using ItAssetsFront.Models.EmployeeModels;

namespace ItAssetsFront.Models.DeviceModels
{
    public class DeviceTransfer
    {
      public Guid  id { get; set; }

      public Guid oldEmpId { get; set; }
      public Guid newEmpId { get; set; }

      public GetAllEmployee emp1 { get; set; }

      public GetAllEmployee emp2 { get; set; }
      public Guid deviceID { get; set; }

      public Device device { get; set; }

      public DateOnly dateOnly { get; set; }

    }
}
