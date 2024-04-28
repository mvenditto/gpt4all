namespace Gpt4All;

public interface IGpt4AllModelFactory
{
    IGpt4AllModel LoadModel(
        string modelPath,
        string device = "cpu",
        int maxContextSize = 2048,
        int numGpuLayers = 100);
}
