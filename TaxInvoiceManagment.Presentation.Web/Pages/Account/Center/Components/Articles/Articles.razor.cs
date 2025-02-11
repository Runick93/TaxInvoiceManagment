using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using TaxInvoiceManagment.Presentation.Web.Models;

namespace TaxInvoiceManagment.Presentation.Web.Pages.Account.Center
{
    public partial class Articles
    {
        [Parameter] public IList<ListItemDataType> List { get; set; }
    }
}