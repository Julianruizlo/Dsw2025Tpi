using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Data.Repositories;
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
        //Como actualizar un producto por id y no por sku, en caso de que lo hagamos por sku, habria que arreglar la ultima parte para que no actualice el id cada vez que haya cambios
        public async Task<ProductModel.Response> UpdateProduct(Guid id, ProductModel.Request request)
        {
            var exist = await _repository.GetById<Product>(id);
            if (exist == null)
                throw new EntityNotFoundException($"No se encontró un producto con el ID: {id}");

            if (string.IsNullOrWhiteSpace(request.Sku) ||
                string.IsNullOrWhiteSpace(request.InternalCode) ||
                string.IsNullOrWhiteSpace(request.Name) ||
                string.IsNullOrWhiteSpace(request.Description) ||
                request.CurrentUnitPrice <= 0 ||
                request.StockQuantity < 0)
            {
                throw new ArgumentException("Valores para el producto no válidos");
            }

            // Actualiza solo las propiedades necesarias, el Id no se toca
            exist.Sku = request.Sku;
            exist.InternalCode = request.InternalCode;
            exist.Name = request.Name;
            exist.Description = request.Description;
            exist.CurrentUnitPrice = request.CurrentUnitPrice;
            exist.StockQuantity = request.StockQuantity;
            exist.IsActive = request.IsActive;

            await _repository.Update(exist);

            return new ProductModel.Response
           (
                exist.Id,
                exist.Sku,
                exist.InternalCode,
                exist.Name,
                exist.Description,
                exist.CurrentUnitPrice,
                exist.StockQuantity,
                exist.IsActive
            );
        }

        public async Task<ProductModel.Response> PatchProduct(Guid id,ProductModel.Request request)
        {
            var exist = await _repository.GetById<Product>(id);
            if (exist == null)
                throw new EntityNotFoundException($"No se encontró un producto con el ID: {id}");

            if (string.IsNullOrWhiteSpace(request.Sku) || string.IsNullOrWhiteSpace(request.InternalCode) ||
                string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Description) ||
                request.CurrentUnitPrice <= 0 || request.StockQuantity < 0 ||
                !request.IsActive
                )
            {

                throw new ArgumentException("Valores para el producto no válidos");
            }

            var product = new Product(request.Sku, request.InternalCode, request.Name, request.Description, request.CurrentUnitPrice, request.StockQuantity, request.IsActive);
            product.IsActive = false;
            await _repository.Update(product);

            return new ProductModel.Response(
                product.Id,
                product.Sku,
                product.InternalCode,
                product.Name,
                product.Description,
                product.CurrentUnitPrice,
                product.StockQuantity,
                product.IsActive
            );
        }


    }
}
