using ECommerce.Application.DTOs.Product;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Interfaces;

namespace ECommerce.Application.Services;

public class ProductService(IUnitOfWork unitOfWork)
{
    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await unitOfWork.Repository<Product>().GetAllAsync();
        
        return products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            StockQuantity = p.StockQuantity,
            ImageUrl = p.ImageUrl,
            IsActive = p.IsActive,
            CategoryId = p.CategoryId,
            CategoryName = p.Category?.Name ?? string.Empty
        });
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await unitOfWork.Repository<Product>().GetByIdAsync(id);
        if (product is null) return null;

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            ImageUrl = product.ImageUrl,
            IsActive = product.IsActive,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name ?? string.Empty
        };
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        // Business Logic — Validation
        if (dto.Price <= 0)
            throw new ArgumentException("Price must be greater than zero");

        if (dto.StockQuantity < 0)
            throw new ArgumentException("Stock quantity cannot be negative");

        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            StockQuantity = dto.StockQuantity,
            ImageUrl = dto.ImageUrl,
            CategoryId = dto.CategoryId
        };

        await unitOfWork.Repository<Product>().AddAsync(product);
        await unitOfWork.SaveChangesAsync();

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            ImageUrl = product.ImageUrl,
            IsActive = product.IsActive,
            CategoryId = product.CategoryId
        };
    }

    public async Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto)
    {
        var product = await unitOfWork.Repository<Product>().GetByIdAsync(id);
        if (product is null) return null;

        // Business Logic — Validation
        if (dto.Price is not null && dto.Price <= 0)
            throw new ArgumentException("Price must be greater than zero");

        if (dto.StockQuantity is not null && dto.StockQuantity < 0)
            throw new ArgumentException("Stock quantity cannot be negative");

        // Update only provided fields
        if (dto.Name is not null) product.Name = dto.Name;
        if (dto.Description is not null) product.Description = dto.Description;
        if (dto.Price is not null) product.Price = dto.Price.Value;
        if (dto.StockQuantity is not null) product.StockQuantity = dto.StockQuantity.Value;
        if (dto.ImageUrl is not null) product.ImageUrl = dto.ImageUrl;
        if (dto.IsActive is not null) product.IsActive = dto.IsActive.Value;

        unitOfWork.Repository<Product>().Update(product);
        await unitOfWork.SaveChangesAsync();

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            ImageUrl = product.ImageUrl,
            IsActive = product.IsActive,
            CategoryId = product.CategoryId
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await unitOfWork.Repository<Product>().GetByIdAsync(id);
        if (product is null) return false;

        unitOfWork.Repository<Product>().Remove(product); // Soft Delete
        await unitOfWork.SaveChangesAsync();
        return true;
    }
}