

using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using System.ClientModel;
//get credential from user secret
IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

var credential = new ApiKeyCredential(config["AvalAi:Token"] ?? throw new InvalidOperationException());
var options = new OpenAIClientOptions
{
    Endpoint = new Uri("https://api.avalai.ir/v1")
};

//create a chat client 
IChatClient client = new OpenAIClient(credential, options).GetChatClient("gpt-5.4-mini").AsIChatClient();

//Create an Embedding generator("text-embedding-3-small" is an example)
IEmbeddingGenerator<string,Embedding<float>> embeddingGenerator =
    new OpenAIClient(credential,options).GetEmbeddingClient("text-embedding-3-small").AsIEmbeddingGenerator();

//1: Generate a single embedding
var embedding = await embeddingGenerator.GenerateVectorAsync("Hello, world!");
Console.WriteLine($"Embedding dimensions: {embedding.Span.Length}");
foreach(var value in embedding.Span)
{
    Console.Write("{0:0.00}", value);
}



