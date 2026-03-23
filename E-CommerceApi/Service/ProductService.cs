using E_CommerceApi.DTOs.Product;
using E_CommerceApi.Interfaces;
using E_CommerceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceApi.Service;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResponse<ProductResponseDto>> GetAllProducts(
        int page,
        int pageSize,
        string? category,
        int? miniPrice)
    {
        
        // 1. Get all
        var query = await _repository.GetAllProductsAsync();
      
      if (!string.IsNullOrEmpty(category))
      {
          query = query.Where(x => x.Category == category);
      }

      if (miniPrice != null)
      {
          query = query.Where(x => x.Price >= miniPrice.Value);
      }

      var totalCount = await query.CountAsync();
      var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
      
      var items = await query
          .Skip((page - 1) * pageSize)
          .Take(pageSize)
          .Select(p => new ProductResponseDto(
              p.Id,
              p.Name,
              p.Price,
              p.Description,
              p.Category,
              p.Stock))
          .ToListAsync();

      var meta = new PaginationMetaDto(
          page,
          pageSize,
          totalPages,
          totalCount,
          HasNext: page < totalPages,
          HasPrevious: page > 1
      );

      return new PagedResponse<ProductResponseDto>(items, meta);
    }

    public async Task<ProductResponseDto> GetProductById(int id)
    {
        var product = await _repository.GetProductByIdAsync(id);
        
        return new ProductResponseDto(
            product.Id,
            product.Name,
            product.Price,
            product.Description,
            product.Category,
            product.Stock
        );
    }

    public async Task<ProductResponseDto> CreateProduct(ProductRequestDto request)
    {
        var newProduct = new Product
        {
            Name = request.Name,
            Price = request.Price,
            Description = request.Description,
            Category = request.Category,
            Stock = request.Stock
        }; 
        
       var createdProduct = await _repository.CreateProductAsync(newProduct);

        return new ProductResponseDto(
            createdProduct.Id,
            createdProduct.Name,
            createdProduct.Price,
            createdProduct.Description,
            createdProduct.Category,
            createdProduct.Stock
        );
    }

    public async Task<bool> UpdateProduct(int id, ProductUpdateDto request)
    {
        var product = new Product
        {
            Id = id,
            Name = request.Name,
            Price = request.Price,
            Description = request.Description,
            Category = request.Category,
            Stock = request.Stock
        };
        return await _repository.UpdateProductAsync(product);
    }

    public async Task<bool> DeleteProduct(int id)
    {
        return await _repository.DeleteProductAsync(id);
    }
}