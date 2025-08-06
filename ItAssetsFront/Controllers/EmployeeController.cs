using ItAssetsFront.Models.EmployeeModels;
using ItAssetsFront.Models.SupplierModels;
using ItAssetsFront.Services.EmployeeService;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace ItAssetsFront.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeService _ser;

        public EmployeeController(EmployeeService ser)
        {
            _ser = ser;
        }
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;

            var allemployees = await _ser.GetAllEmployeesAsync();
            var pagedemployees = allemployees.ToPagedList(pageNumber, pageSize);

            return View(pagedemployees);
        }

  
        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        public async Task<IActionResult> Create(PostEmployee model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var added = await _ser.AddEmployeeAsync(model);
            if (added != null)
            {
                TempData["Success"] = "Employee created successfully.";
                return RedirectToAction(nameof(Index));
            }


            TempData["Error"] = "Something went wrong!";
            return View(model);
        }

     
        public async Task<IActionResult> Edit(Guid id)
        {
            var emp = await _ser.GetEmployeeByIdAsync(id);
            if (emp == null)
                return NotFound();

            return View(emp);
        }

       
        [HttpPost]
        public async Task<IActionResult> Edit(GetAllEmployee model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var updated = await _ser.UpdateEmployeeAsync(model);
            if (updated != null)
            {
                TempData["Success"] = "Employee updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Something went wrong!";
            return View(model);
        }

     
        public async Task<IActionResult> Delete(Guid id)
        {

            var resultMessage = await _ser.DeleteEmployeeAsync(id);

            if (!string.IsNullOrEmpty(resultMessage))
                TempData["Success"] = resultMessage;
            else
                TempData["Error"] = "Employee not found or could not be deleted due to it has been assigned with a device.";
            return RedirectToAction(nameof(Index));
        }
    }
}
