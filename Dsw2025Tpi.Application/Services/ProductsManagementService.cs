using Dsw2025Ej15.Application.Exceptions;
using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Services
{
    public class ProductsManagementService
    {
        private readonly IRepository _repository;

        public ProductsManagementService(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<ProductModel.Response?> GetProductById(Guid id)
        {
            var product = await _repository.GetById<Product>(id, nameof(Product));
            return product != null ?
                new ProductModel.Response(product.Id, product.Sku,product.InternalCode, product.Name, product.Description, product.CurrentUnitPrice, product.StockQuantity,product.IsActive ) :
                null;
        }

        public async Task<IEnumerable<ProductModel.Response>?> GetProducts()
        {
            return (await _repository
                .GetFiltered<Product>(p => p.IsActive, nameof(Product)))?
                .Select(p => new ProductModel.Response(p.Id, p.Sku, p.InternalCode, p.Name, p.Description,
                p.CurrentUnitPrice, p.StockQuantity,p.IsActive));
        }

        public async Task<ProductModel.Response> AddProduct(ProductModel.Request request)
        {
            if (string.IsNullOrWhiteSpace(request.Sku) || string.IsNullOrWhiteSpace(request.InternalCode) ||
                string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Description) ||
                request.CurrentUnitPrice <= 0 || request.StockQuantity < 0 ||
                !request.IsActive
                ) 
            {

                throw new ArgumentException("Valores para el producto no válidos");
            }


            var exist = await _repository.First<Product>(p => p.Sku == request.Sku);
            if (exist != null) throw new DuplicatedEntityException($"Ya existe un producto con el Sku {request.Sku}");
            var product = new Product(request.Sku, request.InternalCode, request.Name, request.Description, request.CurrentUnitPrice, request.StockQuantity, request.IsActive);
            await _repository.Add(product);
            return new ProductModel.Response(product.Id, product.Sku, product.InternalCode, product.Name, product.Description,
                product.CurrentUnitPrice, product.StockQuantity, product.IsActive);
        }
    }
}
