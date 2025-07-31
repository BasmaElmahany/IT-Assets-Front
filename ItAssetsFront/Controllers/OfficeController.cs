using ItAssetsFront.Models.CategoryModels;
using ItAssetsFront.Models.OfficeModels;
using ItAssetsFront.Services.CategoryService;
using ItAssetsFront.Services.officeService;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace ItAssetsFront.Controllers
{
    public class OfficeController : Controller
    {
        private readonly officeService _ser;
        public OfficeController(officeService ser)
        {
            _ser = ser;
        }
        // GET: /Category
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;

            var allOffices = await _ser.GetAllOfficeAsync();
            var pagedOffices = allOffices.ToPagedList(pageNumber, pageSize);

            return View(pagedOffices);
        }

        // GET: /Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Category/Create
        [HttpPost]
        public async Task<IActionResult> Create(postOffice model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var added = await _ser.AddOfficeAsync(model);
            if (added != null)
            {
                TempData["Success"] = "Office created successfully.";
                return RedirectToAction(nameof(Index));
            }


            TempData["Error"] = "Something went wrong!";
            return View(model);
        }

        // GET: /Category/Edit/{id}
        public async Task<IActionResult> Edit(Guid id)
        {
            var cat = await _ser.GetOfficeByIdAsync(id);
            if (cat == null)
                return NotFound();

            return View(cat);
        }

        // POST: /Category/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(Office model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var updated = await _ser.UpdateOfficeAsync(model);
            if (updated != null)
            {
                TempData["Success"] = "Office updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Something went wrong!";
            return View(model);
        }

        // GET: /Category/Delete/{id}
        public async Task<IActionResult> Delete(Guid id)
        {

            var resultMessage = await _ser.DeleteOfficeAsync(id);

            if (!string.IsNullOrEmpty(resultMessage))
                TempData["Success"] = resultMessage;
            else
                TempData["Error"] = "Office not found or could not be deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
