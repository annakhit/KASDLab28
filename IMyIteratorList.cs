public interface IMyIteratorList<T>
{
    bool HasNext();

    T Next();

    bool HasPrevious();

    T Previous();

    int NextIndex();

    int PreviousIndex();

    void Remove();

    void Set(T element);

    void Add(T element);
}