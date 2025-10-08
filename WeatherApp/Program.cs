using WeatherApp.Contracts;
using WeatherApp.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<IWeatherService, WeatherService>();
builder.Services.AddControllers();
var app = builder.Build();
app.MapControllers();

var file = SharedPaths.GetApiUrlFilePath();

app.Lifetime.ApplicationStarted.Register(() =>
{
    Directory.CreateDirectory(Path.GetDirectoryName(file)!);
    File.WriteAllText(file, app.Urls.FirstOrDefault());
});

AppDomain.CurrentDomain.ProcessExit += (_, _) =>
{
    if (File.Exists(file))
        File.Delete(file);
};

app.Run();