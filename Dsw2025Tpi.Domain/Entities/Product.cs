using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities;

 
public class Product : EntityBase
{

    public Product(string sku, string internalCode, string name, string description, decimal currentUnitPrice, int stockQuantity, bool isAtive)
        {

          
            Sku = sku;
            InternalCode = internalCode;
            Name = name;
            Description = description;
            CurrentUnitPrice = currentUnitPrice;
            StockQuantity = stockQuantity;
            IsAtive = isAtive;
        }
    public string? Sku { get; set; } 
    public string? InternalCode { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal CurrentUnitPrice { get; set; }
    public int StockQuantity { get; set; }
    public bool IsAtive { get; set; }


    
    public ICollection<OrderItem>? Items { get; set; }
}
