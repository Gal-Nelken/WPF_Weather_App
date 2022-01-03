using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WPA_Wether_App.Model;

namespace WPA_Wether_App.ViewModel.Helpers
{
    public class AccuWeatherHelper
    {
        public const string BASE_URL = "http://dataservice.accuweather.com/";
        public const string AUTOCOMPLETE_ENDPOINT = "locations/v1/cities/autocomplete";
        public const string CURRENT_CONDITIONS_ENDPOINT = "currentconditions/v1/";
        public const string API_KEY = "?apikey="; // ENTER API KEY FROM https://developer.accuweather.com

        public static async Task<List<City>> GetCities(string query)
        {
            List<City> cities = new List<City>();

            string url = $"{BASE_URL}{AUTOCOMPLETE_ENDPOINT}{API_KEY}&q={query}";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();

                cities = JsonConvert.DeserializeObject<List<City>>(json);
            }

            return cities;
        }


        public static async Task<CurrentConditions> GetCurrentConditions(string cityKey)
        {
            CurrentConditions currnetCondions = new CurrentConditions();

            string url = $"{BASE_URL}{CURRENT_CONDITIONS_ENDPOINT}{cityKey}{API_KEY}";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                string json = await response.Content.ReadAsStringAsync();

                currnetCondions = (JsonConvert.DeserializeObject<List<CurrentConditions>>(json)).FirstOrDefault();
            }

            return currnetCondions;
        }
    }
}
