using Radzen.Blazor;
using SafeDesk365.SDK.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SafeDesk365.TeamsTab.Components
{
    public partial class DeskList
    {
        RadzenDataGrid<Desk> DeskGrid;
        public IList<Desk> Desks { get; set; }
        bool isLoading = true;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Desks = await SafeDesk365Service.GetDesks();
                isLoading = false;
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }
    }   
}
