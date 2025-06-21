using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;

namespace Dsw2025Tpi.Data.Repositories
{
    public class InMemory : IRepository
    {
        private List<Product>? _products;
        private List<Order>? _orders;
        private List<Customer>? _customers;
        private List<OrderItem>? _orderItems;

        public InMemory()
        {
            LoadProducts();
        }

        private void LoadProducts()
        {
            var json = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Sources\\products.json"));
            _products = JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });
        }

        private List<T>? GetList<T>() where T : EntityBase
        {
            return typeof(T).Name switch
            {
                nameof(Product) => _products as List<T>,
                _ => throw new NotSupportedException(),
            };
        }

        public async Task<T?> GetById<T>(Guid id, params string[] include) where T : EntityBase
        {
            return await Task.FromResult(GetList<T>()?.FirstOrDefault(e => e.Id == id));
        }

        public async Task<IEnumerable<T>?> GetAll<T>(params string[] include) where T : EntityBase
        {
            return await Task.FromResult(GetList<T>());
        }

        public async Task<T> Add<T>(T entity) where T : EntityBase
        {
            GetList<T>()?.Add(entity);
            return await Task.FromResult(entity);
        }

        public Task<T> Update<T>(T entity) where T : EntityBase
        {
            throw new NotImplementedException();
        }

        public Task<T> Delete<T>(T entity) where T : EntityBase
        {
            throw new NotImplementedException();
        }

        public async Task<T?> First<T>(Expression<Func<T, bool>> predicate, params string[] include) where T : EntityBase
        {
            var product = GetList<T>()?.FirstOrDefault(predicate.Compile());
            return await Task.FromResult(product);
        }

        public async Task<IEnumerable<T>?> GetFiltered<T>(Expression<Func<T, bool>> predicate, params string[] include) where T : EntityBase
        {
            var products = GetList<T>()?.Where(predicate.Compile());
            return await Task.FromResult(products);
        }
    }
}
