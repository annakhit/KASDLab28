public interface IMyCollection<T>
{
    void Add(T e);

    void AddAll(IMyCollection<T> a);

    void Clear();

    bool Contains(object obj);

    bool ContainsAll(IMyCollection<T> collection);

    bool IsEmpty();

    void Remove(object obj);

    void RemoveAll(IMyCollection<T> a);

    void RetainAll(IMyCollection<T> a);

    int Size();

    T[] ToArray();

    T[] ToArray(ref T[] a);
}
