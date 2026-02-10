namespace Snake3DWorld.Loader.Service
{
    public interface ILoadGameService
    {
        void LoadGame(string levelId);
        void LoadMainMenu();
    }
}