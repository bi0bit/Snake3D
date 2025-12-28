using UnityEngine.AddressableAssets;

namespace TestSnake.Loader.Service
{
    public interface ILoadGameService
    {
        void LoadGame(string levelId);
        void LoadMainMenu();
    }
}