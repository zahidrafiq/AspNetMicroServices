﻿using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;
        public ProductRepository(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products
                          .Find(prop => true)
                          .ToListAsync();
          
        }
        public async Task<Product> GetProduct(string id)
        {
            return await _context.Products
                    .Find(p => p.Id == id)
                    .FirstOrDefaultAsync();
        }
    
        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, categoryName);
            return await _context.Products
                    .Find(filter)
                    .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);
            return await _context.Products
                    .Find(filter)
                    .ToListAsync();
        }

        public async Task CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }


        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);

            var delResult = await _context.Products
                                .DeleteOneAsync(filter);

            return delResult.IsAcknowledged && delResult.DeletedCount > 0;
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateresult = await _context.Products
                                .ReplaceOneAsync(filter:x => x.Id==product.Id, replacement:product);

            return updateresult.IsAcknowledged
                && updateresult.ModifiedCount > 0;
        }
    }
}