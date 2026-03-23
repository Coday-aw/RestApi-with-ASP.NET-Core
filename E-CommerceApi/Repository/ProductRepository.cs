using E_CommerceApi.Data;
using E_CommerceApi.DTOs.Product;
using E_CommerceApi.Exceptions;
using E_CommerceApi.Models;
using E_CommerceApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

namespace E_CommerceApi.Repository;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;
    private readonly HybridCache _cache;

    public ProductRepository(ApplicationDbContext context, HybridCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<IQueryable<Product>> GetAllProductsAsync()
    {
        return await Task.FromResult(_context.Products.AsQueryable());
    }
    

    public async Task<Product> GetProductByIdAsync(int id)
    {
        
        var cacheKey = $"product_{id}";

       
        var product = await _cache.GetOrCreateAsync(
            cacheKey,
            async cancel =>
            {
                return await _context.Products.FirstOrDefaultAsync(p => p.Id == id, cancel);
            },
            new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(5)
            });
        
        if (product == null) throw new NotFoundException($"product with id {id} not found");
        
        return product;
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        var newProduct = new Product
        {
            Name = product.Name,
            Price = product.Price,
            Description = product.Description,
            Category = product.Category,
            Stock = product.Stock
        };

        await _context.Products.AddAsync(newProduct);
        await _context.SaveChangesAsync();
        
        return newProduct;
    }

    public async Task<bool> UpdateProductAsync( Product product)
    {
        var findProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);

        if (findProduct == null) 
            throw new NotFoundException($"product with id {product.Id} not found");

        findProduct.Name = product.Name;
        findProduct.Price = product.Price;
        findProduct.Description = product.Description;
        findProduct.Category = product.Category;
        findProduct.Stock = product.Stock;
        
        await _context.SaveChangesAsync();
        await _cache.RemoveAsync($"product_{product.Id}");
       
        return true;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (product == null) 
            throw new  NotFoundException($"product with id {id} not found");

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        await _cache.RemoveAsync($"product_{id}");

        return true;
    }
}