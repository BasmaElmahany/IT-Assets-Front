using ItAssetsFront.Models.OfficeModels;
using ItAssetsFront.Models.SupplierModels;
using ItAssetsFront.Services.officeService;
using ItAssetsFront.Services.SupplierService;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace ItAssetsFront.Controllers
{
    public class SupplierController : Controller
    {
        private readonly SupplierService _ser;
        public SupplierController(SupplierService ser)
        {
            _ser = ser;
        }
        // GET: /Category
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;

            var allsuppliers = await _ser.GetAllSupplierAsync();
            var pagedsuppliers = allsuppliers.ToPagedList(pageNumber, pageSize);

            return View(pagedsuppliers);
        }

        // GET: /Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Category/Create
        [HttpPost]
        public async Task<IActionResult> Create(postSupplier model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var added = await _ser.AddSupplierAsync(model);
            if (added != null)
            {
                TempData["Success"] = "Supplier created successfully.";
                return RedirectToAction(nameof(Index));
            }


            TempData["Error"] = "Something went wrong!";
            return View(model);
        }

        // GET: /Category/Edit/{id}
        public async Task<IActionResult> Edit(Guid id)
        {
            var cat = await _ser.GetSupplierByIdAsync(id);
            if (cat == null)
                return NotFound();

            return View(cat);
        }

        // POST: /Category/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(Supplier model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var updated = await _ser.UpdateSupplierAsync(model);
            if (updated != null)
            {
                TempData["Success"] = "Supplier updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Something went wrong!";
            return View(model);
        }

        // GET: /Category/Delete/{id}
        public async Task<IActionResult> Delete(Guid id)
        {

            var resultMessage = await _ser.DeleteSupplierAsync(id);

            if (!string.IsNullOrEmpty(resultMessage))
                TempData["Success"] = resultMessage;
            else
                TempData["Error"] = "Supplier not found or could not be deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
