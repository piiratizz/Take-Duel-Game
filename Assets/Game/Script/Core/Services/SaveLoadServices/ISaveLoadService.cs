public interface ISaveLoadService<T>
{
    void Save(T data);
    T Load();
}