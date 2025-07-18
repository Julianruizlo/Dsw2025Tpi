﻿using Dsw2025Tpi.Application.Dtos;
using System;

namespace Dsw2025Tpi.Application.Validation
{
    public static class OrderValidator
    {
        public static void Validate(OrderModel.RequestOrderModel request)
        {
            if (request == null)
                throw new InvalidOperationException("La orden no puede ser nula.");

            if (request.CustomerId == Guid.Empty)
                throw new InvalidOperationException("El cliente es obligatorio.");

            if (string.IsNullOrWhiteSpace(request.ShippingAddress) || request.ShippingAddress.Length > 256)
                throw new InvalidOperationException("La dirección de envío es obligatoria y no puede superar los 256 caracteres.");

            if (string.IsNullOrWhiteSpace(request.BillingAddress) || request.BillingAddress.Length > 256)
                throw new InvalidOperationException("La dirección de facturación es obligatoria y no puede superar los 256 caracteres.");

            if (request.Items == null || request.Items.Count == 0)
                throw new InvalidOperationException("Debe incluir al menos un ítem en la orden.");
        }
    }
}
