namespace WeatherApp.Contracts
{
    public static class SharedPaths
    {
        public static string GetApiUrlFilePath()
        {
            return Path.Combine(Directory.GetParent(AppContext.BaseDirectory)!
                .Parent!.Parent!.Parent!.Parent!.FullName, "api_url.txt");
        }
    }
}