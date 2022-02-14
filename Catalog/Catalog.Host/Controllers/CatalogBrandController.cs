using System.Net;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Response;
using Catalog.Host.Services.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers;

[ApiController]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogBrandController : ControllerBase
{
    private readonly ILogger<CatalogBrandController> _logger;
    private readonly ICatalogBrandService _catalogBrandService;

    public CatalogBrandController(
        ILogger<CatalogBrandController> logger,
        ICatalogBrandService brandItemService)
    {
        _logger = logger;
        _catalogBrandService = brandItemService;
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> Add(CreateBrandRequest request)
    {
        await _catalogBrandService.Add(request.Brand);
        return Ok();
    }

    [HttpDelete]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> Delete(RemoveBrandRequest request)
    {
        await _catalogBrandService.Remove(request.Id);
        return Ok();
    }

    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(UpdateBrandRequest request)
    {
        await _catalogBrandService.Update(request.Id, request.Brand);
        return Ok();
    }
}