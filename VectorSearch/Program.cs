

using CommunityToolkit.VectorData.InMemory;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using System.ClientModel;
using VectorSearch;

//get credential from user secret
IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

var credential = new ApiKeyCredential(config["AvalAi:Token"] ?? throw new InvalidOperationException());
var options = new OpenAIClientOptions
{
    Endpoint = new Uri("https://api.avalai.ir/v1")
};

//create an embedding generator
IEmbeddingGenerator<string,Embedding<float>> embeddingGenerator = 
    new OpenAIClient(credential, options).GetEmbeddingClient("text-embedding-3-small").AsIEmbeddingGenerator();



//create and populate the vector store
var vectorStore = new InMemoryVectorStore();

var movieStore = vectorStore.GetCollection<int, Movie>("movies");

await movieStore.EnsureCollectionExistsAsync();

foreach(var movie in MovieData.Movies)
{
    //generate the embedding vector for the movie description 
    movie.Vector = await embeddingGenerator.GenerateVectorAsync(movie.Description);

    //add the overall movie to the in-memory vector store's movies collection
    await movieStore.UpsertAsync(movie);
}

//generate the embedding from the user's prompt
//var query = "I want to see family friendly movie";
var query = "A science fiction movie about space travel";
var queryEmbedding = await embeddingGenerator.GenerateVectorAsync(query);

//search the knowlege store based on user's prompt
var searchResults = movieStore.SearchAsync(queryEmbedding, top: 2);

//see the relevant results
await foreach(var result in searchResults)
{
    Console.WriteLine($"Title: {result.Record.Title}");
    Console.WriteLine($"Description: {result.Record.Description}");
    Console.WriteLine($"Score:{result.Score}");
    Console.WriteLine();
}