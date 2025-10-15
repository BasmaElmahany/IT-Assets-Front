using ItAssetsFront.Models.DeviceModels;
using ItAssetsFront.Services.BrandService;
using ItAssetsFront.Services.CategoryService;
using ItAssetsFront.Services.DeviceService;
using ItAssetsFront.Services.SupplierService;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace ItAssetsFront.Controllers
{
    public class DeviceController : Controller
    {
        private readonly DeviceService _ser;
        private readonly brandService _brandService;
        private readonly categoryService _categoryService;
        private readonly SupplierService _supplierService;
        public DeviceController (DeviceService ser , brandService brandService , categoryService categoryService , SupplierService supplierService )
        {

            _ser = ser;
            _brandService = brandService;
            _categoryService = categoryService;
            _supplierService = supplierService;

        }
        public async Task<IActionResult> Index(int? page, Guid? brandId, Guid? categoryId, string search)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;

            // Get all devices
            var devices = await _ser.GetAllDevicesAsync();

            // 🔍 Filtering
            if (brandId.HasValue && brandId.Value != Guid.Empty)
                devices = devices.Where(d => d.brand.id == brandId.Value).ToList();

            if (categoryId.HasValue && categoryId.Value != Guid.Empty)
                devices = devices.Where(d => d.category.id == categoryId.Value).ToList();

            if (!string.IsNullOrEmpty(search))
                devices = devices
                    .Where(d => d.name.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            // 📑 Pagination
            var pagedDevices = devices.ToPagedList(pageNumber, pageSize);

            // Dropdown data
            ViewBag.Brands = await _brandService.GetAllBrandsAsync();
            ViewBag.Categories = await _categoryService.GetAllCategoryAsync();

            // Keep filter values for view
            ViewBag.SelectedBrand = brandId;
            ViewBag.SelectedCategory = categoryId;
            ViewBag.Search = search;

            return View(pagedDevices);
        }


        public async Task<IActionResult> CreateAsync()
        {

            ViewBag.Brands = await _brandService.GetAllBrandsAsync();
            ViewBag.Categories = await _categoryService.GetAllCategoryAsync();
            ViewBag.Suppliers = await _supplierService.GetAllSupplierAsync();
            return View();
        }

        // Create - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(postDevice model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Brands = await _brandService.GetAllBrandsAsync();
                ViewBag.Categories = await _categoryService.GetAllCategoryAsync();
                ViewBag.Suppliers = await _supplierService.GetAllSupplierAsync();
                return View(model);
            }

            var success = await _ser.CreateDeviceAsync(model);
            if (success)
            {
                TempData["Success"] = "Device created successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Failed to create device.";
            ViewBag.Brands = await _brandService.GetAllBrandsAsync();
            ViewBag.Categories = await _categoryService.GetAllCategoryAsync();
            ViewBag.Suppliers = await _supplierService.GetAllSupplierAsync();
            return View(model);
        }






        public async Task<IActionResult> BulkCreate()
        {
            ViewBag.Brands = await _brandService.GetAllBrandsAsync();
            ViewBag.Categories = await _categoryService.GetAllCategoryAsync();
            ViewBag.Suppliers = await _supplierService.GetAllSupplierAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulkCreate(BulkDeviceRequest model)
        {
            if (!ModelState.IsValid || model.Devices == null || model.Devices.Count == 0)
            {
                TempData["Error"] = "You must provide at least one device.";
                ViewBag.Brands = await _brandService.GetAllBrandsAsync();
                ViewBag.Categories = await _categoryService.GetAllCategoryAsync();
                ViewBag.Suppliers = await _supplierService.GetAllSupplierAsync();
                return View(model);
            }

            var success = await _ser.CreateBulkDevicesAsync(model);

            if (success)
            {
                TempData["Success"] = "All devices created successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Some devices failed to create.";
            ViewBag.Brands = await _brandService.GetAllBrandsAsync();
            ViewBag.Categories = await _categoryService.GetAllCategoryAsync();
            ViewBag.Suppliers = await _supplierService.GetAllSupplierAsync();
            return View(model);
        }





        // Edit - GET
        public async Task<IActionResult> Edit(Guid id)
        {
            ViewBag.Brands = await _brandService.GetAllBrandsAsync();
            ViewBag.Categories = await _categoryService.GetAllCategoryAsync();
            ViewBag.Suppliers = await _supplierService.GetAllSupplierAsync();
            var devices = await _ser.GetAllDevicesAsync();
            var device = devices.FirstOrDefault(d => d.id == id);

            if (device == null)
            {
                TempData["Error"] = "Device not found.";
                return RedirectToAction(nameof(Index));
            }

            var model = new EditDevice
            {   id = device.id ,
                Name = device.name,
                SerialNumber = device.serialNumber,
                ExistingPhotoUrl = device.photoUrl,
                Status = device.status,
                BrandId = device.brand.id,
                CategoryID = device.category.id,
                SupplierID = device.supplier.id,
                IsFaulty = device.isFaulty,
                IsAvailable = device.isAvailable,
                price= device.price
            };

            return View(model);
        }

        // Edit - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EditDevice model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Brands = await _brandService.GetAllBrandsAsync();
                ViewBag.Categories = await _categoryService.GetAllCategoryAsync();
                ViewBag.Suppliers = await _supplierService.GetAllSupplierAsync();
                return View(model);

            }

            var success = await _ser.UpdateDeviceAsync(id, model);
            if (success)
            {
                TempData["Success"] = "Device updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Failed to update device.";
            return View(model);
        }

        // Delete - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _ser.DeleteDeviceAsync(id);
            if (success)
                TempData["Success"] = "Device deleted successfully.";
            else
                TempData["Error"] = "Device not found or could not be deleted.";

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> ExportDevicesToExcel()
        {
            var excelBytes = await _ser.ExportDevicesToExcelAsync();

            if (excelBytes == null)
                return NotFound("No devices found.");

            return File(
                excelBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Devices.xlsx"
            );
        }


    }
}
