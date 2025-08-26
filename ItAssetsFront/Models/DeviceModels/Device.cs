             using ItAssetsFront.Models.BrandModels;
using ItAssetsFront.Models.CategoryModels;
using ItAssetsFront.Models.SupplierModels;

namespace ItAssetsFront.Models.DeviceModels
{
    public class Device
    {
       public Guid id { get; set; }

        public string? photoUrl { get; set; }

        public string  name { get; set; }
        public string serialNumber { get; set; }

        public string status { get; set; }
        public string Spex { get; set; }

        public int Warranty { get; set; }

        public Guid brandId { get; set; }

        public getAllBrands brand {  get; set; }

        public Guid categoryID { get; set; }

        public getAllCategories category { get; set; }

        public Guid supplierID { get; set; }

        public Supplier supplier { get; set; }

        public bool isFaulty { get; set; }

        public bool isAvailable { get; set; }

        public double price { get; set; }
    }
}
