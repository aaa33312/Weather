using System.Net.Http.Json;
using System.Text.Json;
using WeatherApp.Contracts;

class Program
{
    static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        string apiBaseUrl = GetApiBaseUrl();
        string exit = "exit";

        var client = new HttpClient { BaseAddress = new Uri(apiBaseUrl) };

        Console.WriteLine("Введите название города (например, Москва).");
        Console.WriteLine("Для выхода введите: " + exit + "\n");

        while (true)
        {
            Console.Write("Город: ");
            var city = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(city))
                continue;

            if (city.Equals(exit, StringComparison.OrdinalIgnoreCase))
                break;

            try
            {
                var response = await client.GetAsync($"/api/weather?city={Uri.EscapeDataString(city)}");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Ошибка: {response.StatusCode}\n");
                    continue;
                }

                var weather = await response.Content.ReadFromJsonAsync<WeatherData>(
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (weather == null)
                {
                    Console.WriteLine("Ошибка чтения данных.\n");
                    continue;
                }

                Console.WriteLine($"\nПогода в городе {weather.City}: {weather.Description}");
                Console.WriteLine($"Температура: {weather.Temperature} \n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}\n");
            }
        }
    }

    private static string GetApiBaseUrl()
    {
        string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WeatherApp");
        string file = Path.Combine(folder, "api_url.txt");

        return File.ReadAllText(file).Trim();
    }
}