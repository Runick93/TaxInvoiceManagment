using AntDesign;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using TaxInvoiceManagment.Presentation.Web.Models;

namespace TaxInvoiceManagment.Presentation.Web.Pages.Account.Center
{
    public partial class Applications
    {
        private readonly ListGridType _listGridType = new ListGridType
        {
            Gutter = 24,
            Column = 4
        };

        [Parameter] public IList<ListItemDataType> List { get; set; }
    }
}