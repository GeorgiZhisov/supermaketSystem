using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupermarketManagerSystem.Services.Contracts;
using SupermarketManagerSystem.Web.ViewModels;

namespace SupermarketManagerSystem.Web.Controllers;

public class CategoriesController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IActionResult> All()
    {
        var categories = await _categoryService.GetAllAsync();
        var viewModels = categories.Select(c => new CategoryViewModel
        {
            Id = c.Id, Name = c.Name, Description = c.Description, ProductCount = c.ProductCount
        });
        return View(viewModels);
    }

    [Authorize(Roles = "Administrator")]
    public IActionResult Add() => View(new CategoryViewModel());

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(CategoryViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        await _categoryService.CreateAsync(new Services.Models.CategoryServiceModel
        {
            Name = model.Name, Description = model.Description
        });
        TempData["Success"] = $"Category \"{model.Name}\" was added successfully.";
        return RedirectToAction(nameof(All));
    }

    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Edit(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null) return NotFound();
        return View(new CategoryViewModel { Id = category.Id, Name = category.Name, Description = category.Description });
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CategoryViewModel model)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid) return View(model);
        await _categoryService.UpdateAsync(new Services.Models.CategoryServiceModel
        {
            Id = model.Id, Name = model.Name, Description = model.Description
        });
        TempData["Success"] = $"Category \"{model.Name}\" was updated.";
        return RedirectToAction(nameof(All));
    }

    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null) return NotFound();
        return View(new CategoryViewModel { Id = category.Id, Name = category.Name, Description = category.Description });
    }

    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Administrator")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _categoryService.DeleteAsync(id);
        TempData["Success"] = "Category was deleted.";
        return RedirectToAction(nameof(All));
    }
}
