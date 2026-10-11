

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
ChatResponse response = await client.GetResponseAsync("What is AI? Explain in max 20 words");
Console.WriteLine(response);
