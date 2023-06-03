using Gpt4All;
using Gpt4All.Chat;

if (args.Length < 1)
{
    Console.WriteLine("Usage: Gpt4All.Samples <model-path>");
    return;
}

var modelFactory = new Gpt4AllModelFactory();
var modelPath = args[0];
using var model = modelFactory.LoadModel(modelPath);

Console.WriteLine("\nHELP: type your message or !R to regenerate the response.");

model.PromptFormatter = new ChatPromptFormatter();
model.SetThreadCount(8);

// create a new chat
var chat = model.CreateNewChat();

ITextPredictionStreamingResult? lastResult = null;

while (true)
{
    Console.WriteLine("\n[User]:");

    var userInput = Console.ReadLine();

    if (string.IsNullOrEmpty(userInput)) return;

    switch (userInput)
    {
        case "!R":
            if (lastResult == null) break;
            lastResult = await RegenerateResponse();
            break;
        default:
            lastResult = await GeneratedResponse(userInput);
            break;
    }
}

async Task<ITextPredictionStreamingResult> GeneratedResponse(string userInput)
{
    // add the user message to the chat
    chat.AddMessage(ChatRole.User, userInput);

    // ask the model to produce an "assistant" message
    var message = await model.GetStreamingMessageAsync(chat);

    Console.WriteLine("\n[Assistant]:");

    await foreach (var token in message.GetPredictionStreamingAsync())
    {
        Console.Write(token);
    }

    Console.WriteLine();
    return message;
}

async Task<ITextPredictionStreamingResult> RegenerateResponse()
{
    var message = await model.RegenerateChatResponse(
        chat,
        lastResult.Usage.TotalTokens // past conversation tokens
    );

    await foreach (var token in message.GetPredictionStreamingAsync())
    {
        Console.Write(token);
    }

    Console.WriteLine();
    return message;
}

class ChatPromptFormatter : DefaultPromptFormatter
{
    public ChatPromptFormatter()
    {
        _template = "### Human:\n{0}\n### Assistant:\n";
    }
}
