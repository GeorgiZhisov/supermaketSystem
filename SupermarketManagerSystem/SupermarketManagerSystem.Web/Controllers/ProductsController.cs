using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SupermarketManagerSystem.Services.Contracts;
using SupermarketManagerSystem.Services.Models;
using SupermarketManagerSystem.Web.ViewModels;

namespace SupermarketManagerSystem.Web.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly ISupplierService _supplierService;

    public ProductsController(IProductService productService, ICategoryService categoryService, ISupplierService supplierService)
    {
        _productService = productService;
        _categoryService = categoryService;
        _supplierService = supplierService;
    }

    public async Task<IActionResult> All(string? search, int? categoryId)
    {
        var products = await _productService.GetAllAsync(search, categoryId);
        var categories = await _categoryService.GetAllAsync();

        ViewBag.SearchTerm = search;
        ViewBag.SelectedCategory = categoryId;
        ViewBag.Categories = new SelectList(categories, "Id", "Name", categoryId);

        var viewModels = products.Select(p => new ProductListViewModel
        {
            Id = p.Id, Name = p.Name, Barcode = p.Barcode, Price = p.Price,
            StockQuantity = p.StockQuantity, MinimumStockLevel = p.MinimumStockLevel,
            CategoryName = p.CategoryName, SupplierName = p.SupplierName, IsActive = p.IsActive
        });
        return View(viewModels);
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound();
        return View(product);
    }

    [Authorize]
    public async Task<IActionResult> Add()
    {
        var vm = await BuildFormViewModel(new ProductFormViewModel());
        return View(vm);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(ProductFormViewModel model)
    {
        if (!ModelState.IsValid)
            return View(await BuildFormViewModel(model));

        var serviceModel = MapToServiceModel(model);
        var id = await _productService.CreateAsync(serviceModel);
        TempData["Success"] = $"Product \"{model.Name}\" was added successfully.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound();

        var vm = await BuildFormViewModel(new ProductFormViewModel
        {
            Id = product.Id, Name = product.Name, Description = product.Description,
            Barcode = product.Barcode, Price = product.Price, CostPrice = product.CostPrice,
            StockQuantity = product.StockQuantity, MinimumStockLevel = product.MinimumStockLevel,
            Unit = product.Unit, ImageUrl = product.ImageUrl, IsActive = product.IsActive,
            CategoryId = product.CategoryId, SupplierId = product.SupplierId
        });
        return View(vm);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductFormViewModel model)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid)
            return View(await BuildFormViewModel(model));

        var serviceModel = MapToServiceModel(model);
        var success = await _productService.UpdateAsync(serviceModel);
        if (!success) return NotFound();

        TempData["Success"] = $"Product \"{model.Name}\" was updated successfully.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound();
        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Administrator")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var success = await _productService.DeleteAsync(id);
        if (!success) return NotFound();
        TempData["Success"] = "Product was deactivated successfully.";
        return RedirectToAction(nameof(All));
    }

    private async Task<ProductFormViewModel> BuildFormViewModel(ProductFormViewModel vm)
    {
        var categories = await _categoryService.GetAllAsync();
        var suppliers = await _supplierService.GetAllAsync(activeOnly: true);
        vm.Categories = categories.Select(c => new SelectListItem(c.Name, c.Id.ToString()));
        vm.Suppliers = suppliers.Select(s => new SelectListItem(s.Name, s.Id.ToString()));
        return vm;
    }

    private static ProductServiceModel MapToServiceModel(ProductFormViewModel vm) => new()
    {
        Id = vm.Id, Name = vm.Name, Description = vm.Description, Barcode = vm.Barcode,
        Price = vm.Price, CostPrice = vm.CostPrice, StockQuantity = vm.StockQuantity,
        MinimumStockLevel = vm.MinimumStockLevel, Unit = vm.Unit, ImageUrl = vm.ImageUrl,
        IsActive = vm.IsActive, CategoryId = vm.CategoryId, SupplierId = vm.SupplierId
    };
}
