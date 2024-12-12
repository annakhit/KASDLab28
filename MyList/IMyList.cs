public interface IMyList<T> :  IMyCollection<T>
{
    void Add(int index, T element);

    void AddAll(int index, IMyCollection<T> a);

    int IndexOf(object o);

    int LastIndexOf(object o);

    void Set(int index, T element);

    T Get(int index);

    T Remove(int index);

    IMyList<T> SubList(int fromIndex, int toIndex);

    IMyIteratorList<T> ListIterator();

    IMyIteratorList<T> ListIterator(int index);
}