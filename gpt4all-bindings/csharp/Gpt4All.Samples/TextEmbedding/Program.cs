using Gpt4All;

var modelFactory = new Gpt4AllModelFactory();

if (args.Length < 2)
{
    Console.WriteLine("Usage: TextEmbedding /path/to/ggml-all-MiniLM-L6-v2-f16 <text>");
    return;
}

var modelPath = args[0];
var text = args[1];

using var model = modelFactory.LoadModel(modelPath);

var result = await model.GenerateEmbeddingAsync(text);

Console.WriteLine($"Generated embeddings length: {result.Embeddings.Length}.");

for (var i = 0; i < (int)Math.Min(10, result.Embeddings.Length); i++)
{
    Console.Write($"{result.Embeddings[i]} ");
}
