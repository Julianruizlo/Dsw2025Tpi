using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities;


public class Product : EntityBase
{
    public Product(string sku, string internalCode, string name, string description, decimal currentUnitPrice, int stockQuantity, bool isActive)
    {
        Sku = sku;
        InternalCode = internalCode;
        Name = name;
        Description = description;
        CurrentUnitPrice = currentUnitPrice;
        StockQuantity = stockQuantity;
        IsActive = isActive;
    }

    public string Sku { get; set; }
    public string InternalCode { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal CurrentUnitPrice
    {
        get => CurrentUnitPrice;
        set
        {
            if (value <= 0)
                throw new ArgumentException("El precio debe ser mayor a 0.");
            CurrentUnitPrice = value;
        }
    }
    public int StockQuantity
    {
        get => StockQuantity;
        set
        {
            if (value < 0)
                throw new ArgumentException("La cantidad de stock no puede ser negativa.");
            StockQuantity= value;
        }
    }
    public bool IsActive { get; set; }



    public ICollection<OrderItem>? Items { get; set; }

    public void Toggle()
    {
       IsActive = !IsActive;
    }
}
