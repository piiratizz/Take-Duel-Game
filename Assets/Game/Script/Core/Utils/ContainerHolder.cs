using Zenject;

public class ContainerHolder
{
    public static DiContainer Instance { get; private set; }

    public static void AttachContainer(DiContainer container)
    {
        Instance = container;
    }

    public static T Resolve<T>()
    {
        return Instance.Resolve<T>();
    }
}