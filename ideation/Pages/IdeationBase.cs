
using Microsoft.AspNetCore.Components;
//using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Memory;


namespace ideation.Pages
{

    public class IdeationBase : ComponentBase
    {
        [Inject]
        private IConfiguration config { get; set; } = null!;
        public string LLMResponse = String.Empty;
        

        protected override async Task OnInitializedAsync()
        {
            string? myAzureOpenAIEmbedDeployment = config["AzureOpenAIEmbedDeployment"];
            string? myAzureOpenAIDeployment = config["AzureOpenAIDeployment"];
            string? myAOAIEndpoint = config["AzureOpenAIEndpoint"];
            string? myAOAIKey = config["AzureOpenAIKey"];
            string? myQdrantEndpoint = config["QdrantEndpoint"];

            IKernel kernel = Kernel.Builder
                // .WithLogger(ConsoleLogger.Log)
                .WithAzureTextCompletionService(myAzureOpenAIDeployment!, myAOAIEndpoint!, myAOAIKey!)
                .WithAzureTextEmbeddingGenerationService(myAzureOpenAIEmbedDeployment!, myAOAIEndpoint!, myAOAIKey!)
                //.WithMemoryStorage(memoryStore)
                .WithMemoryStorage(new VolatileMemoryStore())
                //.WithQdrantMemoryStore(Env.Var("QDRANT_ENDPOINT"), 1536) // This method offers an alternative approach to registering Qdrant memory store.
                .Build();

            var prompt = @"{{$input}}

One line TLDR with the fewest words.";

            var summarize = kernel.CreateSemanticFunction(prompt);

            string textBrady = @"
Tom Brady won a super bowl in the first year he played in the NFL.
Tom Brady won the super bowl 9 times in his 20 year career.
He retired after winning a super bowl with the Tampa Bay Bucs.";


            var Tricky = await summarize.InvokeAsync(textBrady);
            LLMResponse = Tricky.ToString();

        }


    }
}