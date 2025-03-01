﻿using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Domain.Entities;

namespace TaxInvoiceManagment.Persistence.Repositories
{
    public class TaxableItemRepository : Repository<TaxableItem>, ITaxableItemRepository
    {
        private readonly TaxInvoiceManagmentDbContext _context;

        public TaxableItemRepository(TaxInvoiceManagmentDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
