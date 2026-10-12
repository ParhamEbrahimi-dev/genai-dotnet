
using Microsoft.Extensions.AI;
using OllamaSharp;

//create a chat client
IChatClient client = new OllamaApiClient(new Uri("http://localhost:11434"), "llama3.2");


#region Basic Completion

////send Prompt & get response
//string prompt = "What is AI? Explain in max 20 words";
//Console.WriteLine($"user >>> {prompt}");

//ChatResponse response = await client.GetResponseAsync(prompt);
//Console.WriteLine($"assistant >>> {response}");
//Console.WriteLine($"Tokens used: in={response.Usage?.InputTokenCount},out={response.Usage?.OutputTokenCount}");

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