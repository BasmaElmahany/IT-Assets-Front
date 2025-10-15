using ItAssetsFront.Models.AssignDevice;
using ItAssetsFront.Services.AssignService;
using ItAssetsFront.Services.DeviceService;
using ItAssetsFront.Services.EmployeeService;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace ItAssetsFront.Controllers
{
    public class DeviceAssignController : Controller
    {
        private readonly DeviceService _Devser;
        private readonly EmployeeService _employee;
        private readonly AssignService _ser;

        public DeviceAssignController(DeviceService Devser, EmployeeService employee, AssignService ser)
        {
            _ser = ser;
            _employee = employee;
            _Devser = Devser;
        }

        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;
            var devices = await _ser.GetAllDevicesAsync();
            var pageddevices = devices.ToPagedList(pageNumber, pageSize);
            return View(pageddevices);
        }

        public async Task<IActionResult> Assign()
        {
            ViewBag.Devices = await _Devser.GetAllDevicesAsync();
            ViewBag.Employees = await _employee.GetAllEmployeesAsync();
            return View();
        }

        // Create - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(Assign model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Devices = await _Devser.GetAllDevicesAsync();
                ViewBag.Employees = await _employee.GetAllEmployeesAsync();
                return View(model);
            }

            var assignedDevice = await _ser.AddDeviceAsync(model);
            if (assignedDevice != null)
            {
                TempData["Success"] = "Device Assigned successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Failed to Assign device.";
            ViewBag.Devices = await _Devser.GetAllDevicesAsync();
            ViewBag.Employees = await _employee.GetAllEmployeesAsync();
            return View(model);
        }

        public async Task<IActionResult> Return(Guid id)
        {
           
            var assignedDevice = (await _ser.GetAllDevicesAsync())
                .FirstOrDefault(x => x.id == id);

            if (assignedDevice == null)
            {
                TempData["Error"] = "Assigned device not found.";
                return RedirectToAction(nameof(Index));
            }

            var model = new Return
            {
                id = assignedDevice.id,
                deviceID = assignedDevice.deviceID,
                returnDate = DateOnly.FromDateTime(DateTime.Now) 
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(Return model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _ser.ReturnDeviceAsync(model);
            if (result != null)
            {
                TempData["Success"] = "Device returned successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Failed to return device.";
            return View(model);
        }

        public async Task<IActionResult> Transfer(Guid employeeId, Guid deviceId)
        {
            ViewBag.Employees = await _employee.GetAllEmployeesAsync();
            var model = new DeviceTransfer
            {
                oldEmpId = employeeId,
                deviceID = deviceId,
                dateOnly = DateOnly.FromDateTime(DateTime.Now)
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Transfer(DeviceTransfer model)
        {
            if (!ModelState.IsValid) { ViewBag.Employees = await _employee.GetAllEmployeesAsync(); }
            var result = await _ser.transferDeviceAsync(model);
            if (result != null)
            {
                TempData["Success"] = "Device returned successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Failed to return device.";
            return View(model);
        }
               


         
        
        public async Task<IActionResult> Delete(Guid id)
        {

            var resultMessage = await _ser.DeleteAssignAsync(id);

            if (!string.IsNullOrEmpty(resultMessage))
                TempData["Success"] = resultMessage;
            else
                TempData["Error"] = "Couldn't Return the assigned device";
            return RedirectToAction(nameof(Index));
        }
    }
}
