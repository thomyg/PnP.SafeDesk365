using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using SafeDesk365.SDK.Models;

namespace SafeDesk365.DesignPoCWebApp.Components
{
    public partial class DeskAvailabilityList
    {
        [Parameter, EditorRequired]
        public IList<DeskAvailability> DeskAvailabilities { get; set; }

        RadzenDataGrid<DeskAvailability> DeskAvailabilitiesGrid;
        DeskAvailability DeskAvailabilityToInsert;


        protected override void OnInitialized()
        {
            base.OnInitialized();



            // For demo purposes only
            // var sortedBookings = Booking.FakeData.Generate(20).ToList().OrderBy(x => x.Date).ToList();
            //bookings = sortedBookings;
            // For production
            //orders = dbContext.Orders.Include("Customer").Include("Employee");
        }

        async Task EditRow(DeskAvailability order)
        {
            //await bookingsGrid.EditRow(order);
        }

        void OnUpdateRow(DeskAvailability order)
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

        async Task SaveRow(DeskAvailability order)
        {
            //if (order == bookingToInsert)
            //{
            //    bookingToInsert = null;
            //}

            //await bookingsGrid.UpdateRow(order);
        }

        void CancelEdit(DeskAvailability order)
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

        async Task DeleteRow(DeskAvailability order)
        {
            //if (order == bookingToInsert)
            //{
            //    bookingToInsert = null;
            //}

            //if (orders.Contains(order))
            //{
            //    dbContext.Remove<Order>(order);

            //    // For demo purposes only
            //    orders.Remove(order);

            //    // For production
            //    //dbContext.SaveChanges();

            //    await bookingsGrid.Reload();
            //}
            //else
            //{
            //    bookingsGrid.CancelEditRow(order);
            //}
        }



        async Task InsertRow()
        {
            //bookingToInsert = new Booking();
            //await bookingsGrid.InsertRow(bookingToInsert);
        }

        void OnCreateRow(DeskAvailability order)
        {
            //dbContext.Add(order);

            //// For demo purposes only
            //order.Customer = dbContext.Customers.Find(order.CustomerID);
            //order.Employee = dbContext.Employees.Find(order.EmployeeID);

            //// For production
            ////dbContext.SaveChanges();
        }
    }
}
