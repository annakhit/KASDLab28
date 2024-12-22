public interface IMyNavigableSet<T> : IMySet<T>
{
    T Ceiling(T fromElement);

    T Floor(T fromElement);

    T Higher(T fromElement);

    T Lower(T fromElement);

    T PollLast();

    T PollFirst();
}
