using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Dtos;

public record OrderModel
{
    public record Request( DateTime Date, string? ShippingAddres, string? BillingAddress, string? Notes, Guid CustomerId);

    public record Response(Guid Id, DateTime Date, string? ShippingAddres, string? BillingAddress, string? Notes, Guid CustomerId);

}
