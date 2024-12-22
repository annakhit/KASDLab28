public interface IMySet<T> : IMyCollection<T>
{
    T First();

    T Last();

    IMySet<T> SubSet(T fromElement, T toElement);

    IMySet<T> HeadSet(T toElement);

    IMySet<T> TailSet(T fromElement);
}