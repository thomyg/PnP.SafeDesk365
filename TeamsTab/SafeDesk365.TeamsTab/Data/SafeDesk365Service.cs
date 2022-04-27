using Azure.Identity;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Microsoft.Kiota.Abstractions.Authentication;
using SafeDesk365.SDK;
using SafeDesk365.SDK.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Kiota.Authentication.Azure;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.TeamsFx;

namespace SafeDesk365.TeamsTab.Data
{
    public class SafeDesk365Service
    {
        ApiClient apiClient;
        IConfiguration configuration;
        TeamsFx teamsFx;
        TeamsUserCredential teamsUserCredential;

        //public SafeDesk365Service()
        //{
            
        //}

        public SafeDesk365Service(IConfiguration configuration, TeamsFx teamsFx, TeamsUserCredential teamsUserCredential)
        {
            this.configuration = configuration;
            this.teamsFx = teamsFx;
            this.teamsUserCredential = teamsUserCredential;
            this.apiClient = GetApiClient();
        }

        ApiClient GetApiClient()
        {
            ApiClient result;

            string secret = configuration["TeamsFx:Authentication:ClientSecret"];
            string clientId = configuration["TeamsFx:Authentication:ClientId"];
            string tenantId = configuration["SafeDesk365:TenantId"];
            string allowedHost = configuration["SafeDesk365:AllowedHost"];
            string apiScope = configuration["SafeDesk365:ApiScope"];
            string apiBaseUrl = configuration["SafeDesk365:ApiBaseUrl"];
            
            var credential = new ClientSecretCredential(tenantId, clientId, secret);
            var allowedHosts = new[] { allowedHost };
            var apiScopes = new[] { apiScope };    
            
            var authProvider = new AzureIdentityAuthenticationProvider(credential, allowedHosts, apiScopes);
            var requestAdapter = new HttpClientRequestAdapter(authProvider);
            requestAdapter.BaseUrl = apiBaseUrl;
            result = new ApiClient(requestAdapter);

            return result;
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
                if (location != "")
                    q.Location = location;
                if (date != "")
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
                b.UserEmail = email;
            });

            return Convert.ToInt32(result);
        }

        public async Task<List<Desk>> GetDesks()
        {
            var result = await apiClient.Api.Desks.GetAsync();
            return result.ToList<Desk>();
        }
    }
}
