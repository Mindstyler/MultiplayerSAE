using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

internal static class SceneLoader
{
    internal static async Task LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!loadOperation.isDone)
        {
            await Task.Yield();
        }
    }
}
