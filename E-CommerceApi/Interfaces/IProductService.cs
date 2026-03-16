using E_CommerceApi.DTOs.Product;
namespace E_CommerceApi.Interfaces;

public interface IProductService
{
    Task<PagedResponse<ProductResponseDto>> GetAllProducts(int page, int pageSize, string? category,
        int? miniPrice);

    Task<ProductResponseDto> GetProductById(int id);

    Task<ProductResponseDto> CreateProduct(ProductRequestDto request);

    Task<bool> UpdateProduct(int id, ProductUpdateDto request);
    Task<bool> DeleteProduct(int id);
}