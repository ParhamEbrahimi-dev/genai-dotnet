
using CommunityToolkit.VectorData.InMemory;
using Microsoft.Extensions.AI;
using OllamaSharp;
using VectorSearch_Ollama;


//create an embedding generator
IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator =
    new OllamaApiClient(new Uri("http://localhost:11434"), "all-minilm");

//create and populate the vector store
var vectorStore = new InMemoryVectorStore();

var movieStore = vectorStore.GetCollection<int, Movie>("movies");

await movieStore.EnsureCollectionExistsAsync();

foreach (var movie in MovieData.Movies)
{
    //generate the embedding vector for the movie description 
    movie.Vector = await embeddingGenerator.GenerateVectorAsync(movie.Description);

    //add the overall movie to the in-memory vector store's movies collection
    await movieStore.UpsertAsync(movie);
}

//generate the embedding from the user's prompt
var query = "I want to see family friendly movie";
//var query = "A science fiction movie about space travel";
var queryEmbedding = await embeddingGenerator.GenerateVectorAsync(query);

//search the knowlege store based on user's prompt
var searchResults = movieStore.SearchAsync(queryEmbedding, top: 2);

//see the relevant results
await foreach (var result in searchResults)
{
    Console.WriteLine($"Title: {result.Record.Title}");
    Console.WriteLine($"Description: {result.Record.Description}");
    Console.WriteLine($"Score:{result.Score}");
    Console.WriteLine();
}