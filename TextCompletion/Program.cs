

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

//var classificationPrompt = """
//    Please classify following sentences into categories:
//    - 'complaint'
//    - 'suggestion'
//    - 'praise'
//    - 'other'

//    1) "I Love the new layout!"
//    2) "you should add a night mode"
//    3) "when i try to login , it keep failing!"
//    4) "this app is decent"

//    """;

//Console.WriteLine($"user >>> {classificationPrompt}");
//ChatResponse classificationResponse = await client.GetResponseAsync(classificationPrompt);
//Console.WriteLine($"assistant >>> \n {classificationResponse}");
//Console.WriteLine($"Tokens used: in={classificationResponse.Usage?.InputTokenCount},out={classificationResponse.Usage?.OutputTokenCount}");

#endregion

#region Summarization

//var summaryPrompt = """
//    summarize the following blog in 1 concise sentences:

//    "Microservices is an architecture where an application is divided into small, independent services that communicate over a network. Each service handles a specific function and can be developed and deployed separately.

//    Services can be built using different programming languages and frameworks.
//    Each microservice is loosely coupled and can be developed, deployed, and scaled independently."

//    """;
//Console.WriteLine($"user >>> {summaryPrompt}");
//ChatResponse summaryResponse = await client.GetResponseAsync(summaryPrompt);
//Console.WriteLine($"assistant >>> \n {summaryResponse}");

#endregion

#region Sentiment Analysis

var analysisPrompt = """
    you will analize the sentiment of the following product reviews.
    Each lines is its own review.Output the sentiment of each review in a bulleted list and then provide a generate sentiment of all reviews.

    I bought this product and its amazing ,I love it!
    This Product is terrible , I hate it.
    I'm not sure about this product .It's Okay.
    I found this Product based on the others reviews. It Worked for a bit,and then it didn't.
    """;

Console.WriteLine($"user >>> {analysisPrompt}");
ChatResponse analysisResponse = await client.GetResponseAsync(analysisPrompt);
Console.WriteLine($"assistant >>> \n{analysisResponse}");
Console.WriteLine($"Tokens used: in={analysisResponse.Usage?.InputTokenCount},out={analysisResponse.Usage?.OutputTokenCount}");

#endregion

