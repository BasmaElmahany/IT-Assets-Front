using ItAssetsFront.Models.DeviceMaintainance;
using ItAssetsFront.Models.DeviceRequest;
using ItAssetsFront.Services.CategoryService;
using ItAssetsFront.Services.DeviceMaintainanceService;
using ItAssetsFront.Services.DeviceRequestService;
using ItAssetsFront.Services.DeviceService;
using ItAssetsFront.Services.officeService;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace ItAssetsFront.Controllers
{
    public class DeviceMaintainanceController : Controller
    {
        private readonly DeviceService _Devser; 
        private readonly MaintainanceRequest _ser;

        public DeviceMaintainanceController(DeviceService Devser, MaintainanceRequest ser)
        {
          
            _Devser = Devser;
            _ser = ser;

        }
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;
            var requests = await _ser.GetAllSchedulesAsync();
            var pagedRequests = requests.ToPagedList(pageNumber, pageSize);
            return View(pagedRequests);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Devices = await _Devser.GetAllDevicesAsync();
          
            return View();
        }

        // Create - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostDeviceMaintainance model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Devices = await _Devser.GetAllDevicesAsync();
                return View(model);
            }

            var success = await _ser.AddScheduleAsync(model);
            if (success != null)
            {
                TempData["Success"] = "Device Maintainance Schedule created successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Failed to create Maintainance Schedule.";
            ViewBag.Devices = await _Devser.GetAllDevicesAsync();
            return View(model);
        }


        public async Task<IActionResult> Edit(Guid id)
        {
            ViewBag.Devices = await _Devser.GetAllDevicesAsync();
            var req = await _ser.GetScheduleByIdAsync(id);
            if (req == null)
            {
                return NotFound();
            }
            var exist = new UpdateDeviceMaintainance
            {
                id = req.id,
                deviceID = req.deviceID,
                description = req.description,
                isComplete = req.isComplete,
                date = req.date,
            };
            return View(exist);
        }

        // Create - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateDeviceMaintainance model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Devices = await _Devser.GetAllDevicesAsync();
                return View(model);
            }

            var success = await _ser.UpdateRequestAsync(model);
            if (success != null)
            {
                TempData["Success"] = "Device Maintainance Schedule Updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Failed to Updated Device Maintainance Schedule.";
            ViewBag.Devices = await _Devser.GetAllDevicesAsync();
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
