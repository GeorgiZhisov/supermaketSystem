using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupermarketManagerSystem.Services.Contracts;
using SupermarketManagerSystem.Services.Models;
using SupermarketManagerSystem.Web.ViewModels;

namespace SupermarketManagerSystem.Web.Controllers;

[Authorize]
public class SuppliersController : Controller
{
    private readonly ISupplierService _supplierService;

    public SuppliersController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    public async Task<IActionResult> All()
    {
        var suppliers = await _supplierService.GetAllAsync();
        var viewModels = suppliers.Select(s => new SupplierViewModel
        {
            Id = s.Id, Name = s.Name, ContactPerson = s.ContactPerson,
            Email = s.Email, Phone = s.Phone, Address = s.Address,
            IsActive = s.IsActive, ProductCount = s.ProductCount
        });
        return View(viewModels);
    }

    public async Task<IActionResult> Details(int id)
    {
        var supplier = await _supplierService.GetByIdAsync(id);
        if (supplier == null) return NotFound();
        return View(new SupplierViewModel
        {
            Id = supplier.Id, Name = supplier.Name, ContactPerson = supplier.ContactPerson,
            Email = supplier.Email, Phone = supplier.Phone, Address = supplier.Address,
            IsActive = supplier.IsActive, ProductCount = supplier.ProductCount
        });
    }

    [Authorize(Roles = "Administrator")]
    public IActionResult Add() => View(new SupplierViewModel());

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(SupplierViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        await _supplierService.CreateAsync(new SupplierServiceModel
        {
            Name = model.Name, ContactPerson = model.ContactPerson, Email = model.Email,
            Phone = model.Phone, Address = model.Address, IsActive = model.IsActive
        });
        TempData["Success"] = $"Supplier \"{model.Name}\" was added.";
        return RedirectToAction(nameof(All));
    }

    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Edit(int id)
    {
        var supplier = await _supplierService.GetByIdAsync(id);
        if (supplier == null) return NotFound();
        return View(new SupplierViewModel
        {
            Id = supplier.Id, Name = supplier.Name, ContactPerson = supplier.ContactPerson,
            Email = supplier.Email, Phone = supplier.Phone, Address = supplier.Address, IsActive = supplier.IsActive
        });
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SupplierViewModel model)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid) return View(model);
        await _supplierService.UpdateAsync(new SupplierServiceModel
        {
            Id = model.Id, Name = model.Name, ContactPerson = model.ContactPerson, Email = model.Email,
            Phone = model.Phone, Address = model.Address, IsActive = model.IsActive
        });
        TempData["Success"] = $"Supplier \"{model.Name}\" was updated.";
        return RedirectToAction(nameof(All));
    }
}
