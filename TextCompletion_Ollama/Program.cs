
using Microsoft.Extensions.AI;
using OllamaSharp;

//create a chat client
IChatClient client = new OllamaApiClient(new Uri("http://localhost:11434"), "llama3.2");
