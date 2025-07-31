using ItAssetsFront.Models.CategoryModels;
using ItAssetsFront.Services.CategoryService;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace ItAssetsFront.Controllers
{
    public class CategoryController : Controller
    {
        private readonly categoryService _ser;
        public CategoryController(categoryService ser)
        {
            _ser = ser;
        }
        // GET: /Category
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;

            var allcategories = await _ser.GetAllCategoryAsync();
            var pagedcategories = allcategories.ToPagedList(pageNumber, pageSize);

            return View(pagedcategories);
        }

        // GET: /Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Category/Create
        [HttpPost]
        public async Task<IActionResult> Create(postCategory model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var added = await _ser.AddCategoryAsync(model);
            if (added != null)
            {
                TempData["Success"] = "Category created successfully.";
                return RedirectToAction(nameof(Index));
            }


            TempData["Error"] = "Something went wrong!";
            return View(model);
        }

        // GET: /Category/Edit/{id}
        public async Task<IActionResult> Edit(Guid id)
        {
            var cat = await _ser.GetCategoryByIdAsync(id);
            if (cat == null) 
                return NotFound();

            return View(cat);
        }

        // POST: /Category/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(getAllCategories model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var updated = await _ser.UpdateCategoryAsync(model);
            if (updated != null)
            {
                TempData["Success"] = "Category updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Something went wrong!";
            return View(model);
        }

        // GET: /Category/Delete/{id}
        public async Task<IActionResult> Delete(Guid id)
        {

            var resultMessage = await _ser.DeleteCategoryAsync(id);

            if (!string.IsNullOrEmpty(resultMessage))
                TempData["Success"] = resultMessage;
            else
                TempData["Error"] = "Category not found or could not be deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}