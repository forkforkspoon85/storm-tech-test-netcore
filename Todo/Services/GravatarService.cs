using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Threading.Tasks;
using Todo.Models.Gravatar;

namespace Todo.Services
{
    public class GravatarService : IGravatarService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public GravatarService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<GravatarProfile> GetProfile(string emailAddress)
        {
            //TODO:Add caching(in mem or redis)
            var client = _httpClientFactory.CreateClient("Gravatar");

            var response = await client.GetAsync(GenertateUrl(emailAddress));

            if(!response.IsSuccessStatusCode) 
            {
                //TODO: log
                throw new ArgumentException($"Cannot find profile for {emailAddress}");
            }
            var results = await JsonSerializer.DeserializeAsync<GravatarReponse>(await response.Content.ReadAsStreamAsync());
            if (results == null || !results.Profiles.Any())
            {
                throw new Exception("Issue deserializing gravatar response");//TODO: create customer exception for this
            }
            return results.Profiles.First();
        }

        public async Task<IEnumerable<GravatarProfile>> GetProfiles(IEnumerable<string> emailAddresses)
        {
            return await Task.WhenAll(emailAddresses.Select(emailAddress => GetProfile(emailAddress)));
        }

        private string GenertateUrl(string emailAddress) => $"{Gravatar.GetHash(emailAddress)}.json";

    }
}
