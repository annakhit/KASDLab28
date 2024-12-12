public interface IMyDequee<T> : IMyCollection<T>
{
    void AddFirst(T obj);

    void AddLast(T obj);

    void Push(T obj);

    bool OfferFirst(T obj);

    bool OfferLast(T obj);

    bool RemoveLastOccurrence(object obj);

    bool RemoveFirstOccurrence(object obj);

    T GetFirst();

    T GetLast();

    T Pop();

    T PeekFirst();

    T PeekLast();

    T PollFirst();

    T PollLast();

    T RemoveFirst();

    T RemoveLast();
}