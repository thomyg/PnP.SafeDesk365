using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using SafeDesk365.SDK;
using Microsoft.Kiota.Authentication.Azure;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Azure.Identity;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


var clientId = "YOUR_CLIENT_ID";

// The auth provider will only authorize requests to
// the allowed hosts, in this case Microsoft Graph
var allowedHosts = new[] { "graph.microsoft.com" };
var graphScopes = new[] { "User.Read" };

//var obo = new OnBehalfOfCredential();

var credential = new DeviceCodeCredential((code, cancellation) =>
{
    Console.WriteLine(code.Message);
    return Task.FromResult(0);
},
clientId);

var authProvider = new AnonymousAuthenticationProvider();
var requestAdapter = new HttpClientRequestAdapter(authProvider);
var client = new ApiClient(requestAdapter);
var bookings = await client.Api.Bookings.GetAsync();

int x = 0;

