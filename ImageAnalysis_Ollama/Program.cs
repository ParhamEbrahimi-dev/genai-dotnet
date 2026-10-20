using Microsoft.Extensions.AI;
using OllamaSharp;

//create a chat client 
IChatClient client = new OllamaApiClient(new Uri("http://localhost:11434"), "llava");

//user prompts 
var propmtDescribe = "Describe the image";
var promptAnalyze = "How many red cars are in the picture? and what other cars color are there?";

//prompts
var systemPrompt = "You are a usefull asistant that describes images using a direct style.";
var userPrompt = promptAnalyze;

List<ChatMessage> messages = [
    new ChatMessage(ChatRole.System,systemPrompt),
    new ChatMessage(ChatRole.User,userPrompt)
    ];

//read the image bytes,create a new image content part and add int to the messages.
var imageFileName = "cars.png";
string image = Path.Combine(Directory.GetCurrentDirectory(), "images", imageFileName);

AIContent aic = new DataContent(File.ReadAllBytes(image), "image/png");
var message = new ChatMessage(ChatRole.User, [aic]);
messages.Add(message);

//send the messages to the assistant
var response = await client.GetResponseAsync(messages);
Console.WriteLine($"Prompt: {userPrompt}");
Console.WriteLine($"Image: {imageFileName}");
Console.WriteLine($"Response: {response.Messages[0]}");
Console.WriteLine($"Tokens used: in={response.Usage?.InputTokenCount},out={response.Usage?.OutputTokenCount}");
