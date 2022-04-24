using SafeDesk365.SDK.Models;
using Radzen.Blazor;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace SafeDesk365.DesignPoCWebApp.Components
{
    public partial class BookingsList
    {

        [Parameter, EditorRequired]
        public IList<Booking> Bookings { get; set; }

        RadzenDataGrid<Booking> bookingsGrid;
        
        Booking bookingToInsert;
        
        protected override void OnInitialized()
        {
            base.OnInitialized();



            // For demo purposes only
           // var sortedBookings = Booking.FakeData.Generate(20).ToList().OrderBy(x => x.Date).ToList();
            //bookings = sortedBookings;
            // For production
            //orders = dbContext.Orders.Include("Customer").Include("Employee");
        }

        async Task InsertRow()
        {
            //bookingToInsert = new Booking();
            //await bookingsGrid.InsertRow(bookingToInsert);

            var result = await DialogService.OpenAsync<DeskAvailabilityList>("Book a desk");

            int x = 10;

        }

    async Task EditRow(Booking order)
        {
            //await bookingsGrid.EditRow(order);
        }

        void OnUpdateRow(Booking order)
        {
            //if (order == bookingToInsert)
            //{
            //    bookingToInsert = null;
            //}

            //dbContext.Update(order);

            //// For demo purposes only
            //order.Customer = dbContext.Customers.Find(order.CustomerID);
            //order.Employee = dbContext.Employees.Find(order.EmployeeID);

            // For production
            //dbContext.SaveChanges();
        }

        async Task SaveRow(Booking order)
        {
            //if (order == bookingToInsert)
            //{
            //    bookingToInsert = null;
            //}

            //await bookingsGrid.UpdateRow(order);
        }

        void CancelEdit(Booking order)
        {
            //if (order == bookingToInsert)
            //{
            //    bookingToInsert = null;
            //}

            //bookingsGrid.CancelEditRow(order);

            //// For production
            //var orderEntry = dbContext.Entry(order);
            //if (orderEntry.State == EntityState.Modified)
            //{
            //    orderEntry.CurrentValues.SetValues(orderEntry.OriginalValues);
            //    orderEntry.State = EntityState.Unchanged;
            //}
        }

        async Task DeleteRow(Booking order)
        {
            await SafeDesk365Service.DeleteBooking(order.Id.ToString());
            StateHasChanged();
        }



        

        void OnCreateRow(Booking order)
        {
            
        }
    }
}
