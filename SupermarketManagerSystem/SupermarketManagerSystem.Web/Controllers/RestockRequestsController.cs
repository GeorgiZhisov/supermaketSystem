using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SupermarketManagerSystem.Services.Contracts;
using SupermarketManagerSystem.Services.Models;
using SupermarketManagerSystem.Web.ViewModels;

namespace SupermarketManagerSystem.Web.Controllers;

[Authorize]
public class RestockRequestsController : Controller
{
    private readonly IRestockRequestService _restockService;
    private readonly IProductService _productService;
    private readonly ISupplierService _supplierService;

    public RestockRequestsController(IRestockRequestService restockService, IProductService productService, ISupplierService supplierService)
    {
        _restockService = restockService;
        _productService = productService;
        _supplierService = supplierService;
    }

    public async Task<IActionResult> All(string? status)
    {
        var requests = await _restockService.GetAllAsync(status);
        ViewBag.StatusFilter = status;
        var viewModels = requests.Select(r => new RestockRequestViewModel
        {
            Id = r.Id, ProductId = r.ProductId, ProductName = r.ProductName,
            SupplierId = r.SupplierId, SupplierName = r.SupplierName,
            RequestedQuantity = r.RequestedQuantity, EstimatedCost = r.EstimatedCost,
            Status = r.Status, Notes = r.Notes, RequestedOn = r.RequestedOn,
            ApprovedOn = r.ApprovedOn, ReceivedOn = r.ReceivedOn
        });
        return View(viewModels);
    }

    public async Task<IActionResult> Add()
    {
        return View(await BuildFormViewModel(new RestockRequestViewModel()));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(RestockRequestViewModel model)
    {
        if (!ModelState.IsValid)
            return View(await BuildFormViewModel(model));

        await _restockService.CreateAsync(new RestockRequestServiceModel
        {
            ProductId = model.ProductId, SupplierId = model.SupplierId,
            RequestedQuantity = model.RequestedQuantity, EstimatedCost = model.EstimatedCost,
            Notes = model.Notes
        });
        TempData["Success"] = "Restock request submitted.";
        return RedirectToAction(nameof(All));
    }

    [Authorize(Roles = "Administrator")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id)
    {
        var success = await _restockService.ApproveAsync(id);
        TempData[success ? "Success" : "Error"] = success ? "Request approved." : "Could not approve request.";
        return RedirectToAction(nameof(All));
    }

    [Authorize(Roles = "Administrator")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id)
    {
        var success = await _restockService.CancelAsync(id);
        TempData[success ? "Success" : "Error"] = success ? "Request cancelled." : "Could not cancel request.";
        return RedirectToAction(nameof(All));
    }

    [Authorize(Roles = "Administrator")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkReceived(int id)
    {
        var success = await _restockService.MarkAsReceivedAsync(id);
        TempData[success ? "Success" : "Error"] = success ? "Request marked as received." : "Error.";
        return RedirectToAction(nameof(All));
    }

    private async Task<RestockRequestViewModel> BuildFormViewModel(RestockRequestViewModel vm)
    {
        var products = await _productService.GetAllAsync(activeOnly: true);
        var suppliers = await _supplierService.GetAllAsync(activeOnly: true);
        vm.Products = products.Select(p => new SelectListItem($"{p.Name} (Stock: {p.StockQuantity})", p.Id.ToString()));
        vm.Suppliers = suppliers.Select(s => new SelectListItem(s.Name, s.Id.ToString()));
        return vm;
    }
}
