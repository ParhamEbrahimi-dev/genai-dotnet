

//get credentials from user secret

using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using System.ClientModel;

IConfiguration config = new  ConfigurationBuilder().AddUserSecrets<Program>().Build();

var credential = new ApiKeyCredential(config["AvalAi:Token"] ?? throw new InvalidDataException());
var options = new OpenAIClientOptions()
{
    Endpoint = new Uri("https://api.avalai.ir/v1")
};

//create Chat Client
IChatClient client = new OpenAIClient(credential, options).GetChatClient("gpt-5.4-mini").AsIChatClient();


//send Prompt & get response
string prompt = "What is AI? Explain in max 20 words";
Console.WriteLine($"user >>> {prompt}");

ChatResponse response = await client.GetResponseAsync(prompt);
Console.WriteLine($"assistant >>> {response}");
Console.WriteLine($"Tokens used: in={response.Usage?.InputTokenCount},out={response.Usage?.OutputTokenCount}");


