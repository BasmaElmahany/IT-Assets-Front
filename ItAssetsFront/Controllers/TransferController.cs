using ItAssetsFront.Services.AssignService;
using ItAssetsFront.Services.BrandService;
using ItAssetsFront.Services.CategoryService;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace ItAssetsFront.Controllers
{
    public class TransferController : Controller
    {
        private readonly TransferService _ser;
        private readonly brandService _brandService;
        private readonly categoryService _categoryService;
        public TransferController(TransferService ser, brandService brandService, categoryService categoryService)
        {
            _ser = ser;
            _brandService = brandService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index(int? page, Guid? brandId, Guid? categoryId, string? search)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;

            ViewBag.Brands = await _brandService.GetAllBrandsAsync();
            ViewBag.Categories = await _categoryService.GetAllCategoryAsync();

            var transfers = await _ser.GetAllDevicesTransferAsync();

            if (brandId.HasValue && brandId.Value != Guid.Empty)
                transfers = transfers.Where(t => t.device != null && t.device.brandId == brandId.Value).ToList();

            if (categoryId.HasValue && categoryId.Value != Guid.Empty)
                transfers = transfers.Where(t => t.device != null && t.device.categoryID == categoryId.Value).ToList();

            if (!string.IsNullOrWhiteSpace(search))
                transfers = transfers.Where(t =>
                    (t.device?.name?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (t.emp1?.name?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (t.emp2?.name?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false)
                ).ToList();

            var pagedTransfers = transfers.ToPagedList(pageNumber, pageSize);

            ViewBag.SelectedBrand = brandId;
            ViewBag.SelectedCategory = categoryId;
            ViewBag.Search = search;

            return View(pagedTransfers);
        }
        [HttpGet]
        public async Task<IActionResult> ExportToExcel()
        {
            var fileBytes = await _ser.ExportTransfersToExcelAsync();
            if (fileBytes == null)
                return NotFound("No transfers found to export.");

            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Transfers.xlsx");
        }


    }
}
