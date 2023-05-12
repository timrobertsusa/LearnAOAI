
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;


//public static IConfiguration Configuration { get; set; }

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", true, true)   
    .AddUserSecrets<Program>()
    .Build();

Console.WriteLine("Hello, World!");

Console.WriteLine($"Hello, {config["Bobo"]}");
Console.WriteLine($"appSettings, {config["AzureKeyVaultEndpoint"]}");

