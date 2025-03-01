﻿using Microsoft.EntityFrameworkCore;

namespace TaxInvoiceManagment.Persistence.Tests
{
    public class DbContextHelper
    {
        public static TaxInvoiceManagmentDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<TaxInvoiceManagmentDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Genera un nombre único para cada prueba
                .Options;

            var context = new TaxInvoiceManagmentDbContext(options);
            context.Database.EnsureCreated(); // Crea las tablas en la base de datos
            return context;
        }
    }
}
