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
        // cache key is the product id
        var cacheKey = $"product_{id}";
        // check if the product is cached if not get product from database 
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
        
        return product;
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        // create new product
        var newProduct = new Product
        {
            Name = product.Name,
            Price = product.Price,
            Description = product.Description,
            Category = product.Category,
            Stock = product.Stock
        };

        // add to database 
        await _context.Products.AddAsync(newProduct);
        // save changes
        await _context.SaveChangesAsync();
        
        return newProduct;
    }

    public async Task<bool> UpdateProductAsync( Product product)
    {
        // find product
        var findProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
        // if not found throw not found exception 
        if (findProduct == null)
            throw new NotFoundException($"product with id {product.Id} not found");
        // update the product with new changes
        findProduct.Name = product.Name;
        findProduct.Price = product.Price;
        findProduct.Description = product.Description;
        findProduct.Category = product.Category;
        findProduct.Stock = product.Stock;
        
        //  save changes and remove from cached
        await _context.SaveChangesAsync();
        await _cache.RemoveAsync($"product_{product.Id}");
       
        return true;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        //  find product 
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        //  if not found throw exception 
        if (product == null) 
            throw new  NotFoundException($"product with id {id} not found");
        // remove product
        _context.Products.Remove(product);
        // save changes 
        await _context.SaveChangesAsync();
        // remove from cached
        await _cache.RemoveAsync($"product_{id}");

        return true;
    }
}