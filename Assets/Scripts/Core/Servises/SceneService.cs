using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Core.Service
{
    public class SceneService : ISceneService
    {
        public async Task LoadSceneAsync(string sceneName, bool additive = false)
        {
            var mode = additive ? LoadSceneMode.Additive : LoadSceneMode.Single;
            var asyncOp = SceneManager.LoadSceneAsync(sceneName, mode);

            while (!asyncOp.isDone)
                await Task.Yield();
        }

        public async Task UnloadSceneAsync(string sceneName)
        {
            if (!SceneManager.GetSceneByName(sceneName).isLoaded)
                return;

            var asyncOp = SceneManager.UnloadSceneAsync(sceneName);

            while (!asyncOp.isDone)
                await Task.Yield();
        }

        public string GetActiveSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }
    }
}