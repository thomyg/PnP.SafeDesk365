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
using Microsoft.Graph;
using Azure.Core;
using SafeDesk365.TeamsTab.Interop.TeamsSDK;

namespace SafeDesk365.TeamsTab.Data
{
    public delegate void Notify();
    
    public class SafeDesk365Service
    {
        public event Notify OnBookingAdded;
        
        ApiClient apiClient;
        IConfiguration configuration;
        TeamsFx teamsFx;
        TeamsUserCredential teamsUserCredential;
        MicrosoftTeams microsoftTeams;

        private readonly string _scope = "User.Read";
        GraphServiceClient GraphClient;


        public SafeDesk365Service(IConfiguration configuration, TeamsFx teamsFx, TeamsUserCredential teamsUserCredential,
            MicrosoftTeams microsoftTeams)
        {
            this.configuration = configuration;
            this.teamsFx = teamsFx;
            this.teamsUserCredential = teamsUserCredential;
            this.microsoftTeams = microsoftTeams;
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

        public async Task<string> GetCurrentUserEmail()
        {
            GraphClient = await GetGraphServiceClient();
            User user = await GraphClient.Me.Request().GetAsync();
            return user.Mail;
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

        public async Task<List<SDK.Models.Location>> GetLocations()
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

            BookingAdded();

            return Convert.ToInt32(result);
        }

        public async Task<List<Desk>> GetDesks()
        {
            var result = await apiClient.Api.Desks.GetAsync();
            return result.ToList<Desk>();
        }

        public virtual void BookingAdded()
        {
            OnBookingAdded?.Invoke();
        }

        private async Task<GraphServiceClient> GetGraphServiceClient()
        {
            await microsoftTeams.InitializeAsync();
            var isInTeams = await microsoftTeams.IsInTeams();

            if (isInTeams)
            {
                await teamsUserCredential.GetTokenAsync(new TokenRequestContext(new string[] { _scope }), new System.Threading.CancellationToken());
                var msGraphAuthProvider = new MsGraphAuthProvider(teamsUserCredential, _scope);
                var client = new GraphServiceClient(msGraphAuthProvider);

                return client;
            }

            return null;
        }
    }
}
