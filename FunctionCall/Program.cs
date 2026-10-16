


using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using System.ClientModel;

//get credential from user secrets
IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

var credential = new ApiKeyCredential(config["AvalAi:Token"] ?? throw new InvalidOperationException());
var options = new OpenAIClientOptions
{
    Endpoint = new Uri("https://api.avalai.ir/v1")
};

//create chat client 
IChatClient client = new ChatClientBuilder(new OpenAIClient(credential, options).GetChatClient("gpt-5-mini").AsIChatClient())
    .UseFunctionInvocation().Build();

var chatOptions = new ChatOptions
{
    Tools = [AIFunctionFactory.Create((string location,string unit) => {
        //Here you would call a weather Api to get the weather for the location 

        var tempreture = Random.Shared.Next(5,20);
        var condition = Random.Shared.Next(0,1) == 0 ? "sunny" : "rainy";

        return $"The Weather is {tempreture} degrees C and {condition}";
    }
    ,"get_current_weather"
    ,"Get the current Weather in a given location")]
};

List<ChatMessage> chatHistory = [
    new(ChatRole.System,"""
    You are a friendly hiking enthusiast who helps people discover fun hikes in their area.
    You are upbeat and friendly.
    """)
    ];

//weather conversation relevnt to registered function.
chatHistory.Add(new(ChatRole.User, """
    I Live in Tehran and I'm looking for a moderate intensity hike.
    What's the current weather like?
    """));

Console.WriteLine($"{chatHistory.Last().Role} >>> {chatHistory.Last()}");

ChatResponse response = await client.GetResponseAsync(chatHistory,chatOptions);

chatHistory.Add(new(ChatRole.Assistant, response.Text));

Console.WriteLine($"{chatHistory.Last().Role} >>> {chatHistory.Last()}");
Console.WriteLine($"Tokens used: in={response.Usage?.InputTokenCount},out={response.Usage?.OutputTokenCount}");

