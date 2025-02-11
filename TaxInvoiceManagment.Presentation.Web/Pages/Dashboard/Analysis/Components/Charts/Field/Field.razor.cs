using Microsoft.AspNetCore.Components;

namespace TaxInvoiceManagment.Presentation.Web.Pages.Dashboard.Analysis
{
    public partial class Field
    {
        [Parameter]
        public string Label { get; set; }

        [Parameter]
        public string Value { get; set; }
    }
}