using System.Threading.Tasks;

namespace Core.Service
{
    public interface ISceneService
    {
        Task LoadSceneAsync(string sceneName, bool additive = false);
        Task UnloadSceneAsync(string sceneName);
        string GetActiveSceneName();
    }
}