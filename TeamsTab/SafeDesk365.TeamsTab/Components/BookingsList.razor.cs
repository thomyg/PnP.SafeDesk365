using SafeDesk365.SDK.Models;
using Radzen.Blazor;
using Microsoft.AspNetCore.Components;
using Radzen;

using System.Collections.Generic;
using SafeDesk365.TeamsTab.Data;
using System.Threading.Tasks;
using SafeDesk365.TeamsTab.Pages;

namespace SafeDesk365.TeamsTab.Components
{
    public partial class BookingsList
    {

        [Parameter, EditorRequired]
        public BookingListType listType { get; set; }

        [Parameter, EditorRequired]
        public string UserEmail { get; set; }
        
        public IList<Booking> bookings { get; set; }
        RadzenDataGrid<Booking> bookingsGrid;        
        Booking bookingToInsert;
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

            if (listType == BookingListType.All)
                bookings = await SafeDesk365Service.GetBookings(UserEmail);
            
            if(listType == BookingListType.Upcoming)
                bookings = await SafeDesk365Service.GetUpcomingBookings(UserEmail);

            isLoading = false;
            StateHasChanged();
        }

        async Task InsertRow()
        {            
            var result = await DialogService.OpenAsync<NewBooking>("Book a desk",null,
                new DialogOptions() { Width="1024px", Height="800px", Resizable=true, Draggable=true});

            int x = 10;

        }

        async Task DeleteRow(Booking order)
        {
            isLoading = true;
            await SafeDesk365Service.DeleteBooking(order.Id.ToString());
            isLoading = false;
            StateHasChanged();
        }

    }

    public enum BookingListType
    {
        All,
        Upcoming
    }
}
