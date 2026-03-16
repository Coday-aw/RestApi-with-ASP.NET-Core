using E_CommerceApi.DTOs.Product;
using E_CommerceApi.Models;

namespace E_CommerceApi.Interfaces;

public interface IProductRepository
{
    Task<IQueryable<Product>> GetAllProductsAsync();

    Task<Product> GetProductByIdAsync(int id);

    Task<Product> CreateProductAsync(Product product);

    Task<bool> UpdateProductAsync(Product product);

    Task<bool> DeleteProductAsync(int id);
}