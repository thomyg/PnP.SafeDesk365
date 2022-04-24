using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using SafeDesk365.SDK.Models;

namespace SafeDesk365.DesignPoCWebApp.Components
{
    public partial class DeskAvailabilityList
    {        
        public IList<DeskAvailability> DeskAvailabilities { get; set; }

        [Parameter, EditorRequired]
        public DisplayType ListType { get; set; }

        [Parameter]
        public string UserEmail { get; set; } = "";

        RadzenDataGrid<DeskAvailability> DeskAvailabilitiesGrid;
        DeskAvailability DeskAvailabilityToInsert;

        bool isLoading = true;

        protected override void OnInitialized()
        {
            base.OnInitialized();

        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if(firstRender)
                await LoadData();

            await base.OnAfterRenderAsync(firstRender);
        }

        async Task LoadData()
        {
            isLoading = true;
            DeskAvailabilities = await SafeDesk365Service.GetDeskAvailability();
            isLoading = false;
            StateHasChanged();
        }


        async void OnCreateBooking(DeskAvailability da)
        {
            string id = da.Id.ToString();
            var result = await SafeDesk365Service.CreateBooking(id, "thomy@zxqg4.onmicrosoft.com");
            if (result > 0)
            {
                var message = new NotificationMessage()
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Desk booked",
                    Detail = "Booking Id is: " + Convert.ToString(result),
                    Duration = 4000
                };
                NotificationService.Notify(message);
            }
                
            LoadData();
        }
    }

    public enum DisplayType
    {
        Details,
        Simple
    }
}
