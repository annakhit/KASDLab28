public interface IMyQueue<T> : IMyCollection<T>
{
    T Element();

    T Peek();

    T Poll();
}