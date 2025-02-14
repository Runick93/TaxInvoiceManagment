using Microsoft.AspNetCore.Components;

namespace TaxInvoiceManagment.Presentation.Web.Pages.Dashboard.Monitor
{
    public partial class TagCloud
    {
        [Parameter]
        public object[] Data { get; set; }

        [Parameter]
        public int? Height { get; set; }
    }
}