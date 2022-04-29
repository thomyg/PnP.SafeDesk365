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
        public BookingListType ListType { get; set; }

        [Parameter, EditorRequired]
        public BookingListFilter FilterType { get; set; }
        
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
            if (firstRender)
            {
                await LoadData();
                SafeDesk365Service.OnBookingAdded += DataChanged;
            }
           
            await base.OnAfterRenderAsync(firstRender);
        }

        async Task LoadData()
        {
            isLoading = true;
            string currentUserEmail = await SafeDesk365Service.GetCurrentUserEmail();

            if (ListType == BookingListType.All)
                bookings = await SafeDesk365Service.GetBookings(currentUserEmail);
            
            if(ListType == BookingListType.Upcoming)
                bookings = await SafeDesk365Service.GetUpcomingBookings(currentUserEmail);

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

        private async void DataChanged()
        {
            await LoadData();
            _ = InvokeAsync(() => StateHasChanged());
        }

    }

    public enum BookingListType
    {
        All,
        Upcoming
    }

    public enum BookingListFilter
    {
        All,
        CurrentUser
    }
}
