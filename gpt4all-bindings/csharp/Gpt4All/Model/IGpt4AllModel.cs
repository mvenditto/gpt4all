using Gpt4All.Chat;

namespace Gpt4All;

public interface IGpt4AllModel : ITextPrediction, IChatCompletion, IDisposable
{
    public void SetThreadCount(int threadCount);
}
