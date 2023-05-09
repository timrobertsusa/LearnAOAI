
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;


//public static IConfiguration Configuration { get; set; }

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

Console.WriteLine("Hello, World!");

Console.WriteLine($"Hello, {config["Bobo"]}");
