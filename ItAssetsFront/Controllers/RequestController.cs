using ItAssetsFront.Models.DeviceModels;
using ItAssetsFront.Models.DeviceRequest;
using ItAssetsFront.Services.BrandService;
using ItAssetsFront.Services.CategoryService;
using ItAssetsFront.Services.DeviceRequestService;
using ItAssetsFront.Services.officeService;
using ItAssetsFront.Services.SupplierService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList.Extensions;
namespace ItAssetsFront.Controllers
{
    public class RequestController : Controller
    {
        private readonly categoryService _cat;
        private readonly officeService _off;
        private readonly RequestService _ser;

        public RequestController(categoryService cat , officeService off , RequestService ser )
        {
            _cat = cat;
            _off = off;
            _ser = ser;

        }
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;
            var requests = await _ser.GetAllRequestsAsync();
            var pagedRequests = requests.ToPagedList(pageNumber, pageSize);
            return View(pagedRequests);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _cat.GetAllCategoryAsync();
            ViewBag.Offices = await _off.GetAllOfficeAsync();
            return View();
        }

        // Create - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostRequest model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _cat.GetAllCategoryAsync();
                ViewBag.Offices = await _off.GetAllOfficeAsync();
                return View(model);
            }

            var success = await _ser.AddRequestAsync(model);
            if (success!=null)
            {
                TempData["Success"] = "Device Request created successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Failed to create DeviceRequest.";
            ViewBag.Categories = await _cat.GetAllCategoryAsync();
            ViewBag.Offices = await _off.GetAllOfficeAsync();
            return View(model);
        }


        public async Task<IActionResult> Edit(Guid id)
        {
            ViewBag.Categories = await _cat.GetAllCategoryAsync();
            ViewBag.Offices = await _off.GetAllOfficeAsync();
            var req = await _ser.GetReqByIdAsync(id);
            if (req == null)
            {
                return NotFound();
            }
            var exist = new EditRequest
            {
                id = req.id,
                categoryID = req.categoryID,
                officeId = req.officeId,
                deviceCount = req.deviceCount,
                deviceName = req.deviceName,
                date = req.date,
            };
            return View(exist);
        }

        // Create - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditRequest model )
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _cat.GetAllCategoryAsync();
                ViewBag.Offices = await _off.GetAllOfficeAsync();
                return View(model);
            }

            var success = await _ser.UpdateRequestAsync(model);
            if (success != null)
            {
                TempData["Success"] = "Device Request Updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Failed to Updated DeviceRequest.";
            ViewBag.Categories = await _cat.GetAllCategoryAsync();
            ViewBag.Offices = await _off.GetAllOfficeAsync();
            return View(model);
        }


        public async Task<IActionResult> Delete(Guid id)
        {

            var resultMessage = await _ser.DeleteRequestAsync(id);

            if (!string.IsNullOrEmpty(resultMessage))
                TempData["Success"] = resultMessage;
            else
                TempData["Error"] = "Request not found.";
            return RedirectToAction(nameof(Index));
        }



    }
}
