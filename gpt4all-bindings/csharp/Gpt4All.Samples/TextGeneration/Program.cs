using Gpt4All;

if (args.Length < 2)
{
    Console.WriteLine($"Usage: TextCompletion <model-path> <prompt>");
    return;
}

var modelPath = args[0];
var prompt = args[1];

var modelFactory = new Gpt4AllModelFactory();

using var model = modelFactory.LoadModel(modelPath);

var result = await model.GetStreamingPredictionAsync(prompt);

await foreach (var token in result.GetPredictionStreamingAsync())
{
    Console.Write(token);
}

Console.WriteLine();
