using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SupermarketManagerSystem.Data.Models;
using SupermarketManagerSystem.Services.Contracts;
using SupermarketManagerSystem.Services.Models;
using SupermarketManagerSystem.Web.ViewModels;

namespace SupermarketManagerSystem.Web.Controllers;

[Authorize(Roles = "Administrator")]
public class EmployeesController : Controller
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public async Task<IActionResult> All()
    {
        var employees = await _employeeService.GetAllAsync();
        var viewModels = employees.Select(e => new EmployeeViewModel
        {
            Id = e.Id, FirstName = e.FirstName, LastName = e.LastName,
            Email = e.Email, Phone = e.Phone, Position = e.Position,
            HireDate = e.HireDate, IsActive = e.IsActive
        });
        return View(viewModels);
    }

    public async Task<IActionResult> Details(int id)
    {
        var employee = await _employeeService.GetByIdAsync(id);
        if (employee == null) return NotFound();
        return View(new EmployeeViewModel
        {
            Id = employee.Id, FirstName = employee.FirstName, LastName = employee.LastName,
            Email = employee.Email, Phone = employee.Phone, Position = employee.Position,
            HireDate = employee.HireDate, IsActive = employee.IsActive, Notes = employee.Notes
        });
    }

    public IActionResult Add()
    {
        var vm = new EmployeeViewModel { Positions = GetPositionsList() };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(EmployeeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Positions = GetPositionsList();
            return View(model);
        }
        await _employeeService.CreateAsync(new EmployeeServiceModel
        {
            FirstName = model.FirstName, LastName = model.LastName, Email = model.Email,
            Phone = model.Phone, Position = model.Position, HireDate = model.HireDate,
            IsActive = model.IsActive, Notes = model.Notes
        });
        TempData["Success"] = $"Employee {model.FirstName} {model.LastName} was added.";
        return RedirectToAction(nameof(All));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var employee = await _employeeService.GetByIdAsync(id);
        if (employee == null) return NotFound();
        return View(new EmployeeViewModel
        {
            Id = employee.Id, FirstName = employee.FirstName, LastName = employee.LastName,
            Email = employee.Email, Phone = employee.Phone, Position = employee.Position,
            HireDate = employee.HireDate, IsActive = employee.IsActive, Notes = employee.Notes,
            Positions = GetPositionsList()
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EmployeeViewModel model)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid) { model.Positions = GetPositionsList(); return View(model); }
        await _employeeService.UpdateAsync(new EmployeeServiceModel
        {
            Id = model.Id, FirstName = model.FirstName, LastName = model.LastName, Email = model.Email,
            Phone = model.Phone, Position = model.Position, IsActive = model.IsActive, Notes = model.Notes,
            HireDate = model.HireDate
        });
        TempData["Success"] = "Employee updated.";
        return RedirectToAction(nameof(All));
    }

    private static IEnumerable<SelectListItem> GetPositionsList() =>
        Enum.GetValues<EmployeePosition>().Select(p => new SelectListItem(p.ToString(), p.ToString()));
}
