using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.SemanticKernel;


var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", true, true)   
    .AddUserSecrets<Program>()
    .Build();

string? myAzureOpenAIDeployment = config["AzureOpenAIDeployment"];
string? myAOAIEndpoint = config["AzureOpenAIEndpoint"];
string? myAOAIKey = config["AzureOpenAIKey"];

Console.WriteLine(myAOAIEndpoint);
Console.WriteLine(myAOAIKey);

var kernel = Kernel.Builder.Build();

// Azure OpenAI
//kernel.Config.AddAzureTextCompletionService("davinci-azure", "text-davinci-003", myAOAIEndpoint!, myAOAIKey);
kernel.Config.AddAzureTextCompletionService(myAzureOpenAIDeployment!, myAOAIEndpoint!, myAOAIKey!);  // 0.13.442.1-preview.




var prompt = @"{{$input}}

One line TLDR with the fewest words.";

var summarize = kernel.CreateSemanticFunction(prompt);

string textBrady = @"
Tom Brady won a super bowl in the first year he played in the NFL.
Tom Brady won the super bowl 9 times in his 20 year career.
He retired after winning a super bowl with the Tampa Bay Bucs.";


Console.WriteLine(await summarize.InvokeAsync(textBrady));


