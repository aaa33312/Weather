namespace WeatherApp.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _http;
        public WeatherService(HttpClient http) => _http = http;

        public async Task<string?> GetWeatherAsync(string city)
        {
            var url = $"https://wttr.in/{Uri.EscapeDataString(city)}?format=j1";
            var resp = await _http.GetAsync(url);
            return resp.IsSuccessStatusCode ? await resp.Content.ReadAsStringAsync() : null;
        }
    }
}
