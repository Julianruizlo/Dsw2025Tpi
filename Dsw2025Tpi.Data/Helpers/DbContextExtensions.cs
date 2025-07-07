﻿using Dsw2025Tpi.Data;
using Dsw2025Tpi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Data.Helpers
{
    public static class DbContextExtensions //Si no anda hacer como facundo, no usar generico...
    {
        public static void Seedwork<T>(this Dsw2025TpiContext context, string dataSource) where T : class
        {
            if (context.Set<T>().Any()) return;
            var json = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, dataSource));
            var entities = JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });
            if (entities == null || entities.Count == 0) return;
            context.Set<T>().AddRange(entities);
            context.SaveChanges();
        }
    }
}
