
using Microsoft.Extensions.AI;
using OllamaSharp;

//create a chat client
IChatClient client = new OllamaApiClient(new Uri("http://localhost:11434"), "llama3.2");


#region Basic Completion

//send Prompt & get response
string prompt = "What is AI? Explain in max 20 words";
Console.WriteLine($"user >>> {prompt}");

ChatResponse response = await client.GetResponseAsync(prompt);
Console.WriteLine($"assistant >>> {response}");
Console.WriteLine($"Tokens used: in={response.Usage?.InputTokenCount},out={response.Usage?.OutputTokenCount}");

#endregion