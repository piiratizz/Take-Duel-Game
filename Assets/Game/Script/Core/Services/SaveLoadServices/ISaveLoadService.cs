public interface ISaveLoadService
{
    void Save(PlayerData data);
    PlayerData Load();
}