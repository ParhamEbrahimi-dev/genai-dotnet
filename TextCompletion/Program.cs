

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


#region Basic Completion

////send Prompt & get response
//string prompt = "What is AI? Explain in max 20 words";
//Console.WriteLine($"user >>> {prompt}");

//ChatResponse response = await client.GetResponseAsync(prompt);
//Console.WriteLine($"assistant >>> {response}");
//Console.WriteLine($"Tokens used: in={response.Usage?.InputTokenCount},out={response.Usage?.OutputTokenCount}");

#endregion


#region Streaming

//string prompt = "What is AI? Explain in max 200 words";
//Console.WriteLine($"user >>> {prompt}");

//var streamResponse = client.GetStreamingResponseAsync(prompt);
//await foreach(var message in streamResponse)
//{
//    Console.Write(message.Text);
//}

#endregion

#region Classification

var classificationPrompt = """
    Please classify following sentences into categories:
    - 'complaint'
    - 'suggestion'
    - 'praise'
    - 'other'

    1) "I Love the new layout!"
    2) "you should add a night mode"
    3) "when i try to login , it keep failing!"
    4) "this app is decent"
    
    """;

Console.WriteLine($"user >>> {classificationPrompt}");
ChatResponse classificationResponse = await client.GetResponseAsync(classificationPrompt);
Console.WriteLine($"assistant >>> \n {classificationResponse}");
Console.WriteLine($"Tokens used: in={classificationResponse.Usage?.InputTokenCount},out={classificationResponse.Usage?.OutputTokenCount}");

#endregion

#region Summarization

#endregion

#region Sentiment Analysis

#endregion