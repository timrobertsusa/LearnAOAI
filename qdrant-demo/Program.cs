using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel.Connectors.Memory.Qdrant;
//using Microsoft.SemanticKernel.KernelExtensions;
//using System.IO;
//using Microsoft.SemanticKernel.Configuration;
//using Microsoft.SemanticKernel.SemanticFunctions;
//using Microsoft.SemanticKernel.CoreSkills;



var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", true, true)   
    .AddUserSecrets<Program>()
    .Build();

string? myAzureOpenAIEmbedDeployment = config["AzureOpenAIEmbedDeployment"];
string? myAzureOpenAIDeployment = config["AzureOpenAIDeployment"];
string? myAOAIEndpoint = config["AzureOpenAIEndpoint"];
string? myAOAIKey = config["AzureOpenAIKey"];    


Console.WriteLine(myAOAIEndpoint);
Console.WriteLine(myAOAIKey);

// var kernel = Kernel.Builder.Build();
// kernel.Config.AddAzureTextCompletionService(myAzureOpenAIDeployment!, myAOAIEndpoint!, myAOAIKey!);  // 0.13.442.1-preview.
// kernel.Config.AddAzureTextEmbeddingGenerationService(myAzureOpenAIDeployment!, myAOAIEndpoint!, myAOAIKey!);  // 0.13.442.1-preview.

//string MemoryCollectionName = "qdrant-test";
QdrantMemoryStore memoryStore = new("http://localhost:6333", 1536);


IKernel kernel = Kernel.Builder
            // .WithLogger(ConsoleLogger.Log)
            .WithAzureTextCompletionService(myAzureOpenAIDeployment!, myAOAIEndpoint!, myAOAIKey!)
            .WithAzureTextEmbeddingGenerationService(myAzureOpenAIEmbedDeployment!, myAOAIEndpoint!, myAOAIKey!)            
            .WithMemoryStorage(memoryStore)
            //.WithMemoryStorage(new VolatileMemoryStore())
            //.WithQdrantMemoryStore(Env.Var("QDRANT_ENDPOINT"), 1536) // This method offers an alternative approach to registering Qdrant memory store.
            .Build();


// var kernel =  Microsoft.SemanticKernel.Kernel.Builder
// .Configure(c =>
// {    
//     c.AddTextCompletionService()
//     c.AddAzureTextCompletionService(myAzureOpenAIDeployment!,myAOAIEndpoint!,myAOAIKey!);        
//     c.AddAzureTextEmbeddingGenerationService(myAzureOpenAIDeployment!,myAOAIEndpoint!,myAOAIKey!);
    
// })
// .WithMemoryStorage(new VolatileMemoryStore())
// .Build();


const string memoryCollectionName = "Facts About Me";

await kernel.Memory.SaveInformationAsync(memoryCollectionName, id: "LinkedIn Bio", 
    text: "I currently work in the hotel industry at the front desk. I won the best team player award.");

await kernel.Memory.SaveInformationAsync(memoryCollectionName, id: "LinkedIn History", 
    text: "I have worked as a tourist operator for 8 years. I have also worked as a banking associate for 3 years.");

await kernel.Memory.SaveInformationAsync(memoryCollectionName, id: "Recent Facebook Post", 
    text: "My new dog Trixie is the cutest thing you've ever seen. She's just 2 years old.");
    
await kernel.Memory.SaveInformationAsync(memoryCollectionName, id: "Old Facebook Post", 
    text: "Can you believe the size of the trees in Yellowstone? They're huge! I'm so committed to forestry concerns.");

Console.WriteLine("Four GIGANTIC vectors were generated just now from those 4 pieces of text above.");

Console.WriteLine("Press enter to continue...");
Console.ReadLine();


var prompt = @"{{$input}}

One line TLDR with the fewest words.";

var summarize = kernel.CreateSemanticFunction(prompt);

string textBrady = @"
Tom Brady won a super bowl in the first year he played in the NFL.
Tom Brady won the super bowl 9 times in his 20 year career.
He retired after winning a super bowl with the Tampa Bay Bucs.";


Console.WriteLine(await summarize.InvokeAsync(textBrady));


