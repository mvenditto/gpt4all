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
            await RegenerateResponse();
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

    // stream the predicted tokens
    await foreach (var token in message.GetPredictionStreamingAsync())
    {
        Console.Write(token);
    }

    Console.WriteLine();
    return message;
}

async Task RegenerateResponse()
{
    // get the last message from the user
    var lastUserMsg = chat.Messages.SkipLast(1).TakeLast(1).Single();

    // delete the last prompt/response couple of messages
    chat.Messages = chat.Messages.Take(chat.Messages.Count - 2).ToList();

    // adjust the number of tokens in past conversation
    chat.Context.PastNum -= Math.Max(0, chat.Context.PastNum - lastResult.Usage.TotalTokens);

    // regenerate the response
    lastResult = await GeneratedResponse(lastUserMsg.Content);
}

class ChatPromptFormatter : DefaultPromptFormatter
{
    public ChatPromptFormatter()
    {
        _template = "### Human:\n{0}\n### Assistant:\n";
    }
}
