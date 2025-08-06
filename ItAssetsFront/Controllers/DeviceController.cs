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
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;
            var devices = await _ser.GetAllDevicesAsync();
            var pageddevices = devices.ToPagedList(pageNumber, pageSize);
            return View(pageddevices);
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
                Qty = device.qty
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

    }
}
