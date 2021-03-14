using ContractorSearch.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ContractorSearch.Hubs
{
    public class GoogleMapsService
    {
        public GoogleMapsService() { }
        public string CustomerGeocodingURL(Customer customer)
        {
            return $"https://maps.googleapis.com/maps/api/geocode/json?address={customer.AddressLine1}+{customer.AddressLine2}+{customer.City}+{customer.State}+{customer.ZipCode}+&key=" + ApiKeys.GoogleMaps;
        }
        public async Task<Customer> GeocodeCustomerAddress(Customer customer)
        {
            string apiUrl = CustomerGeocodingURL(customer);

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    JObject jsonResults = JsonConvert.DeserializeObject<JObject>(data);
                    JToken results = jsonResults["results"][0];
                    JToken location = results["geometry"]["location"];

                    customer.Latitude = (double)location["lat"];
                    customer.Longitude = (double)location["lng"];
                }
            }
            return customer;
        }
        public string ContractorGeocodingURL(Contractor contractor)
        {
            return $"https://maps.googleapis.com/maps/api/geocode/json?address={contractor.AddressLine1}+{contractor.AddressLine2}+{contractor.City}+{contractor.State}+{contractor.ZipCode}+&key=" + ApiKeys.GoogleMaps;
        }
        public async Task<Contractor> GeocodeContractorAddress(Contractor contractor)
        {
            string apiUrl = ContractorGeocodingURL(contractor);

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    JObject jsonResults = JsonConvert.DeserializeObject<JObject>(data);
                    JToken results = jsonResults["results"][0];
                    JToken location = results["geometry"]["location"];

                    contractor.Latitude = (double)location["lat"];
                    contractor.Longitude = (double)location["lng"];
                }
            }
            return contractor;
        }
    }
}
