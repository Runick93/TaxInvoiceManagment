using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using TaxInvoiceManagment.Presentation.Web.Models;
using TaxInvoiceManagment.Presentation.Web.Services;

namespace TaxInvoiceManagment.Presentation.Web.Pages.Account.Settings
{
    public partial class BaseView
    {
        private CurrentUser _currentUser = new CurrentUser();

        [Inject] protected IUserService UserService { get; set; }

        private void HandleFinish()
        {
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _currentUser = await UserService.GetCurrentUserAsync();
        }
    }
}