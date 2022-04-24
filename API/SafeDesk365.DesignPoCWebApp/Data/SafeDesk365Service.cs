using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using SafeDesk365.SDK;
using SafeDesk365.SDK.Api.Bookings;
using SafeDesk365.SDK.Models;

namespace SafeDesk365.DesignPoCWebApp.Data
{
    public class SafeDesk365Service
    {

        ApiClient apiClient;

        public SafeDesk365Service()
        {
            var authProvider = new AnonymousAuthenticationProvider();
            var requestAdapter = new HttpClientRequestAdapter(authProvider);
            apiClient = new ApiClient(requestAdapter);
        }

        public async Task<List<Booking>> GetBookings(string email = "", string location = "")
        {
            var result = await apiClient.Api.Bookings.GetAsync(q =>
            {
                q.UserEmail = email;
                q.Location = location;
            });

            return result.ToList();
        }

        public async Task<List<Booking>> GetUpcomingBookings(string email = "", string location = "")
        {
            var result = await apiClient.Api.Bookings.Upcoming.GetAsync(q =>
            {
                q.UserEmail = email;
                q.Location = location;
            });

            return result.ToList();
        }

        public async Task<List<DeskAvailability>> GetDeskAvailability(string date = "", string location = "")
        {
            var result = await apiClient.Api.DeskAvailabilities.Upcoming.GetAsync(q =>
            {
                if(location != "")    
                    q.Location = location;
                if(date != "")
                    q.SelectedDate = date;
            });

            return result.ToList();
        }

        public async Task<List<Location>> GetLocations()
        {
            var result = await apiClient.Api.Locations.GetAsync();
            return result.ToList();
        }

        public async Task DeleteBooking(string id)
        {            
            await apiClient.Api.Bookings[id].DeleteAsync();
        }

        public async Task<int> CreateBooking(string id, string email)
        {
            var result = await apiClient.Api.Bookings.Availability[id].PostAsync(b =>
            {
                b.UserEmail=email;
            });

            return Convert.ToInt32(result);
        }
    }
}
