

//get credentials from user secret

using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
//using OpenAI.Chat;
using System.ClientModel;
using System.Text.Json.Serialization;


IConfiguration config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

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

//var analysisPrompt = """
//    you will analize the sentiment of the following product reviews.
//    Each lines is its own review.Output the sentiment of each review in a bulleted list and then provide a generate sentiment of all reviews.

//    I bought this product and its amazing ,I love it!
//    This Product is terrible , I hate it.
//    I'm not sure about this product .It's Okay.
//    I found this Product based on the others reviews. It Worked for a bit,and then it didn't.
//    """;

//Console.WriteLine($"user >>> {analysisPrompt}");
//ChatResponse analysisResponse = await client.GetResponseAsync(analysisPrompt);
//Console.WriteLine($"assistant >>> \n{analysisResponse}");
//Console.WriteLine($"Tokens used: in={analysisResponse.Usage?.InputTokenCount},out={analysisResponse.Usage?.OutputTokenCount}");

#endregion

#region Structured Output

//var carListings = new[]
//{
//    "Check out this stylish 2019 Toyota Camry. It has a clean title, only 40,000 miles on the odometer, and a well-maintained interior. The car offers great fuel efficiency, a spacious trunk, and modern safety features like lane departure alert. Minimum offer price: $18,000. Contact Metro Auto at (555) 111-2222 to schedule a test drive.",
//    "Lease this sporty 2021 Honda Civic! With only 10,000 miles, it includes a sunroof, premium sound system, and backup camera. Perfect for city driving with its compact size and great fuel mileage. Located in Uptown Motors, monthly lease starts at $250 (excl. taxes). Call (555) 333-4444 for more info.",
//    "A classic 1968 Ford Mustang, perfect for enthusiasts. The vehicle needs some interior restoration, but the engine runs smoothly. V8 engine, manual transmission, around 80,000 miles. This vintage gem is priced at $25,000. Contact Retro Wheels at (555) 777-8888 if you’re interested.",
//    "Brand new 2023 Tesla Model 3 for lease. Zero miles, fully electric, autopilot capabilities, and a sleek design. Monthly lease starts at $450. Clean lines, minimalist interior, top-notch performance. For more details, call EVolution Cars at (555) 999-0000.",
//    "Selling a 2015 Subaru Outback in good condition. 60,000 miles on it, includes all-wheel drive, heated seats, and ample cargo space for family getaways. Minimum offer price: $14,000. Contact Forrest Autos at (555) 222-1212 if you want a reliable adventure companion.",
//};


//foreach (var listingText in carListings)
//{
//    string carDetailsPrompt = $"""
//        Convert the following car listing into a JSON object Matching this C# Schema:
//        Condition: "New" or "Used"
//        Make: (car manufacturer)
//        Model: (car model)
//        Year: (four-digit year)
//        ListingType: "Sale" or "Lease"
//        Price: integer only
//        Features: Array of short string
//        TenWordSummary: exactly ten words to summarize this listing

//        Here is the Listing:
//        {listingText}
//        """;

//    var response = await client.GetResponseAsync<CarDetails>(carDetailsPrompt);

//    if(response.TryGetResult(out var info))
//    {
//        //Convert the CarDetails Object to JSON for desplay
//        Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(info,new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));
//    }
//    else
//    {
//        Console.WriteLine("Response was not in the expected format.");
//    }

//}

//class CarDetails
//{
//    public required string Condition { get; set; }
//    public required string Make { get; set; }
//    public required string Model { get; set; }
//    public int Year { get; set; }
//    public CarListingType ListingType { get; set; }
//    public int Price { get; set; }
//    public required string[] Features { get; set; }
//    public required string TenWordSummary { get; set; }
//}

//[JsonConverter(typeof(JsonStringEnumConverter))]
//enum CarListingType { Sale, Lease };


#endregion

#region ChatApp

//Start the conversation with context for AI Model
List<ChatMessage> chatHistory = new()
{
    new ChatMessage(ChatRole.System,"""
    You are a friendly hiking enthusiast who helps people discover fun hikes in their area.
    You introduce yourself when first saying hello.
    When helping people out, you always ask them for this information
    to inform the hiking recommendation you provide:

    1. The location where they would like to hike
    2. What hiking intensity they are looking for

    You will then provide three suggestions for nearby hikes that vary in length
    after you get that information. You will also share an interesting fact about
    the local nature on the hikes when making a recommendation. At the end of your
    response, ask if there is anything else you can help with.
    """)
};

while (true)
{
    //get user prompt and add to chat history
    Console.WriteLine("Your Prompt:");
    var userPrompt = Console.ReadLine();
    chatHistory.Add(new ChatMessage(ChatRole.User, userPrompt));

    //Stream the AI Response and Add to chat history
    Console.WriteLine("AI Response:");
    var response = "";
    await foreach (var item in client.GetStreamingResponseAsync(chatHistory))
    {
        Console.Write(item.Text);
        response += item.Text;
    }
    chatHistory.Add(new ChatMessage(ChatRole.Assistant, response));
    Console.WriteLine();

}


#endregion