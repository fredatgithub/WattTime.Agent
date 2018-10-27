using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WattTime.Agent.Interaction;
using WattTime.Agent.Services;
using Microsoft.Extensions.DependencyInjection;
using WattTime.Agent.Services.Client;
using Microsoft.Rest;
using System.Net;
using Alexa.NET;
using BingMapsRESTToolkit;
using Microsoft.Extensions.Configuration;

namespace WattTime.Agent.Handlers
{
    public class LaunchRequestHandler : IRequestHandler
    {
        public bool CanHandle(IServiceProvider context, SkillRequest request) =>
            request.Request.Type == InteractionModel.RequestType.LaunchRequest;

        public async Task<SkillResponse> Handle(IServiceProvider context, SkillRequest request)
        {
            var skillSurveyService = context.GetService<IWattTimeService>();
            var configuration = context.GetService<IConfiguration>();

            var intent = ((IntentRequest)request.Request).Intent;

            var addressInformationClient = new SkillAddressInformationClient(
                new Uri(request.Context.System.ApiEndpoint, UriKind.Absolute),
                new TokenCredentials(request.Context.System.ApiAccessToken));

            var response = await addressInformationClient.GetCountryAndPostalCodeWithHttpMessagesAsync(
                request.Context.System.Device.DeviceID);

            if (response.Response.StatusCode == HttpStatusCode.OK)
            {
                var countryandPostalCode = response.Body;

                var mapsRequest = new GeocodeRequest()
                {
                    BingMapsKey = configuration["Authentication:Bing:MapsKey"],
                    MaxResults = 1,
                    Address = new SimpleAddress()
                    {
                        CountryRegion = countryandPostalCode.CountryCode,
                        PostalCode = countryandPostalCode.PostalCode,
                    }
                };

                var mapsResponse = await ServiceManager.GetResponseAsync(mapsRequest);
                var location = mapsResponse.ResourceSets.FirstOrDefault()?.Resources.FirstOrDefault() as Location;
                var coordinates = location.GeocodePoints.FirstOrDefault()?.Coordinates;

                var credentials = new BasicAuthenticationCredentials()
                {
                    UserName = configuration["Authentication:WattTime:Username"],
                    Password = configuration["Authentication:WattTime:Password"]
                };
                var tokenClient = new WattTimeClient(credentials);
                var token = await tokenClient.GetTokenAsync();
                var tokenCredentials = new TokenCredentials(token.TokenProperty);

                var client = new WattTimeClient(tokenCredentials);

                var ba = await client.GetBalancingAuthorityByLocationAsync(coordinates[0], coordinates[1]);
                var index = await client.GetIndexAsync(ba.Abbrev, "percent");


                return ResponseBuilder.Tell(
                    $"On the percentage scale where 0 means extremely clean and 100 means extremely dirty the index for your location is {index.Percent}.");
            }
            else
            {
                var text = "Welcome to WattTime. I can check the current carbon emission levels in your area. " +
                    "To do that, I need permissions to access your location. " +
                    "Please open companion app and accept request I just submitted.";

                return new SkillResponse()
                {
                    Response = new ResponseBody()
                    {
                        ShouldEndSession = true,
                        OutputSpeech = new PlainTextOutputSpeech()
                        {
                            Text = text
                        },
                        Card = new AskForPermissionsConsentCard()
                        {
                            Permissions = new List<string>()
                            {
                                 InteractionModel.Permissison.CountryAndPostalCode
                            }
                        }
                    }
                };
            }
        }
    }
}
