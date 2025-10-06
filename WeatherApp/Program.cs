using WeatherApp.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<IWeatherService, WeatherService>();
builder.Services.AddControllers();
var app = builder.Build();
app.MapControllers();

app.Lifetime.ApplicationStarted.Register(() =>
    File.WriteAllText(
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "WeatherApp", "api_url.txt"),
        app.Urls.First()
    ));

app.Lifetime.ApplicationStopping.Register(() =>
    File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    "WeatherApp", "api_url.txt")));

app.Run();