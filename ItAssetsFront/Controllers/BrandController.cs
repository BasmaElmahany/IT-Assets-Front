using ItAssetsFront.Models.BrandModels;
using ItAssetsFront.Services.BrandService;
using Microsoft.AspNetCore.Mvc;

using X.PagedList;
using X.PagedList.Extensions;

namespace ItAssetsFront.Controllers
{
    public class BrandController : Controller
    {
        private readonly brandService _brandService;
        public BrandController(brandService brandService)
        {
            _brandService = brandService;
        }
        // GET: /Brand
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;

            var allBrands = await _brandService.GetAllBrandsAsync(); 
            var pagedBrands = allBrands.ToPagedList(pageNumber, pageSize);

            return View(pagedBrands);
        }

        // GET: /Brand/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Brand/Create
        [HttpPost]
        public async Task<IActionResult> Create(postBrand model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var added = await _brandService.AddBrandAsync(model);
            if (added != null)
            {
                TempData["Success"] = "Brand created successfully.";
                return RedirectToAction(nameof(Index));
            }

         
            TempData["Error"] = "Something went wrong!";
            return View(model);
        }

        // GET: /Brand/Edit/{id}
        public async Task<IActionResult> Edit(Guid id)
        {
            var brand = await _brandService.GetBrandByIdAsync(id);
            if (brand == null)
                return NotFound();

            return View(brand);
        }

        // POST: /Brand/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(getAllBrands model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var updated = await _brandService.UpdateBrandAsync(model);
            if (updated != null)
            {
                TempData["Success"] = "Brand updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Something went wrong!";
            return View(model);
        }

        // GET: /Brand/Delete/{id}
        public async Task<IActionResult> Delete(Guid id)
        {

            var resultMessage = await _brandService.DeleteBrandAsync(id);

            if (!string.IsNullOrEmpty(resultMessage))
                TempData["Success"] = resultMessage;
            else
                TempData["Error"] = "Brand not found or could not be deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}