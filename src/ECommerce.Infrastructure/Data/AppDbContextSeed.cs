using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Data;

public static class AppDbContextSeed
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (!await context.Categories.AnyAsync())
        {
            var electronics = new Category { Name = "Electronics", Description = "Electronic devices and gadgets" };
            var clothing = new Category { Name = "Clothing", Description = "Fashion and apparel" };
            var books = new Category { Name = "Books", Description = "All kinds of books" };
            var furniture = new Category { Name = "Furniture", Description = "Home and office furniture" };
            var sports = new Category { Name = "Sports", Description = "Sports and fitness equipment" };
            var beauty = new Category { Name = "Beauty", Description = "Beauty and personal care" };

            await context.Categories.AddRangeAsync(electronics, clothing, books, furniture, sports, beauty);
            await context.SaveChangesAsync();

            // Sub-Categories
            var subCategories = new List<Category>
            {
                // Electronics
                new() { Name = "Phones", Description = "Smartphones and accessories", ParentCategoryId = electronics.Id },
                new() { Name = "Laptops", Description = "Laptops and notebooks", ParentCategoryId = electronics.Id },
                new() { Name = "TVs", Description = "Smart TVs and displays", ParentCategoryId = electronics.Id },
                new() { Name = "Cameras", Description = "Digital cameras and lenses", ParentCategoryId = electronics.Id },
                new() { Name = "Audio", Description = "Headphones and speakers", ParentCategoryId = electronics.Id },

                // Clothing
                new() { Name = "Men's Clothing", Description = "Fashion for men", ParentCategoryId = clothing.Id },
                new() { Name = "Women's Clothing", Description = "Fashion for women", ParentCategoryId = clothing.Id },
                new() { Name = "Kids' Clothing", Description = "Fashion for kids", ParentCategoryId = clothing.Id },
                new() { Name = "Shoes", Description = "All kinds of shoes", ParentCategoryId = clothing.Id },

                // Books
                new() { Name = "Programming", Description = "Software development books", ParentCategoryId = books.Id },
                new() { Name = "Science", Description = "Science and nature books", ParentCategoryId = books.Id },
                new() { Name = "Business", Description = "Business and finance books", ParentCategoryId = books.Id },
                new() { Name = "Fiction", Description = "Novels and stories", ParentCategoryId = books.Id },

                // Furniture
                new() { Name = "Bedroom", Description = "Bedroom furniture", ParentCategoryId = furniture.Id },
                new() { Name = "Office", Description = "Office furniture", ParentCategoryId = furniture.Id },
                new() { Name = "Living Room", Description = "Living room furniture", ParentCategoryId = furniture.Id },

                // Sports
                new() { Name = "Gym Equipment", Description = "Home and gym fitness tools", ParentCategoryId = sports.Id },
                new() { Name = "Outdoor", Description = "Outdoor and camping gear", ParentCategoryId = sports.Id },
                new() { Name = "Team Sports", Description = "Football, basketball, etc.", ParentCategoryId = sports.Id },

                // Beauty
                new() { Name = "Skincare", Description = "Face and body skincare", ParentCategoryId = beauty.Id },
                new() { Name = "Haircare", Description = "Hair products and tools", ParentCategoryId = beauty.Id },
                new() { Name = "Makeup", Description = "Cosmetics and makeup", ParentCategoryId = beauty.Id },
            };

            await context.Categories.AddRangeAsync(subCategories);
            await context.SaveChangesAsync();

            // Products
            var products = new List<Product>
            {
                // Phones
                new() { Name = "iPhone 15 Pro", Description = "Apple iPhone 15 Pro 256GB", Price = 1199.99m, StockQuantity = 50, CategoryId = subCategories[0].Id },
                new() { Name = "Samsung Galaxy S24", Description = "Samsung flagship 2024", Price = 999.99m, StockQuantity = 40, CategoryId = subCategories[0].Id },
                new() { Name = "Google Pixel 8", Description = "Google Pixel 8 128GB", Price = 699.99m, StockQuantity = 30, CategoryId = subCategories[0].Id },
                new() { Name = "OnePlus 12", Description = "OnePlus 12 5G", Price = 799.99m, StockQuantity = 25, CategoryId = subCategories[0].Id },

                // Laptops
                new() { Name = "MacBook Pro M3", Description = "Apple MacBook Pro 14-inch M3", Price = 1999.99m, StockQuantity = 20, CategoryId = subCategories[1].Id },
                new() { Name = "Dell XPS 15", Description = "Dell XPS 15 OLED 2024", Price = 1799.99m, StockQuantity = 15, CategoryId = subCategories[1].Id },
                new() { Name = "Lenovo ThinkPad X1", Description = "ThinkPad X1 Carbon Gen 11", Price = 1599.99m, StockQuantity = 18, CategoryId = subCategories[1].Id },
                new() { Name = "ASUS ROG Zephyrus", Description = "Gaming laptop RTX 4070", Price = 2199.99m, StockQuantity = 10, CategoryId = subCategories[1].Id },

                // TVs
                new() { Name = "Samsung QLED 65\"", Description = "Samsung 65-inch QLED 4K", Price = 1299.99m, StockQuantity = 12, CategoryId = subCategories[2].Id },
                new() { Name = "LG OLED 55\"", Description = "LG 55-inch OLED evo", Price = 1499.99m, StockQuantity = 8, CategoryId = subCategories[2].Id },
                new() { Name = "Sony Bravia 75\"", Description = "Sony 75-inch 4K Mini LED", Price = 2499.99m, StockQuantity = 5, CategoryId = subCategories[2].Id },

                // Cameras
                new() { Name = "Sony A7 IV", Description = "Sony Alpha A7 IV Mirrorless", Price = 2499.99m, StockQuantity = 10, CategoryId = subCategories[3].Id },
                new() { Name = "Canon EOS R6", Description = "Canon EOS R6 Mark II", Price = 2299.99m, StockQuantity = 8, CategoryId = subCategories[3].Id },

                // Audio
                new() { Name = "AirPods Pro 2", Description = "Apple AirPods Pro 2nd Gen", Price = 249.99m, StockQuantity = 60, CategoryId = subCategories[4].Id },
                new() { Name = "Sony WH-1000XM5", Description = "Sony Noise Cancelling Headphones", Price = 349.99m, StockQuantity = 35, CategoryId = subCategories[4].Id },
                new() { Name = "Bose QC45", Description = "Bose QuietComfort 45", Price = 279.99m, StockQuantity = 40, CategoryId = subCategories[4].Id },

                // Men's Clothing
                new() { Name = "Levi's 501 Jeans", Description = "Classic straight fit jeans", Price = 69.99m, StockQuantity = 80, CategoryId = subCategories[5].Id },
                new() { Name = "Nike Polo Shirt", Description = "Nike Dri-FIT polo", Price = 45.99m, StockQuantity = 100, CategoryId = subCategories[5].Id },

                // Women's Clothing
                new() { Name = "Zara Floral Dress", Description = "Summer floral midi dress", Price = 59.99m, StockQuantity = 70, CategoryId = subCategories[6].Id },
                new() { Name = "H&M Blazer", Description = "Classic women's blazer", Price = 79.99m, StockQuantity = 50, CategoryId = subCategories[6].Id },

                // Shoes
                new() { Name = "Nike Air Max 270", Description = "Nike Air Max 270 sneakers", Price = 149.99m, StockQuantity = 60, CategoryId = subCategories[8].Id },
                new() { Name = "Adidas Ultraboost", Description = "Adidas Ultraboost 23", Price = 179.99m, StockQuantity = 45, CategoryId = subCategories[8].Id },

                // Programming Books
                new() { Name = "Clean Code", Description = "By Robert C. Martin", Price = 49.99m, StockQuantity = 30, CategoryId = subCategories[9].Id },
                new() { Name = "The Pragmatic Programmer", Description = "By David Thomas & Andrew Hunt", Price = 54.99m, StockQuantity = 25, CategoryId = subCategories[9].Id },
                new() { Name = "Design Patterns", Description = "Gang of Four", Price = 59.99m, StockQuantity = 20, CategoryId = subCategories[9].Id },
                new() { Name = "C# in Depth", Description = "By Jon Skeet", Price = 44.99m, StockQuantity = 35, CategoryId = subCategories[9].Id },

                // Office Furniture
                new() { Name = "Ergonomic Chair", Description = "Herman Miller Aeron Chair", Price = 1299.99m, StockQuantity = 15, CategoryId = subCategories[14].Id },
                new() { Name = "Standing Desk", Description = "Flexispot Electric Standing Desk", Price = 599.99m, StockQuantity = 20, CategoryId = subCategories[14].Id },

                // Gym Equipment
                new() { Name = "Adjustable Dumbbells", Description = "Bowflex 552 Adjustable Dumbbells", Price = 399.99m, StockQuantity = 25, CategoryId = subCategories[16].Id },
                new() { Name = "Yoga Mat", Description = "Lululemon 5mm Yoga Mat", Price = 88.99m, StockQuantity = 50, CategoryId = subCategories[16].Id },
                new() { Name = "Resistance Bands Set", Description = "5-piece resistance bands", Price = 29.99m, StockQuantity = 80, CategoryId = subCategories[16].Id },

                // Skincare
                new() { Name = "CeraVe Moisturizer", Description = "Daily moisturizing lotion", Price = 19.99m, StockQuantity = 100, CategoryId = subCategories[19].Id },
                new() { Name = "The Ordinary Serum", Description = "Hyaluronic acid 2% + B5", Price = 12.99m, StockQuantity = 90, CategoryId = subCategories[19].Id },
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }

        // Users
        if (!await context.Users.AnyAsync())
        {
            var users = new List<User>
            {
                new() { FirstName = "Ahmed", LastName = "Ali", Email = "ahmed@test.com", PasswordHash = "hashed_password" },
                new() { FirstName = "Sara", LastName = "Mohamed", Email = "sara@test.com", PasswordHash = "hashed_password" },
                new() { FirstName = "Omar", LastName = "Hassan", Email = "omar@test.com", PasswordHash = "hashed_password" },
            };
            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }
    }
}