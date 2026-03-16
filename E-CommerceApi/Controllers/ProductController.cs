using E_CommerceApi.DTOs.Product;
using E_CommerceApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceApi.Controllers;

[Route("api/products")]
[ApiController]
public class ProductController : ControllerBase
{
   private readonly IProductService _productService;

   public ProductController(IProductService productService)
   {
      _productService = productService;
   }

   [Authorize(Roles = "Admin")]
   [HttpPost]
   public async Task<ActionResult<ProductResponseDto>> CreateProduct([FromBody] ProductRequestDto productRequest)
   {
      var response  =  await _productService.CreateProduct(productRequest);
      return CreatedAtAction(nameof(GetProductById), new { id = response.Id }, response);
   }

   [HttpGet]
   public async Task<ActionResult<PagedResponse<ProductResponseDto>>> GetAllProductsAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string?  category = null,[FromQuery] int? miniPrice = null)
   {
      pageSize = Math.Clamp(pageSize, 1, 100);
      var products = await _productService.GetAllProducts(page, pageSize, category, miniPrice);
      return Ok(products);
   }
   
   [HttpGet("{id}")]
   public async Task<ActionResult<ProductResponseDto>> GetProductById(int id)
   {
      var response = await _productService.GetProductById(id);
      return Ok(response);
   }

   [Authorize(Roles = "Admin")]
   [HttpPut("{id}")]
   public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDto productRequest)
   { 
      await _productService.UpdateProduct(id, productRequest);
      return NoContent();
   }

   [Authorize(Roles = "Admin")]
   [HttpDelete("{id}")]
   public async Task<IActionResult> DeleteProduct(int id)
   {
      await _productService.DeleteProduct(id);
      return NoContent();
   }
}