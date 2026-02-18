using ECommerce.Application.DTOs.Order;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Infrastructure.Interfaces;

namespace ECommerce.Application.Services;

public class OrderService(IUnitOfWork unitOfWork)
{
    public async Task<OrderDto?> GetByIdAsync(int id)
    {
        var order = await unitOfWork.Repository<Order>().GetByIdAsync(id);
        if (order is null) return null;

        return new OrderDto
        {
            Id = order.Id,
            UserId = order.UserId,
            Status = order.Status,
            TotalAmount = order.TotalAmount,
            ShippingAddress = order.ShippingAddress,
            CreatedAt = order.CreatedAt,
            Items = order.OrderItems.Select(oi => new OrderItemDto
            {
                ProductId = oi.ProductId,
                ProductName = oi.Product?.Name ?? string.Empty,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice
            }).ToList()
        };
    }

    public async Task<OrderDto> CreateAsync(CreateOrderDto dto)
    {
        var productRepo = unitOfWork.Repository<Product>();
        var orderItems = new List<OrderItem>();
        decimal totalAmount = 0;

        // Business Logic — التحقق والحسابات
        foreach (var item in dto.Items)
        {
            var product = await productRepo.GetByIdAsync(item.ProductId);
            
            if (product is null)
                throw new Exception($"Product with ID {item.ProductId} not found");

            if (product.StockQuantity < item.Quantity)
                throw new Exception($"Insufficient stock for product '{product.Name}'");

            if (!product.IsActive)
                throw new Exception($"Product '{product.Name}' is not active");

            // تحديث الـ Stock
            product.StockQuantity -= item.Quantity;
            productRepo.Update(product);

            // حساب السعر
            var unitPrice = product.Price;
            totalAmount += unitPrice * item.Quantity;

            orderItems.Add(new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = unitPrice
            });
        }

        // إنشاء الـ Order
        var order = new Order
        {
            UserId = dto.UserId,
            ShippingAddress = dto.ShippingAddress,
            TotalAmount = totalAmount,
            Status = OrderStatus.Pending,
            OrderItems = orderItems
        };

        await unitOfWork.Repository<Order>().AddAsync(order);
        
        // Save All — كل العمليات في Transaction واحدة
        await unitOfWork.SaveChangesAsync();

        return new OrderDto
        {
            Id = order.Id,
            UserId = order.UserId,
            Status = order.Status,
            TotalAmount = order.TotalAmount,
            ShippingAddress = order.ShippingAddress,
            CreatedAt = order.CreatedAt,
            Items = orderItems.Select(oi => new OrderItemDto
            {
                ProductId = oi.ProductId,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice
            }).ToList()
        };
    }

    public async Task<bool> UpdateStatusAsync(int id, OrderStatus newStatus)
    {
        var order = await unitOfWork.Repository<Order>().GetByIdAsync(id);
        if (order is null) return false;

        order.Status = newStatus;
        
        if (newStatus == OrderStatus.Shipped)
            order.ShippedAt = DateTime.UtcNow;
        
        if (newStatus == OrderStatus.Delivered)
            order.DeliveredAt = DateTime.UtcNow;

        unitOfWork.Repository<Order>().Update(order);
        await unitOfWork.SaveChangesAsync();
        return true;
    }
}